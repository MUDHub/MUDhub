import { Injectable } from '@angular/core';
import { IMessage } from '../model/chat/IMessage';
import { Subject } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class ChatService {

	private NewGlobalMessageSubject = new Subject<IMessage>();
	public NewGlobalMessage$ = this.NewGlobalMessageSubject.asObservable();

	private NewRoomMessageSubject = new Subject<IMessage>();
	public NewRoomMessage$ = this.NewRoomMessageSubject.asObservable();


	private _globalHistory: IMessage[] = [
		{
			content: 'Hello global world!',
			sender: 'Mario Lang',
		},
		{
			content: 'Hello global world!',
			sender: 'Mario Lang',
		},
		{
			content: 'Hello global world!',
			sender: 'Mario Lang',
		},
	];

	private _roomHistory: IMessage[] = [
		{
			content: 'Hello room world!',
			sender: 'Mario Lang',
		},
	];

	public get globalHistory() {
		return this._globalHistory;
	}
	public get roomHistory() {
		return this._roomHistory;
	}
}
