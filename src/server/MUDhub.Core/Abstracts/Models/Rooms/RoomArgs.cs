namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class RoomArgs
    {
        public string Description { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageKey { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsDefaultRoom { get; set; } = false;
    }
}
