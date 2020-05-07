import { IMud } from './IMud';

export interface IMudCreateRequest {
	name: string;
	description: string;
	isPublic: boolean;
	autoRestart: boolean;
}

export interface IMudCreateResponse {
	mud: IMud;
}

export interface IMudDeleteResponse {}

export interface IRequestResponse {
	mudGameId: string;
	mudGameName: string;
	userEmail: string;
	userid: string;
	state: MudJoinState;
}

export enum MudJoinState { Requested, Accepted, Rejected }
