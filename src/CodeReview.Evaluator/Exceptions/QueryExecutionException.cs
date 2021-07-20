using System;

namespace GodelTech.CodeReview.Evaluator.Exceptions
{
    public class QueryExecutionException : Exception
    {
        public QueryExecutionException(string message)
            : base(message)
        {
        }
    }
}
