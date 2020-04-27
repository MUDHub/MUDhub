import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateMudComponent } from './create-mud.component';

describe('CreateMudComponent', () => {
  let component: CreateMudComponent;
  let fixture: ComponentFixture<CreateMudComponent>;

  beforeEach(async(() => {
	TestBed.configureTestingModule({
		declarations: [ CreateMudComponent ]
	})
	.compileComponents();
  }));

  beforeEach(() => {
	fixture = TestBed.createComponent(CreateMudComponent);
	component = fixture.componentInstance;
	fixture.detectChanges();
  });

  it('should create', () => {
	expect(component).toBeTruthy();
  });
});
