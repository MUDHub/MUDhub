using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Core.Configurations
{
    public class ServerConfiguration
    {
        public DatabaseConfiguration Database { get; set; } = new DatabaseConfiguration();

        public SpaConfiguration Spa { get; set; } = new SpaConfiguration();

        public MailConfiguration Mail { get; set; } = new MailConfiguration();

        public string TokenSecret { get; set; } = "Secret, that should never be used.";

        public bool MudAutoRestart { get; set; } = true;
    }
}
