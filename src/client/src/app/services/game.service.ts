import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { IMessage } from '../model/chat/IMessage';

import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

import { environment as env } from 'src/environments/environment';

@Injectable({
	providedIn: 'root',
})
export class GameService {
	constructor() {
		this.connection = new HubConnectionBuilder()
			.configureLogging(LogLevel.Debug)
			.withUrl(env.signalr.url)
			.withAutomaticReconnect()
			.build();


		// this.connection.start().then(() => console.log('SignalR connected'));
	}

	connection: HubConnection;

	private NewGlobalMessageSubject = new Subject<IMessage>();
	public NewGlobalMessage$ = this.NewGlobalMessageSubject.asObservable();
}
