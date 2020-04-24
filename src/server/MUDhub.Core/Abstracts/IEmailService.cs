using MUDhub.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts
{
    public interface IEmailService
    {
        bool Send(MailMaker mailmaker);
    }
}
