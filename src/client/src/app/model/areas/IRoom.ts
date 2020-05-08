import { IArea } from './IArea';

export interface IRoom {
	roomId?: string;
	name: string;
	description?: string;
	enterMessage?: string;
	x: number;
	y: number;
	isDefaultRoom: boolean;
	area?: IArea;
}
