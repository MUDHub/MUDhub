import { IBaseResponse } from '../BaseResponse';

export interface IRoomDeleteResponse extends IBaseResponse {
	isDefaultRoom: boolean;
}
