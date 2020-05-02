import { TestBed } from '@angular/core/testing';

import { MudService } from './mud.service';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

describe('MudService', () => {
	let service: MudService;

	let httpClientStub: Partial<HttpClient>;
	httpClientStub = {
		get(url: string) {
			return of(undefined);
		},
	};

	beforeEach(() => {
		TestBed.configureTestingModule({
			providers: [
				{
					provide: HttpClient,
					useValue: httpClientStub
				}
			]
		});
		service = TestBed.inject(MudService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
