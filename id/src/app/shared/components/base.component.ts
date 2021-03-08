import {AfterViewInit, Component, OnDestroy, OnInit} from '@angular/core';
import {environment} from 'src/environments/environment';


@Component({
  selector: 'app-base',
  template: '',
})
export class BaseComponent implements OnInit, OnDestroy, AfterViewInit {

  public env = environment;

  private scriptsOfComponent: Array<string> = [];
  private _scripts: string[] = [

  ];

  constructor() {
    this.scriptsOfComponent = [...this._scripts];
  }

  get scripts() {
    return this._scripts;
  }

  // Merge script có sẵn và truyền vào
  set scripts(list: Array<string>) {
    this._scripts = [...this._scripts, ...list];
  }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    try {
      // Load scripts có sẵn và thêm vào từ component child
      this.loadScript();
    } catch (error) {
      console.log(`Have error when load scripts`, error.message);
    }
  }

  private addScript(source, sources: Array<string>, indexNewSource?: number): void {

    // Hết dữ liệu trong hàng đợi
    if (!source) {
      return;
    }

    let newSource;
    if (indexNewSource) {
      newSource = sources[indexNewSource];
      indexNewSource++;
    } else {
      newSource = sources.shift();
    }

    const isLibrary = source.search(/min.js/) >= 0;
    const isJQuery = source.search(/jquery.min.js/) >= 0;

    const vm = this;
    const head = document.getElementsByTagName('head')[0];
    const id = source.replace(/[/:.]/gi, '_');

    // Kiểm tra sự tồn tại của script này thông qua id
    // Không cài đặt các thư viện lại
    if (document.querySelector(`#${id}`) && isLibrary) {
      return vm.addScript(newSource, sources, indexNewSource);
    }

    // Gỡ bỏ và cài lại để khởi tạo lại các script nếu không phải là thư viện
    if (document.querySelector(`#${id}`) && !isLibrary) {
      document.querySelector(`#${id}`).remove();
    }

    const script: any = document.createElement('script');
    script.id = id;
    script.type = 'text/javascript';

    // Thêm source script tiếp theo, đồng bộ theo thứ tự
    // Add jquery đầu tiền, nếu có jquery rồi thì mới thêm các thư viện khác
    if (script.readyState) {  // only required for IE <9
      script.onreadystatechange = () => {
        if (script.readyState === 'loaded' || script.readyState === 'complete') {
          script.onreadystatechange = null;
          vm.addScript(newSource, sources, indexNewSource);
        }
      };
    } else {  // Others
      script.onload = () => {
        vm.addScript(newSource, sources, indexNewSource);
      };
    }
    if (!this.env.production) {
      console.log(`%cInstalled ... ${source}`, 'color: gray;');
    }

    script.src = source;
    head.appendChild(script);
  }

  // Load script có sẵn và thêm từ component
  private loadScript(): void {
    this.addScript(this._scripts.shift(), this._scripts);
  }

  reloadScript(): void {
    this.addScript(this.scriptsOfComponent[0], this.scriptsOfComponent, 1);
  }


  ngOnDestroy() {

  }

}
