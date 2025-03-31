using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GotorzProject.Shared
{
    public class EmployeeRegisterModel : RegisterModel
    {
        [Required]
        [MustHaveOneElement]
        public List<string> AssignedRoles { get; set; }
    }
}
