using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace HexEditor.Structures
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class ITEM_TYPE
    {
        public int id;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 41)]
        public string name;
        public byte level;
        public short weight;
        public int price;
        public int id_action;
        public int life;
        public int max_en;
        public int charge_en;
        public int max_power;
        public int charge_power;
        public int amount_limit;
        public int ident;
        public int equip_type;
        public byte equip_level;
        public byte equip_skill;
        public ushort gem1;
        public ushort gem2;
        public ushort gem3;
        public uint magic1;
        public uint magic2;
        public uint magic3;
        public ushort max_range;
        public ushort atk_speed;
        public short nicety;
        public short pack_size;
        public short pack_width;
        public ushort max_atk;
        public ushort min_atk;
        public ushort hot_atk;
        public ushort shake_atk;
        public ushort sting_atk;
        public short decay_atk;
        public ushort defence_max;
        public short defence_percent;
        public ushort hot_def;
        public ushort shake_def;
        public ushort cold_def;
        public ushort light_def;
        public short electric_def;
        public uint shape;
        public ushort Emoney;
        public int Req_Engine;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 514)]
        [JsonProperty("mo_ta")]
        public string Description;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ITEM_TYPE_WRAPPER
    {
        [JsonIgnore]
        public int Count;

        [JsonIgnore]
        public IntPtr ListIdPtr;

        [JsonIgnore]
        public IntPtr ItemPtr => ListIdPtr + (Count * sizeof(int));

        public static ITEM_TYPE_WRAPPER Initialize(byte[] byteArray)
        {
            var handlePtr = MarshalHelper.BytesToPointer(byteArray);
            var wrapper = (ITEM_TYPE_WRAPPER)Marshal.PtrToStructure(handlePtr, typeof(ITEM_TYPE_WRAPPER));
            wrapper.ListIdPtr = handlePtr + sizeof(int);
            return wrapper;
        }

        public void UpdateListIds(int[] lstId)
        {
            this.Count = lstId.Length;
            var byteArray = lstId.SelectMany(x => BitConverter.GetBytes(x)).ToList();
            var byteOffsets = Enumerable.Range(0, this.Count * Marshal.SizeOf(typeof(ITEM_TYPE))).Select(x => (byte)0x0).ToArray();
            byteArray.AddRange(byteOffsets);
            this.ListIdPtr = MarshalHelper.BytesToPointer(byteArray.ToArray());
        }

        public void UpdateItem(ITEM_TYPE[] items)
        {
            var byteArray = new List<byte>();
            if (this.ListIdPtr == IntPtr.Zero)
            {
                // Ghi đè lên list_item_id
                this.Count = items.Length;
                byteArray = items.Select(x => x.id).SelectMany(x => BitConverter.GetBytes(x)).ToList();
            }
            var dumyBytes = items.SelectMany(x => MarshalHelper.StructToBytes(x, Marshal.SizeOf(x))).ToArray();
            byteArray.AddRange(dumyBytes);
            this.ListIdPtr = MarshalHelper.BytesToPointer(byteArray.ToArray());
        }

        [JsonProperty("danh_sach")]
        [JsonIgnore]
        public int[] GetListIds
        {
            get
            {
                if (this.ListIdPtr == IntPtr.Zero)
                {
                    return Array.Empty<int>();
                }
                var datas = new List<int>();
                for (int i = 0; i < Count; i++)
                {
                    var offset = i * sizeof(int);
                    var id = (int)Marshal.PtrToStructure(this.ListIdPtr + offset, typeof(int));
                    if (id == 0)
                    {
                        return Array.Empty<int>();
                    }
                    datas.Add(id);
                }
                return datas.ToArray();
            }
            set
            {
                UpdateListIds(value);
            }
        }
        [JsonProperty("item_type")]
        public ITEM_TYPE[] GetItems
        {
            get
            {
                if (this.ItemPtr == IntPtr.Zero)
                {
                    return Array.Empty<ITEM_TYPE>();
                }
                var datas = new List<ITEM_TYPE>();
                for (int i = 0; i < Count; i++)
                {
                    var offset = i * Marshal.SizeOf(typeof(ITEM_TYPE));
                    var item = (ITEM_TYPE)Marshal.PtrToStructure(this.ItemPtr + offset, typeof(ITEM_TYPE));
                    if (item.id == 0)
                    {
                        return Array.Empty<ITEM_TYPE>();
                    }

                    //item.name = StringHelper.RemoveDiacritics(item.name);
                    //item.Description = StringHelper.RemoveDiacritics(item.Description);
                    datas.Add(item);
                }
                return datas.ToArray();
            }
            set
            {
                UpdateItem(value);
            }
        }

        public byte[] Build()
        {

            var builder = new List<byte>();
            builder.AddRange(BitConverter.GetBytes(this.Count));
            var itemIds = this.GetListIds;
            foreach (var id in itemIds)
            {
                builder.AddRange(BitConverter.GetBytes(id));
            }
            var items = this.GetItems;
            foreach (var item in items)
            {
                builder.AddRange(MarshalHelper.StructToBytes(item, Marshal.SizeOf(item)));
            }
            return builder.ToArray();
        }
    }

}
