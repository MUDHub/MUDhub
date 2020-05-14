import { Component, OnInit } from '@angular/core';
import { CommandService } from 'src/app/services/command.service';

@Component({
	selector: 'mh-game-input',
	templateUrl: './game-input.component.html',
	styleUrls: ['./game-input.component.scss'],
})
export class GameInputComponent implements OnInit {
	constructor(private command: CommandService) {}

	ngOnInit() {}


	onInput(element: HTMLInputElement) {
		try {
			const value = element.value;
			this.command.handleInput(value);
			element.value = '';
		} catch (err) {

		}
	}
}
