using System;

namespace GodelTech.CodeReview.Evaluator.Exceptions
{
    public class QueryExecutionException : Exception
    {
        public QueryExecutionException()
        {
        }

        public QueryExecutionException(string message)
            : base(message)
        {
        }

        public QueryExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
