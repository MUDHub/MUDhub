import { Component, OnInit } from '@angular/core';
import { IMessage } from '../chat-message/chat-message.component';

@Component({
	selector: 'mh-chat-room',
	templateUrl: './chat-room.component.html',
	styleUrls: ['./chat-room.component.scss'],
})
export class ChatRoomComponent implements OnInit {
	constructor() {}

	messages: IMessage[] = [];

	ngOnInit(): void {}

	onChatMessage(message: string) {
		console.log('room message:', message);
	}
}
