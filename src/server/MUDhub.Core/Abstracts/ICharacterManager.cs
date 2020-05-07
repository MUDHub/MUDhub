using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MUDhub.Core.Abstracts.Models;
using MUDhub.Core.Models.Characters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MUDhub.Core.Abstracts.Models.Characters;


namespace MUDhub.Core.Abstracts
{
    public interface ICharacterManager
    {
        Task<CharacterResult> CreateCharacterAsync(string userid, string mudid, CharacterArgs args);
        Task<CharacterResult> RemoveCharacterAsync(string userid, string characterid);
        Task<CharacterRaceResult> CreateRaceAsync(string userid, string mudid, CharacterRaceArgs args);
        Task<CharacterRaceResult> RemoveRaceAsync(string userid, string raceid);
        Task<CharacterClassResult> CreateClassAsync(string userid, string mudid, CharacterClassArgs args);
        Task<CharacterClassResult> RemoveClassAsync(string userid, string classid);
    }
}
