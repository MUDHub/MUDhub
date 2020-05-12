import { IMessage } from './IMessage';

export interface IGlobalMessage extends IMessage {
	isServerMessage: boolean;
}
