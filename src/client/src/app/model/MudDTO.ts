import { IMud } from './IMud';

export interface IMudCreateRequest {
	name: string;
	description: string;
	public: boolean;
	autoRestart: boolean;
}

export interface IMudCreateResponse {
	mud: IMud;
}
