using System;
using System.Collections.Generic;
using System.Reflection;

namespace GodelTech.CodeReview.Evaluator.Utils
{
    public interface ICommandLineOptionsTypeProvider
    {
        ICollection<Type> GetOptions(Assembly assembly);
    }
}