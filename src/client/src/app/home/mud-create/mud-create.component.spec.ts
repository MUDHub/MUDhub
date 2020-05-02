import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MudCreateComponent } from './mud-create.component';

describe('MudCreateComponent', () => {
  let component: MudCreateComponent;
  let fixture: ComponentFixture<MudCreateComponent>;

  beforeEach(async(() => {
	TestBed.configureTestingModule({
		declarations: [ MudCreateComponent ]
	})
	.compileComponents();
  }));

  beforeEach(() => {
	fixture = TestBed.createComponent(MudCreateComponent);
	component = fixture.componentInstance;
	fixture.detectChanges();
  });

  it('should create', () => {
	expect(component).toBeTruthy();
  });
});
