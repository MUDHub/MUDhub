using System;
using System.Collections.Generic;
using System.Text;
using MUDhub.Core.Models;
using MUDhub.Core.Services.Models;

namespace MUDhub.Core.Abstracts
{
    public interface ILoginService
    {
        LoginResult LoginAsync();
    }
}
