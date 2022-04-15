using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoZero.MVVM.Model
{
    public class AccountModel
    {
        public string Username { get; set; }

        public bool AutoAnswer { get; set; }
        public bool AutoAtom { get; set; }
        public string FuncName { get; set; }
    }
}
