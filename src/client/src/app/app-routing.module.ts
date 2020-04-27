import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';


const routes: Routes = [
	{
		path: '',
		redirectTo: 'login',
		pathMatch: 'full'
	},
	{
		path: 'login',
		loadChildren: () => import('./login/login.module').then(m => m.LoginModule)
	},
	{
		path: 'config',
		loadChildren: () => import('./configuration/configuration.module').then(m => m.ConfigurationModule)
	},
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule { }
