using AutoZero.Cores;
using AutoZero.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AutoZero.MVVM.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<AccountModel> Accounts { get; set; }
        public Dispatcher Dispatcher { get; set;  }

        private AccountModel _currentAccount { get; set; }
        public AccountModel CurrentAccount
        {
            get { return _currentAccount; }
            set
            {
                this._currentAccount = value;
                this.RaisePropertyChanged("CurrentAccount");
            }
        }
        private string _loginServer { get; set; }
        public string LoginServer
        {
            get
            {
                return _loginServer;
            }
            set
            {
                this._loginServer = value;
                this.RaisePropertyChanged("LoginServer");
            }
        }
        private string _gameServer { get; set; }
        public string GameServer
        {
            get {  return _gameServer; }
            set
            {
                this._gameServer = value;
                this.RaisePropertyChanged("GameServer");
            }
        }
        public MainViewModel()
        {
            this.Accounts = new ObservableCollection<AccountModel>();
            this.LoginServer = "OFFLINE";
            this.GameServer = "OFFLINE";

        }

    }
}
