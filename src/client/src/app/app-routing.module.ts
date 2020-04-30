import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

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
		canActivate: [AuthGuard],
		loadChildren: () =>
			import('./configuration/configuration.module').then(
				(m) => m.ConfigurationModule
			),
	},
	{
		path: 'game',
		canActivate: [AuthGuard],
		loadChildren: () =>
			import('./game/game.module').then((m) => m.GameModule),
	},
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule {}
