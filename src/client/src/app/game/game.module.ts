import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GameShellComponent } from './game-shell.component';
import { GameRoutingModule } from './game-routing.module';
import { ChatComponent } from './chat/chat.component';
import { GameComponent } from './game/game.component';

@NgModule({
	declarations: [GameShellComponent, ChatComponent, GameComponent],
	imports: [CommonModule, GameRoutingModule],
})
export class GameModule {}
