using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace HexEditor.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public class SHOP_ITEM
    {
        [JsonProperty("item_type_id")]
        public uint Id;

        public ushort Shape;

        [JsonProperty("chua_ro_0")] 
        public byte Unknow0;

        [JsonProperty("vi_tri_X")]
        public byte X;

        [JsonProperty("vi_tri_Y")]
        public byte Y;

        [JsonProperty("chua_ro_1")]
        public byte Unknow1;

        [JsonProperty("chua_ro_2")]
        public byte Unknow2;

        [JsonProperty("chua_ro_3")]
        public byte Unknow3;

        [JsonIgnore] public int TotalBytes => Marshal.SizeOf(typeof(SHOP_ITEM));

    }
    [StructLayout(LayoutKind.Sequential)]
    public class SHOP_PAGE
    {
        [JsonProperty("chua_ro_0")]
        public byte Unknown0;

        [JsonProperty("dai")]
        public byte Height;

        [JsonProperty("rong")]
        public byte Width;

        [JsonIgnore] public byte Totals;
        [JsonIgnore] public IntPtr ItemPtr;
        [JsonIgnore] public int Offset => Marshal.SizeOf(this) - 0x4;

        [JsonIgnore] public int TotalBytes => Offset + this.Items.Sum(x => x.TotalBytes);

        [JsonProperty("vat_pham")]
        public SHOP_ITEM[] Items
        {
            get
            {
                if (this.ItemPtr == IntPtr.Zero)
                {
                    return Array.Empty<SHOP_ITEM>();
                }
                var lst = new List<SHOP_ITEM>();
                for (int i = 0; i < Totals; i++)
                {
                    var offset = this.ItemPtr + (i * Marshal.SizeOf(typeof(SHOP_ITEM)));
                    var shopItem = (SHOP_ITEM)Marshal.PtrToStructure(offset, typeof(SHOP_ITEM));
                    if(shopItem.Unknow3 != 204)
                    {

                    }
                    lst.Add(shopItem);
                }
                return lst.ToArray();
            }
        }

        public byte[] Build()
        {
            var bytes = MarshalHelper.StructToBytes(this, this.Offset).ToList();
            var byteArray = this.Items.SelectMany(item => MarshalHelper.StructToBytes(item, Marshal.SizeOf(item))).ToList();
            bytes.AddRange(byteArray);
            return bytes.ToArray();
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public class SHOP
    {
        [JsonProperty("shop_id")]
        public uint ShopId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        [JsonIgnore] public byte[] ShopNameByteArray;

        [JsonProperty("chua_ro")]
        public int Unknown;

        [JsonIgnore] public uint TotalPage;

        [JsonIgnore] public IntPtr ShopPagePtr;

        [JsonIgnore] public int Offset => Marshal.SizeOf(this) - 0x4;

        [JsonIgnore] public int TotalBytes => Offset + this.Pages.Sum(x => x.TotalBytes);

        
        [JsonIgnore]
        public string Name
        {
            get
            {
                var gb2312 = Encoding.GetEncoding("gb2312");
                var text = gb2312.GetString(ShopNameByteArray);
                return text;
            }
            set
            {
                var gb2312 = Encoding.GetEncoding("gb2312");
                this.ShopNameByteArray = gb2312.GetBytes(value);
            }
        }

        [JsonProperty("sheet")]
        public SHOP_PAGE[] Pages
        {
            get
            {
                if (this.ShopPagePtr == IntPtr.Zero)
                {
                    return Array.Empty<SHOP_PAGE>();
                }
                var lst = new List<SHOP_PAGE>();
                int offset = 0;
                for (int i = 0; i < this.TotalPage; i++)
                {
                    var shopPage = (SHOP_PAGE)Marshal.PtrToStructure(this.ShopPagePtr + offset, typeof(SHOP_PAGE));
                    shopPage.ItemPtr = this.ShopPagePtr + offset + shopPage.Offset;
                    lst.Add(shopPage);
                    offset += shopPage.TotalBytes;
                }
                return lst.ToArray();
            }
            set
            {
                this.TotalPage = (uint)value.Length;
                var byteArray = value.SelectMany(item => MarshalHelper.StructToBytes(item, Marshal.SizeOf(item))).ToArray();
                this.ShopPagePtr = MarshalHelper.BytesToPointer(byteArray);
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class SHOP_WRAPPER
    {
        [JsonIgnore] public int Total;

        [JsonIgnore] public IntPtr ShopPtr;

        [JsonIgnore] public int Offset => Marshal.SizeOf(this) - 0x4;

        public static SHOP_WRAPPER Initialize(byte[] source)
        {
            var handlePtr = MarshalHelper.BytesToPointer(source);
            var wrapper = (SHOP_WRAPPER)Marshal.PtrToStructure(handlePtr, typeof(SHOP_WRAPPER));
            wrapper.ShopPtr = handlePtr + sizeof(int);
            return wrapper;
        }

        [JsonProperty("danh_sach_cua_hang")]
        public SHOP[] Shops
        {
            get
            {
                if (this.ShopPtr == IntPtr.Zero)
                {
                    return Array.Empty<SHOP>();
                }
                var lst = new List<SHOP>();
                int offset = 0;
                for (int i = 0; i < Total; i++)
                {
                    var shop = (SHOP)Marshal.PtrToStructure(this.ShopPtr + offset, typeof(SHOP));
                    shop.ShopPagePtr = this.ShopPtr + offset + shop.Offset;
                    lst.Add(shop);
                    offset += shop.TotalBytes;

                }
                return lst.ToArray();
            }
            set
            {
                this.Total = value.Length;
                var byteArray = value.SelectMany(item => MarshalHelper.StructToBytes(item, Marshal.SizeOf(item))).ToArray();
                this.ShopPtr = MarshalHelper.BytesToPointer(byteArray);
            }
        }
    }
}
