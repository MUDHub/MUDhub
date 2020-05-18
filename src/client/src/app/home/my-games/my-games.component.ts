import { Component, OnInit } from '@angular/core';
import { CharacterService } from 'src/app/services/character.service';
import { AuthService } from 'src/app/services/auth.service';
import { IMud } from 'src/app/model/muds/IMud';
import { ICharacter } from 'src/app/model/character/ICharacter';

@Component({
	templateUrl: './my-games.component.html',
	styleUrls: ['./my-games.component.scss'],
})
export class MyGamesComponent implements OnInit {
	constructor(private characterService: CharacterService, private auth: AuthService) {}

	charsByMuds: { mud: IMud; chars: ICharacter[] }[] = [];

	async ngOnInit() {
		const chars = await this.characterService.getCharactersForPlayer(this.auth.user.id);

		for (const char of chars) {
			const container = this.charsByMuds.find(m => char.mud.mudId === m.mud.mudId);
			if (container) {
				if (container.chars) {
					container.chars.push(char);
				} else {
					container.chars = [char];
				}
			} else {
				this.charsByMuds.push({
					mud: char.mud,
					chars: [char],
				});
			}
		}
	}


	async deleteCharacter(char: ICharacter) {
		try {
			await this.characterService.deleteCharacter(char.id);

			for (let i = 0; i < this.charsByMuds.length; i++) {
				for (let j = 0; j < this.charsByMuds[i].chars.length; j++) {
					const character = this.charsByMuds[i].chars[j];
					if (character && character.id === char.id) {
						this.charsByMuds[i].chars.splice(j, 1);
					}
				}
			}

			for (let i = 0; i < this.charsByMuds.length; i++) {
				if (this.charsByMuds[i].chars?.length === 0 ?? true) {
					this.charsByMuds.splice(i, 1);
				}
			}

		} catch (err) {
			console.error('Error while deleting character', err);
		}
	}
}
