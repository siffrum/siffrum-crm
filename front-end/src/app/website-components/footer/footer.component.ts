import { Component } from '@angular/core';

@Component({
    selector: 'app-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.scss'],
    standalone: false
})
export class FooterComponent {

  public navigateToSection(hashValue: string) {
    switch (hashValue) {
      case '#about':
      case '#contact':
      case '#pricing':
      case '#home':
        window.location.assign("/website" + hashValue);
        break;
        default:
    }
  }
}
