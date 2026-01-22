import { Component, EventEmitter, Input, Output } from '@angular/core';

type ChipVariant = 'success' | 'warning' | 'error' | 'info';
type BadgeVariant = 'system' | 'custom';
type NoticeVariant = 'info' | 'warning' | 'error';

@Component({
  selector: 'pp-table',
  standalone: true,
  template: '<table class="mat-table"><ng-content></ng-content></table>',
})
export class PpTable {}

@Component({
  selector: 'pp-pagination',
  standalone: true,
  template: `
    <div class="pagination">
      <div class="pagination__info">{{ info }}</div>
      <div class="pagination__controls">
        <button class="icon-button" [disabled]="previousDisabled" (click)="previous.emit()">
          <span class="material-icons-outlined">chevron_left</span>
        </button>
        <button class="icon-button" [disabled]="nextDisabled" (click)="next.emit()">
          <span class="material-icons-outlined">chevron_right</span>
        </button>
      </div>
    </div>
  `,
})
export class PpPagination {
  @Input() info = '';
  @Input() previousDisabled = false;
  @Input() nextDisabled = false;
  @Output() previous = new EventEmitter<void>();
  @Output() next = new EventEmitter<void>();
}

@Component({
  selector: 'pp-tab-bar',
  standalone: true,
  template: '<div class="tab-bar"><ng-content></ng-content></div>',
})
export class PpTabBar {}

@Component({
  selector: 'pp-tab',
  standalone: true,
  template: `
    <div class="tab" [class.tab--active]="active">
      <ng-content></ng-content>
    </div>
  `,
})
export class PpTab {
  @Input() active = false;
}

@Component({
  selector: 'pp-chip',
  standalone: true,
  template: `
    <span
      class="chip"
      [class.chip--success]="variant === 'success'"
      [class.chip--warning]="variant === 'warning'"
      [class.chip--error]="variant === 'error'"
      [class.chip--info]="variant === 'info'"
    >
      <ng-content></ng-content>
    </span>
  `,
})
export class PpChip {
  @Input() variant: ChipVariant = 'info';
}

@Component({
  selector: 'pp-badge',
  standalone: true,
  template: `
    <span class="badge" [class.badge--system]="variant === 'system'" [class.badge--custom]="variant === 'custom'">
      <ng-content></ng-content>
    </span>
  `,
})
export class PpBadge {
  @Input() variant: BadgeVariant = 'system';
}

@Component({
  selector: 'pp-notice',
  standalone: true,
  template: `
    <div
      class="notice"
      [class.notice--info]="variant === 'info'"
      [class.notice--warning]="variant === 'warning'"
      [class.notice--error]="variant === 'error'"
    >
      <ng-content></ng-content>
    </div>
  `,
})
export class PpNotice {
  @Input() variant: NoticeVariant = 'info';
}

@Component({
  selector: 'pp-tag',
  standalone: true,
  template: '<span class="tag"><ng-content></ng-content></span>',
})
export class PpTag {}

@Component({
  selector: 'pp-role-list',
  standalone: true,
  template: '<div class="role-list"><ng-content></ng-content></div>',
})
export class PpRoleList {}

@Component({
  selector: 'pp-role-chip',
  standalone: true,
  template: '<span class="role-chip"><ng-content></ng-content></span>',
})
export class PpRoleChip {}
