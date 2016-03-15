using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Main.Interfaces;

namespace AgaHackTools.Example.Shared.Structs
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Entity
    {
        [FieldOffset(Offsets.m_iVirtualTable)]
        public uint m_iVirtualTable;

        [FieldOffset(Offsets.m_iID)]
        public int m_iID;

        [FieldOffset(Offsets.m_iDormant)]
        public byte m_iDormant;

        [FieldOffset(Offsets.m_hOwner)]
        public short m_hOwner;


        public bool IsValid()
        {
            return this.m_iID >= 0 && this.m_iDormant != 1 && this.m_iVirtualTable > 0 && this.m_iVirtualTable != 0xFFFFFFFF;
        }
        public int GetClientClass(ISmartMemory memUtils)
        {
            try
            {
                uint function = memUtils.Read<uint>((IntPtr)(m_iVirtualTable + 2 * 0x04));
                if (function != 0xFFFFFFFF)
                    return memUtils.Read<int>((IntPtr)(function + 0x01));
                else
                    return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }
        public ClassID GetClassID(ISmartMemory memUtils)
        {
            int clientClass = -1;
            try
            {
                clientClass = GetClientClass(memUtils);
                if (clientClass != -1 && clientClass != 0)
                {
                    var lol = memUtils.Read<int>((IntPtr)(clientClass + 20));
                    return (ClassID)lol;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return (ClassID)clientClass;
        }
        //public string GetName(ISmartMemory memUtils)
        //{
        //    int clientClass = GetClientClass(memUtils);
        //    if (clientClass != -1)
        //    {
        //        int ptr = memUtils.Read<int>((IntPtr)(GetClassID(memUtils) + 8));
        //        return memUtils.ReadString((IntPtr)(ptr + 8), Encoding.ASCII, 32);
        //    }
        //    return "none";
        //}
        public static Entity NullEntity => new Entity() { m_iID = 0, m_iVirtualTable = 0, m_hOwner = 0, m_iDormant = 0 };
        
    }
}
