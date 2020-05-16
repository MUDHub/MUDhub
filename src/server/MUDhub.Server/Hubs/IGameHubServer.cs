using MUDhub.Server.Hubs.Models;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs
{
    public interface IGameHubServer
    {
        Task<JoinMudGameResult> TryJoinMudGame(string characterid);
        Task<EnterRoomResult> TryEnterRoom(Direction direction, string? portalArg = null);
        Task<TransferItemResult> TryTransferItem(string ItemName, ItemTransferMethode reason);

        Task<InventoryResult> GetInventory(bool getActualRoomInventory);

        Task SendGlobalMessage(string message);
        //Todo: after navigation
        Task SendRoomMessage(string message);
        Task<SendPrivateMessageResult> SendPrivateMesage(string message, string targetCharacterName);
    }

    public enum ItemTransferMethode
    {
        Pickup = 0,
        Drop = 1
    }
}
