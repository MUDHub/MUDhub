import { Component, ViewChild, ElementRef, OnInit, AfterViewChecked } from '@angular/core';
import { IMessage } from 'src/app/model/chat/IMessage';
import { ChatService } from 'src/app/services/chat.service';

@Component({
	selector: 'mh-chat-room',
	templateUrl: './chat-room.component.html',
	styleUrls: ['./chat-room.component.scss'],
})
export class ChatRoomComponent implements OnInit, AfterViewChecked {
	constructor(private chat: ChatService) {}

	@ViewChild('scrollMe') private scrollContainer: ElementRef;

	messages: IMessage[] = this.chat.roomHistory;

	ngOnInit() {
		this.scrollToBottom();
	}
	ngAfterViewChecked() {
		this.scrollToBottom();
	}

	async onChatMessage(message: string) {
		try {
			await this.chat.sendRoomMessage(message);
			this.scrollToBottom();
		} catch (err) {
			console.error('Error while sending room message', err);
		}
	}

	private scrollToBottom() {
		try {
			this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
		} catch {}
	}
}
