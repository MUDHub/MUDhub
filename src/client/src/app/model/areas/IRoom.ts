import { IArea } from './IArea';
import { IConnection } from './IConnection';

export interface IRoom {
	roomId?: string;
	name: string;
	description?: string;
	enterMessage?: string;
	x: number;
	y: number;
	isDefaultRoom: boolean;
	area?: IArea;
	connections: {
		north: boolean;
		south: boolean;
		east: boolean;
		west: boolean;
		portals: IConnection[];
	};
}
