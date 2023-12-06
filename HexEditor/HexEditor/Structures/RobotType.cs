using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace HexEditor.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    public class ROBOT_TYPE
    {
        public int id;
        public uint price;
        public short hp;
        public ushort hp_inc;
        public ushort total_inside_size;
        public ushort sizex;
        public int weight;
        public ushort attack_inc;
        public ushort def_inc;
        public ushort power_inc;
        public ushort powerrevert_inc;
        public ushort en_inc;
        public ushort enrevert_inc;
        public uint skill1;
        public uint skill2;
        public uint skill3;
        public ushort req_lev;
        public ushort req_weapon_skill;
        public uint req_sex;
        public ushort hot_def;
        public ushort shake_def;
        public ushort cold_def;
        public ushort decay_def;
        public uint look;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
        [JsonProperty("typename_20_ky_tu")]
        public string typename;
        public uint amount_add;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 17)]
        [JsonProperty("robotname_17_ky_tu")]
        public string robotname;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 259)]
        [JsonProperty("NONE_259_ky_tu")]
        public string NONE;
        public uint exp_type;

        public byte[] Build() => MarshalHelper.StructToBytes(this, Marshal.SizeOf(this));
    }

    [StructLayout(LayoutKind.Sequential)]
    public class ROBOT_TYPE_WRAPPER
    {
        [JsonIgnore]
        public int Total;
        [JsonIgnore]
        public IntPtr IdPtr;
        [JsonIgnore]
        public IntPtr RobotTypePtr => this.IdPtr + (Total * Marshal.SizeOf(typeof(int)));

        [JsonIgnore]
        public int[] Ids
        {
            get
            {
                if (this.IdPtr == IntPtr.Zero)
                {
                    return Array.Empty<int>();
                }
                var datas = new List<int>();
                for (int i = 0; i < Total; i++)
                {
                    var offset = i * sizeof(int);
                    var id = (int)Marshal.PtrToStructure(this.IdPtr + offset, typeof(int));
                    if (id == 0)
                    {
                        return Array.Empty<int>();
                    }
                    datas.Add(id);
                }
                return datas.ToArray();
            }
        }
        [JsonProperty("danh_sach")]
        public ROBOT_TYPE[] RobotTypes
        {
            get
            {
                if (this.RobotTypePtr == IntPtr.Zero)
                {
                    return Array.Empty<ROBOT_TYPE>();
                }
                var datas = new List<ROBOT_TYPE>();
                for (int i = 0; i < Total; i++)
                {
                    var offset = i * Marshal.SizeOf(typeof(ROBOT_TYPE));
                    var item = (ROBOT_TYPE)Marshal.PtrToStructure(this.RobotTypePtr + offset, typeof(ROBOT_TYPE));
                    if (item.id == 0)
                    {
                        return Array.Empty<ROBOT_TYPE>();
                    }
                    datas.Add(item);
                }
                return datas.GroupBy(x => x.id).Select(x => x.First()).ToArray();
            }
            set
            {
                var byteArray = new List<byte>();
                this.Total = value.Length;
                if (this.IdPtr == IntPtr.Zero)
                {
                    // Ghi đè lên list_item_id
                    byteArray = value.Select(x => x.id).SelectMany(x => BitConverter.GetBytes(x)).ToList();
                }
                var dumyBytes = value.SelectMany(x => x.Build()).ToArray();
                byteArray.AddRange(dumyBytes);
                this.IdPtr = MarshalHelper.BytesToPointer(byteArray.ToArray());
            }
        }

        public static ROBOT_TYPE_WRAPPER Initialize(byte[] source)
        {
            var handlePtr = MarshalHelper.BytesToPointer(source);
            var wrapper = (ROBOT_TYPE_WRAPPER)Marshal.PtrToStructure(handlePtr, typeof(ROBOT_TYPE_WRAPPER));
            wrapper.IdPtr = handlePtr + sizeof(int);
            return wrapper;
        }

        public byte[] Build()
        {

            var builder = new List<byte>();
            builder.AddRange(BitConverter.GetBytes(this.Total));
            var itemIds = this.Ids;
            foreach (var id in itemIds)
            {
                builder.AddRange(BitConverter.GetBytes(id));
            }
            var items = this.RobotTypes;
            foreach (var item in items)
            {
                builder.AddRange(MarshalHelper.StructToBytes(item, Marshal.SizeOf(item)));
            }
            return builder.ToArray();
        }
    }
}
