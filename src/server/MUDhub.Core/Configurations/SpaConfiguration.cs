namespace MUDhub.Core.Configurations
{
    public class SpaConfiguration
    {
        public bool IntegratedHosting { get; set; } = true;
        public string RelativePath { get; set; } = "client";
        public string ExternalHostingUrl { get; set; } = "http://localhost:4200";
    }
}