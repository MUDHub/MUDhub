import { Component, OnInit, ElementRef, AfterViewChecked } from '@angular/core';
import { MessageType } from 'src/app/model/game/MessageType';
import { GameService } from 'src/app/services/game.service';
import { CommandService } from 'src/app/services/command.service';

@Component({
  selector: 'mh-game-chat',
  templateUrl: './game-chat.component.html',
  styleUrls: ['./game-chat.component.scss']
})
export class GameChatComponent implements OnInit, AfterViewChecked {
	constructor(private game: GameService, private command: CommandService, private elRef: ElementRef) { }

	MessageType = MessageType;

	chat: { message: string, type: MessageType }[] = [
		{
			message: 'Gebe "zeige Befehle" für eine Übersicht aller Befehle ein',
			type: MessageType.Server
		}
	];


	ngOnInit(): void {
		this.game.NewGameMessage$.subscribe(message => {
			this.chat.push({
				message,
				type: MessageType.Server
			});
			this.scrollToBottom();
		});

		this.game.Error$.subscribe(error => {
			this.chat.push({
				message: error,
				type: MessageType.Error
			});
			this.scrollToBottom();
		});

		this.command.GameInput$.subscribe(input => {
			this.chat.push({
				message: input,
				type: MessageType.Client
			});
		});

		this.scrollToBottom();
	}

	ngAfterViewChecked() {
		this.scrollToBottom();
	}


	private scrollToBottom() {
		try {
			this.elRef.nativeElement.scrollTop = this.elRef.nativeElement.scrollHeight;
		} catch { }
	}
}
