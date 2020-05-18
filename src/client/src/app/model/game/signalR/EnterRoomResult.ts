import { ISignalRBaseResult } from './SignalRBaseResult';
import { IRoom } from '../../areas/IRoom';

export interface IEnterRoomResult extends ISignalRBaseResult {
	activeRoom: IRoom;
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
