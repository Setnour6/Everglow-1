﻿using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.ZY.WorldSystem;

namespace Everglow.Sources.Modules.ZY.Common
{
    internal class PlayerManager : ModPlayer
    {
        [ProfilerMeasure]
        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            if (newPlayer)
            {
                Everglow.PacketResolver.Send(new WorldVersionPacket());
            }
        }
    }
}
