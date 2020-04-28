import { Component, OnInit } from '@angular/core';

@Component({
	selector: 'mh-chat-shell',
	templateUrl: './chat-shell.component.html',
	styleUrls: ['./chat-shell.component.scss'],
})
export class ChatShellComponent implements OnInit {
	constructor() {}

	activeChat = 'global';

	ngOnInit(): void {}
}
