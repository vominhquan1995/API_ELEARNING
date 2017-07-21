
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.Models
{
    public class Notification : Base
    {
       public String Title { set; get; }
       public String Body { set; get; }
       public String urlImage { set; get; }
       public DateTime dateStart { get; set; }
       public DateTime dateEnd { get; set; }
    }
}
