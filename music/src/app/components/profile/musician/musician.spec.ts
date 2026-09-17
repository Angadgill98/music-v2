import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Musician } from './musician';

describe('Musician', () => {
  let component: Musician;
  let fixture: ComponentFixture<Musician>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Musician],
    }).compileComponents();

    fixture = TestBed.createComponent(Musician);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
