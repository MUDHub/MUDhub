using Microsoft.Extensions.Logging;
using MUDhub.Core.Abstracts;
using MUDhub.Core.Abstracts.Models.Inventories;
using MUDhub.Core.Models.Inventories;
using MUDhub.Core.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MUDhub.Core.Services
{
    internal class ItemManager : IItemManager
    {
        private readonly MudDbContext _context;
        private readonly ILogger? _logger;

        public ItemManager(MudDbContext context, ILogger<AreaManager>? logger = null)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// A new item is created in the MudGame.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="mudId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<ItemResult> CreateItemAsync(string userId, string mudId, ItemArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein User mit der UserId: '{userId}' gefunden."
                };
            }

            var mud = await _context.MudGames.FindAsync(mudId)
                .ConfigureAwait(false);
            if (mud is null)
            {
                var message = $"No MudGame with the mudId: '{mudId}' was found.";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein MudGame mit der MudGameId: '{mudId}' gefunden."
                };
            }

            if (!IsUserOwner(user, mudId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{mud.Name}'";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der User: '{user.Lastname}' hat keine Berechtigung für das MudGame: '{mud.Name}'"
                };
            }

            var item = new Item()
            {
                Description = args.Description,
                ImageKey = args.ImageKey,
                Name = args.Name,
                Weight = args.Weight,
                MudGame = mud
            };
            _context.Items.Add(item);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"A item: '{item.Id}' was created in MudGame: '{mud.Name}' from '{user.Email}'");
            return new ItemResult()
            {
                Item = item
            };
        }

        /// <summary>
        /// An item is updated.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task<ItemResult> UpdateItemAsync(string userId, string itemId, ItemArgs args)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein User mit der UserId: '{userId}' gefunden."
                };
            }

            var item = await _context.Items.FindAsync(itemId)
                .ConfigureAwait(false);
            if (item is null)
            {
                var message = $"No Item with the itemId: '{itemId}' was found.";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Item mit der ItemId: '{itemId}' gefunden."
                };
            }

            if (!IsUserOwner(user, item.MudGameId))
            {
                var message = $"The user: '{user.Lastname}' is not the owner of the MudGame: '{item.MudGame.Name}'";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der User: '{user.Lastname}' hat keine Berechtigung für das MudGame: '{item.MudGame.Name}'"
                };
            }

            if (args.ImageKey != null)
            {
                item.ImageKey = args.ImageKey;
            }
            if (args.Name != null)
            {
                item.Name = args.Name;
            }
            if (args.Description != null)
            {
                item.Description = args.Description;
            }
            item.Weight = args.Weight;

            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The item: '{item.Name}' was updated from '{user.Email}': {Environment.NewLine}" +
                $"- Name: {args.Name ?? "<no modification>"} {Environment.NewLine}" +
                $"- Description: {args.Description ?? "<no modification>"} {Environment.NewLine}" +
                $"- ImageKey: {args.ImageKey ?? "<no modification>"}");
            return new ItemResult()
            {
                Item = item
            };
        }

        /// <summary>
        /// An item is deleted.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<ItemResult> DeleteItemAsync(string userId, string itemId)
        {
            var user = await GetUserById(userId).ConfigureAwait(false);
            if (user is null)
            {
                var message = $"No user with the UserId: '{userId}' was found.";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein User mit der UserId: '{userId}' gefunden."
                };
            }
            var item = await _context.Items.FindAsync(itemId)
                .ConfigureAwait(false);
            if (item is null)
            {
                var message = $"No Item with the itemId: '{itemId}' was found.";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Kein Item mit der ItemId: '{itemId}' gefunden."
                };
            }

            if (!IsUserOwner(user, item.MudGameId))
            {
                var message = $"The user: {user.Lastname} is not the owner of the MudGame: {item.MudGameId}";
                _logger?.LogWarning(message);
                return new ItemResult()
                {
                    Success = false,
                    Errormessage = message,
                    DisplayMessage = $"Der User: '{user.Lastname}' hat keine Berechtigung für das MudGame: '{item.MudGame.Name}'"
                };
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync()
                .ConfigureAwait(false);
            _logger?.LogInformation($"The item: '{item.Id}' has been removed from the MudGame: '{item.MudGameId}' from '{user.Email}'");
            return new ItemResult()
            {
                Item = item
            };
        }

        /// <summary>
        /// Is the user really the owner of the MudGame?
        /// </summary>
        /// <param name="user"></param>
        /// <param name="gameId"></param>
        /// <returns></returns>
        private static bool IsUserOwner(User user, string gameId)
        {
            var mudGameOwner = user.MudGames.FirstOrDefault(mg => mg.Id == gameId);
            return !(mudGameOwner is null);
        }

        /// <summary>
        /// The UserId is used to determine the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<User> GetUserById(string userId)
        {
            return await _context.Users.FindAsync(userId)
                .ConfigureAwait(false);
        }
    }
}
