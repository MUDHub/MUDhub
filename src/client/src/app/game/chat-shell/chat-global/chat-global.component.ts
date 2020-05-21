import { Component, ViewChild, ElementRef, OnInit, AfterViewChecked } from '@angular/core';
import { IMessage } from 'src/app/model/chat/IMessage';
import { ChatService } from 'src/app/services/chat.service';

@Component({
	selector: 'mh-chat-global',
	templateUrl: './chat-global.component.html',
	styleUrls: ['./chat-global.component.scss'],
})
export class ChatGlobalComponent implements OnInit, AfterViewChecked {
	constructor(private chat: ChatService) {}

	@ViewChild('scrollMe') private scrollContainer: ElementRef;

	messages: IMessage[] = this.chat.globalHistory;

	ngOnInit() {
		this.scrollToBottom();
	}
	ngAfterViewChecked() {
		this.scrollToBottom();
	}

	async onChatMessage(message: string) {
		try {
			await this.chat.sendGlobalMessage(message);
			this.scrollToBottom();
		} catch (err) {
			console.error('Error while sending global message', err);
		}
	}

	private scrollToBottom() {
		try {
			this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
		} catch { }
	}
}
