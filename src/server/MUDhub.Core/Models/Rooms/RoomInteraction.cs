using System;

namespace MUDhub.Core.Models.Rooms
{
    public class RoomInteraction
    {
        public RoomInteraction()
        {
            Id = Guid.NewGuid().ToString();
        }

        public RoomInteraction(string id)
        {
            Id = id;
        }

        public string Id { get; }
        public string Description { get; set; } = string.Empty;
        public string ExecutionMessage { get; set; } = string.Empty;
        public virtual Room Room { get; set; } = new Room();

        public InteractionType Type { get; set; } = InteractionType.Mob;

        public string RelatedId { get; set; } = string.Empty;
        public string GameId { get; set; } = string.Empty;
    }
}
