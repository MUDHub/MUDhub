import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IArea } from 'src/app/model/areas/IArea';
import { IRoom } from 'src/app/model/areas/IRoom';
import { AreaService } from 'src/app/services/area.service';
import {
	IConnectionCreateRequest,
	LockType,
} from 'src/app/model/areas/ConnectionsDTO';

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

		const portals = (
			await this.areaService.getRoom(this.mudid, this.areaid, this.roomid)
		).connections.portals;

		portals.map<IPortal>(p => {
			return {
				room1: p.room1Id,
				room2: p.room2Id
			};
		});

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

		console.log(args);

		await this.areaService.createConnection(this.mudid, this.areaid, args);
	}
}

export interface IPortal {
	room1: string;
	room2: string;
}
