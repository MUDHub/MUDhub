import { Component, OnInit } from '@angular/core';

@Component({
	selector: 'mh-game-shell',
	templateUrl: './game-shell.component.html',
	styleUrls: ['./game-shell.component.scss'],
})
export class GameShellComponent implements OnInit {
	constructor() {}

	showChat = true;

	ngOnInit(): void {}
}
