using MUDhub.Core.Abstracts.Models.Characters;
using System.Threading.Tasks;


namespace MUDhub.Core.Abstracts
{
    public interface ICharacterManager
    {
        Task<CharacterResult> CreateCharacterAsync(string userid, string mudid, CharacterArgs args);
        Task<CharacterResult> RemoveCharacterAsync(string userid, string characterid);
        Task<CharacterRaceResult> CreateRaceAsync(string userid, string mudid, CharacterRaceArgs args);
        Task<CharacterRaceResult> UpdateRaceAsync(string userid, string classid, CharacterRaceArgs args);
        Task<CharacterRaceResult> RemoveRaceAsync(string userid, string raceid);
        Task<CharacterClassResult> CreateClassAsync(string userid, string mudid, CharacterClassArgs args);
        Task<CharacterClassResult> UpdateClassAsync(string userid, string raceid, CharacterClassArgs args);
        Task<CharacterClassResult> RemoveClassAsync(string userid, string classid);
    }
}
