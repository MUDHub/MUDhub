import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatSnackBarModule } from '@angular/material/snack-bar';

import { GameShellComponent } from './game-shell.component';
import { GameRoutingModule } from './game-routing.module';
import { GameComponent } from './game/game.component';
import { ChatShellComponent } from './chat-shell/chat-shell.component';
import { ChatGlobalComponent } from './chat-shell/chat-global/chat-global.component';
import { ChatRoomComponent } from './chat-shell/chat-room/chat-room.component';
import { ChatMessageComponent } from './chat-shell/chat-message/chat-message.component';
import { ChatInputComponent } from './chat-shell/chat-input/chat-input.component';
import { MapComponent } from './game/map/map.component';
import { GameInputComponent } from './game/game-input/game-input.component';
import { GameChatComponent } from './game/game-chat/game-chat.component';
import { CharInfoComponent } from './game/char-info/char-info.component';

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
		MapComponent,
		GameInputComponent,
		GameChatComponent,
		CharInfoComponent,
	],
	imports: [CommonModule, GameRoutingModule, MatSnackBarModule],
})
export class GameModule {}
