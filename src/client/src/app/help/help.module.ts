import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HelpLoginComponent } from './help-login/help-login.component';
import { FaqComponent } from './faq/faq.component';
import { HelpRoutingModule } from './help-routing.module';

import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { HelpComponent } from './help.component';
import { IntroductionComponent } from './introduction/introduction.component';

@NgModule({
  declarations: [
    HelpLoginComponent,
    FaqComponent,
    HelpComponent,
    IntroductionComponent
  ],
  imports: [
    CommonModule,
    HelpRoutingModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule
  ]
})
export class HelpModule { }
