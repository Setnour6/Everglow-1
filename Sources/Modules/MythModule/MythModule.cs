﻿using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.UI;
using Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds;
using Everglow.Sources.Modules.MythModule.TheFirefly.UI;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Shaders;

namespace Everglow.Sources.Modules.MythModule
{
    public class MythModule: IModule
    {
        public string Name { get; } = "神话";

        private Asset<Effect> m_waveDisortionScreen = null;

        public void Load( )
        {
            ContainerPage.RegisterContainerPage(new FireflyContainerPage()); //在IModule类内手动注册容器页.

            // 水波扰动Shader
            m_waveDisortionScreen = ModContent.Request<Effect>("Everglow/Sources/Modules/ExampleModule/Effects/WaterDisortion", AssetRequestMode.ImmediateLoad);
            ReplaceEffectPass = m_waveDisortionScreen.Value.CurrentTechnique.Passes[0];

            On.Terraria.GameContent.Shaders.WaterShaderData.Update += WaterShaderData_Update;
            IL.Terraria.GameContent.Shaders.WaterShaderData.Apply += WaterShaderData_Apply;


            On.Terraria.GameContent.Shaders.WaterShaderData.StepLiquids += WaterShaderData_StepLiquids;
        }

        private void WaterShaderData_StepLiquids(On.Terraria.GameContent.Shaders.WaterShaderData.orig_StepLiquids orig, WaterShaderData self)
        {
            orig(self);

            // Dust生成，太过耗时
            //var distortionTarget = (RenderTarget2D)typeof(WaterShaderData).GetField("_distortionTarget", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self);
            //var lastDistortionDrawOffset = (Vector2)typeof(WaterShaderData).GetField("_lastDistortionDrawOffset", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self);

            //Color[] colors = new Color[distortionTarget.Width * distortionTarget.Height];
            //distortionTarget.GetData<Color>(colors);

            //Vector2 offscreen = new Vector2(Main.offScreenRange, Main.offScreenRange);
            //Vector2 resol = new Vector2(Main.screenWidth, Main.screenHeight);
            //Vector2 value = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
            //Vector2 value2 = (Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange)) - Main.screenPosition - value;
            //Vector2 offset = -(value2 * 0.25f - lastDistortionDrawOffset) / new Vector2(distortionTarget.Width, distortionTarget.Height);

            //for (int i = 0; i < colors.Length; i++)
            //{
            //    int row = i / distortionTarget.Width;
            //    int col = i % distortionTarget.Width;

            //    float y = (row + 0.5f) / distortionTarget.Height;
            //    float x = (col + 0.5f) / distortionTarget.Width;

            //    float v = (colors[i].R / 255f) * 2 - 1;
            //    if (v > 0.05)
            //    {
            //        Vector2 pos = new Vector2(x, y) - offset;
            //        pos *= Main.GameViewMatrix.Zoom;

            //        var dust = Dust.NewDustDirect(Main.screenPosition + resol * pos, 1, 1, 6);
            //        dust.velocity *= 0f;
            //        dust.noGravity = true;
            //    }
            //}
        }

        private void WaterShaderData_Update(On.Terraria.GameContent.Shaders.WaterShaderData.orig_Update orig, WaterShaderData self, GameTime gameTime)
        {
            // 关掉_useViscosityFilter来防止出现明显视觉bug
            orig(self, gameTime);
            if (ModContent.GetInstance<MothBackground>().BiomeActive())
            {
                self._useViscosityFilter = false;
            }
        }

        public static EffectPass ReplaceEffectPass = null;



        private void WaterShaderData_Apply(MonoMod.Cil.ILContext il)
        {
            var c = new ILCursor(il);
            // Try to find where 566 is placed onto the stack
            if (!c.TryGotoNext(i => i.MatchCall(typeof(ScreenShaderData).FullName, "Apply")))
                return; // Patch unable to be applied

            //c.Remove();
            c.Index++;

            c.Emit(Mono.Cecil.Cil.OpCodes.Ldarg_0);
            c.Emit(Mono.Cecil.Cil.OpCodes.Ldsfld, typeof(MythModule).GetField("ReplaceEffectPass"));
            c.EmitDelegate<Action<WaterShaderData, EffectPass>>((shaderData, effect) =>
            {
                if (!ModContent.GetInstance<MothBackground>().BiomeActive())
                {
                    return;
                }
                var targetPos = shaderData.Shader.Parameters["uTargetPosition"].GetValueVector2();
                var imageOffset = shaderData.Shader.Parameters["uImageOffset"].GetValueVector2();
                var screenPos = Main.screenPosition - targetPos;
                var screenResolution = shaderData.Shader.Parameters["uScreenResolution"].GetValueVector2();
                var intensity = shaderData.Shader.Parameters["uIntensity"].GetValueSingle();
                var progress = shaderData.Shader.Parameters["uProgress"].GetValueSingle();
                var zoom = shaderData.Shader.Parameters["uZoom"].GetValueVector2();
                var noiseSize = shaderData.Shader.Parameters["uImageSize1"].GetValueVector2();
                var waterTargetSize = shaderData.Shader.Parameters["uImageSize3"].GetValueVector2();



                var shader = m_waveDisortionScreen.Value;
                shader.Parameters["cb0"].SetValue(new Vector4(1 / noiseSize.X, 1 / noiseSize.Y, 0, 0));
                shader.Parameters["cb1"].SetValue(new Vector4(progress * 0.05f, 0, 0, 0));
                shader.Parameters["cb2"].SetValue(new Vector4(-progress * 0.05f, 0, 0, 0));
                shader.Parameters["cb3"].SetValue(new Vector4(1f / zoom.X, 1f / zoom.Y, 0, 0));
                shader.Parameters["cb4"].SetValue(new Vector4(32 / screenResolution.X, 320 / screenResolution.Y, 0, 0));
                shader.Parameters["cb5"].SetValue(new Vector4(1f / waterTargetSize.X, 1f / waterTargetSize.Y, 0, 0));
                shader.Parameters["cb6"].SetValue(new Vector4(screenResolution, 0, 0));
                shader.Parameters["cb7"].SetValue(new Vector4(screenPos, 0, 0));
                shader.Parameters["cb8"].SetValue(new Vector4(targetPos, 0, 0));
                shader.Parameters["cb9"].SetValue(new Vector4(intensity, 0, 0, 0));
                shader.Parameters["cb10"].SetValue(new Vector4(imageOffset, 0, 0));
                shader.Parameters["uThreashhold"].SetValue(0.02f);
                shader.Parameters["uPower"].SetValue(1.5f);
                shader.Parameters["uColor"].SetValue(new Vector3(0, 0.5f, 1.0f));


                effect.Apply();
            });
        }



        public void Unload( )
        {
            ReplaceEffectPass = null;
        }
    }
}