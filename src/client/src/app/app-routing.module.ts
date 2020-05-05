import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { NotFoundComponent } from './not-found.component';

const routes: Routes = [
	{
		path: '',
		canActivate: [AuthGuard],
		loadChildren: () =>
			import('./home/home.module').then(m => m.HomeModule),
	},
	{
		path: 'login',
		loadChildren: () =>
			import('./login/login.module').then(m => m.LoginModule),
	},
	{
		path: 'game',
		canActivate: [AuthGuard],
		loadChildren: () =>
			import('./game/game.module').then(m => m.GameModule),
	},
	{
		path: '**',
		component: NotFoundComponent,
	},
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule],
})
export class AppRoutingModule { }
