namespace Helis.Files.Sorter.Impl
{
    public class FileSorter : IFileSorter
    {
        private readonly IPartitioner _partitioner = new FixSizePartitioner();
        private readonly IMergeSort _mergeSort = new MergeSort();
        public async Task Sort(SorterOptions options)
        {
            var chunkFiles = await  _partitioner.CreateSortedChunksAsync(options);
            await _mergeSort.MergeChunksAsync(options, chunkFiles);
        }
    }
}
