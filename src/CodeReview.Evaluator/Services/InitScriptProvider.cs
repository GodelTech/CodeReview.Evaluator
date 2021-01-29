using System.IO;

namespace ReviewItEasy.Evaluator.Services
{
    public class InitScriptProvider : IInitScriptProvider
    {
        public string GetDbInitScript()
        {
            using (var stream = typeof(InitScriptProvider).Assembly.GetManifestResourceStream("ReviewItEasy.Evaluator.Resources.CreateDatabase.sql"))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
