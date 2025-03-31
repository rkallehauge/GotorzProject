using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotorzProject.Shared
{
    public class MustHaveOneElementAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IEnumerable<object>;
            if (list != null)
            {
                return list.Count() > 0;
            }
            return false;
        }
    }
}
