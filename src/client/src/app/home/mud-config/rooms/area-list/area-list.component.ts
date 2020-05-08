import { Component, OnInit, Input } from '@angular/core';
import { IArea } from 'src/app/model/areas/IArea';

@Component({
	selector: 'mh-area-list',
	templateUrl: './area-list.component.html',
	styleUrls: ['./area-list.component.scss'],
})
export class AreaListComponent implements OnInit {
	constructor() {}

	@Input() areas: IArea[];

	ngOnInit(): void {}
}
