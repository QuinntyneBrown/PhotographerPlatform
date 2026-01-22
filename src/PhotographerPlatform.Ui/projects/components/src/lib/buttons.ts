import { Component, Input } from '@angular/core';

type ButtonVariant = 'primary' | 'outlined' | 'danger';

@Component({
  selector: 'pp-button',
  standalone: true,
  template: `
    <button
      class="btn"
      [class.btn--primary]="variant === 'primary'"
      [class.btn--outlined]="variant === 'outlined'"
      [class.btn--danger]="variant === 'danger'"
      [disabled]="disabled"
      [attr.type]="type"
    >
      <ng-content></ng-content>
    </button>
  `,
})
export class PpButton {
  @Input() variant: ButtonVariant = 'primary';
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled = false;
}

@Component({
  selector: 'pp-icon-button',
  standalone: true,
  template: `
    <button class="icon-button" [disabled]="disabled" [attr.type]="type">
      <ng-content></ng-content>
    </button>
  `,
})
export class PpIconButton {
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled = false;
}

@Component({
  selector: 'pp-fab',
  standalone: true,
  template: `
    <button class="fab" [disabled]="disabled" [attr.type]="type">
      <ng-content></ng-content>
    </button>
  `,
})
export class PpFab {
  @Input() type: 'button' | 'submit' | 'reset' = 'button';
  @Input() disabled = false;
}
