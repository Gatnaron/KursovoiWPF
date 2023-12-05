using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KursovoiWPF.ValidationRules
{
    internal class CodeRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
       System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
            {
                PageMain.canSave = false;
                return new ValidationResult(false, "Код товара введён некорректно! ");
            }
            string code = value.ToString();
            Regex regex = new Regex(@"\D{3}-\d{3}-\d{3}-\D{3}-\d{3}");
            if (regex.IsMatch(code))
            {
                PageMain.canSave = true;
                Console.WriteLine("1");
                return new ValidationResult(true, null);
            }
            else
            {
                PageMain.canSave = false;
                return new ValidationResult(false,
                   "Код товара должен иметь формат XXX-000-000-XXX-000");
            }
        }
    }
}
