using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace KursovoiWPF.ValidationRules
{
    public class EmailRule : ValidationRule
    {
        public override ValidationResult Validate(object value,
       System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Адрес электронной почты не задан! ");

            string email = value.ToString();
            Regex regex = new Regex("^\\S +@\\S +\\.\\S + $");
            if (email.Contains("@") && email.Contains("."))
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false,
                   "Адрес электронной почты должен содержать символы @ и(.) точки \n Шаблон адреса: adress@mymail.com");
            }
        }
    }
}
