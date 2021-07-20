using System.Collections.Generic;

namespace API.Common
{
    public static class RewardShopConstant
    {
        public static IDictionary<int, long> Reward = new Dictionary<int, long>
        {
            [1] = 100000,
            [2] = 200000,
            [3] = 500000,
            [4] = 1000000,
            [5] = 2000000,
            [6] = 5000000,
            [7] = 10000000,
        };
    }
}
