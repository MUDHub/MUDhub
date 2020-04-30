using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models;
using MUDhub.Core.Abstracts.Models;

namespace MUDhub.Core.Abstracts
{
    public interface ILoginService
    {
        Task<LoginResult> LoginUserAsync(string email, string password);
    }
}
