using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models
{
    public class UpdateUserArgs
    {
        public string Name { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
    }
}
