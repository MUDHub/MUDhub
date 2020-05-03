import { IUser } from './IUser';

export interface IMud {
	mudId: string;
	name: string;
	description: string;
	isPublic: string;
	autoRestart: boolean;
	owner: IUser;
}
