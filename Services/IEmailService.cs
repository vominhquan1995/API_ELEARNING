using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api_ELearning.Services
{
    public interface IEmailService
    {
        Task<bool> SendMailRecoveryPassword(string email);
    }
}
