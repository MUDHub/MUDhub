import { Component, OnInit, Input } from '@angular/core';

@Component({
	selector: 'mh-chat-message',
	templateUrl: './chat-message.component.html',
	styleUrls: ['./chat-message.component.scss']
})
export class ChatMessageComponent {

	@Input()
	message: IMessage;

}


export interface IMessage {
	sender: string;
	content: string;
}
