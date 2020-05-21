import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AreaService } from 'src/app/services/area.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IRoomCreateRequest } from 'src/app/model/areas/RoomDTO';
import { ImageService } from 'src/app/services/image.service';
import { IImageUploadResponse } from 'src/app/model/FileUploadDTO';

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
		enterMessage: new FormControl(''),
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
			const room = await this.areaService.getRoom(this.roomid);
			this.form.get('name').setValue(room.name);
			this.form.get('description').setValue(room.description);
			this.form.get('enterMessage').setValue(room.enterMessage);
			this.form.get('isDefault').setValue(room.isDefaultRoom);
			this.form.get('imageKey').setValue(room.imageKey);
		}
	}

	fileSelectionChanged(files: FileList) {
		this.selectedImage = files.item(0);
	}


	async onSubmit() {
		if (this.selectedImage) {
			try {
				const response = await this.imageService.uploadFile(this.selectedImage);
				this.form.get('imageKey').setValue(response.imageUrl);
			} catch (err) {
				console.error('Error while uploading image', err);
			}
		}
		const room: IRoomCreateRequest = {
			name: this.form.get('name').value,
			description: this.form.get('description').value,
			isDefaultRoom: this.form.get('isDefault').value,
			enterMessage: this.form.get('enterMessage').value,
			imageKey: this.form.get('imageKey').value,
			areaId: this.areaid,
			x: this.position.x,
			y: this.position.y,
		};

		if (!this.isInEditMode) {
			this.areaService.createRoom(room);
		} else {
			this.areaService.updateRoom(this.roomid, room);
		}

		this.router.navigate([this.isInEditMode ? '../../' : '../'], {
			relativeTo: this.route,
		});
	}
}
