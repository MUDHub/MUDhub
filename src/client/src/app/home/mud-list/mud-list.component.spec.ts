import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MudListComponent } from './mud-list.component';

describe('MudListComponent', () => {
  let component: MudListComponent;
  let fixture: ComponentFixture<MudListComponent>;

  beforeEach(async(() => {
	TestBed.configureTestingModule({
		declarations: [ MudListComponent ]
	})
	.compileComponents();
  }));

  beforeEach(() => {
	fixture = TestBed.createComponent(MudListComponent);
	component = fixture.componentInstance;
	fixture.detectChanges();
  });

  it('should create', () => {
	expect(component).toBeTruthy();
  });
});
