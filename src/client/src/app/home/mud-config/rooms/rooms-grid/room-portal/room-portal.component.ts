import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IArea } from 'src/app/model/areas/IArea';
import { IRoom } from 'src/app/model/areas/IRoom';
import { AreaService } from 'src/app/services/area.service';
import { IConnection } from 'src/app/model/areas/IConnection';
import { IConnectionCreateRequest, LockType } from 'src/app/model/areas/ConnectionsDTO';

@Component({
	templateUrl: './room-portal.component.html',
	styleUrls: ['./room-portal.component.scss'],
})
export class RoomPortalComponent implements OnInit {
	constructor(private route: ActivatedRoute, private areaService: AreaService) {}

	mudid: string;
	areaid: string;
	roomid: string;

	areas: IArea[];
	private _selectedArea: IArea;
	get selectedArea() { return this._selectedArea; }
	set selectedArea(value: IArea) {
		this._selectedArea = value;
		this.areaService.getRooms(this.mudid, value.areaId).then(rooms => this.rooms = rooms);
	}

	rooms: IRoom[];
	selectedRoom: IRoom;


	async ngOnInit() {
		this.mudid = this.route.snapshot.params.mudid;
		this.areaid = this.route.snapshot.params.areaid;
		this.roomid = this.route.snapshot.params.roomid;

		this.areas = await this.areaService.getAreasForMUD(this.mudid);
	}


	async changeSelectedArea(area: IArea) {
		this.selectedArea = area;
		if (this.selectedArea) {
			this.rooms = await this.areaService.getRooms(this.mudid, this.selectedArea.areaId);
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

		await this.areaService.createConnection(this.mudid, this.areaid, args);
	}
}
