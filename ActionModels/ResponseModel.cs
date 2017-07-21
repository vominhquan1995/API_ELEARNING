using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.ActionModels
{
    public class ResponseModel
    {
        public int serviceCode { get; set; }
        public string serviceMsg { get; set; }
        public object result { get; set; }
    }
}
