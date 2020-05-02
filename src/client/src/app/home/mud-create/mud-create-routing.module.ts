import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MudCreateComponent } from './mud-create.component';
import { GeneralComponent } from './general/general.component';

const routes: Routes = [
	{
		path: '',
		component: MudCreateComponent,
		children: [
			{
				path: '',
				component: GeneralComponent,
			},
		],
	},
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class MudCreateRoutingModule {}
