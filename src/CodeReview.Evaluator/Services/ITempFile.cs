using System;

namespace ReviewItEasy.Evaluator.Services
{
    public interface ITempFile : IDisposable
    {
        public string FilePath { get; }
    }
}