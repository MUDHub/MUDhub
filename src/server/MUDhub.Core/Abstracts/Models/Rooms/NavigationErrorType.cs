namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public enum NavigationErrorType
    {
        LockedByInteraction,
        LockedByRessource,
        RoomsAreNotConnected,
        IdenticalRooms,
        NoCharacterFound,
        NoTargetRoomFound,
        NoPortalFound
    }
}
