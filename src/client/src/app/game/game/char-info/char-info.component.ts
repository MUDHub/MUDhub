import { Component, OnInit } from '@angular/core';
import { ICharacter } from 'src/app/model/character/ICharacter';
import { ActivatedRoute } from '@angular/router';
import { CharacterService } from 'src/app/services/character.service';

@Component({
	selector: 'mh-char-info',
	templateUrl: './char-info.component.html',
	styleUrls: ['./char-info.component.scss'],
})
export class CharInfoComponent implements OnInit {
	constructor(private route: ActivatedRoute, private characterService: CharacterService) {}

	character: ICharacter;

	async ngOnInit() {
		const charid = this.route.snapshot.params.characterid;
		try {
			this.character = await this.characterService.getCharacter(charid);

		} catch (err) {
			console.error('Error while loading character info', err);
		}
	}
}
