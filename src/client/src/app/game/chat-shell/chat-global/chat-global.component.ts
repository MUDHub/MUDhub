import { Component } from '@angular/core';
import { IMessage } from 'src/app/model/IMessage';
import { ChatService } from 'src/app/services/chat.service';

@Component({
	selector: 'mh-chat-global',
	templateUrl: './chat-global.component.html',
	styleUrls: ['./chat-global.component.scss'],
})
export class ChatGlobalComponent {
	constructor(private chat: ChatService) {}

	messages: IMessage[] = this.chat.globalHistory;

	onChatMessage(message: string) {
		// TODO: add chat service logic
		this.messages.push({
			content: message,
		});
	}
}
