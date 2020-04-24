using MUDhub.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string receiver, string resetKey);
    }
}
