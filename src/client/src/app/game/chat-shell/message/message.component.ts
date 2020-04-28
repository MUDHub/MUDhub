import { Component, OnInit, Input } from '@angular/core';

@Component({
	selector: 'mh-message',
	templateUrl: './message.component.html',
	styleUrls: ['./message.component.scss']
})
export class MessageComponent {

	@Input()
	message: IMessage;

}


export interface IMessage {
	sender: string;
	content: string;
}
