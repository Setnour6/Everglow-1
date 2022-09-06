﻿using Everglow.Sources.Modules.MythModule.TheFirefly;
using Terraria.Audio;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MEACModule;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class NavyThunderBomb : ModProjectile, IWarpProjectile
    {
        private float r = 0;
        private Vector2 v0;
        private int Fra = 0;
        private int FraX = 0;
        private int FraY = 0;
        private float Stre2 = 1;
        public override string Texture => "Everglow/Sources/Modules/MythModule/TheFirefly/Projectiles/MothBall";
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Navy Thunder Bomb");
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {

            Projectile.velocity *= 0.95f;
            if (Stre2 > 0)
            {
                Stre2 -= 0.005f;
            }
            if (Projectile.timeLeft > 240)
            {
                r += 1f;
            }
            if (Projectile.timeLeft <= 240 && Projectile.timeLeft >= 60)
            {
                r = 60 + (float)(10 * Math.Sin((Projectile.timeLeft - 60) / 60d * Math.PI));
            }
            if (Projectile.timeLeft < 60 && r > 0.5f)
            {
                r -= 1f;
            }
            Fra = ((600 - Projectile.timeLeft) / 3) % 30;
            FraX = (Fra % 6) * 270;
            FraY = (Fra / 6) * 290;
            if (v0 != Vector2.Zero)
            {
                // Projectile.position = v0 - new Vector2(Dx, Dy) / 2f;
            }
        }
        public override void Kill(int timeLeft)
        {
            /*震屏
            MythPlayer mplayer = Terraria.Main.player[Terraria.Main.myPlayer].GetModPlayer<MythPlayer>();
            mplayer.Shake = 6;
            float Str = 1;

            mplayer.ShakeStrength = Str;*/
            SoundEngine.PlaySound(SoundID.DD2_ExplosiveTrapExplode, Projectile.Center);
            for (int h = 0; h < 120; h += 3)
            {
                Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d + Projectile.ai[0]) + 5).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
                int r = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.PureBlue>(), 0, 0, 0, default(Color), 15f * Main.rand.NextFloat(0.7f, 2.9f));
                Main.dust[r].noGravity = true;
                Main.dust[r].velocity = v3 * 6;
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 6.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 4.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 180; y += 3)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 6.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.0f, 27.5f)).RotatedByRandom(Math.PI * 2d);
            }
            for (int y = 0; y < 36; y++)
            {
                int index = Dust.NewDust(Projectile.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<Dusts.BlueGlow>(), 0f, 0f, 100, default(Color), Main.rand.NextFloat(1.3f, 4.2f));
                Main.dust[index].noGravity = true;
                Main.dust[index].velocity = new Vector2(Main.rand.NextFloat(0.0f, 2.5f), Main.rand.NextFloat(1.8f, 5.5f)).RotatedByRandom(Math.PI * 2d);
            }
          
            base.Kill(timeLeft);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return false;
        }
        private void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        public void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex)
        {
            float Wid = 6f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            for (int x = 0; x < 3; x++)
            {
                float Value0 = (float)(Main.time / 291d + 20) % 1f;
                float Value1 = (float)(Main.time / 291d + 20.03) % 1f;
                vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 1, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));
            }


            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D Water = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/ElecLine");
            Texture2D WaterS = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade");
            float value0 = (float)(Math.Sin(800d / (double)(Projectile.timeLeft + 35)) * 0.75f + 0.25f) * (300 - Projectile.timeLeft) / 300f;
            value0 = Math.Max(0, value0);
            //DrawTexCircle(132, 22, new Color(value0, value0, value0, value0), Projectile.Center - Main.screenPosition, WaterS, Main.time / 17);
            DrawTexCircle(122, 42, new Color(0.33f * value0, 0.33f * value0, 0.33f * value0, 0.33f * value0), Projectile.Center - Main.screenPosition, WaterS, -Main.time / 17);
            DrawTexCircle(132, 32, new Color(0, 0.45f * value0, 1f * value0, 0), Projectile.Center - Main.screenPosition, Water, Main.time / 17);
            DrawTexCircle(122, 42, new Color(0, 0.15f * value0, 0.33f * value0, 0), Projectile.Center - Main.screenPosition, Water, -Main.time / 17);

            Main.spriteBatch.Draw(ModContent.Request<Texture2D>(Texture).Value, Projectile.Center - Main.screenPosition, new Rectangle(FraX, FraY + 10, 270, 270), new Color(1f, 1f, 1f, 0), Projectile.rotation, new Vector2(135f, 135f), r / 420f, SpriteEffects.None, 0f);
            Texture2D Light = Common.MythContent.QuickTexture("TheFirefly/Projectiles/CorruptLight");
            Main.spriteBatch.Draw(Light, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(Stre2, Stre2, Stre2, 0), Projectile.rotation, new Vector2(168f, 168f), Projectile.scale * r / 210f, SpriteEffects.None, 0);
            return false;
        }
        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            float value0 = (float)(Math.Sin(800d / (double)(Projectile.timeLeft + 35)) * 0.75f + 0.25f) * (300 - Projectile.timeLeft) / 300f;
            value0 = Math.Max(0, value0);
            DrawTexCircle(122, 52, new Color(0.4f * value0, 0.42f * value0, 1f * value0, 0), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/ElecLine"), Main.time / 17);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}