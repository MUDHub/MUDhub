import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { IMessage } from '../model/IMessage';

@Injectable({
	providedIn: 'root',
})
export class GameService {
	constructor() {}

	private NewGlobalMessageSubject = new Subject<IMessage>();
	public NewGlobalMessage$ = this.NewGlobalMessageSubject.asObservable();
}
