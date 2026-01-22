import { Component, Input } from '@angular/core';

@Component({
  selector: 'pp-card',
  standalone: true,
  template: `
    <div class="mat-card" [class.mat-card--padded]="padded">
      <ng-content></ng-content>
    </div>
  `,
})
export class PpCard {
  @Input() padded = false;
}

@Component({
  selector: 'pp-card-header',
  standalone: true,
  template: '<div class="mat-card__header"><ng-content></ng-content></div>',
})
export class PpCardHeader {}

@Component({
  selector: 'pp-card-title',
  standalone: true,
  template: '<div class="mat-card__title"><ng-content></ng-content></div>',
})
export class PpCardTitle {}

@Component({
  selector: 'pp-card-subtitle',
  standalone: true,
  template: '<div class="mat-card__subtitle"><ng-content></ng-content></div>',
})
export class PpCardSubtitle {}

@Component({
  selector: 'pp-card-content',
  standalone: true,
  template: '<div class="mat-card__content"><ng-content></ng-content></div>',
})
export class PpCardContent {}

@Component({
  selector: 'pp-card-actions',
  standalone: true,
  template: '<div class="mat-card__actions"><ng-content></ng-content></div>',
})
export class PpCardActions {}

@Component({
  selector: 'pp-stats-grid',
  standalone: true,
  template: '<div class="stats-grid"><ng-content></ng-content></div>',
})
export class PpStatsGrid {}

@Component({
  selector: 'pp-stat-card',
  standalone: true,
  template: `
    <div class="stat-card">
      <div class="stat-card__value">{{ value }}</div>
      <div class="stat-card__label">{{ label }}</div>
    </div>
  `,
})
export class PpStatCard {
  @Input() value = '';
  @Input() label = '';
}
