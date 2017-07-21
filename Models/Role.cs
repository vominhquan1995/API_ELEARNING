using System;
using System.Collections.Generic;
using System.Text;

namespace Api_ELearning.Models
{
    public class Role : Base
    {
        public string RoleName { get; set; }
        public string Description { get; set; }
        public ICollection<Account> Accounts { get; set; }
    }
}
