namespace MUDhub.Core.Abstracts.Models
{
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
