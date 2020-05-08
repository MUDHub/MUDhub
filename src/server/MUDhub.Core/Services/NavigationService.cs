using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Rooms;
using MUDhub.Core.Configurations;
using MUDhub.Core.Models.Connections;

namespace MUDhub.Core.Services
{
    internal class NavigationService : INavigationService
    {
        private readonly MudDbContext _dbContext;
        private readonly ILogger? _logger;


        internal NavigationService(MudDbContext context, ServerConfiguration options, ILogger<LoginService>? logger = null)
        {
            _dbContext = context;
            _logger = logger;
        }

        public Task<bool> JoinWorldAsync(string mudId, string characterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LeaveWorldAsync(string characterId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The character can change the room.
        /// </summary>
        /// <param name="characterId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<NavigationResult> TryEnterRoomAsync(string characterId, string roomId)
        {
            var character = await _dbContext.Characters.FirstOrDefaultAsync(c => c.Id == characterId)
                .ConfigureAwait(false);

            if (character is null)
            {
                _logger?.LogWarning($"No character was found with the id: {characterId}");
                return new NavigationResult()
                {
                    Success = false,
                    ErrorType = NavigationErrorType.NoCharacterFound,
                    Errormessage = $"No character was found with the id: {characterId}"
                };
            }

            var targetRoom = await _dbContext.Rooms.FirstOrDefaultAsync(r => r.Id == roomId)
                .ConfigureAwait(false);
            if (targetRoom is null)
            {
                _logger?.LogWarning($"No target room was found with the id: {roomId}");
                return new NavigationResult()
                {
                    Success = false,
                    ActiveRoom = character.ActualRoom,
                    ErrorType = NavigationErrorType.NoTargetRoomFound,
                    Errormessage = $"No target room was found with the id: {roomId}"
                };
            }

            if (character.ActualRoom.Id == roomId)
            {
                _logger?.LogWarning($"No changing to the same room possible. RoomId: {roomId}");
                return new NavigationResult()
                {
                    Success = false,
                    ActiveRoom = character.ActualRoom,
                    ErrorType = NavigationErrorType.IdenticalRooms,
                    Errormessage = $"No changing to the same room possible. RoomId: {roomId}"
                };
            }

            var connection = await _dbContext.RoomConnections.FirstOrDefaultAsync(
                rc => (rc.Room1 == targetRoom && rc.Room2 == character.ActualRoom) ||
                (rc.Room1 == character.ActualRoom && rc.Room2 == targetRoom))
                .ConfigureAwait(false);

            if (connection.LockType != LockType.NoLock)
            {
                //TODO: Prüfe Bedingungen für andere LockTypes
            }

            _logger?.LogInformation($"The character changed from room: {roomId} to room: {targetRoom.Id}");
            return new NavigationResult()
            {
                ActiveRoom = targetRoom
            };
        }
    }
}
