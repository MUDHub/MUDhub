﻿using MUDhub.Core.Models.Rooms;

namespace MUDhub.Core.Abstracts.Models.Rooms
{
    public class NavigationResult : BaseResult
    {
        public NavigationErrorType ErrorType { get; set; }
        public Room? ActiveRoom { get; set; }
    }
}
