using MUDhub.Core.Models.Characters;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Server.Helpers
{
    public class SignalRConnectionHandler
    {
        private readonly ConcurrentDictionary<string, string> _converter;

        public SignalRConnectionHandler()
        {
            _converter = new ConcurrentDictionary<string, string>();
        }

        public string? GetConnectionId(string characterId)
        {
            var result = _converter.TryGetValue(characterId, out string? connId);
            return result ? connId! : null;
        }

        public void AddConnectionId(string characterId, string connectionId) 
            => _converter.AddOrUpdate(characterId, connectionId, (_, n) => n);

        public void RemoveConnectionId(string characterId)
            => _converter.Remove(characterId, out var _);
    }
}
