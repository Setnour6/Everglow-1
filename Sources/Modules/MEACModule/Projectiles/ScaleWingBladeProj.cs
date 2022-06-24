﻿using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MEACModule.Projectiles
{
    public class ScaleWingBladeProj : MeleeProj
    {
        public override void SetDef()
        {
            maxAttackType = 3;
            TrailLength = 20;
            shadertype = "Trail";
        }
        public override string TrailColorTex()
        {
            return "Everglow/Sources/Modules/MEACModule/Images/TestColor";
        }
        public override float TrailAlpha(float factor)
        {
            return base.TrailAlpha(factor) * 1.5f;
        }
        public override BlendState TrailBlendState()
        {
            return BlendState.NonPremultiplied;
        }
        public override void DrawSelf(SpriteBatch spriteBatch, Color lightColor)
        {
            if (attackType != 114514)
            {
                Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
                //Main.spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(lightColor), mVec.ToRotation(), new Vector2(0, tex.Height / 2), new Vector2(mVec.Length() / tex.Width, 1.2f) * projectile.scale, SpriteEffects.None, 0);

                float texWidth = 85;//转换成水平贴图时候的宽度
                float texHeight = 30;//转换成水平贴图时候的高度


                float exScale = 1;
                if (longHandle)
                {
                    exScale += 1f;
                }
                Vector2 origin = new Vector2(longHandle ? texWidth / 2 : 5, texHeight / 2);

                Vector2 Zoom = new Vector2(exScale * mainVec.Length() / tex.Width, 1.2f) * projectile.scale;
                double baseRotation = 0.79;//这个是刀刃倾斜度与水平的夹角
                double ProjRotation = mainVec.ToRotation() + Math.PI / 4;

                float QuarterSqrtTwo = 0.35355f;

                Vector2 drawCenter = projectile.Center - Main.screenPosition;
                Vector2 INormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation - Math.PI / 4)) * Zoom.Y * 0.85f;
                Vector2 JNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(ProjRotation - (baseRotation + Math.PI / 4)) * Zoom.X * 0.85f;

                Vector2 ITexNormal = new Vector2(texHeight * QuarterSqrtTwo).RotatedBy(-(baseRotation - Math.PI / 4));
                ITexNormal.X /= tex.Width;
                ITexNormal.Y /= tex.Height;
                Vector2 JTexNormal = new Vector2(texWidth * QuarterSqrtTwo).RotatedBy(-(baseRotation + Math.PI / 4));
                JTexNormal.X /= tex.Width;
                JTexNormal.Y /= tex.Height;
                Vector2 TopLeft/*原水平贴图的左上角,以此类推*/ = INormal;
                Vector2 TopRight = JNormal * 2 + INormal;
                Vector2 BottomLeft = -INormal;
                Vector2 BottomRight = JNormal * 2 - INormal;


                Vector2 sourceTopLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
                Vector2 sourceTopRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
                Vector2 sourceBottomLeft = new Vector2(0.5f) - ITexNormal - JTexNormal;
                Vector2 sourceBottomRight = new Vector2(0.5f) - ITexNormal + JTexNormal;

                if(Main.player[Projectile.owner].direction == -1)
                {
                    sourceTopLeft = sourceBottomLeft;
                    sourceTopRight = sourceBottomRight;
                    sourceBottomLeft = new Vector2(0.5f) + ITexNormal - JTexNormal;
                    sourceBottomRight = new Vector2(0.5f) + ITexNormal + JTexNormal;
                }

                List<Vertex2D> vertex2Ds = new List<Vertex2D>
                {
                    new Vertex2D(drawCenter + TopLeft, projectile.GetAlpha(lightColor), new Vector3(sourceTopLeft.X, sourceTopLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, projectile.GetAlpha(lightColor), new Vector3(sourceTopRight.X, sourceTopRight.Y, 0)),
                    new Vertex2D(drawCenter + BottomLeft, projectile.GetAlpha(lightColor), new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),

                    new Vertex2D(drawCenter + BottomRight, projectile.GetAlpha(lightColor), new Vector3(sourceBottomRight.X, sourceBottomRight.Y, 0)),
                    new Vertex2D(drawCenter + BottomLeft, projectile.GetAlpha(lightColor), new Vector3(sourceBottomLeft.X, sourceBottomLeft.Y, 0)),
                    new Vertex2D(drawCenter + TopRight, projectile.GetAlpha(lightColor), new Vector3(sourceTopRight.X, sourceTopRight.Y, 0))
                };

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertex2Ds.ToArray(), 0, vertex2Ds.Count - 2);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            }

            else
            {
               
            }
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void Attack()
        {
            Player player = Main.player[projectile.owner];
            UseTrail = true;
            if (attackType == 0)
            {
                if (Timer < 30)//前摇
                {
                    UseTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.5f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, -1.2f), 0.08f);
                    mainVec += projectile.DirectionFrom(player.Center) * 3;
                    projectile.rotation = mainVec.ToRotation();
                }
                if (Timer == 30)
                    AttSound(SoundID.Item1);
                if (Timer > 30 && Timer < 50)
                {
                    isAttacking = true;
                    projectile.rotation += projectile.spriteDirection * 0.25f;
                    mainVec = Vector2Elipse(120, projectile.rotation, 0.6f);
                }
                if (Timer > 70)
                {
                    NextAttackType();
                }
            }
            if (attackType == 1)
            {
                if (Timer == 0)
                {
                    LockPlayerDir(player);
                    UseTrail = false;
                    projectile.rotation = -MathHelper.PiOver2 - player.direction * 0.6f;
                }
                if (Timer == 1)
                    AttSound(SoundID.Item1);
                if (Timer < 20)
                {
                    isAttacking = true;
                    projectile.rotation += projectile.spriteDirection * 0.22f;
                    mainVec = Vector2Elipse(120, projectile.rotation, -0.6f);
                }
                if (Timer > 40)
                {
                    NextAttackType();
                }
            }
            if (attackType == 2)
            {
                if (Timer == 0)
                {
                    LockPlayerDir(player);
                    UseTrail = false;
                    projectile.rotation = -MathHelper.PiOver2 - player.direction * 0.1f;
                }
                if (Timer < 60)
                {
                    isAttacking = true;
                    if (Timer % 15 == 0)
                    {
                        AttSound(SoundID.Item1);
                        UseTrail = false;
                    }
                    if (Timer % 30 < 15)
                    {
                        projectile.rotation += projectile.spriteDirection * 0.3f;
                        mainVec = Vector2Elipse(120, projectile.rotation, 1f);
                    }
                    else
                    {
                        projectile.rotation -= projectile.spriteDirection * 0.3f;
                        mainVec = Vector2Elipse(120, projectile.rotation, -1f);
                    }

                }
                if (Timer > 70)
                {
                    NextAttackType();
                }
            }
            if (attackType == 3)
            {
                if (Timer < 30)//前摇
                {
                    UseTrail = false;
                    LockPlayerDir(player);
                    float targetRot = -MathHelper.PiOver2 - player.direction * 0.7f;
                    mainVec = Vector2.Lerp(mainVec, Vector2Elipse(80, targetRot, -1.2f), 0.08f);
                    mainVec += projectile.DirectionFrom(player.Center) * 3;
                    projectile.rotation = mainVec.ToRotation();
                }
                if (Timer > 30 && Timer < 40)
                {
                    isAttacking = true;
                    projectile.rotation += projectile.spriteDirection * 0.55f;
                    mainVec = Vector2Elipse(120, projectile.rotation, -1.2f);
                }
                if (Timer == 30)
                    AttSound(SoundID.Item1);
                if (Timer > 30 && Timer < 50)
                {

                }
                if (Timer > 100)
                {
                    NextAttackType();
                }
            }
            if (isAttacking)
                for (int i = 1; i < 4; i++)
                {
                    Dust d = Dust.NewDustDirect(projectile.Center + i * mainVec / 3, 20, 20, 172, 0, 0, 0, default, 1.5f);
                    d.velocity *= 0;
                    d.noGravity = true;
                }
        }
    }
}

