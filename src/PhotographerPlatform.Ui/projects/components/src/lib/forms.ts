import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'pp-form-grid',
  standalone: true,
  template: '<div class="form-grid"><ng-content></ng-content></div>',
})
export class PpFormGrid {}

@Component({
  selector: 'pp-form-row',
  standalone: true,
  template: '<div class="form-row"><ng-content></ng-content></div>',
})
export class PpFormRow {}

@Component({
  selector: 'pp-form-field',
  standalone: true,
  template: '<div class="form-field"><ng-content></ng-content></div>',
})
export class PpFormField {}

@Component({
  selector: 'pp-input',
  standalone: true,
  template: `
    <input
      class="input"
      [attr.type]="type"
      [attr.placeholder]="placeholder"
      [value]="value"
      [disabled]="disabled"
      (input)="onInput($event)"
    />
  `,
})
export class PpInput {
  @Input() type = 'text';
  @Input() placeholder = '';
  @Input() value = '';
  @Input() disabled = false;
  @Output() valueChange = new EventEmitter<string>();

  onInput(event: Event) {
    const target = event.target as HTMLInputElement;
    this.valueChange.emit(target.value);
  }
}

@Component({
  selector: 'pp-textarea',
  standalone: true,
  template: `
    <textarea
      class="input"
      [attr.placeholder]="placeholder"
      [value]="value"
      [disabled]="disabled"
      (input)="onInput($event)"
    ></textarea>
  `,
})
export class PpTextarea {
  @Input() placeholder = '';
  @Input() value = '';
  @Input() disabled = false;
  @Output() valueChange = new EventEmitter<string>();

  onInput(event: Event) {
    const target = event.target as HTMLTextAreaElement;
    this.valueChange.emit(target.value);
  }
}

@Component({
  selector: 'pp-select',
  standalone: true,
  template: `
    <select class="select-input" [disabled]="disabled" [value]="value" (change)="onChange($event)">
      <ng-content></ng-content>
    </select>
  `,
})
export class PpSelect {
  @Input() value = '';
  @Input() disabled = false;
  @Output() valueChange = new EventEmitter<string>();

  onChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    this.valueChange.emit(target.value);
  }
}

@Component({
  selector: 'pp-filter-bar',
  standalone: true,
  template: '<div class="filter-bar"><ng-content></ng-content></div>',
})
export class PpFilterBar {}

@Component({
  selector: 'pp-search-field',
  standalone: true,
  template: `
    <div class="search-field">
      <span class="material-icons-outlined search-icon">search</span>
      <input
        class="search-input"
        [attr.placeholder]="placeholder"
        [value]="value"
        [disabled]="disabled"
        (input)="onInput($event)"
      />
    </div>
  `,
})
export class PpSearchField {
  @Input() placeholder = '';
  @Input() value = '';
  @Input() disabled = false;
  @Output() valueChange = new EventEmitter<string>();

  onInput(event: Event) {
    const target = event.target as HTMLInputElement;
    this.valueChange.emit(target.value);
  }
}
