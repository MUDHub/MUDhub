import { Component, OnInit, Input, HostBinding } from '@angular/core';
import { IMessage } from 'src/app/model/chat/IMessage';

@Component({
	selector: 'mh-chat-message',
	templateUrl: './chat-message.component.html',
	styleUrls: ['./chat-message.component.scss']
})
export class ChatMessageComponent {

	@Input()
	message: IMessage;

	@HostBinding('class.self')
	get self() { return this.message.sender === undefined; }

	@HostBinding('class.server-message')
	get serverMessage() { return this.message.isServerMessage; }

}



