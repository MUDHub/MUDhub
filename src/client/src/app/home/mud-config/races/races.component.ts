import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
	templateUrl: './races.component.html',
	styleUrls: ['./races.component.scss'],
})
export class RacesComponent implements OnInit {
	constructor(private route: ActivatedRoute) {}

	mudId: string;

	ngOnInit(): void {
		this.mudId = this.route.snapshot.params.mudid;
	}
}
