using AutoZero.command;
using System;
using System.Linq;

namespace AutoZero.Helpers
{
    public static class UserHelper
    {
        public static string GetUsername(byte[] packet)
        {
            bool isUsername(byte[] pt)
            {
                return pt.vnToStringHex().StartsWith(PacketContants.USERNAME.vnToStringHex());
            }
            var pkgs = packet.ToList();
            while (pkgs.Any())
            {
                var length = pkgs.Take(2).ToArray().ToInt32();
                if (length == 0) break;
                var pkg = pkgs.Take(length).ToArray();
                if(isUsername(pkg))
                {
                    var username = pkg.Skip(0x25).ToArray().GetString()
                        .Replace("\0", "")
                        .Replace("\r", " ")
                        .Replace("VIP LEVEL [0]", "")
                        .Replace("VIP LEVEL [1]", "")
                        .Replace("VIP LEVEL [2]", "")
                        .Replace("VIP LEVEL [3]", "")
                        .Replace("VIP LEVEL [4]", "")
                        .Replace("VIP LEVEL [5]", "")
                        .Replace("VIP LEVEL [6]", "")
                        .Trim()
                        ;
                    return username;
                }
                pkgs = pkgs.Skip(length).ToList();
            }
            return String.Empty;
        }
    }
}
