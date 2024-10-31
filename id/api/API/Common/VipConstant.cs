using System.Collections;
using System.Collections.Generic;

namespace API.Common
{
    public static class VipConstant
    {
        public static int VIP_1 = 1000000;
        public static int VIP_2 = 2000000;
        public static int VIP_3 = 4000000;
        public static int VIP_4 = 8000000;
        public static int VIP_5 = 16000000;
        public static int VIP_6 = 30000000;

        public static IDictionary<int, long> VIP = new Dictionary<int, long>
        {
            [1] = VIP_1,
            [2] = VIP_2,
            [3] = VIP_3,
            [4] = VIP_4,
            [5] = VIP_5,
            [6] = VIP_6,
        };
    }
}
