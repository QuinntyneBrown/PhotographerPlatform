import { Component, Input } from '@angular/core';

@Component({
  selector: 'pp-media-grid',
  standalone: true,
  template: '<div class="media-grid"><ng-content></ng-content></div>',
})
export class PpMediaGrid {}

@Component({
  selector: 'pp-media-card',
  standalone: true,
  template: '<div class="media-card"><ng-content></ng-content></div>',
})
export class PpMediaCard {}

@Component({
  selector: 'pp-media-thumb',
  standalone: true,
  template: '<div class="media-thumb"><ng-content></ng-content></div>',
})
export class PpMediaThumb {}

@Component({
  selector: 'pp-media-meta',
  standalone: true,
  template: '<div class="media-meta"><ng-content></ng-content></div>',
})
export class PpMediaMeta {}

@Component({
  selector: 'pp-avatar',
  standalone: true,
  template: '<div class="avatar" [class.avatar--large]="size === \'large\'"><ng-content></ng-content></div>',
})
export class PpAvatar {
  @Input() size: 'default' | 'large' = 'default';
}

@Component({
  selector: 'pp-entity-icon',
  standalone: true,
  template: '<div class="entity-icon"><ng-content></ng-content></div>',
})
export class PpEntityIcon {}
