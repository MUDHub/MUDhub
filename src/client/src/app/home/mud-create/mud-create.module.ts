import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatInputModule } from '@angular/material/input';
import { MatSliderModule } from '@angular/material/slider';
import { MatButtonModule } from '@angular/material/button';

import { MudCreateRoutingModule } from './mud-create-routing.module';
import { MudCreateComponent } from './mud-create.component';
import { GeneralComponent } from './general/general.component';

@NgModule({
	declarations: [MudCreateComponent, GeneralComponent],
	imports: [
		CommonModule,
		MudCreateRoutingModule,
		MatInputModule,
		MatSliderModule,
		MatButtonModule,
	],
})
export class MudCreateModule {}
