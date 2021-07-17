namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IInitScriptProvider
    {
        string GetIssuesDbScript();
        string GetFileDetailsDbScript();
    }
}