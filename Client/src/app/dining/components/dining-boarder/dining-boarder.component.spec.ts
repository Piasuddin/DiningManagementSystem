import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DiningBoarderComponent } from './dining-boarder.component';

describe('DiningBoarderComponent', () => {
  let component: DiningBoarderComponent;
  let fixture: ComponentFixture<DiningBoarderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DiningBoarderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DiningBoarderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
