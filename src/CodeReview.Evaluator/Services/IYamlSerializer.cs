namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IYamlSerializer
    {
        T Deserialize<T>(string content);
        string Serialize(object person);
    }
}