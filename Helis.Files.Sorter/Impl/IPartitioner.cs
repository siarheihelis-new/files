namespace Helis.Files.Sorter.Impl
{
    internal interface IPartitioner
    {
        public Task<IList<string>> CreateSortedChunksAsync(SorterOptions options, CancellationToken cancellationToken = default);
    }
}
