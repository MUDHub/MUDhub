import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RacesComponent } from './races.component';

describe('RacesComponent', () => {
  let component: RacesComponent;
  let fixture: ComponentFixture<RacesComponent>;

  beforeEach(async(() => {
	TestBed.configureTestingModule({
		declarations: [ RacesComponent ]
	})
	.compileComponents();
  }));

  beforeEach(() => {
	fixture = TestBed.createComponent(RacesComponent);
	component = fixture.componentInstance;
	fixture.detectChanges();
  });

  it('should create', () => {
	expect(component).toBeTruthy();
  });
});
