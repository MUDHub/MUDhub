import { Injectable } from '@angular/core';
import { IMessage } from '../model/IMessage';

@Injectable({
	providedIn: 'root',
})
export class ChatService {
	constructor() {}

	private _histories: {
		room: IMessage[],
		global: IMessage[]
	} = {
		global: [
			{
				content: 'Hello global world!',
				sender: 'Mario Lang'
			},
			{
				content: 'Hello global world!',
				sender: 'Mario Lang'
			},
			{
				content: 'Hello global world!',
				sender: 'Mario Lang'
			},
		],
		room: [
			{
				content: 'Hello room world!',
				sender: 'Mario Lang'
			},
		]
	};

	public get globalHistory() {
		return this._histories.global;
	}
	public get roomHistory() {
		return this._histories.room;
	}
}
