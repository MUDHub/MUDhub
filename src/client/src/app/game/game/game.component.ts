import { Component, OnInit, OnDestroy } from '@angular/core';
import { GameService } from 'src/app/services/game.service';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';

@Component({
	selector: 'mh-game',
	templateUrl: './game.component.html',
	styleUrls: ['./game.component.scss'],
})
export class GameComponent implements OnInit, OnDestroy {
	constructor(private game: GameService, private route: ActivatedRoute, private router: Router) {}

	private characterId: string;

	ngOnInit() {
		this.characterId = this.route.snapshot.params.characterid;
		console.log('character-id:', this.characterId);
	}

	ngOnDestroy() {
		// TODO: leave game when user activly closes the game
	}

}
