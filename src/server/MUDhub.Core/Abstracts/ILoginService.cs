using System;
using System.Collections.Generic;
using System.Text;
using MUDhub.Core.Models;

namespace MUDhub.Core.Abstracts
{
    public interface ILoginService
    {
        LoginResult LoginAsync();
    }
}
