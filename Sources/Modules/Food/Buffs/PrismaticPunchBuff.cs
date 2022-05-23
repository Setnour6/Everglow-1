﻿namespace Everglow.Sources.Modules.Food.Buffs
{
    public class PrismaticPunchBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PrismaticPunchBuff");
            Description.SetDefault("高雅兴致 \n 短时间内增加10召唤栏 ");
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // 添加这个，这样护士在治疗时就不会去除buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.maxMinions += 10; //加10召唤栏
            player.GetKnockback(DamageClass.Summon) *= 2f; // 击退加倍
            player.GetDamage(DamageClass.Summon).Base += 0.5f; // 加50%伤害
        }
    }
}

