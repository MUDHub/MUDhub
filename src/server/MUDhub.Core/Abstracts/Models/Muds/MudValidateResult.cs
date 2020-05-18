using System;
using System.Collections.Generic;
using System.Text;

namespace MUDhub.Core.Abstracts.Models.Muds
{
    public class MudValidateResult
    {

        public MudValidateResult(IEnumerable<MudValidateErrorMessage> errorMessages)
        {
            ErrorMessages = errorMessages;
        }

        public IEnumerable<MudValidateErrorMessage> ErrorMessages { get; }
        public bool Valid { get; set; }

        public string ExecutionError { get; set; } = string.Empty;
        public bool Success { get; set; }
    }

    public class MudValidateErrorMessage
    {
        public ErrorRegion Region { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public enum ErrorRegion
    {
        General = 0,
        Races = 1,
        Classes = 2,
        Items = 3,
        Areas = 4,
    }
}
