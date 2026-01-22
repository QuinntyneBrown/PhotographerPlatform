import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'pp-app-shell',
  standalone: true,
  template: '<div class="app-container"><ng-content></ng-content></div>',
})
export class PpAppShell {}

@Component({
  selector: 'pp-toolbar',
  standalone: true,
  template: '<header class="mat-toolbar"><ng-content></ng-content></header>',
})
export class PpToolbar {}

@Component({
  selector: 'pp-toolbar-title',
  standalone: true,
  template: '<h1 class="mat-toolbar__title"><ng-content></ng-content></h1>',
})
export class PpToolbarTitle {}

@Component({
  selector: 'pp-toolbar-spacer',
  standalone: true,
  template: '<div class="mat-toolbar__spacer"></div>',
})
export class PpToolbarSpacer {}

@Component({
  selector: 'pp-layout-container',
  standalone: true,
  template: '<div class="layout-container"><ng-content></ng-content></div>',
})
export class PpLayoutContainer {}

@Component({
  selector: 'pp-sidenav',
  standalone: true,
  template: '<nav class="mat-sidenav"><ng-content></ng-content></nav>',
})
export class PpSidenav {}

@Component({
  selector: 'pp-nav-list',
  standalone: true,
  template: '<ul class="mat-nav-list"><ng-content></ng-content></ul>',
})
export class PpNavList {}

@Component({
  selector: 'pp-nav-item',
  standalone: true,
  imports: [CommonModule],
  template: `
    <li class="mat-list-item" [class.mat-list-item--active]="active">
      <span *ngIf="icon" class="material-icons-outlined mat-list-item__icon">{{ icon }}</span>
      <span><ng-content></ng-content></span>
    </li>
  `,
})
export class PpNavItem {
  @Input() icon = '';
  @Input() active = false;
}

@Component({
  selector: 'pp-main-content',
  standalone: true,
  template: `
    <main class="main-content" [class.main-content--centered]="centered">
      <ng-content></ng-content>
    </main>
  `,
})
export class PpMainContent {
  @Input() centered = false;
}

@Component({
  selector: 'pp-page-header',
  standalone: true,
  template: '<div class="page-header"><ng-content></ng-content></div>',
})
export class PpPageHeader {}

@Component({
  selector: 'pp-page-title',
  standalone: true,
  template: '<div class="page-title"><ng-content></ng-content></div>',
})
export class PpPageTitle {}

@Component({
  selector: 'pp-page-subtitle',
  standalone: true,
  template: '<div class="page-subtitle"><ng-content></ng-content></div>',
})
export class PpPageSubtitle {}

@Component({
  selector: 'pp-section',
  standalone: true,
  template: '<div class="section"><ng-content></ng-content></div>',
})
export class PpSection {}

@Component({
  selector: 'pp-split',
  standalone: true,
  template: '<div class="split"><ng-content></ng-content></div>',
})
export class PpSplit {}

@Component({
  selector: 'pp-user-cell',
  standalone: true,
  template: '<div class="user-cell"><ng-content></ng-content></div>',
})
export class PpUserCell {}
