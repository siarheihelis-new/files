using System.Buffers;
using System.Collections.Concurrent;

namespace Helis.Files.Sorter.Impl
{
    internal class FixSizePartitioner : IPartitioner
    {
        public async Task<IList<string>> CreateSortedChunksAsync(SorterOptions options, CancellationToken cancellationToken = default)
        {
            var chunks = new List<string>();
            using var sortWriteSemaphore = new SemaphoreSlim(options.MaxDegreeOfParallelism);
            var sortWriteTasks = new List<Task>();
            ConcurrentBag<string> chunkPaths = new ConcurrentBag<string>();
            using (var reader = new StreamReader(options.InputFile, System.Text.Encoding.UTF8, true, options.BufferSize))
            {
                InMemoryChunk chunk = new InMemoryChunk(options);
                

                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    chunk.Add(line);
                    // When chunk is full, sort it and save to temporary file
                    if (chunk.IsFull)
                    {
                        await sortWriteSemaphore.WaitAsync(cancellationToken);
                        InMemoryChunk chunkCopy = chunk; // Capture the current chunk for the task
                        chunk = new InMemoryChunk(options); // Start a new chunk for the next lines
                        var sortWriteTask = Task.Run(() =>
                        {
                            try
                            {
                                string chunkFile = SaveSortedChunk(chunkCopy, options);
                                chunkPaths.Add(chunkFile);
                            }
                            finally
                            {
                                sortWriteSemaphore.Release();
                            }
                        }, cancellationToken);
                        sortWriteTasks.Add(sortWriteTask);
                    }
                }

                // Handle remaining lines in the last chunk
                if (!chunk.IsEmpty)
                {
                    await sortWriteSemaphore.WaitAsync(cancellationToken);
                    var sortWriteTask = Task.Run(() =>
                    {
                        try
                        {
                            string chunkFile = SaveSortedChunk(chunk, options);
                            chunkPaths.Add(chunkFile);
                        }
                        finally
                        {
                            sortWriteSemaphore.Release();
                        }
                    }, cancellationToken);
                    sortWriteTasks.Add(sortWriteTask);
                }
                await Task.WhenAll(sortWriteTasks);
                return chunkPaths.ToList();

            }
        }

        private string SaveSortedChunk(InMemoryChunk chunk, SorterOptions options)
        {
            chunk.Sort();

            // Create a temporary file for this chunk
            string tempFile = Path.Combine(options.BaseTempDirectory, $"chunk_{Guid.NewGuid()}.tmp");

            using (var writer = new StreamWriter(tempFile, false, System.Text.Encoding.UTF8, options.BufferSize))
            {
                chunk.WriteTo(writer);
            }
            chunk.Dispose();
            return tempFile;
        }
    }
}
