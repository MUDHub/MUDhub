import { IBaseResponse } from './BaseResponse';

export interface IImageUploadResponse extends IBaseResponse {
	imageUrl: string;
}
