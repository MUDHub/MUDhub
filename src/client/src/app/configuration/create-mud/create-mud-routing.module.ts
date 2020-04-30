import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { CreateMudComponent } from './create-mud.component';
import { GeneralComponent } from './general/general.component';
import { AreaListComponent } from './rooms/area-list/area-list.component';
import { RoomMatrixComponent } from './rooms/room-matrix/room-matrix.component';
import { ItemsComponent } from './items/items.component';
import { FinishComponent } from './finish/finish.component';
import { ClassComponent } from './class/class.component';
import { RaceComponent } from './race/race.component';

const routes: Routes = [
	{
		path: '',
		component: CreateMudComponent,
	},
	{
		path: 'general',
		component: GeneralComponent
	},
	{
		path: 'rooms',
		component: GeneralComponent,
	}, /*
	{
		path: 'rooms/area-list',
		component: AreaListComponent
	},
	{
		path: 'rooms/room-matrix',
		component: RoomMatrixComponent
	},*/
	{
		path: 'items',
		component: ItemsComponent
	},
	{
		path: 'race',
		component: RaceComponent
	},
	{
		path: 'class',
		component: ClassComponent
	},
	{
		path: 'finish',
		component: FinishComponent
	},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CreateMudRoutingModule { }
