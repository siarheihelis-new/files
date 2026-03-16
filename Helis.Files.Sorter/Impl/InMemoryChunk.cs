using System.Buffers;

namespace Helis.Files.Sorter.Impl
{
    internal sealed class InMemoryChunk : IDisposable   
    {
        private string[] _data;
        private int _offset = 0;
        private long _size = 0;
        private bool _disposed = false;
        private readonly SorterOptions _options;
        private readonly LineComparer _comparer = new LineComparer();

        public InMemoryChunk(SorterOptions options)
        {
            _data = ArrayPool<string>.Shared.Rent(options.MaxInMemoryChunkSize);
            _options = options;
        }

        public void Add(string line)
        {
            //we don't check for overflow here, because we assume the caller will not add more lines than the chunk can hold
            _data[_offset] = line;
            _offset++;
            _size +=  line.Length*2L; // Approximate size in bytes (don't include new line)
        }
        public bool IsFull => _offset >= _options.MaxInMemoryChunkSize || _size >= _options.MaxMemoryPerChunk;
        public bool IsEmpty => _offset == 0;
        public int Count => _offset;
        public void Sort()
        {
            Array.Sort(_data, 0, _offset, _comparer);
        }

        public void WriteTo(StreamWriter writer)
        {
            for (int i = 0; i < _offset; i++)
            {
                writer.WriteLine(_data[i]);
            }
        }

        public async Task WriteToAsync(StreamWriter writer, CancellationToken cancellationToken = default)
        {

            for (int i = 0; i < _offset; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await writer.WriteLineAsync(_data[i].AsMemory(), cancellationToken);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (_data != null)
            {
                ArrayPool<string>.Shared.Return(_data);
            }

            _disposed = true;
        }

        ~InMemoryChunk()
        {
            Dispose(false);
        }
    }
}
