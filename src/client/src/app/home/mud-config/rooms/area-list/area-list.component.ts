import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IArea } from 'src/app/model/areas/IArea';
import { AreaService } from 'src/app/services/area.service';
import { ActivatedRoute } from '@angular/router';

@Component({
	selector: 'mh-area-list',
	templateUrl: './area-list.component.html',
	styleUrls: ['./area-list.component.scss'],
})
export class AreaListComponent implements OnInit {
	constructor(private route: ActivatedRoute, private areaService: AreaService) {}

	mudid: string;
	areas: IArea[];

	isFormActive = false;

	async ngOnInit() {
		this.mudid = this.route.snapshot.params.mudid;
		this.areas = await this.areaService.getAreasForMUD(this.mudid);
	}


	async onAdd(input: HTMLInputElement) {
		const name = input.value;

	}
}
