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
import { IEnterRoomResult, NavigationErrorType } from '../model/game/signalR/EnterRoomResult';
import { ItemTransferMethod } from '../model/game/signalR/ItemTransferMethod';
import { ICommand } from './command.service';
import { IInventoryResult } from '../model/game/signalR/InventoryResult';
import { IRoom } from '../model/areas/IRoom';
import { IRoomConnectionsResult } from '../model/game/signalR/RoomConnectionsResult';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';

@Injectable({
	providedIn: 'root',
})
export class GameService {
	constructor(private auth: AuthService, private router: Router) {
		this.connection = new HubConnectionBuilder()
			.configureLogging(env.signalr.logLevel)
			.withUrl(env.signalr.url, {
				accessTokenFactory: () => this.auth.token,
			})
			.withAutomaticReconnect([0, 1000, 2000, 3000, 5000, 10000, null])
			.build();

		this.connection.onreconnecting(err => {
			console.warn('Lost connection to server, reconnection...');
			this.ErrorSubject.next('Die Verbindung zum Server wurde unterbrochen, versuche neu zu verbinden...');
		});

		this.connection.onreconnected(() => {
			this.NewGameMessageSubject.next('Verbindung wiederhergestellt!');
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

		this.connection.on('kickFromServer', async () => {
			await this.exitGame();
			await Swal.fire({
				icon: 'warning',
				title: 'MUD wurde beendet',
				timer: 5000,
				timerProgressBar: true,
				allowOutsideClick: false,
				confirmButtonText: 'OK',
				text: 'Das MUD wurde vom MUD-Master gestopped, du wirst auf die Startseite umgeleitet'
			});
			this.router.navigate(['/']);
		});
	}

	connection: HubConnection;

	private OnExitSubject = new Subject();
	public OnExit$ = this.OnExitSubject.asObservable();

	private NewGameMessageSubject = new Subject<string>();
	public NewGameMessage$ = this.NewGameMessageSubject.asObservable();

	private ErrorSubject = new Subject<string>();
	public Error$ = this.ErrorSubject.asObservable();

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

	private ChangeRoomSubject = new Subject<{ room: IRoom; areaId: string }>();
	public ChangeRoom$ = this.ChangeRoomSubject.asObservable();

	public async joinGame(characterid: string) {
		if (this.connection.state === HubConnectionState.Disconnected) {
			await this.connection.start();

			const joinResult = await this.connection.invoke<IJoinMudGameResult>('tryJoinMudGame', characterid);
			if (joinResult.success) {
				this.ChangeRoomSubject.next({ room: joinResult.room, areaId: joinResult.areaId });
				this.NewGameMessageSubject.next(joinResult.room.enterMessage);
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

	public async tryEnterRoom(direction: Direction, portalName?: string) {
		const result = await this.connection.invoke<IEnterRoomResult>('tryEnterRoom', direction, portalName);
		if (!result.success) {
			switch (result.errorType) {
				case NavigationErrorType.NoPortalFound:
					this.NewGameMessageSubject.next(`Kein Portal '${portalName}' gefunden`);
					break;
				case NavigationErrorType.RoomsAreNotConnected:
					this.NewGameMessageSubject.next(`Kann nicht nach ${this.getDirection(direction)} gehen`);
					break;
				case NavigationErrorType.NoTargetRoomFound:
					this.NewGameMessageSubject.next('Raum nicht gefunden');
					break;
				case NavigationErrorType.LockedByInteraction:
				case NavigationErrorType.LockedByRessource:
					this.NewGameMessageSubject.next('Der Raum ist verschlossen...');
					break;
			}
		} else {
			this.ChangeRoomSubject.next({
				areaId: result.activeAreaId,
				room: result.activeRoom,
			});
			this.NewGameMessageSubject.next(result.activeRoom.enterMessage);
		}
	}

	getDirection(dir: Direction) {
		switch (dir) {
			case Direction.NORTH:
				return 'Norden';
			case Direction.SOUTH:
				return 'Süden';
			case Direction.EAST:
				return 'Osten';
			case Direction.WEST:
				return 'Westen';
			case Direction.PORTAL:
				return 'Portal';
		}
	}

	public async transferItem(itemName: string, method: ItemTransferMethod) {
		const result = await this.connection.invoke<ISignalRBaseResult>('tryTransferItem', itemName, method);
		this.NewGameMessageSubject.next(result.displayMessage);
	}

	public async showPlayerInventory() {
		const result = await this.connection.invoke<IInventoryResult>('getInventory', false);
		if (result.success) {
			if (result.items.length > 0) {
				let text = 'Im Inventar befinden sich:<br>';
				text += result.items.map(item => item.itemName).join(', ');
				this.NewGameMessageSubject.next(text);
			} else {
				this.NewGameMessageSubject.next('Du hast keine Gegenstände im Inventar...');
			}
		} else {
			console.error('Error while fetching players inventory', result);
		}
	}

	public async showCommands(commands: ICommand[]) {
		let text = 'Folgende Befehle können ausgeführt werden:<br>';
		for (const command of commands) {
			text += ' - ' + command.keyword + (command.shorthand ? ' (' + command.shorthand + ')' : '');
			text += ' ' + command.arguments.map(a => '&lt;' + a.name + '&gt;').join(' ');
			text += '<br>';
			text += '<span style="margin-left: 1rem">' + command.description + '</span>';
			text += '<br>';
		}

		this.NewGameMessageSubject.next(text);
	}

	public async showRoomInventory() {
		const result = await this.connection.invoke<IInventoryResult>('getInventory', true);
		if (result.success) {
			if (result.items.length > 0) {
				let text = 'Im Raum befinden sich:<br>';
				text += result.items.map(item => item.itemName).join(', ');
				this.NewGameMessageSubject.next(text);
			} else {
				this.NewGameMessageSubject.next('Im Raum befinden sich keine Gegenstände...');
			}
		} else {
			console.warn(result);
		}
	}

	public async showExits() {
		try {
			const rooms = await this.connection.invoke<IRoomConnectionsResult[]>('getRoomConnections');
			let text = 'An diesen Raum grenzen folgende Räume an:<br>';
			for (const room of rooms) {
				text += `Im ${this.getDirection(room.direction)}: ${room.roomName}, ${room.description}<br>`;
			}
			this.NewGameMessageSubject.next(text);
		} catch (err) {
			console.error('Error while fetching exits', err);
		}
	}
}
