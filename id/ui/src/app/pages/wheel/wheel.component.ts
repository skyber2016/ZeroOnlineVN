import { Component, OnInit } from '@angular/core';

declare const $:any;
@Component({
  selector: 'app-wheel',
  templateUrl: './wheel.component.html',
  styleUrls: ['./wheel.component.css']
})
export class WheelComponent implements OnInit {
  private hieu_ung = {
    el: '#rotate-play',		//Set element
    stop_point: null,
    interval_id: null,
    rotate_count: 4,
    old_point : {
      check : false,
      value_old : null,
      value_new : null
    },
    play: function()
    {
      if (!this.stop_point)
        return;
      if(this.old_point.value_old == null){
        this.old_point.value_old = this.old_point.value_new;
      }

      $(this).css('-webkit-transform', 'rotate('+this.old_point.value_old+'deg)');
      $(this).css('-moz-transform', 'rotate('+this.old_point.value_old+'deg)');
      $(this).css('transform', 'rotate('+this.old_point.value_old+'deg)');

      let v_old = this.old_point.value_old;
      let v_stop = this.stop_point;
      let element = this.el;
      let v_this = this;

      $(this.el).animate({  transform: v_stop }, {
          step: function(now,fx) {
            fx.start = v_old;
            if(now >= v_old){
              $(this).css('-webkit-transform','rotate('+now+'deg)');
              $(this).css('-moz-transform','rotate('+now+'deg)');
              $(this).css('transform','rotate('+now+'deg)');
            }
          },
          duration: 5000
        }, 'ease-out'
      );
      //},0)

    },
    stop: function ()
    {
      $(this.el).stop();
    },
    setStopPoint: function(deg)		//params
    {
      if(this.stop_point != null){
        this.old_point.check = true;
      }
      let valueArrPoint = deg;
      console.log('Giá trị quay : ' + valueArrPoint);
      this.stop_point = this.rotate_count*360 + valueArrPoint;
      console.log('Giá trị quay thực tế : ' + this.stop_point);
      if(this.old_point.check){
        this.old_point.value_old = this.old_point.value_new;
        this.old_point.value_new = valueArrPoint;
      }else{
        this.old_point.value_old = 0;
        this.old_point.value_new = valueArrPoint;
      }
    }
  };
  constructor() { }

  ngOnInit(): void {
  }

  onWheel():void {
    this.hieu_ung.setStopPoint(parseFloat('45'));
    this.hieu_ung.play();
    setTimeout(function () {
      //alert("Chúc mừng!");
      //$('#p-overlay').removeClass('active');
    }, 5500);
  }

}
