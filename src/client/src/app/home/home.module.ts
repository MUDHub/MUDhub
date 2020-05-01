import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeShellComponent } from './home-shell/home-shell.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';

import { MudListComponent } from './mud-list/mud-list.component';
import { MyGamesComponent } from './my-games/my-games.component';
import { MyMudsComponent } from './my-muds/my-muds.component';
import { ProfileComponent } from './profile/profile.component';

@NgModule({
	declarations: [HomeShellComponent, MudListComponent, MyGamesComponent, MyMudsComponent, ProfileComponent],
	imports: [
		CommonModule,
		HomeRoutingModule,
		MatToolbarModule,
		MatButtonModule,
		MatMenuModule,
	],
})
export class HomeModule {}
