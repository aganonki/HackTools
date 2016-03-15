using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgaHackTools.Example.Shared;
using AgaHackTools.Example.Shared.Structs;
using AgaHackTools.Main.Default;
using AgaHackTools.Main.Hook.WndProc;
using AgaHackTools.Main.Interfaces;

namespace Example.Internal
{
    public class Stuff : IPulsableElement
    {
        private CSGOData csgo;
        private ILog Logger = Log.GetLogger("Internal Pulser");

        public Stuff(CSGOData cs)
        {
            csgo = cs;
        }
        public void Pulse()
        {
            if (csgo.IsNotPlaying)
                return;
            if(!CSGOData.LocalPlayer.IsValid())
                return;
            if(csgo.TraceRay==null)
                return;
            Logger.Info("Pulse start");
            for (var x=0;x<64 ;x++)
            {
                if(!CSGOData.Entity[x].Entity.IsValid()|| !CSGOData.Entity[x].IsPlayer)
                    return;
                if (CSGOData.Entity[x].Player.m_iTeam==CSGOData.LocalPlayer.m_iTeam)
                    return;
                Logger.Info("PLayer "+x);
                csgo.TraceRay.IsVisable(CSGOData.LocalPlayerAddress,
                CSGOData.LocalPlayer.m_vecOrigin + CSGOData.LocalPlayer.m_vecViewOffset, CSGOData.Entity[x].Player.GetBoneVector(Bones.Head));
            }
            

        }
    }
}
