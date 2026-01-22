import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'pp-mobile-shell',
  standalone: true,
  template: '<div class="mobile-shell"><ng-content></ng-content></div>',
})
export class PpMobileShell {}

@Component({
  selector: 'pp-mobile-topbar',
  standalone: true,
  template: '<header class="mobile-topbar"><ng-content></ng-content></header>',
})
export class PpMobileTopbar {}

@Component({
  selector: 'pp-mobile-content',
  standalone: true,
  template: '<main class="mobile-content"><ng-content></ng-content></main>',
})
export class PpMobileContent {}

@Component({
  selector: 'pp-mobile-nav',
  standalone: true,
  template: '<nav class="mobile-nav"><ng-content></ng-content></nav>',
})
export class PpMobileNav {}

@Component({
  selector: 'pp-mobile-nav-item',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="mobile-nav__item" [class.mobile-nav__item--active]="active">
      <span *ngIf="icon" class="material-icons-outlined">{{ icon }}</span>
      <ng-content></ng-content>
    </div>
  `,
})
export class PpMobileNavItem {
  @Input() icon = '';
  @Input() active = false;
}
