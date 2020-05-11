import { IMud } from './IMud';

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
	race: {
		description: string;
		raceId: string;
		mudGameId: string;
		imageKey: string;
		name: string;
	};
	displayMessage: string;
	errormessage: string;
	succeeded: boolean;
}

export interface IMudClassResponse {
	class: {
		description: string;
		classId: string;
		mudGameId: string;
		imageKey: string;
		name: string;
	};
	displayMessage: string;
	errormessage: string;
	succeeded: boolean;
}

export interface IMudItemResponse {
	item: {
		itemId: string;
		name: string;
		description: string;
		weight: number;
		imageKey: string;
	};
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
