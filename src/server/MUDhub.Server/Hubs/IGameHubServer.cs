using MUDhub.Server.Hubs.Models;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs
{
    public interface IGameHubServer
    {
        Task<JoinMudGameResult> TryJoinMudGame(string characterid);
        Task<EnterRoomResult> TryEnterRoom(Direction direction, string? portalArg = null);
        Task<TransferItemResult> TryTransferItem(string itemid, string targetid);



        Task SendGlobalMessage(string message);
        //Todo: after navigation
        Task SendRoomMessage(string message);
        Task<SendPrivateMessageResult> SendPrivateMesage(string message, string targetCharacterName);
    }
}
