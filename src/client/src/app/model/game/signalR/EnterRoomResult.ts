import { ISignalRBaseResult } from './SignalRBaseResult';

export interface IEnterRoomResult extends ISignalRBaseResult {
	activeRoomId: string;
	activeAreaId: string;
	errorType: NavigationErrorType;
}

export enum NavigationErrorType {
	LockedByInteraction,
	LockedByRessource,
	RoomsAreNotConnected,
	IdenticalRooms,
	NoCharacterFound,
	NoTargetRoomFound
}
