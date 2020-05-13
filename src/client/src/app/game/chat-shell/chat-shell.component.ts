import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
	selector: 'mh-chat-shell',
	templateUrl: './chat-shell.component.html',
	styleUrls: ['./chat-shell.component.scss'],
})
export class ChatShellComponent implements OnInit {
	constructor() {}

	isOpened = false;

	activeChat = 'global';

	@Output() close = new EventEmitter();

	ngOnInit(): void {}
}
