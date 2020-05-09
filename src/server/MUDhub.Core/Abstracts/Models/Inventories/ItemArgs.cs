namespace MUDhub.Core.Abstracts.Models.Inventories
{
    public class ItemArgs
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Weight { get; set; }
        public string ImageKey { get; set; } = string.Empty;
    }
}
