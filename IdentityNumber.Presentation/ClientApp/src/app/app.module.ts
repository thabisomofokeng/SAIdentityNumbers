import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { AppComponent } from "./app.component";
//import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
//import { library } from '@fortawesome/fontawesome-svg-core';
//import { faPlus, faMinus } from '@fortawesome/free-solid-svg-icons';
import { NavMenuComponent } from "./nav-menu/nav-menu.component";
import { IdentityNumberInputComponent } from "./identity-numbers/identity-number-input/identity-number-input.component";
import { IdentityNumberUploadComponent } from "./identity-numbers/identity-number-upload/identity-number-upload.component";
import { IdentityNumbersValidComponent } from "./identity-numbers/identity-numbers-valid/identity-numbers-valid.component";
import { IdentityNumbersInvalidComponent } from "./identity-numbers/identity-numbers-invalid/identity-numbers-invalid.component";
import { IdentityNumberService } from "./identity-numbers/identity-number.service";
import { FileUploadService } from "./identity-numbers/identity-number-upload/identity-number-upload.service";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    IdentityNumberInputComponent,
    IdentityNumberUploadComponent,
    IdentityNumbersValidComponent,
    IdentityNumbersInvalidComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: "", component: IdentityNumberInputComponent, pathMatch: "full" },
      { path: "identity-number-input", component: IdentityNumberInputComponent },
      { path: "identity-number-upload", component: IdentityNumberUploadComponent }
    ]),
    NgbModule.forRoot(),
    //FontAwesomeModule
  ],
  providers: [IdentityNumberService, FileUploadService],
  bootstrap: [AppComponent]
})
export class AppModule {
  //constructor() {
  //  library.add(faPlus, faMinus);
  //}
}
