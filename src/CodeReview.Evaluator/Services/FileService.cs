using System;
using System.IO;
using System.Threading.Tasks;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class FileService : IFileService
    {
        public Task<string> ReadAllTextAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));

            return File.ReadAllTextAsync(path);
        }

        public Task WriteAllTextAsync(string path, string text)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));

            return File.WriteAllTextAsync(path, text);
        }

        public FileStream Create(string path)
        {
            return File.Create(path);
        }

        public FileStream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        public bool Exists(string path)
        {
            return File.Exists(path);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public ITempFile CreateTempFile()
        {
            var tempFilePath = Path.GetTempFileName();

            return new TempFile(tempFilePath, () => File.Delete(tempFilePath));
        }
    }


    public class TempFile : ITempFile
    {
        private readonly Action _onDispose;

        public string FilePath { get; }

        public TempFile(string filePath, Action onDispose)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(filePath));

            FilePath = filePath;
            
            _onDispose = onDispose ?? throw new ArgumentNullException(nameof(onDispose));
        }

        void IDisposable.Dispose()
        {
            _onDispose();
        }
    }
}
