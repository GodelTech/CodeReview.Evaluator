﻿using System.IO;
using System.Threading.Tasks;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IFileService
    {
        Task<string> ReadAllTextAsync(string path);
        Task WriteAllTextAsync(string path, string text);
        FileStream OpenRead(string path);
        bool Exists(string path);
    }
}