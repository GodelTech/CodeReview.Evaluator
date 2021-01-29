using System.IO;

namespace ReviewItEasy.Evaluator.Services
{
    public interface IDirectoryService
    {
        bool Exists(string path);
        string[] GetFiles(string path, string pattern, SearchOption options);
        DirectoryInfo CreateDirectory(string path);
    }
}