using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace MosaicoSolutions.CSharpProperties
{
    [Serializable]
    public class JsonSchemaValidationException : Exception
    {
        private const string MessageDefault = @"Error while validating Json";

        private readonly IList<string> _messages = new List<string>();

        public JsonSchemaValidationException()
        {
        }

        public JsonSchemaValidationException(IList<string> messages) => _messages = messages ?? new List<string>();

        public JsonSchemaValidationException(string message) : base(message)
        {
        }

        public JsonSchemaValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JsonSchemaValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message
            => MessageDefault + (ContainsErroMessages
                                    ? $": \n{JoinMessages()}" 
                                    : ".");

        private bool ContainsErroMessages => _messages.Any();

        private string JoinMessages()
            => _messages.Aggregate(new StringBuilder(), 
                                  (stringBuilder, message) => stringBuilder.AppendLine(message),
                                  stringBuilder => stringBuilder.ToString());
    }
}