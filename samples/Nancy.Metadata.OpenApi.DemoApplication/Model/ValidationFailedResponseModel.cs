using System.Collections.Generic;
using Nancy.Validation;

namespace Infocode.Nancy.Metadata.OpenApi.DemoApplication.Model
{
    public class ValidationFailedResponseModel
    {
        public ValidationFailedResponseModel(string message)
        {
            Messages = new List<string>() { message };
        }

        public ValidationFailedResponseModel(ModelValidationResult validationResult)
        {
            var messages = new List<string>();

            foreach (var errorGroup in validationResult.Errors)
            {
                foreach (var error in errorGroup.Value)
                {
                    messages.Add(error.ErrorMessage);
                }
            }

            Messages = messages;
        }

        public IEnumerable<string> Messages { get; set; }
    }
}
