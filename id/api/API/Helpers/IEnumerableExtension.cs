using API.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API
{
    public static class IEnumerableExtension
    {
        private static List<WheelItem> Items { get; set; }
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomNumberGenerator.GetInt32(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T ItemRandom<T>(this IList<T> list)
        {
            return list[RandomNumberGenerator.GetInt32(0 ,list.Count - 1)];
        }

        public static List<WheelItem> GetItems(this List<WheelItem> items)
        {
            if(Items == null)
            {
                Items = new List<WheelItem>();
                foreach (var item in items)
                {
                    for (int i = 0; i < item.Rate; i++)
                    {
                        Items.Add(item);
                    }
                }
                Items.Shuffle();
            }
            return Items;
        }
    }
}
