import { Component, OnInit, HostBinding } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
		private mudService: MudService,
		private characterService: CharacterService
	) {}

	@HostBinding('@slideInOutAnimation') get slideInOut() {
		return '';
	}

	mud: IMud;
	races: IMudRace[];
	classes: IMudClass[];

	previousChars = [{}];

	character = new FormGroup({
		name: new FormControl('', [Validators.required, Validators.minLength(3)]),
		race: new FormControl('', Validators.required),
		class: new FormControl('', Validators.required)
	});

	async ngOnInit() {
		this.route.queryParams.subscribe(
			async query => await this.loadMudInfo(query.mudid)
		);
	}

	async loadMudInfo(id: string) {
		if (id) {
			this.mud = await this.mudService.getById(id);
			this.classes = await this.mudService.getMudClass(this.mud.mudId);
			this.races = await this.mudService.getMudRace(this.mud.mudId);
		}
	}

	async join() {
		console.log(
			'joining mud:',
			this.mud,
			'with character:',
			this.character.value
		);
	}
}
