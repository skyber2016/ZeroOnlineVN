import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoinToZpsComponent } from './coin-to-zps.component';

describe('CoinToZpsComponent', () => {
  let component: CoinToZpsComponent;
  let fixture: ComponentFixture<CoinToZpsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CoinToZpsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CoinToZpsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
