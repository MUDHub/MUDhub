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
