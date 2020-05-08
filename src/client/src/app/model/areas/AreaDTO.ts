import { IMud } from '../muds/IMud';
import { IBaseResponse } from '../BaseResponse';

export interface IAreaCreateResponse extends IBaseResponse {

}
export interface IAreaCreateRequest {
	name: string;
	description: string;
}
