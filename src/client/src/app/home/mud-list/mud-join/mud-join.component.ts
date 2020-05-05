import { Component, OnInit, HostBinding } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MudService } from 'src/app/services/mud.service';
import { IMud } from 'src/app/model/IMud';
import {
	trigger,
	state,
	style,
	transition,
	animate,
} from '@angular/animations';
import { FormControl, Validators } from '@angular/forms';

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
		private mudService: MudService
	) {}

	@HostBinding('@slideInOutAnimation') get slideInOut() {
		return '';
	}

	mud: IMud;
	previousChars = [{}];

	characterName = new FormControl('', [
		Validators.required,
		Validators.minLength(3),
	]);

	async ngOnInit() {
		this.route.queryParams.subscribe(
			async query => await this.loadMudInfo(query.mudid)
		);
	}

	async loadMudInfo(id: string) {
		if (id) {
			this.mud = await this.mudService.getById(id);
		}
	}

	async join() {
		console.log(
			'joining mud: ',
			this.mud,
			'with character-name:',
			this.characterName.value
		);
	}
}
