using MUDhub.Core.Models.Connections;

namespace MUDhub.Core.Abstracts.Models.Connections
{
    public class LockArgs
    {
        public LockType LockType { get; set; }
        public string LockDescription { get; set; } = string.Empty;
        public string LockAssociatedId { get; set; } = string.Empty;
    }
}
