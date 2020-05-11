using System;

namespace MUDhub.Core.Models.Users
{
    [Flags]
    public enum Roles
    {
        Player = 1,
        Master = 2,
        Admin = 4,
    }
}
