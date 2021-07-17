using System.IO;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class InitScriptProvider : IInitScriptProvider
    {
        public string GetIssuesDbScript() => GetDbInitScript("IssueDbScript.sql");
        public string GetFileDetailsDbScript() => GetDbInitScript("FileDetailsDbScript.sql");
        
        private static string GetDbInitScript(string scriptName)
        {
            using var stream = typeof(InitScriptProvider).Assembly.GetManifestResourceStream($"GodelTech.CodeReview.Evaluator.Resources.{scriptName}");
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
