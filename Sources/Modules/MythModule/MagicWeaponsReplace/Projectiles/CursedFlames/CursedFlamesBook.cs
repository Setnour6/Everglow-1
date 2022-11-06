﻿namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CursedFlames
{
    internal class CursedFlamesBook : MagicBookProjectile
    {
        public override void SetDef()
        {
            DustType = DustID.CursedTorch;
            ItemType = ItemID.CursedFlames;
            ProjType = ModContent.ProjectileType<CursedFlamesII>();
            MulDamage = 1.6f;
            MulVelocity = 0.3f;
        }
    }
}