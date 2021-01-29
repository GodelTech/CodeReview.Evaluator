using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace GodelTech.CodeReview.Evaluator.Models
{
    public class FilterDefinition : FilterDefinitionBase, IYamlConvertible
    {
        public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
        {
            if (parser.TryConsume<Scalar>(out var scalar))
            {
                Pattern = scalar.Value;
            }
            else
            {
                var values = (FilterDefinitionBase)nestedObjectDeserializer(typeof(FilterDefinitionBase));

                Pattern = values.Pattern;
                IsRegex = values.IsRegex;
            }
        }

        public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            if (!IsRegex)
            {
                nestedObjectSerializer(Pattern);
            }
            else
            {
                nestedObjectSerializer(new FilterDefinitionBase
                {
                    Pattern = Pattern,
                    IsRegex = IsRegex
                });
            }
        }
    }
}