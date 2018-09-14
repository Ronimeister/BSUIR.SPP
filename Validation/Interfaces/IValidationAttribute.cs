using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Interfaces
{
    public interface IValidationAttribute
    {
        string ErrorMessage { get; }
        bool IsValid(object value);
    }
}
