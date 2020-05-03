import { IUser } from './IUser';

export interface ILoginResponse {
	token: string;
	user: IUser;
}

export interface IRegisterResponse {
	user: IUser;
	errormessage: string;
	succeeded: boolean;
}

export interface IPasswordResetResponse { }

export interface IRegistrationRequest {
	firstname: string;
	lastname: string;
	email: string;
	password: string;
}
