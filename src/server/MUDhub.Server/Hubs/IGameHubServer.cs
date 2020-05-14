using MUDhub.Server.Hubs.Models;
using System.Threading.Tasks;

namespace MUDhub.Server.Hubs
{
    public interface IGameHubServer
    {
        Task<JoinMudGameResult> TryJoinMudGame(string characterid);
        Task<JoinRoomResult> TryJoinRoom(string roomid);
        Task<TransferItemResult> TryTransferItem(string itemid, string targetid);



        Task SendGlobalMessage(string message);
        //Todo: after navigation
        Task SendRoomMessage(string message);
        Task<SendPrivateMessageResult> SendPrivateMesage(string message, string targetCharacterName);
    }
}
