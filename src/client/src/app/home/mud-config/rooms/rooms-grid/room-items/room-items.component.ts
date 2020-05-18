import { Component, OnInit } from '@angular/core';
import { IMudItem } from 'src/app/model/muds/MudSetupDTO';
import { MudService } from 'src/app/services/mud.service';
import { AreaService } from 'src/app/services/area.service';
import { ActivatedRoute } from '@angular/router';
import { IItemInstance } from 'src/app/model/areas/IRoom';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import Swal from 'sweetalert2';

@Component({
	templateUrl: './room-items.component.html',
	styleUrls: ['./room-items.component.scss'],
})
export class RoomItemsComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private areaService: AreaService,
		private mudService: MudService
	) {}

	mudid: string;
	roomid: string;
	roomItems: IItemInstance[];

	availableItems: IMudItem[];

	itemAdd = new FormGroup({
		item: new FormControl(undefined, Validators.required),
		count: new FormControl(1, [
			Validators.required,
			Validators.min(1),
			Validators.max(100),
		]),
	});

	async ngOnInit() {
		this.mudid = this.route.snapshot.params.mudid;
		const items = await this.mudService.getItemsForMud(this.mudid);
		this.availableItems = items.sort((i1, i2) =>
			i1.name.localeCompare(i2.name)
		);

		this.roomid = this.route.snapshot.params.roomid;
		await this.loadItems();
	}

	async loadItems() {
		this.roomItems = await this.areaService.getItemsForRoom(this.roomid);
	}

	async onSubmit() {
		const item = this.itemAdd.get('item').value as IMudItem;
		const count = this.itemAdd.get('count').value as number;

		try {
			for (let i = 0; i < count; i++) {
				await this.areaService.addItemToRoom(this.roomid, { itemId: item.id });
			}
		} catch (err) {
			console.error('Error while adding item to room', err);
			await Swal.fire({
				icon: 'error',
				title: 'Fehler',
				text:
					err.error?.displayMessage ||
					err.error?.errormessage ||
					'Fehler beim Hinzufügen des Items',
			});
		} finally {
			await this.loadItems();
		}
	}


	async deleteItem(item: IItemInstance) {
		try {
			await this.areaService.removeItemFromRoom(this.roomid, item.itemInstanceId);
			this.roomItems.splice(this.roomItems.indexOf(item), 1);
		} catch (err) {
			console.error('Error while removing item from room', err);
			await Swal.fire({
				icon: 'error',
				title: 'Fehler',
				text:
					err.error?.displayMessage ||
					err.error?.errormessage ||
					'Fehler beim Entfernen des Items',
			});
		}
	}
}
