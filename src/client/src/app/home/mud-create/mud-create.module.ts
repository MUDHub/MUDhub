import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatInputModule } from '@angular/material/input';
import { MatSliderModule } from '@angular/material/slider';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';

import { MudCreateRoutingModule } from './mud-create-routing.module';
import { MudCreateComponent } from './mud-create.component';
import { GeneralComponent } from './general/general.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
	declarations: [MudCreateComponent, GeneralComponent],
	imports: [
		CommonModule,
		ReactiveFormsModule,
		MudCreateRoutingModule,
		MatInputModule,
		MatSliderModule,
		MatButtonModule,
		MatCheckboxModule,
	],
})
export class MudCreateModule {}
