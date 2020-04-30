import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CreateMudRoutingModule } from './create-mud-routing.module';
import { CreateMudComponent } from './create-mud.component';
import { GeneralComponent } from './general/general.component';
import { RoomsComponent } from './rooms/rooms.component';
import { ItemsComponent } from './items/items.component';
import { RaceComponent } from './race/race.component';
import { ClassComponent } from './class/class.component';
import { FinishComponent } from './finish/finish.component';
import { AreaListComponent } from './rooms/area-list/area-list.component';
import { RoomMatrixComponent } from './rooms/room-matrix/room-matrix.component';


@NgModule({
  declarations: [CreateMudComponent, GeneralComponent, RoomsComponent, ItemsComponent, RaceComponent, ClassComponent, FinishComponent, AreaListComponent, RoomMatrixComponent],
  imports: [
	CommonModule,
	CreateMudRoutingModule
  ]
})
export class CreateMudModule { }
