﻿using Everglow.Sources.Commons.Core.VFX;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    internal class Storm : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 280;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity *= 0;
            if (Projectile.timeLeft > 550)
            {
                Intensity += 9;
            }
            else
            {
                Intensity -= 5;
                if (Intensity <= 0)
                {
                    Projectile.Kill();
                }
            }
            for (int j = 0; j < 12; j++)
            {
                Vector2 v0 = Vector2.Zero;
                float k0 = Main.rand.NextFloat(0f, 1f);
                float k1 = k0 * k0 * k0 * k0;
                Vector2 v1 = new Vector2(Main.rand.NextFloat(-150f, 150f) / (k1 * 10f + 1f), -k1 * 200 + 10);
                if (Collision.SolidCollision(Projectile.Center + v1, 1, 1))
                {
                    continue;
                }
                Dust dust0 = Dust.NewDustDirect(Projectile.Center + v1, 0, 0, ModContent.DustType<Dusts.CrystalAppearStoppedByTileInAStorm>(), v0.X, v0.Y, 100, default(Color), Main.rand.NextFloat(0.3f, 1.6f) * Math.Min(Intensity, 300) / 450f);
                dust0.noGravity = true;
                dust0.color.B = (byte)(v1.Length() / 2f);
                dust0.color.A = (byte)(Intensity / 2);
                dust0.dustIndex = Projectile.whoAmI;
            }

            /*for (int j = 0; j < 4; j++)
            {
                float k2 = Main.rand.NextFloat(0f, 1f);
                float k3 = k2 * k2 * k2 * k2;
                Vector2 v2 = new Vector2(Main.rand.NextFloat(-150f, 150f) / (k3 * 10f + 1f), -k3 * 200 + 10);
                Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + v2, Vector2.Zero, ModContent.ProjectileType<CrystalWind>(), 0, 0, Projectile.owner, Projectile.whoAmI, Intensity / 1400f * Main.rand.NextFloat(0.85f, 1.15f));
                p0.timeLeft = Math.Min(120, Intensity / 2);
                p0.rotation = Main.rand.NextFloat(6.283f);
            }*/
            if(Main.rand.NextBool(10))
            {
                foreach (var target in Main.npc)
                {
                    if (target.active && Main.rand.NextBool(2))
                    {
                        if (!target.dontTakeDamage && !target.friendly && target.knockBackResist > 0)
                        {
                            Vector2 ToTarget = target.Center - (Projectile.Center - new Vector2(0, 150));
                            float dis = ToTarget.Length();
                            if (dis < 800 && ToTarget != Vector2.Zero)
                            {
                                float mess = target.width * target.height;
                                mess = (float)(Math.Sqrt(mess));
                                Vector2 Addvel = Vector2.Normalize(ToTarget) / mess / (dis + 10) * 100f * target.knockBackResist * Intensity;
                                if (!target.noGravity)
                                {
                                    Addvel.Y *= 3f;
                                }
                                target.velocity -= Addvel;
                                if (target.velocity.Length() > 10)
                                {
                                    target.velocity *= 10 / target.velocity.Length();
                                }
                            }
                        }
                    }
                }
                foreach (var target in Main.item)
                {
                    if (target.active && Main.rand.NextBool(2))
                    {
                        Vector2 ToTarget = target.Center - (Projectile.Center - new Vector2(0, 50));
                        float dis = ToTarget.Length();
                        if (dis < 800 && ToTarget != Vector2.Zero)
                        {
                            float mess = target.width * target.height;
                            mess = (float)(Math.Sqrt(mess));
                            Vector2 Addvel = Vector2.Normalize(ToTarget) / mess / (dis + 10) * 50f * Intensity;
                            target.velocity -= Addvel;
                            if (target.velocity.Length() > 10)
                            {
                                target.velocity *= 10 / target.velocity.Length();
                            }
                        }
                    }
                }
                foreach (var target in Main.gore)
                {
                    if (target.active && Main.rand.NextBool(2))
                    {
                        Vector2 ToTarget = target.position - (Projectile.Center - new Vector2(0, 50));
                        float dis = ToTarget.Length();
                        if (dis < 800 && ToTarget != Vector2.Zero)
                        {
                            float mess = target.Width * target.Height;
                            mess = (float)(Math.Sqrt(mess));
                            Vector2 Addvel = Vector2.Normalize(ToTarget) / mess / (dis + 10) * 100f * Intensity;
                            target.velocity -= Addvel;
                            if (target.velocity.Length() > 10)
                            {
                                target.velocity *= 10 / target.velocity.Length();
                            }
                        }
                    }
                }
            }
            for (int j = 0; j < 8; j++)
            {
                CrystalParticleStorm cp = new CrystalParticleStorm
                {
                    timeLeft = 120,
                    size = Main.rand.NextFloat(0.03f, 0.06f),
                    velocity = Vector2.Zero,
                    Active = true,
                    Visible = true,
                    AI0 = Main.rand.NextFloat(6.283f),
                    AI1 = Intensity / 1400f * Main.rand.NextFloat(0.85f, 1.15f),
                    AI2 = 1,
                    position = Projectile.Center,
                    AimCenter = Projectile.Center
                };

                VFXManager.Add(cp);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }

        internal int Intensity = 0;
        internal Vector2 RingPos = Vector2.Zero;
    }
}