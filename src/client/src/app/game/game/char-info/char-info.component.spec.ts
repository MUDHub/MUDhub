import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CharInfoComponent } from './char-info.component';

describe('CharInfoComponent', () => {
  let component: CharInfoComponent;
  let fixture: ComponentFixture<CharInfoComponent>;

  beforeEach(async(() => {
	TestBed.configureTestingModule({
		declarations: [ CharInfoComponent ]
	})
	.compileComponents();
  }));

  beforeEach(() => {
	fixture = TestBed.createComponent(CharInfoComponent);
	component = fixture.componentInstance;
	fixture.detectChanges();
  });

  it('should create', () => {
	expect(component).toBeTruthy();
  });
});
