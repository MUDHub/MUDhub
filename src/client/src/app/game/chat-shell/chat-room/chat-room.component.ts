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

	onChatMessage(message: string) {
		// TODO: add chat service logic
		this.messages.push({
			content: message
		});
	}
}
