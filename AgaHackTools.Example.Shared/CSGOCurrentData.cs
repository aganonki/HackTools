using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgaHackTools.Example.Shared
{
    public class CSGOCurrentData
    {
        public CSGOCurrentData()
        {
            LocalPlayer = new LocalPlayer {m_iID = 0,m_iHealth = 0,m_iTeam = 0};
        }
        public LocalPlayer LocalPlayer;
    }
}
