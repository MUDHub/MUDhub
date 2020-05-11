import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IRoom } from 'src/app/model/areas/IRoom';
import { ActivatedRoute, Router } from '@angular/router';
import { AreaService } from 'src/app/services/area.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IRoomCreateRequest } from 'src/app/model/areas/RoomDTO';
import { ImageService } from 'src/app/services/image.service';
import { IArea } from 'src/app/model/areas/IArea';
import { MatSelectChange } from '@angular/material/select';

@Component({
	templateUrl: './room-create.component.html',
	styleUrls: ['./room-create.component.scss'],
})
export class RoomCreateComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private areaService: AreaService,
		private imageService: ImageService
	) {}

	mudid: string;
	areaid: string;
	roomid?: string;

	get isInEditMode() {
		return this.roomid !== undefined;
	}

	position: { x: number; y: number };

	selectedImage: File;

	form = new FormGroup({
		name: new FormControl('', Validators.required),
		description: new FormControl(''),
		imageKey: new FormControl(''),
		isDefault: new FormControl(false),
	});

	async ngOnInit() {
		const params = this.route.snapshot.queryParams;
		this.position = {
			x: parseInt(params.x, 10),
			y: parseInt(params.y, 10),
		};
		this.mudid = this.route.snapshot.params.mudid;
		this.areaid = this.route.snapshot.params.areaid;
		this.roomid = this.route.snapshot.params.roomid;


		if (this.roomid) {
			const room = await this.areaService.getRoom(
				this.mudid,
				this.areaid,
				this.roomid
			);
			this.form.get('name').setValue(room.name);
			this.form.get('description').setValue(room.description);
			this.form.get('isDefault').setValue(room.isDefaultRoom);
		}
	}

	async onSubmit() {
		let imageKey;
		if (this.selectedImage) {
			imageKey = this.imageService.uploadFile(this.selectedImage);
		}
		const room: IRoomCreateRequest = {
			name: this.form.get('name').value,
			description: this.form.get('description').value,
			isDefaultRoom: this.form.get('isDefault').value,
			imageKey,
			x: this.position.x,
			y: this.position.y,
		};

		if (!this.isInEditMode) {
			this.areaService.createRoom(this.mudid, this.areaid, room);
		} else {
			this.areaService.updateRoom(
				this.mudid,
				this.areaid,
				this.roomid,
				room
			);
		}

		this.router.navigate([this.isInEditMode ? '../../' : '../'], {
			relativeTo: this.route,
		});
	}
}
