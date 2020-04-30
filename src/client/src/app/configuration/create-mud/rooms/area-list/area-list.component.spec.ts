import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AreaListComponent } from './area-list.component';

describe('AreaListComponent', () => {
  let component: AreaListComponent;
  let fixture: ComponentFixture<AreaListComponent>;

  beforeEach(async(() => {
	TestBed.configureTestingModule({
		declarations: [ AreaListComponent ]
	})
	.compileComponents();
  }));

  beforeEach(() => {
	fixture = TestBed.createComponent(AreaListComponent);
	component = fixture.componentInstance;
	fixture.detectChanges();
  });

  it('should create', () => {
	expect(component).toBeTruthy();
  });
});
