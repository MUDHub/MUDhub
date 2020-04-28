import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
	selector: 'mh-chat-input',
	templateUrl: './chat-input.component.html',
	styleUrls: ['./chat-input.component.scss'],
})
export class ChatInputComponent {

	@Output()
	chatMessage = new EventEmitter<string>();

	sendMessage(el: HTMLInputElement) {
		this.chatMessage.emit(el.value);
		el.value = '';
	}

}
