using System.Collections.Concurrent;

namespace Helis.Files.Sorter.Impl
{
    internal class MergeSort : IMergeSort
    {
        private readonly LineComparer _comparer = new LineComparer();

        public async Task MergeChunksAsync(SorterOptions options, IList<string> chunkFiles, CancellationToken cancellationToken = default)
        {
            if (chunkFiles.Count == 0)
            {
                File.WriteAllText(options.OutputFile, "");
                return;
            }

            if (chunkFiles.Count == 1)
            {
                File.Copy(chunkFiles[0], options.OutputFile, true);
                return;
            }

            var currentFiles = new List<string>(chunkFiles);
            int mergeRound = 0;

            while (currentFiles.Count > 1)
            {
                bool isFinalRound = currentFiles.Count <= options.MergeFilesPerRun;
                var mergedFiles = new ConcurrentBag<string>();
                var mergeTasks = new List<Task>();
                var semaphore = new SemaphoreSlim(options.MaxDegreeOfParallelism);

                // Process files in batches 
                for (int i = 0; i < currentFiles.Count; i += options.MergeFilesPerRun)
                {
                    int batchSize = Math.Min(options.MergeFilesPerRun, currentFiles.Count - i);
                    var batchFiles = currentFiles.Skip(i).Take(batchSize).ToList();
                    string mergedFile = isFinalRound
                        ? options.OutputFile 
                        : Path.Combine(options.BaseTempDirectory, $"merged_{mergeRound}_{i}.tmp");

                    await semaphore.WaitAsync(cancellationToken);
                    mergeTasks.Add(Task.Run(() =>
                    {
                        try
                        {
                            MergeBatch(options, batchFiles, mergedFile);
                            mergedFiles.Add(mergedFile);
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }, cancellationToken));
                }

                // Wait for all batch merges to complete
                await Task.WhenAll(mergeTasks);

                // Clean up the current batch of files (skip OutputFile in final round)
                foreach (var file in currentFiles)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        // Ignore deletion errors
                    }
                }

                currentFiles = mergedFiles.ToList();

                mergeRound++;
            }
        }

       

        private void MergeBatch(SorterOptions options, IList<string> batchFiles, string outputFile)
        {
            using (var outputWriter = new StreamWriter(outputFile, false, System.Text.Encoding.UTF8, options.BufferSize))
            {
                var readers = new List<StreamReader>();
                var priorityQueue = new PriorityQueue<(int fileIndex, string line), string>(_comparer);

                try
                {
                    // Open all files in the batch and load the first line from each
                    for (int i = 0; i < batchFiles.Count; i++)
                    {
                        var reader = new StreamReader(batchFiles[i], System.Text.Encoding.UTF8, true, options.BufferSize);
                        readers.Add(reader);

                        string? line = reader.ReadLine();
                        if (line != null)
                        {
                            priorityQueue.Enqueue((i, line), line);
                        }
                    }

                    // Merge all lines from the batch files
                    while (priorityQueue.Count > 0)
                    {
                        var (fileIndex, line) = priorityQueue.Dequeue();
                        outputWriter.WriteLine(line);

                        string? nextLine = readers[fileIndex].ReadLine();

                        // Write consecutive lines from the same stream. It allows to reduce number of Dequeue and Endqueue operations
                        /*while (nextLine != null && 
                               (priorityQueue.Count == 0 || _comparer.Compare(nextLine, priorityQueue.Peek().line) <= 0))
                        {
                            outputWriter.WriteLine(nextLine);
                            nextLine = readers[fileIndex].ReadLine();
                        }*/

                        if (nextLine != null)
                        {
                            priorityQueue.Enqueue((fileIndex, nextLine), nextLine);
                        }
                    }
                }
                finally
                {
                    foreach (var reader in readers)
                    {
                        reader?.Dispose();
                    }
                }
            }
        }
    }
}
