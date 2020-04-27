import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { LoginShellComponent } from './login-shell.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';

const routes: Routes = [
	{
		path: '',
		component: LoginShellComponent,
		children: [
			{
				path: '',
				component: LoginComponent
			},
			{
				path: 'register',
				component: RegisterComponent
			},
			{
				path: 'reset',
				component: ResetComponent
			},
		]
	}
];


@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule]
})
export class LoginRoutingModule { }
