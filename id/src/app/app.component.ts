import {AfterViewInit, Component} from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements AfterViewInit {
  title = 'id';

  css: string[] = [
    '/assets/HT/images/css_x.css',
    '/assets/HT/css/cssmin.css',
    '/assets/HT/css/cssx.css',
    'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css',
    '/assets/HT/css/jquery-ui-1.8.16.custom.css',
    '/assets/HT/css/jquery.css',
    'https://fonts.googleapis.com/css?family=Montserrat:300,300i,400,400i,500,500i,600,600i,700,700i',
    '/assets/HT/css/tooltipster.bundle.min.css',
    '/assets/HT/css/tooltipster-sideTip-punk.min.css',
    '/assets/HT/css/owl.carousel.min.css',
    '/assets/HT/css/owl.theme.default.min.css',
    '/assets/HT/css/style.css',
    '/assets/HT/css/cs.css',
    '/assets/HT/css/style1.css',
    '/assets/HT/css/additions.css',
    '/assets/HT/show/tooltip.css',
    'http://maxcdn.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css',
    'http://taikhoan.mu-hanoi.vn/HT/fontawesome/css/all.css',
    'https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css',

  ];

  addCss(source, allSource: string[]) {
    if (!source) {
      return;
    }
    const css = document.createElement('link');
    css.rel = 'stylesheet';
    css.href = source;
    document.head.append(css);
    this.addCss(allSource.shift(), allSource);
  }

  ngAfterViewInit(): void {
    this.addCss(this.css.shift(), this.css);
  }
}
