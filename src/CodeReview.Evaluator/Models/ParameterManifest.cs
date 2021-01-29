using System;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace GodelTech.CodeReview.Evaluator.Models
{
    // The purpose of this class avoiding infinite deserialization loop
    public class ParameterManifestBase
    {
        [Required]
        [MaxLength(Constants.ValueMaxLength)]
        public string Value { get; set; }
        public bool IsNull { get; set; }
        public bool IsInt { get; set; }
    }
    
    public class ParameterManifest : ParameterManifestBase, IYamlConvertible
    {
        public void Read(IParser parser, Type expectedType, ObjectDeserializer nestedObjectDeserializer)
        {
            if (parser.TryConsume<Scalar>(out var scalar))
            {
                Value = scalar.Value;
            }
            else
            {
                var values = (ParameterManifestBase)nestedObjectDeserializer(typeof(ParameterManifestBase));
                
                Value = values.Value;
                IsInt = values.IsInt;
                IsNull = values.IsNull;
            }
        }

        public void Write(IEmitter emitter, ObjectSerializer nestedObjectSerializer)
        {
            if (!IsNull && !IsInt)
            {
                nestedObjectSerializer(Value);
            }
            else
            {
                nestedObjectSerializer(new ParameterManifestBase
                {
                    Value = Value, 
                    IsInt = IsInt, 
                    IsNull = IsNull
                });
            }
        }
    }
}