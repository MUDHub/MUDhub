import { Component, OnInit } from '@angular/core';
import { IMessage } from '../message/message.component';

@Component({
	selector: 'mh-chat-global',
	templateUrl: './chat-global.component.html',
	styleUrls: ['./chat-global.component.scss'],
})
export class ChatGlobalComponent implements OnInit {
	constructor() {}

	messages: IMessage[] = [
		{
			sender: 'Mario Lang',
			content: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.',
		},
		{
			sender: 'Paul Finkbeiner',
			content: 'Lorem ipsum dolor sit amet.',
		},
	];

	ngOnInit(): void {}
}
