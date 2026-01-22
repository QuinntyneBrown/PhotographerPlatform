import { Component, Input } from '@angular/core';

@Component({
  selector: 'pp-auth-shell',
  standalone: true,
  template: '<div class="auth-shell"><ng-content></ng-content></div>',
})
export class PpAuthShell {}

@Component({
  selector: 'pp-auth-card',
  standalone: true,
  template: `
    <div class="auth-card" [class.auth-card--mobile]="mobile">
      <ng-content></ng-content>
    </div>
  `,
})
export class PpAuthCard {
  @Input() mobile = false;
}

@Component({
  selector: 'pp-auth-brand',
  standalone: true,
  template: '<div class="auth-brand"><ng-content></ng-content></div>',
})
export class PpAuthBrand {}

@Component({
  selector: 'pp-auth-logo',
  standalone: true,
  template: '<div class="auth-logo"><ng-content></ng-content></div>',
})
export class PpAuthLogo {}

@Component({
  selector: 'pp-auth-actions',
  standalone: true,
  template: '<div class="auth-actions"><ng-content></ng-content></div>',
})
export class PpAuthActions {}
