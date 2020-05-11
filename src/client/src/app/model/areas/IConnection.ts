import { LockType } from './ConnectionsDTO';

export interface IConnection {
	id: string;
	room1Id: string;
	room2Id: string;
	lockType: LockType;
	lockDescription: string;
	lockAssociatedId: string;
}
