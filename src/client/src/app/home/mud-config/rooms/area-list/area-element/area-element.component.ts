import {
	Component,
	Input,
	Output,
	EventEmitter,
	Directive,
	ElementRef,
	HostListener,
	ViewChild,
} from '@angular/core';
import { IArea } from 'src/app/model/areas/IArea';

@Component({
	selector: 'mh-area-element',
	templateUrl: './area-element.component.html',
	styleUrls: ['./area-element.component.scss'],
})
export class AreaElementComponent {
	constructor() {}

	@Input() area: IArea;
	@Output() update = new EventEmitter<string>();
	@Output() delete = new EventEmitter<IArea>();
	isEditable = false;

	onUpdate(name: string) {
		if (name !== this.area.name) {
			this.update.emit(name);
		}

		this.isEditable = false;
	}
}
