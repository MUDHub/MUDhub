using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace MUDhub.Core.Abstracts.Models
{
    public class MudCreationArgs
    {
        public MudCreationArgs()
        {

        }

        public MudCreationArgs(MudCreationArgs args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Description = args.Description;
            ImageKey = args.ImageKey;
            IsPublic = args.IsPublic;
            AutoRestart = args.AutoRestart;
        }

        public string? Description { get; set; } = null;
        public string? ImageKey { get; set; } = null;
        public bool? IsPublic { get; set; } = null;
        public bool? AutoRestart { get; set; } = null;
    }

    public class MudUpdateArgs : MudCreationArgs
    {
        public MudUpdateArgs()
        {

        }

        public MudUpdateArgs(MudCreationArgs args) 
            : base(args)
        {

        }
        public string? Name { get; set; } = null;
    }
}
