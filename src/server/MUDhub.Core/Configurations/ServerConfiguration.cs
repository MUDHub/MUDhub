using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Core.Configurations
{
    public class ServerConfiguration
    {
        public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();

        public SpaConfiguration Spa { get; set; } = new SpaConfiguration();

        public MailConfiguration Mail { get; set; } = new MailConfiguration();

        public string TokenSecret { get; set; } = string.Empty;

        public bool MudAutoRestart { get; set; } = true;

        public string ImageResourcePath { get; set; } = $"resources{Path.DirectorySeparatorChar}images";
    }
}
