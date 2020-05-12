import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { GameService } from '../services/game.service';
import Swal from 'sweetalert2';

@Component({
	selector: 'mh-game-shell',
	templateUrl: './game-shell.component.html',
	styleUrls: ['./game-shell.component.scss'],
})
export class GameShellComponent implements OnInit {
	constructor(private route: ActivatedRoute, private router: Router, private game: GameService) {}

	characterid: string;
	showChat = true;

	async ngOnInit() {
		this.characterid = this.route.snapshot.params.characterid;
		try {
			await this.game.joinGame(this.characterid);
		} catch (err) {
			console.error('Error while trying to connect to signalR', err);
			await Swal.fire({
				icon: 'error',
				title: 'Verbindungsfehler',
				text: 'Keine Verbindung zum Echtzeit-Server möglich'
			});
		}
	}



	async exit() {
		await this.game.exitGame();
		// TODO: handle exit
		this.router.navigate(['/']);
	}
}
