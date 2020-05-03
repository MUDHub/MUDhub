import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

import { LoginRoutingModule } from './login-routing.module';
import { LoginShellComponent } from './login-shell.component';
import { RegisterComponent } from './register/register.component';
import { ResetComponent } from './reset/reset.component';
import { LoginComponent } from './login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ResetFormComponent } from './reset-form/reset-form.component';

@NgModule({
	declarations: [
		LoginShellComponent,
		LoginComponent,
		RegisterComponent,
		ResetComponent,
		ResetFormComponent,
	],
	imports: [
		CommonModule,
		LoginRoutingModule,
		ReactiveFormsModule,
		MatButtonModule,
		MatInputModule,
	],
})
export class LoginModule {}
