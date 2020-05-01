import { Component, OnInit } from '@angular/core';
import { IRoom } from 'src/app/model/IRoom';
import { RoomsService } from 'src/app/services/rooms.service';

@Component({
	selector: 'mh-map',
	templateUrl: './map.component.html',
	styleUrls: ['./map.component.scss'],
})
export class MapComponent implements OnInit {
	constructor(private roomsService: RoomsService) {}

	activeArea = 'Morgenland';

	rooms: IRoom[] = [];

	map: IRoom[][] = [[]];


	get width() {
		return this.map[0]?.length;
	}
	get height() {
		return this.map.length;
	}


	ngOnInit(): void {
		this.updateMap();
	}

	updateMap() {
		// TODO: replace with correct implementation
		this.rooms = this.roomsService.getRoomsForArea('');
		this.renderMap(this.rooms);
	}


	renderMap(list: IRoom[]) {
		for (const room of list) {
			while (this.height - 1 < room.position.y) {
				this.map.push(Array(this.width).fill(undefined));
			}

			while (this.width - 1 < room.position.x) {
				this.map.forEach((row) => {
					row.push(undefined);
				});
			}

			this.map[room.position.y][room.position.x] = room;
		}
	}
}
