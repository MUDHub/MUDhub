import { IBaseResponse } from '../BaseResponse';
import { IRoom } from './IRoom';

export interface IRoomDeleteResponse extends IBaseResponse {
	isDefaultRoom: boolean;
}

export interface IRoomCreateRequest {
	name: string;
	description: string;
	imageKey: string;
	x: number;
	y: number;
	areaId: string;
	isDefaultRoom: boolean;
}

export interface IRoomUpdateRequest {
	name: string;
	description: string;
	imageKey: string;
	x: number;
	y: number;
	isDefaultRoom: boolean;
}

export interface IRoomCreateResponse extends IBaseResponse {
	room: IRoom;
}

export interface IRoomCreateItemRequest {
	itemId: string;
}
