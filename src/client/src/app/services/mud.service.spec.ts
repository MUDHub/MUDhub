import { TestBed } from '@angular/core/testing';

import { MudService } from './mud.service';

describe('MudService', () => {
  let service: MudService;

  beforeEach(() => {
	TestBed.configureTestingModule({});
	service = TestBed.inject(MudService);
  });

  it('should be created', () => {
	expect(service).toBeTruthy();
  });
});
