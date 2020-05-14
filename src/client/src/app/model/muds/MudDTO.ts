import { IMud } from './IMud';
import { IMudItem, IMudClass, IMudRace } from './MudSetupDTO';

export interface IMudCreateRequest {
	name: string;
	description: string;
	isPublic: boolean;
	autoRestart: boolean;
	imagekey?: string;
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

export enum MudJoinState {
	Requested,
	Accepted,
	Rejected,
}

export interface IMudRaceResponse {
	race: IMudRace;
	displayMessage: string;
	errormessage: string;
	succeeded: boolean;
}

export interface IMudClassResponse {
	class: IMudClass;
	displayMessage: string;
	errormessage: string;
	succeeded: boolean;
}

export interface IMudItemResponse {
	item: IMudItem;
	displayMessage: string;
	errormessage: string;
	succeeded: boolean;
}

export interface IMudRaceRequest {
	name: string;
	description: string;
	imageKey: string;
	mudId: string;
}

export interface IMudClassRequest {
	name: string;
	description: string;
	imageKey: string;
	mudId: string;
}

export interface IMudItemRequest {
	name: string;
	description: string;
	weight: number;
	imageKey: string;
	mudId: string;
}
