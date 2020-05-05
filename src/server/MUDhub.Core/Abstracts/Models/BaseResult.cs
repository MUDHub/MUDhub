using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models
{
    public class BaseResult
    {
        public bool Success { get; set; } = true;
        public string Errormessage { get; set; } = string.Empty;
    }
}
