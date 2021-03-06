import { Component, OnInit, HostBinding } from '@angular/core';
import { AreaService } from 'src/app/services/area.service';
import { GameService } from 'src/app/services/game.service';
import { IArea } from 'src/app/model/areas/IArea';
import { IRoom } from 'src/app/model/areas/IRoom';
import { environment as env } from 'src/environments/environment';

@Component({
	selector: 'mh-map',
	templateUrl: './map.component.html',
	styleUrls: ['./map.component.scss'],
})
export class MapComponent implements OnInit {
	constructor(private roomsService: AreaService, private game: GameService) {}

	resourceBase = env.resources.base;

	isLoading = false;

	rooms: IRoom[] = [];

	map: IRoom[][] = [[]];
	activeArea: IArea;
	activeRoom: IRoom;

	get width() {
		return this.map[0]?.length;
	}
	get height() {
		return this.map.length;
	}

	ngOnInit() {
		this.game.ChangeRoom$.subscribe(async newRoom => {
			try {
				if (newRoom.areaId !== this.activeArea?.areaId) {
					this.isLoading = true;
					this.activeArea = await this.roomsService.getArea(newRoom.areaId);
					this.map = [[]];
				}
				this.rooms = await this.roomsService.getRoomsForArea(this.activeArea.areaId);
				this.activeRoom = this.rooms.find(r => r.roomId === newRoom.room.roomId);
				this.renderMap(this.rooms);
			} catch (err) {
				console.error('Error while updating map', err);
			} finally {
				this.isLoading = false;
			}
		});
	}

	renderMap(list: IRoom[]) {
		for (const room of list) {
			while (this.height - 1 < room.y) {
				this.map.push(Array(this.width).fill(undefined));
			}

			while (this.width - 1 < room.x) {
				this.map.forEach(row => {
					row.push(undefined);
				});
			}

			this.map[room.y][room.x] = room;
		}
	}
}
