using MUDhub.Core.Abstracts.Models.Rooms;
using System.Threading.Tasks;

namespace MUDhub.Core.Abstracts
{
    public interface INavigationService
    {
        Task<NavigationResult> TryEnterRoomAsync(string characterId, string roomId);
        //Task<bool> JoinWorldAsync(string mudId, string characterId);
        //Task<bool> LeaveWorldAsync(string characterId);
    }
}
