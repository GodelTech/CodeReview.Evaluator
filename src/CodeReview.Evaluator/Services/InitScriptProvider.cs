using System.IO;

namespace GodelTech.CodeReview.Evaluator.Services
{
    public class InitScriptProvider : IInitScriptProvider
    {
        public string GetDbInitScript()
        {
            using (var stream = typeof(InitScriptProvider).Assembly.GetManifestResourceStream("GodelTech.CodeReview.Evaluator.Resources.CreateDatabase.sql"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
