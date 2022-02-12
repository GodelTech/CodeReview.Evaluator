using System;
using System.IO;

namespace CodeReview.Evaluator.IntegrationTests.Utils
{
    internal class TempFile : IDisposable
    {
        public string FilePath { get; }
        public bool Exists => File.Exists(FilePath);

        public TempFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(filePath));

            FilePath = filePath;
        }

        public void Write(string content)
        {
            File.WriteAllText(FilePath, content);
        }

        public void Write(Stream content)
        {
            using var targetStream = File.Create(FilePath);

            content.CopyTo(targetStream);
        }

        public void Dispose()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        public string ReadAllText()
        {
            return File.Exists(FilePath) ? 
                File.ReadAllText(FilePath) : 
                string.Empty;
        }
    }
}