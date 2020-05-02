import { Component, OnInit } from '@angular/core';
import { IMud } from 'src/app/model/IMud';
import { MudService } from 'src/app/services/mud.service';

@Component({
	templateUrl: './mud-list.component.html',
	styleUrls: ['./mud-list.component.scss'],
})
export class MudListComponent implements OnInit {
	constructor(private mud: MudService) {}

	muds: IMud[];

	async ngOnInit() {
		this.muds = await this.mud.getAll();
	}
}
