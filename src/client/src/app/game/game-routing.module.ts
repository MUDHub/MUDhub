import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GameShellComponent } from './game-shell.component';

const routes: Routes = [
	{
		path: ':characterid',
		component: GameShellComponent,
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class GameRoutingModule {}
