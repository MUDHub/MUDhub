import { Component, OnInit } from '@angular/core';
import { IMapRoom } from 'src/app/model/game/IRoom';
import { AreaService } from 'src/app/services/area.service';

@Component({
	selector: 'mh-map',
	templateUrl: './map.component.html',
	styleUrls: ['./map.component.scss'],
})
export class MapComponent implements OnInit {
	constructor(private roomsService: AreaService) {}

	activeArea = 'Morgenland';

	rooms: IMapRoom[] = [];

	map: IMapRoom[][] = [[]];


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
		this.renderMap(this.rooms);
	}


	renderMap(list: IMapRoom[]) {
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
