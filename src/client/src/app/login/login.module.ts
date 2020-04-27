import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { LoginShellComponent } from './login-shell.component';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';
import { LoginComponent } from './login/login.component';


@NgModule({
	declarations: [
		LoginShellComponent,
		LoginComponent,
		RegisterComponent,
		ResetComponent,
	],
	imports: [
		CommonModule,
		LoginRoutingModule
	]
})
export class LoginModule { }
