import {AfterViewInit, Component} from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements AfterViewInit {
  title = 'id';

  css: string[] = [

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
