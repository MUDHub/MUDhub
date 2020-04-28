import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GameShellComponent } from './game-shell.component';
import { GameRoutingModule } from './game-routing.module';
import { GameComponent } from './game/game.component';
import { ChatShellComponent } from './chat-shell/chat-shell.component';
import { ChatGlobalComponent } from './chat-shell/chat-global/chat-global.component';
import { ChatRoomComponent } from './chat-shell/chat-room/chat-room.component';
import { ChatMessageComponent } from './chat-shell/chat-message/chat-message.component';
import { ChatInputComponent } from './chat-shell/chat-input/chat-input.component';

@NgModule({
	declarations: [
		GameShellComponent,
		ChatShellComponent,
		ChatGlobalComponent,
		ChatRoomComponent,
		GameComponent,
		ChatShellComponent,
		ChatGlobalComponent,
		ChatRoomComponent,
		ChatMessageComponent,
		ChatInputComponent,
	],
	imports: [CommonModule, GameRoutingModule],
})
export class GameModule {}
