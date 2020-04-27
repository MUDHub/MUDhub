import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomMatrixComponent } from './room-matrix.component';

describe('RoomMatrixComponent', () => {
  let component: RoomMatrixComponent;
  let fixture: ComponentFixture<RoomMatrixComponent>;

  beforeEach(async(() => {
	TestBed.configureTestingModule({
		declarations: [ RoomMatrixComponent ]
	})
	.compileComponents();
  }));

  beforeEach(() => {
	fixture = TestBed.createComponent(RoomMatrixComponent);
	component = fixture.componentInstance;
	fixture.detectChanges();
  });

  it('should create', () => {
	expect(component).toBeTruthy();
  });
});
