using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models
{
    [Flags]
    public enum Role
    {
        Player = 0,
        Master = 1,
        Admin = 2,
    }
}
