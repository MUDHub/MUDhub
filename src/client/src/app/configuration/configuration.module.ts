import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ConfigurationRoutingModule } from './configuration-routing.module';
import { ConfigurationComponent } from './configuration.component';
import { CreatedGamesComponent } from './created-games/created-games.component';
import { EditMudComponent } from './edit-mud/edit-mud.component';


@NgModule({

	declarations: [ConfigurationComponent, CreatedGamesComponent, EditMudComponent],
	imports: [
		CommonModule,
		ConfigurationRoutingModule
	]
})
export class ConfigurationModule { }
