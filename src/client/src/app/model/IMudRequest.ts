import { MudJoinState } from './MudDTO';

export interface IMudRequest {
	email: string;
	mudId: string;
	state: MudJoinState;
}
