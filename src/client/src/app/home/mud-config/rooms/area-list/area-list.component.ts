import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { IArea } from 'src/app/model/areas/IArea';

@Component({
	selector: 'mh-area-list',
	templateUrl: './area-list.component.html',
	styleUrls: ['./area-list.component.scss'],
})
export class AreaListComponent implements OnInit {
	constructor() {}

	@Input() areas: IArea[];
	@Input() selectedArea: IArea;

	@Output() newArea = new EventEmitter();
	@Output() select = new EventEmitter<IArea>();
	@Output() delete = new EventEmitter<IArea>();

	ngOnInit(): void {}
}
