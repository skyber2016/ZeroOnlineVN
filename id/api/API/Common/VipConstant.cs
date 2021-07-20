using System.Collections;
using System.Collections.Generic;

namespace API.Common
{
    public static class VipConstant
    {
        public static int VIP_1 = 500000;
        public static int VIP_2 = 1000000;
        public static int VIP_3 = 2000000;
        public static int VIP_4 = 6000000;
        public static int VIP_5 = 8000000;
        public static int VIP_6 = 12000000;

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
