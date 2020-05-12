import { Injectable } from '@angular/core';
import { IMessage } from '../model/chat/IMessage';
import { Subject } from 'rxjs';
import { GameService } from './game.service';
import { IGlobalMessage } from '../model/chat/IGlobalMessage';

@Injectable({
	providedIn: 'root',
})
export class ChatService {
	constructor(private game: GameService) {
		game.NewGlobalMessage$.subscribe(m => {
			const message: IGlobalMessage = {
				content: m.message,
				sender: m.caller,
				isServerMessage: m.serverMessage,
			};
			this._globalHistory.push(message);
			this.NewGlobalMessageSubject.next(message);
		});

		game.NewRoomMessage$.subscribe(m => {
			const message: IMessage = {
				content: m.message,
				sender: m.caller,
			};
			this._roomHistory.push(message);
			this.NewRoomMessageSubject.next(message);
		});
	}

	private NewGlobalMessageSubject = new Subject<IGlobalMessage>();
	public NewGlobalMessage$ = this.NewGlobalMessageSubject.asObservable();

	private NewRoomMessageSubject = new Subject<IMessage>();
	public NewRoomMessage$ = this.NewRoomMessageSubject.asObservable();

	private _globalHistory: IGlobalMessage[] = [
		{
			content: 'Hello global world!',
			sender: 'Mario Lang',
			isServerMessage: false,
		},
		{
			content: 'Hello global world!',
			sender: 'Mario Lang',
			isServerMessage: false,
		},
		{
			content: 'Hello global world!',
			sender: 'Mario Lang',
			isServerMessage: false,
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
