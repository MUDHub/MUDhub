import { IUser } from './IUser';

export interface ILoginResponse {
	token: string;
	user: IUser;
}

export interface IRegisterResponse { }

export interface IPasswordResetResponse { }

export interface IRegistrationRequest {
	firstName: string;
	lastName: string;
	email: string;
	password: string;
}
