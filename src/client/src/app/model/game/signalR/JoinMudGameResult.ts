import { ISignalRBaseResult } from './SignalRBaseResult';

export interface IJoinMudGameResult extends ISignalRBaseResult {
	roomId: string;
	areaId: string;
}
