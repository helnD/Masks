using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace View
{
    public class BrushValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if ((string)value == null || (string)value == "")
            {
                return new ValidationResult(false, "Установите значение кисти");
            }

            if (Regex.IsMatch((string)value, "[^0-9]"))
            {
                return new ValidationResult(false, "Неверное значение кисти");

            }
            
            if (int.Parse((string)value) < 0 || int.Parse((string)value) > 255)
            {
                return new ValidationResult(false, "Неверное значение кисти");
            }
            
            return ValidationResult.ValidResult;
        }
    }
}