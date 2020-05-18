import { ISignalRBaseResult } from './SignalRBaseResult';
import { Direction } from '../Direction';

export interface IRoomConnectionsResult {
	roomName: string;
	description: string;
	direction: Direction;
}
