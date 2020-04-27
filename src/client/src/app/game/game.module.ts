import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GameShellComponent } from './game-shell.component';
import { GameRoutingModule } from './game-routing.module';


@NgModule({
	declarations: [GameShellComponent],
	imports: [
		CommonModule,
		GameRoutingModule
	]
})
export class GameModule { }
