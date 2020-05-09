import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, Validators } from '@angular/forms';
import { IMudItem } from 'src/app/model/muds/MudSetupDTO';
import { MudService } from 'src/app/services/mud.service';

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
		private router: Router,
	) {}

	form = this.fb.group({
		name: ['', Validators.required],
		description: ['', Validators.required],
	});
	mudId: string;
	dialog = false;
	items: Array<IMudItem> = [];

	deleteRow(index: number) {
		this.mudService.deleteMudItem(this.mudId, this.items[index]);
		this.items.splice(index, 1);
	}

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;
	}

	changeDialog() {
		this.form.reset();
		this.dialog = !this.dialog;
	}

	async addItem() {

		// Push races Object to the array
		this.items.push({
			name: this.form.get('name').value,
			description: this.form.get('description').value
		});

		this.mudService.addMudItem(
			this.mudId,
			this.items[this.items.length - 1]
		);

		this.changeDialog();
	}
}
