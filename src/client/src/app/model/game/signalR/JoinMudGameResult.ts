import { ISignalRBaseResult } from './SignalRBaseResult';
import { IRoom } from '../../areas/IRoom';

export interface IJoinMudGameResult extends ISignalRBaseResult {
	room: IRoom;
	areaId: string;
}
