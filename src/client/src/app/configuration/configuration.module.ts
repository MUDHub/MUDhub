import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfigurationRoutingModule } from './configuration-routing.module';
import { ConfigurationComponent } from './configuration.component';
import { CreatedGamesComponent } from './created-games/created-games.component';
import { EditMudComponent } from './edit-mud/edit-mud.component';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
	declarations: [
		ConfigurationComponent,
		CreatedGamesComponent,
		EditMudComponent,
	],
	imports: [
		CommonModule,
		ConfigurationRoutingModule,
		MatToolbarModule,
		MatButtonModule,
	],
})
export class ConfigurationModule {}
