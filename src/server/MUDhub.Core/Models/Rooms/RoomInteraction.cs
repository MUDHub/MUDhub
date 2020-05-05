namespace MUDhub.Core.Models.Rooms
{
    public class RoomInteraction
    {

        public RoomInteraction(string roomInteractionId)
        {
            Id = roomInteractionId;
        }

        public string Id { get; }
        public string Description { get; set; } = string.Empty;
        public string ExecutionMessage { get; set; } = string.Empty;
        public Room Room { get; set; } = new Room();

        //Todo: Was ist der Default-Wert für diese Property?
        public InteractionType Type { get; set; } = InteractionType.Mob;

        //Todo: Was ist die RelatedId
        public string RelatedId { get; set; } = string.Empty;
    }
}
