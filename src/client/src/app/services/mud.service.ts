import { Injectable } from '@angular/core';
import { IMudCreateArgs } from '../model/MudDTO';
import { HttpClient } from '@angular/common/http';

@Injectable({
	providedIn: 'root',
})
export class MudService {
	constructor(private http: HttpClient) {}


	createMUD(args: IMudCreateArgs) {

	}
}
