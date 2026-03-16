namespace Helis.Files.Sorter.Impl
{
    internal interface IMergeSort
    {
        Task MergeChunksAsync(SorterOptions options, IList<string> chunkFiles, CancellationToken cancellationToken = default);
    }
}
