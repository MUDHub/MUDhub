import { Component, OnInit } from '@angular/core';
import { MessageType } from 'src/app/model/game/MessageType';
import { GameService } from 'src/app/services/game.service';

@Component({
  selector: 'mh-game-chat',
  templateUrl: './game-chat.component.html',
  styleUrls: ['./game-chat.component.scss']
})
export class GameChatComponent implements OnInit {

	MessageType = MessageType;

	chat: { message: string, type: MessageType }[] = [
		{
			message: 'Gebe "zeige Befehle" für eine Übersicht aller Befehle ein',
			type: MessageType.Server
		}
	];

	constructor(private game: GameService) { }

	ngOnInit(): void {
		this.game.NewGameMessage$.subscribe(message => {
			this.chat.push({
				message,
				type: MessageType.Server
			});
		});

		this.game.Error$.subscribe(error => {
			this.chat.push({
				message: error,
				type: MessageType.Error
			});
		});
	}
}
