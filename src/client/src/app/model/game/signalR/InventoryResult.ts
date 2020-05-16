import { ISignalRBaseResult } from './SignalRBaseResult';
import { IItemInstance } from '../../areas/IRoom';

export interface IInventoryResult extends ISignalRBaseResult {
	items: IItemInstance[];
}
