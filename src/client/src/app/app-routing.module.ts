import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
	{
		path: '',
		redirectTo: 'login',
		pathMatch: 'full',
	},
	{
		path: 'login',
		loadChildren: () =>
			import('./login/login.module').then((m) => m.LoginModule),
	},
	{
		path: 'config',
		loadChildren: () =>
			import('./configuration/configuration.module').then(
				(m) => m.ConfigurationModule
			),
	},
	{
		path: 'game',
		loadChildren: () =>
			import('./game/game.module').then((m) => m.GameModule),
	},
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
