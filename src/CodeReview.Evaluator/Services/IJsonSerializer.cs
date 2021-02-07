namespace GodelTech.CodeReview.Evaluator.Services
{
    public interface IJsonSerializer
    {
        string Serialize(object value);
        T Deserialize<T>(string json);
    }
}