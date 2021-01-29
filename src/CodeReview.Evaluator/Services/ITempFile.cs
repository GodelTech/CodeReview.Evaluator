using System;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface ITempFile : IDisposable
    {
        public string FilePath { get; }
    }
}