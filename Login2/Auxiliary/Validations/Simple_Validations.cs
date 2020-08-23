using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Login2.Auxiliary.Validations
{
    class IsInt_Validation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var str = value as string;      // Convert to our string
            var sb = new StringBuilder();   // For the error messages
            var valid = true;               // Rather obvious
            int temp_int;

            if (!int.TryParse(str, out temp_int))
            {
                valid = false;
                sb.AppendLine("This is not a valid integer number.");
            }

            return new ValidationResult(valid, sb.ToString());
        }
    }
}
