using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared
{
    public static class Offsets
    {
        //LocalPlayer -> m_dwLocalPlayer: _______________ 0x00A9430C
        public static IntPtr LocalPlayer = new IntPtr(0x00A9430C);
        public const int m_iCrossHairID = 0x00008CE4; //Tue Sep 29 21:26:06 2015
        public const int m_iHealth = 0x000000FC; //Tue Sep 29 21:26:06 2015
        public const int m_iTeamNum = 0x000000F0; //Tue Sep 29 21:26:06 2015
        public const int m_dwIndex = 0x00000064; //Tue Sep 29 21:26:06 2015
    }
}
