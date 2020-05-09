import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IRoom } from 'src/app/model/areas/IRoom';
import { ActivatedRoute, Router } from '@angular/router';
import { AreaService } from 'src/app/services/area.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IRoomCreateRequest } from 'src/app/model/areas/RoomDTO';
import { ImageService } from 'src/app/services/image.service';

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

	position: { x: number; y: number };

	selectedImage: File;

	form = new FormGroup({
		name: new FormControl('', Validators.required),
		description: new FormControl(''),
		imageKey: new FormControl(''),
		isDefault: new FormControl(false),
	});

	ngOnInit() {
		const params = this.route.snapshot.queryParams;
		this.position = {
			x: parseInt(params.x, 10),
			y: parseInt(params.y, 10),
		};
		this.mudid = this.route.snapshot.params.mudid;
		this.areaid = this.route.snapshot.params.areaid;
	}

	async onCreate() {
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
		this.areaService.createRoom(this.mudid, this.areaid, room);

		this.router.navigate(['../'], { relativeTo: this.route});
	}
}
