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
				path: '',
				redirectTo: 'my-muds',
				pathMatch: 'full',
			},
			{
				path: 'my-muds',
				component: CreatedGamesComponent,
			},
			{
				path: 'edit',
				component: EditMudComponent,
			},
			{
				path: 'create',
				loadChildren: () =>
					import('./create-mud/create-mud.module').then(
						(m) => m.CreateMudModule
					),
			},
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class ConfigurationRoutingModule {}
