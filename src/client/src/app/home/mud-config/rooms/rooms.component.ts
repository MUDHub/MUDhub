import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IRoom } from 'src/app/model/areas/IRoom';
import { IArea } from 'src/app/model/areas/IArea';
import { AreaService } from 'src/app/services/area.service';
import { IRoomDeleteResponse } from 'src/app/model/areas/RoomDTO';

@Component({
	selector: 'mh-rooms',
	templateUrl: './rooms.component.html',
	styleUrls: ['./rooms.component.scss'],
})
export class RoomsComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private areaService: AreaService
	) {}

	mudId: string;

	rooms: IRoom[][] = [[]];
	areas: IArea[] = [];
	selectedArea: IArea;

	async ngOnInit() {
		this.mudId = this.route.snapshot.params.mudid;
		this.areas = await this.areaService.getAreasForMUD(this.mudId);
		if (this.areas.length > 0) {
			this.selectArea(this.areas[0]);
		}
	}

	onAbort() {
		this.router.navigate(['/my-muds']);
	}

	onLast() {
		this.router.navigate(['/my-muds/' + this.mudId + '/items']);
	}

	async onNext() {
		/* Object erstellen */
		/* Request zur API schicken */

		// Redirect zur nächsten Konfigurationsseite
		this.router.navigate(['/my-muds/' + this.mudId + '/finish']);
	}

	createRoom(position: { x: number; y: number }) {
		console.log(position);
		// TODO: show popup/dialog to get infos for room and then create it via API call
	}

	async selectArea(area: IArea) {
		this.selectedArea = area;

		try {
			const roomsList = await this.areaService.getRooms(
				this.mudId,
				area.areaId
			);
			this.rooms = this.mapRooms(roomsList);
		} catch (err) {
			console.error(
				`Error while fetching rooms for area(${area.areaId})`,
				err
			);
		}
	}

	async addArea() {
		// TODO: show popup/dialog to get infos for area and then create it via API call
		try {
			const response = await this.areaService.createArea(this.mudId, {
				name: `Etage ${Math.floor(Math.random() * 100)}`,
				description: 'Lorem ipsum dolor sit amet',
			});
			this.areas.push(response.area);
		} catch (err) {
			console.error('Error while creating area', err);
		}
	}

	async deleteArea(area: IArea) {
		try {
			await this.areaService.deleteArea(this.mudId, area.areaId);
			this.areas.splice(this.areas.indexOf(area), 1);
		} catch (err) {
			console.error('Error while deleting area', err);
		}
	}

	async deleteRoom(room: IRoom) {
		try {
			await this.areaService.deleteRoom(
				this.mudId,
				room.area.areaId,
				room.roomId
			);
		} catch (err) {
			console.error('Error while deleting room', err);
			const error = err.error as IRoomDeleteResponse;

			const roomsList = await this.areaService.getRooms(
				this.mudId,
				this.selectedArea.areaId
			);
			this.rooms = this.mapRooms(roomsList);

			if (error.isDefaultRoom) {
				alert(
					'Raum kann nicht gelöscht werden, er ist ein Einstiegsraum'
				);
			}
		}
	}

	jump(componentName: string) {
		switch (componentName) {
			case 'races':
				this.router.navigate(['/my-muds/' + this.mudId + '/races']);
				break;
			case 'classes':
				this.router.navigate(['/my-muds/' + this.mudId + '/classes']);
				break;
			case 'items':
				this.router.navigate(['/my-muds/' + this.mudId + '/items']);
				break;
			case 'rooms':
				this.router.navigate(['/my-muds/' + this.mudId + '/rooms']);
				break;
			case 'finish':
				this.router.navigate(['/my-muds/' + this.mudId + '/finish']);
				break;
			default:
				break;
		}
	}

	private mapRooms(roomList: IRoom[]): IRoom[][] {
		const matrix: IRoom[][] = [[]];
		for (const room of roomList) {
			while (matrix.length <= room.y) {
				matrix.push(new Array(matrix[matrix.length - 1].length));
			}

			for (const row of matrix) {
				while (row.length <= room.x) {
					row.push(undefined);
				}
			}

			matrix[room.y][room.x] = room;
		}

		return matrix;
	}
}
