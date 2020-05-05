using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    internal class CharacterManager : ICharacterManager
    {
        public CharacterManager()
        {

        }

        public Task<CharacterResult> CreateCharacterAsync(string userid, string mudid, CharacterArgs args)
        {
            return Task.FromResult(new CharacterResult());
        }

        public Task<CharacterClassResult> CreateClassAsync(string userid, string mudid, CharacterClassArgs args)
        {
            return Task.FromResult(new CharacterClassResult());
        }

        public Task<CharacterRaceResult> CreateRaceAsync(string userid, string mudid, CharacterRaceArgs args)
        {
            return Task.FromResult(new CharacterRaceResult());
        }

        public Task<CharacterResult> RemoveCharacterAsync(string userid, string characterid)
        {
            return Task.FromResult(new CharacterResult());
        }

        public Task<CharacterClassResult> RemoveClassAsync(string userid, string classid)
        {
            return Task.FromResult(new CharacterClassResult());
        }

        public Task<CharacterRaceResult> RemoveRaceAsync(string userid, string raceid)
        {
            return Task.FromResult(new CharacterRaceResult());
        }
    }
}
