import { MudJoinState } from './MudDTO';

export interface IMudRequest {
	userId: string;
	email: string;
	mudId: string;
	state: MudJoinState;
}
