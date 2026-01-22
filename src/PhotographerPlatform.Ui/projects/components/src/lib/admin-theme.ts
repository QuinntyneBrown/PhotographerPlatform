import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'pp-admin-theme',
  standalone: true,
  template: '<ng-content></ng-content>',
  styleUrl: './styles/admin-ui.css',
  encapsulation: ViewEncapsulation.None,
})
export class PpAdminTheme {}
