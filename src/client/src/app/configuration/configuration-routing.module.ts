import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ConfigurationComponent } from './configuration.component';
import { CreatedGamesComponent } from './created-games/created-games.component';
import { EditMudComponent } from './edit-mud/edit-mud.component';

const routes: Routes = [
	{
		path: '',
		component: ConfigurationComponent,
		children: [
			{
				path: '/config/created-games',
				component: CreatedGamesComponent
			},
			{
				path: '/config/edit-mud',
				component: EditMudComponent
			}
		]
	}
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class ConfigurationRoutingModule { }
