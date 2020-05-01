using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Models
{
    [Flags]
    public enum Roles
    {
        Player = 1,
        Master = 2,
        Admin = 4,
    }
}
