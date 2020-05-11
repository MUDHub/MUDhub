import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IArea } from 'src/app/model/areas/IArea';
import { IRoom } from 'src/app/model/areas/IRoom';
import { AreaService } from 'src/app/services/area.service';
import {
	IConnectionCreateRequest,
	LockType,
} from 'src/app/model/areas/ConnectionsDTO';
import Swal from 'sweetalert2';

@Component({
	templateUrl: './room-portal.component.html',
	styleUrls: ['./room-portal.component.scss'],
})
export class RoomPortalComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private areaService: AreaService
	) {}

	mudid: string;
	areaid: string;
	roomid: string;

	portals: IPortal[];

	areas: IArea[];
	private _selectedArea: IArea;
	get selectedArea() {
		return this._selectedArea;
	}
	set selectedArea(value: IArea) {
		this._selectedArea = value;
		this.areaService
			.getRoomsForArea(value.areaId)
			.then(rooms => (this.rooms = rooms));
	}

	rooms: IRoom[];
	selectedRoom: IRoom;

	async ngOnInit() {
		this.mudid = this.route.snapshot.params.mudid;
		this.areaid = this.route.snapshot.params.areaid;
		this.roomid = this.route.snapshot.params.roomid;

		const room = await this.areaService.getRoom(this.roomid);

		const portals: IPortal[] = [];
		for (let portal of room.connections.portals) {
			portals.push({
				room1: await this.areaService.getRoom(portal.room1Id),
				room2: await this.areaService.getRoom(portal.room2Id),
				connectionId: portal.id,
			});
		}

		this.portals = portals;

		this.areas = await this.areaService.getAreasForMud(this.mudid);
	}

	async changeSelectedArea(area: IArea) {
		this.selectedArea = area;
		if (this.selectedArea) {
			this.rooms = await this.areaService.getRoomsForArea(
				this.selectedArea.areaId
			);
		} else {
			this.rooms = undefined;
		}
	}

	async create(area: IArea, room: IRoom) {
		console.log('area', area);
		console.log('room', room);

		const args: IConnectionCreateRequest = {
			roomId1: this.roomid,
			roomId2: room.roomId,
			lockType: LockType.NoLock,
		};

		try {
			const { connection } = await this.areaService.createConnection(
				args
			);

			this.portals.push({
				room1: await this.areaService.getRoom(connection.room1Id),
				room2: await this.areaService.getRoom(connection.room2Id),
				connectionId: connection.id,
			});
		} catch (err) {}
	}

	async deletePortal(portal: IPortal) {
		try {
			await this.areaService.deleteConnection(portal.connectionId);
			this.portals.splice(this.portals.indexOf(portal), 1);
		} catch (err) {
			console.error('Error while deleting portal', err);
			Swal.fire({
				icon: 'error',
				title: 'Fehler',
				text: err.error?.displayMessage || err.error?.errormessage || 'Fehler beim Löschen des Portal'
			});
		}
	}
}

export interface IPortal {
	room1: IRoom;
	room2: IRoom;
	connectionId: string;
}
