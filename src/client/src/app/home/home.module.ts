import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeShellComponent } from './home-shell/home-shell.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatCardModule } from '@angular/material/card';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBarModule } from '@angular/material/snack-bar';

import { MudListComponent } from './mud-list/mud-list.component';
import { MyGamesComponent } from './my-games/my-games.component';
import { MyMudsComponent } from './my-muds/my-muds.component';
import { ProfileComponent } from './profile/profile.component';
import { UsersComponent } from './admin/users/users.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MudJoinComponent } from './mud-list/mud-join/mud-join.component';
import { SharedModule } from '../shared/shared.module';
@NgModule({
	declarations: [
		HomeShellComponent,
		MudListComponent,
		MyGamesComponent,
		MyMudsComponent,
		ProfileComponent,
		UsersComponent,
		MudJoinComponent,
	],
	imports: [
		CommonModule,
		HomeRoutingModule,
		ReactiveFormsModule,
		MatToolbarModule,
		MatButtonModule,
		MatMenuModule,
		MatCardModule,
		MatTableModule,
		MatPaginatorModule,
		MatFormFieldModule,
		MatInputModule,
		MatProgressSpinnerModule,
		SharedModule,
		MatCheckboxModule,
		MatIconModule,
		MatSnackBarModule,
	],
})
export class HomeModule {}
