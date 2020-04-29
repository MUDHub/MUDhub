import { Component, OnInit } from '@angular/core';
import { MessageType } from 'src/app/model/MessageType';

@Component({
  selector: 'mh-game-chat',
  templateUrl: './game-chat.component.html',
  styleUrls: ['./game-chat.component.scss']
})
export class GameChatComponent implements OnInit {

	MessageType = MessageType;

	chat: { message: string, type: MessageType }[] = [
		{
			message: 'Gebe "hilfe" für eine Übersicht aller Befehle ein',
			type: MessageType.Server
		}
	];

	constructor() { }

	ngOnInit(): void {

	}
}
