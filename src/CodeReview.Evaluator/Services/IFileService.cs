using System.IO;
using System.Threading.Tasks;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IFileService
    {
        Task<string> ReadAllTextAsync(string path);
        Task WriteAllTextAsync(string path, string text);
        FileStream Create(string path);
        FileStream OpenRead(string path);
        bool Exists(string path);
        void Delete(string path);
        ITempFile CreateTempFile();
    }
}