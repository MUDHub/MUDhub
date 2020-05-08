import { IMud } from '../muds/IMud';
import { IBaseResponse } from '../BaseResponse';
import { IArea } from './IArea';

export interface IAreaCreateResponse extends IBaseResponse {
	area: IArea;
}
export interface IAreaCreateRequest {
	name: string;
	description: string;
}
