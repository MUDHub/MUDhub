import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
	selector: 'mh-game-shell',
	templateUrl: './game-shell.component.html',
	styleUrls: ['./game-shell.component.scss'],
})
export class GameShellComponent implements OnInit {
	constructor(private router: Router) {}

	showChat = true;

	ngOnInit(): void {}



	exit() {
		// TODO: handle exit
		this.router.navigate(['/']);
	}
}
