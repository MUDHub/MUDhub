import { Injectable } from '@angular/core';
import { Subject, BehaviorSubject } from 'rxjs';
import { IMessage } from '../model/chat/IMessage';

import { HubConnection, HubConnectionBuilder, LogLevel, HubConnectionState } from '@microsoft/signalr';

import { environment as env } from 'src/environments/environment';
import { AuthService } from './auth.service';
import { ISignalRBaseResult } from '../model/game/signalR/SignalRBaseResult';
import { ChatService } from './chat.service';
import { IJoinMudGameResult } from '../model/game/signalR/JoinMudGameResult';
import { Direction } from '../model/game/Direction';

@Injectable({
	providedIn: 'root',
})
export class GameService {
	constructor(private auth: AuthService) {
		this.connection = new HubConnectionBuilder()
			.configureLogging(env.signalr.logLevel)
			.withUrl(env.signalr.url, {
				accessTokenFactory: () => this.auth.token,
			})
			.withAutomaticReconnect()
			.build();

		this.connection.onreconnecting(err => {
			console.log('reconnection...');
			if (err) {
				console.error('Error while reconnection', err);
			}
		});

		this.connection.on('receiveGameMessage', (message: string) => {
			this.NewGameMessageSubject.next(message);
		});

		this.connection.on('receiveGlobalMessage', (message: string, caller: string, serverMessage: boolean) => {
			this.NewGlobalMessageSubject.next({
				message,
				caller,
				serverMessage,
			});
		});

		this.connection.on('receiveRoomMessage', (message: string, caller: string) => {
			this.NewRoomMessageSubject.next({ message, caller });
		});

		this.connection.on('receivePrivateMessage', (message: string, caller: string) => {
			this.NewPrivateMessageSubject.next({ message, caller });
		});
	}

	connection: HubConnection;

	private OnExitSubject = new Subject();
	public OnExit$ = this.OnExitSubject.asObservable();

	private NewGameMessageSubject = new Subject<string>();
	public NewGameMessage$ = this.NewGameMessageSubject.asObservable();

	private NewGlobalMessageSubject = new Subject<{
		message: string;
		caller: string;
		serverMessage: boolean;
	}>();
	public NewGlobalMessage$ = this.NewGlobalMessageSubject.asObservable();

	private NewRoomMessageSubject = new Subject<{
		message: string;
		caller: string;
	}>();
	public NewRoomMessage$ = this.NewRoomMessageSubject.asObservable();

	private NewPrivateMessageSubject = new Subject<{
		message: string;
		caller: string;
	}>();
	public NewPrivateMessage$ = this.NewPrivateMessageSubject.asObservable();

	private ChangeRoomSubject = new Subject<{ roomId: string; areaId: string }>();
	public ChangeRoom$ = this.ChangeRoomSubject.asObservable();

	public async joinGame(characterid: string) {
		if (this.connection.state === HubConnectionState.Disconnected) {
			await this.connection.start();

			const joinResult = await this.connection.invoke<IJoinMudGameResult>('tryJoinMudGame', characterid);
			if (joinResult.success) {
				this.ChangeRoomSubject.next({ roomId: joinResult.roomId, areaId: joinResult.areaId });
			} else {
				throw new Error(`Could not join: ${joinResult.errorMessage}`);
			}
		} else {
			console.warn(
				'SignalR connection state is not disconnected, trying to connect again will result in an error'
			);
		}
	}

	public async exitGame() {
		this.OnExitSubject.next();
		await this.connection.stop();
	}

	public async sendGlobalMessage(message: string) {
		await this.connection.invoke('SendGlobalMessage', message);
	}

	public async sendRoomMessage(message: string) {
		await this.connection.invoke('SendRoomMessage', message);
	}

	public async sendPrivateMessage(message: string, characterName: string) {
		const response = await this.connection.invoke<ISignalRBaseResult>('SendPrivateMessage', message, characterName);
		if (!response.success) {
			throw new Error(response.errorMessage);
		}
	}

	public async tryEnterRoom(direction: Direction) {

	}
}
