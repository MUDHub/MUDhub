using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class RoomInteractionArgs
    {
        public string Description { get; set; } = string.Empty;
        public string ExecutionMessage { get; set; } = string.Empty;
        public string RelatedId { get; set; } = string.Empty;
        public InteractionType Type { get; set; } = InteractionType.Mob;
    }
}
