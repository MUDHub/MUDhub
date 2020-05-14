import { Component } from '@angular/core';
import { IMessage } from 'src/app/model/chat/IMessage';
import { GameService } from 'src/app/services/game.service';
import { ChatService } from 'src/app/services/chat.service';

@Component({
	selector: 'mh-chat-global',
	templateUrl: './chat-global.component.html',
	styleUrls: ['./chat-global.component.scss'],
})
export class ChatGlobalComponent {
	constructor(private chat: ChatService) {}

	messages: IMessage[] = this.chat.globalHistory;

	async onChatMessage(message: string) {
		try {
			await this.chat.sendGlobalMessage(message);
		} catch (err) {
			console.error('Error while sending global message', err);
		}
	}
}
