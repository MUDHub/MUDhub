import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { IMudItem } from 'src/app/model/muds/MudSetupDTO';
import { MudService } from 'src/app/services/mud.service';
import { IMudItemRequest, IMudItemResponse } from 'src/app/model/muds/MudDTO';
import { IImageUploadResponse } from 'src/app/model/FileUploadDTO';

@Component({
	selector: 'mh-items',
	templateUrl: './items.component.html',
	styleUrls: ['./items.component.scss'],
})
export class ItemsComponent implements OnInit {
	constructor(
		private mudService: MudService,
		private fb: FormBuilder,
		private route: ActivatedRoute,
		private router: Router
	) {}

	form = this.fb.group({
		name: ['', Validators.required],
		description: ['', Validators.required],
		weight: ['', Validators.required],
	});
	mudId: string;
	dialog = false;
	items: Array<IMudItem> = [];

	deleteRow(index: number) {
		this.mudService.deleteMudItem(this.mudId, this.items[index]);
		this.items.splice(index, 1);
	}
	async ngOnInit() {
		this.mudId = this.route.snapshot.params.mudid;
		this.items = await this.mudService.getMudItem(this.mudId);
	}

	changeDialog() {
		this.form.reset();
		this.dialog = !this.dialog;
	}

	async addItem() {

		let imageKey: IImageUploadResponse = null;

		const response: IMudItemResponse = await this.mudService.addMudItem(
			this.mudId,
			{
				name: this.form.get('name').value,
				description: this.form.get('description').value,
				weight: this.form.get('weight').value,
				imageKey: imageKey?.imageUrl,
			}
		);

		this.items.push({
			itemId: response.itemId,
			name: response.name,
			description: response.description,
			weight: response.weight,
			imageKey: response.imageKey,
		});

		this.changeDialog();
	}
}
