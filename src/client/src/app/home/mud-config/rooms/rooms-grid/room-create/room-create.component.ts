import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IRoom } from 'src/app/model/areas/IRoom';

@Component({
	templateUrl: './room-create.component.html',
	styleUrls: ['./room-create.component.scss'],
})
export class RoomCreateComponent implements OnInit {
	constructor() {}

	@Input() selectedRoom: IRoom;

	ngOnInit(): void {}


	onCreate() {

	}
}
