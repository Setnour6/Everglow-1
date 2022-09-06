﻿using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
namespace Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles
{
    public class BeadShakeWave : ModProjectile, IWarpProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 24;
            Projectile.height = 24;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            Projectile.hide = true;
        }
       
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
        private void DrawCircle(float radious, float width, Color color, Vector2 center, bool Black = false)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4), color, new Vector3(0.5f, 1, 0)));
                circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4), color, new Vector3(0.5f, 0, 0)));
            }
            circle.Add(new Vertex2D(center + new Vector2(0, radious), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + new Vector2(0, radious + width), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 0)
            {
                Texture2D t = MythContent.QuickTexture("OmniElementItems/Projectiles/Wave");
                if(Black)
                {
                    t = MythContent.QuickTexture("OmniElementItems/Projectiles/WaveBlack");
                }
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            float value = (200 - Projectile.timeLeft) / (float)Projectile.timeLeft * 1.4f;
            float colorV = 0.6f * (1 - value) * Projectile.ai[0];
            float x0 = Projectile.ai[0];
            if (Projectile.ai[1] != 0)
            {
                colorV = 0.6f * (1 - value) * Projectile.ai[1];
                x0 = Projectile.ai[1] * 0.3f;
            }

            if (value < 1)
            {
                DrawCircle(value * 1100 * Projectile.ai[0], 150 * x0 * (1 - value) + 30 * Projectile.ai[0], new Color(colorV, colorV, colorV, 0f), Projectile.Center - Main.screenPosition);
            }
            value -= 0.2f;
            if (value < 1 && value > 0)
            {
                DrawCircle(value * 900 * Projectile.ai[0], 80 * x0 * (1 - value) + 30 * Projectile.ai[0], new Color(colorV, colorV, colorV, 0f), Projectile.Center - Main.screenPosition);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}