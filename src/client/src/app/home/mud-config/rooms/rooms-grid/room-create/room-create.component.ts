import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IRoom } from 'src/app/model/areas/IRoom';
import { ActivatedRoute } from '@angular/router';
import { AreaService } from 'src/app/services/area.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IRoomCreateRequest } from 'src/app/model/areas/RoomDTO';
import { ImageService } from 'src/app/services/image.service';

@Component({
	templateUrl: './room-create.component.html',
	styleUrls: ['./room-create.component.scss'],
})
export class RoomCreateComponent implements OnInit {
	constructor(private route: ActivatedRoute, private areaService: AreaService, private imageService: ImageService) {}

	mudid: string;
	areaid: string;

	position: {x: number, y: number};

	selectedImage: File;

	form = new FormGroup({
		name: new FormControl('', Validators.required),
		description: new FormControl(''),
		imageKey: new FormControl(''),
		isDefault: new FormControl(false)
	});


	ngOnInit() {
		const params = this.route.snapshot.queryParams;
		this.position = {
			x: params.x,
			y: params.y
		};
		this.mudid = params.mudid;
		this.areaid = params.areaid;
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
			y: this.position.y
		};
		this.areaService.createRoom()
	}
}
