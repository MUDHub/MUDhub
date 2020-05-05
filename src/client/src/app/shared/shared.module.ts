import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ExitButtonComponent } from './exit-button/exit-button.component';

@NgModule({
	declarations: [ExitButtonComponent],
	imports: [CommonModule],
	exports: [ExitButtonComponent]
})
export class SharedModule {}
