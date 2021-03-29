import {AfterViewInit, Component, OnInit} from '@angular/core';
// @ts-ignore
import { version } from '../../package.json';
import {LoaderService} from './shared/services/loader.service';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, AfterViewInit {
  title = 'id';
  constructor(private loaderService: LoaderService) {
  }
  css: string[] = [

  ];

  addCss(source, allSource: string[]) {
    if (!source) {
      return;
    }
    const css = document.createElement('link');
    css.rel = 'stylesheet';
    css.href = source + '?version=' + version;
    document.head.append(css);
    this.addCss(allSource.shift(), allSource);
  }

  ngAfterViewInit(): void {
    console.info('App version',version);
    this.addCss(this.css.shift(), this.css);
  }

  ngOnInit(): void {
    this.loaderService.isLoading.subscribe(isLoad => {
      if(isLoad){
        document.getElementById('loading').style.display = 'block';
        document.getElementById('backBg').style.display = 'block';
      }
      else{
        document.getElementById('loading').style.display = 'none';
        document.getElementById('backBg').style.display = 'none';
      }
    })
  }
}
