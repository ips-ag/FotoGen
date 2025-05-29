using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FotoGen.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendTrainingCompletedEmailAsync(string email, string modelName, string link);
        Task SendTrainingFailedEmailAsync(string email, string modelName, string error);
    }
}
