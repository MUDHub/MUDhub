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
					useValue: httpClientStub,
				},
			],
		});
		service = TestBed.inject(MudService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});

	// it('#deleteMud should return value from a promise', (done: DoneFn) => {
	// 	service.deleteMud('test').then(value => {
	// 		expect(value).toBeDefined();
	// 		done();
	// 	});
	// });

	// it('#getAll should return an empty array from the api', (done: DoneFn) => {
	// 	service.getAll().then(value => {
	// 		expect(value).toBe([]);
	// 		done();
	// 	});
	// });

	// it('#create should return a value from the api', (done: DoneFn) => {
	// 	service
	// 		.create({
	// 			name: 'test',
	// 			description: 'test',
	// 			isPublic: true,
	// 			autoRestart: true,
	// 		})
	// 		.then(value => {
	// 			expect(value).toBeDefined();
	// 			done();
	// 		});
	// });
});
