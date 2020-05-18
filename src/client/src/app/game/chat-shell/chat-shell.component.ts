import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ChatService } from 'src/app/services/chat.service';

@Component({
	selector: 'mh-chat-shell',
	templateUrl: './chat-shell.component.html',
	styleUrls: ['./chat-shell.component.scss'],
})
export class ChatShellComponent implements OnInit {
	constructor(public chat: ChatService) {}

	isOpened = false;

	activeChat = 'global';

	@Output() close = new EventEmitter();

	ngOnInit(): void {}
}
