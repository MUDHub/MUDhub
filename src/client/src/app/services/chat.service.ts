import { Injectable } from '@angular/core';
import { IMessage } from '../model/chat/IMessage';
import { Subject } from 'rxjs';
import { GameService } from './game.service';

@Injectable({
	providedIn: 'root',
})
export class ChatService {
	constructor(private game: GameService) {
		game.NewGlobalMessage$.subscribe(m => {
			const message: IMessage = {
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

		game.OnExit$.subscribe(() => {
			this._roomHistory = [];
			this._globalHistory = [];
		});
	}

	private NewGlobalMessageSubject = new Subject<IMessage>();
	public NewGlobalMessage$ = this.NewGlobalMessageSubject.asObservable();

	private NewRoomMessageSubject = new Subject<IMessage>();
	public NewRoomMessage$ = this.NewRoomMessageSubject.asObservable();

	private _globalHistory: IMessage[] = [];

	private _roomHistory: IMessage[] = [];

	public get globalHistory() {
		return this._globalHistory;
	}
	public get roomHistory() {
		return this._roomHistory;
	}

	/**
	 * Sends a global message via signalR and if succeeded adds it to the global message history
	 * @param message The text to send
	 * @throws Error when signalR invocation fails
	 */
	public async sendGlobalMessage(message: string) {
		try {
			await this.game.sendGlobalMessage(message);
			this._globalHistory.push({
				content: message,
				isServerMessage: false,
			});
		} catch (err) {
			throw err;
		}
	}

	/**
	 * Sends a room message via signalR and if succeeded adds it to the room message history
	 * @param message The text to send
	 * @throws Error when signalR invocation fails
	 */
	public async sendRoomMessage(message: string) {
		try {
			await this.game.sendRoomMessage(message);
			this._roomHistory.push({
				content: message,
			});
		} catch (err) {
			throw err;
		}
	}
}
