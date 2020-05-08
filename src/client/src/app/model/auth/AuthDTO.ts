import { IUser } from './IUser';
import { IBaseResponse } from '../BaseResponse';

export interface ILoginResponse {
	token: string;
	user: IUser;
}

export interface IRegisterResponse extends IBaseResponse {
	user: IUser;
}

export interface IPasswordResetRequest {
	passwordResetKey: string;
	newPasword: string;
}

export interface IPasswordResetResponse {}

export interface IRegistrationRequest {
	firstname: string;
	lastname: string;
	email: string;
	password: string;
}
