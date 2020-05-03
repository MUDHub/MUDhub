using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models
{
    public class UpdateUserArgs
    {
        public string? Firstname { get; set; } = null;
        public string? Lastname { get; set; } = null;
    }
}
