using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace WeatherShape.Configuration.Models
{
    public abstract class Validatable
    {
        public bool Validate(Serilog.ILogger logger)
        {
            var result = false;

            var validation = new List<ValidationResult>();

            if (Validator.TryValidateObject(this, new ValidationContext(this), validation, validateAllProperties: true))
            {
                result = true;
            } else
            {
                result = false;

                var validationResult = validation.Aggregate(new StringBuilder(), (sb, vr) => sb.AppendLine(vr.ErrorMessage));
                var message = $"Validation of {this} field failed \n {validationResult}";

                logger.Fatal(message);
            }

            return result;
        }
    }
}
