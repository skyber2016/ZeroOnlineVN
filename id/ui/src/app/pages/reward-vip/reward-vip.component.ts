import {Component, OnInit} from '@angular/core';
import {RewardService} from '../../shared/services/reward.service';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-reward-vip',
  templateUrl: './reward-vip.component.html',
  styleUrls: []
})
export class RewardVipComponent implements OnInit {
  rewards = [];

  constructor(private rewardVipService: RewardService, private toastrService: ToastrService) {
  }

  ngOnInit(): void {
    this.onInitData();
  }
  onInitData(){
    this.rewardVipService.get<any[]>({}).subscribe(result => {
      this.rewards = result;
    });
  }
  onSubmit(reward: any) {
    if (reward.status === 2) {
      this.rewardVipService.post({id: reward.id}).subscribe(resp => {
        this.onInitData();
        this.toastrService.success('Nhận quà thành công');
      });
    }
  }
}
