import { Component } from '@angular/core';
import { IMessage } from '../chat-message/chat-message.component';

@Component({
	selector: 'mh-chat-global',
	templateUrl: './chat-global.component.html',
	styleUrls: ['./chat-global.component.scss'],
})
export class ChatGlobalComponent {
	constructor() {}

	messages: IMessage[] = [
		{
			sender: 'Mario Lang',
			content:
				'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed' +
				'diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua.',
		},
		{
			sender: 'Paul Finkbeiner',
			content: 'Lorem ipsum dolor sit amet.',
		},
	];

	onChatMessage(message: string) {
		console.log('chat message:', message);
	}
}
