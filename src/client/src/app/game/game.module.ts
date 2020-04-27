import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GameShellComponent } from './game-shell.component';
import { GameRoutingModule } from './game-routing.module';
import { GameComponent } from './game/game.component';
import { ChatShellComponent } from './chat-shell/chat-shell.component';
import { ChatPublicComponent } from './chat-shell/chat-public/chat-public.component';
import { ChatRoomComponent } from './chat-shell/chat-room/chat-room.component';

@NgModule({
	declarations: [
		GameShellComponent,
		ChatShellComponent,
		ChatPublicComponent,
		ChatRoomComponent,
		GameComponent,
		ChatShellComponent,
		ChatPublicComponent,
		ChatRoomComponent,
	],
	imports: [CommonModule, GameRoutingModule],
})
export class GameModule {}
