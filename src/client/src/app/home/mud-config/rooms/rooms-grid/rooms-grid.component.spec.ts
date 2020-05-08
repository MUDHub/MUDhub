import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomsGridComponent } from './rooms-grid.component';

describe('RoomsGridComponent', () => {
  let component: RoomsGridComponent;
  let fixture: ComponentFixture<RoomsGridComponent>;

  beforeEach(async(() => {
	TestBed.configureTestingModule({
		declarations: [ RoomsGridComponent ]
	})
	.compileComponents();
  }));

  beforeEach(() => {
	fixture = TestBed.createComponent(RoomsGridComponent);
	component = fixture.componentInstance;
	fixture.detectChanges();
  });

  it('should create', () => {
	expect(component).toBeTruthy();
  });
});
