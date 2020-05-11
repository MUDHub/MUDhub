import { IBaseResponse } from '../BaseResponse';
import { IConnection } from './IConnection';

export interface IConnectionCreateRequest {
	roomId1: string;
	roomId2: string;
	description?: string;
	lockType: LockType;
	lockDescription?: string;
	lockAssociatedId?: string;
}


export interface IConnectionCreateResponse extends IBaseResponse {
	connection: IConnection;
}

export enum LockType {
	NoLock = 0,
	ItemLock = 1,
	InteractionLock = 2,
	CharacterRaceLock = 3,
	CharacterClassLock = 4,
}
