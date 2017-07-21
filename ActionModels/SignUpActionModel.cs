using Api_ELearning.Models;

namespace Api_ELearning.ActionModels
{
    public class SignUpActionModel:Base
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
