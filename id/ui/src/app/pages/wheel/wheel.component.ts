import { Component, OnInit } from '@angular/core';
import { WheelService } from "../../shared/services/wheel.service";
import { MessageService } from "../../shared/services/message.service";
import { version } from '../../../../package.json';
import * as moment from 'moment';
declare const $: any;

@Component({
  selector: 'app-wheel',
  templateUrl: './wheel.component.html',
  styleUrls: ['./wheel.component.css']
})
export class WheelComponent implements OnInit {
  version = version;
  private hieu_ung = {
    el: '#rotate-play',		//Set element
    stop_point: null,
    interval_id: null,
    rotate_count: 4,
    old_point: {
      check: false,
      value_old: null,
      value_new: null
    },
    played: {

    },
    play: function () {
      if (!this.stop_point)
        return;
      if (this.old_point.value_old == null) {
        this.old_point.value_old = this.old_point.value_new;
      }

      $(this).css('-webkit-transform', 'rotate(' + this.old_point.value_old + 'deg)');
      $(this).css('-moz-transform', 'rotate(' + this.old_point.value_old + 'deg)');
      $(this).css('transform', 'rotate(' + this.old_point.value_old + 'deg)');

      let v_old = this.old_point.value_old;
      let v_stop = this.stop_point;
      let element = this.el;
      let v_this = this;

      $(this.el).animate({ transform: v_stop }, {
        step: function (now, fx) {
          const current = parseInt((now / 45).toString());
          if (!v_this.played[current]) {
            v_this.played[current] = true;
            v_this.playSound();
          }
          fx.start = v_old;
          if (now >= v_old) {
            $(this).css('-webkit-transform', 'rotate(' + now + 'deg)');
            $(this).css('-moz-transform', 'rotate(' + now + 'deg)');
            $(this).css('transform', 'rotate(' + now + 'deg)');
          }
        },
        duration: 5000
      }, 'ease-out'
      );
      //},0)

    },
    stop: function () {
      $(this.el).stop();
    },
    playSound: () => {
      this.audio.pause();
      this.audio.currentTime = 0;
      this.audio.play();
    },
    setStopPoint: function (deg)		//params
    {
      if (this.stop_point != null) {
        this.old_point.check = true;
      }
      let valueArrPoint = deg;
      this.stop_point = this.rotate_count * 360 + valueArrPoint;
      if (this.old_point.check) {
        this.old_point.value_old = this.old_point.value_new;
        this.old_point.value_new = valueArrPoint;
      } else {
        this.old_point.value_old = 0;
        this.old_point.value_new = valueArrPoint;
      }
    }
  };
  wheelCount = 0;
  histories = [];
  wheelled = false;
  audio = new Audio('/assets/tick.mp3');
  constructor(private wheelService: WheelService, private message: MessageService) {
  }

  ngOnInit(): void {
    this.initWheel();
  }

  initWheel(): void {
    this.wheelService.get<any>({}).subscribe(resp => {
      this.wheelCount = resp.wheelCount;
      this.histories = resp.histories.map(item => {
        const time: Date = new Date(item.createdDate);
        item.time = this.fromNow(time);
        return item;
      });
    })
  }

  onWheel(): void {
    if (this.wheelled) {
      return;
    }
    this.wheelService.post({}).subscribe(resp => {
      this.wheelled = true;
      this.hieu_ung.setStopPoint(resp.deg);
      this.hieu_ung.play();
      const self = this;

      setTimeout(function () {
        self.message.success(resp.message, 'You receive');
        $('#p-overlay').removeClass('active');
        self.wheelCount = resp.wheelRemain;
        self.histories = resp.histories;
        self.wheelled = false;
        self.hieu_ung.played = {};
      }, 5500);
    });

  }

  fromNow(time: Date): string {
    moment.locale('vi');
    return moment(time, 'yyyy-MM-DD HH:mm:ss').fromNow();
  }

}
