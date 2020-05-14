import { Component } from '@angular/core';
import { IMessage } from 'src/app/model/chat/IMessage';
import { ChatService } from 'src/app/services/chat.service';

@Component({
	selector: 'mh-chat-room',
	templateUrl: './chat-room.component.html',
	styleUrls: ['./chat-room.component.scss'],
})
export class ChatRoomComponent {
	constructor(private chat: ChatService) {}

	messages: IMessage[] = this.chat.roomHistory;

	async onChatMessage(message: string) {

		try {
			await this.chat.sendRoomMessage(message);
		} catch (err) {
			console.error('Error while sending room message', err);
		}
	}
}
