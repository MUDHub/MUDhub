import { Component, OnInit, HostBinding } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MudService } from 'src/app/services/mud.service';
import { IMud } from 'src/app/model/muds/IMud';
import {
	trigger,
	state,
	style,
	transition,
	animate,
} from '@angular/animations';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { CharacterService } from 'src/app/services/character.service';
import { IMudRace, IMudClass } from 'src/app/model/muds/MudSetupDTO';
import { AuthService } from 'src/app/services/auth.service';
import { ICharacter } from 'src/app/model/character/ICharacter';
import { ICharacterCreateRequest } from 'src/app/model/character/CharacterDTO';

@Component({
	templateUrl: './mud-join.component.html',
	styleUrls: ['./mud-join.component.scss'],
	animations: [
		trigger('slideInOutAnimation', [
			// route 'enter' transition
			transition(':enter', [
				// styles at start of transition
				style({
					bottom: '-100%',
				}),
				animate(
					'.8s cubic-bezier(0,1,1,1)',
					style({
						bottom: 0,
					})
				),
			]),
			transition(':leave', [
				animate(
					'.5s ease-in',
					style({
						bottom: '-100%',
					})
				),
			]),
		]),
	],
})
export class MudJoinComponent implements OnInit {
	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private mudService: MudService,
		private characterService: CharacterService,
		private auth: AuthService
	) {}

	@HostBinding('@slideInOutAnimation') get slideInOut() {
		return '';
	}

	mud: IMud;
	races: IMudRace[] = [];
	classes: IMudClass[] = [];

	mudid: string;

	previousChars: ICharacter[] = [];

	character = new FormGroup({
		name: new FormControl('', [Validators.required, Validators.minLength(3)]),
		race: new FormControl('', Validators.required),
		class: new FormControl('', Validators.required)
	});

	async ngOnInit() {
		this.route.queryParams.subscribe(
			async query => {
				this.mudid = query.mudid;
				this.loadData(this.mudid);

			}
		);
	}

	async loadData(mudid: string) {
		if (mudid) {
			this.mud = await this.mudService.getById(mudid);
			this.classes = await this.mudService.getClassForMud(this.mud.mudId);
			this.races = await this.mudService.getRaceForMud(this.mud.mudId);
		}

		this.previousChars = await this.characterService.getCharactersForPlayer(this.mudid, this.auth.user.id);
		console.log(this.previousChars);
	}


	async onSubmit() {
		const args: ICharacterCreateRequest = {
			name: this.character.get('name').value,
			raceId: this.character.get('race').value,
			classId: this.character.get('class').value,
			mudId: this.mudid
		};

		try {
			const response = await this.characterService.createCharacter(this.mudid, args);
			await this.join(response.character.id);
		} catch (err) {
			console.error('Error while creating character', err);
		}
	}


	async join(characterid: string) {
		await this.router.navigate(['/game', characterid]);
	}
}
