﻿using Terraria;
using Terraria.ModLoader;

namespace Everglow.Sources.Modules.Food.Buffs
{
	public class RoastedBirdBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RoastedBirdBuff");
			Description.SetDefault("我是一只小小小鸟 \n 增加飞行能力");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.jumpSpeedBoost += 3;
			player.maxFallSpeed *= 0.5f;
			player.extraFall += 30;
			player.wingAccRunSpeed *= 1.2f;
		}
	}
}

	