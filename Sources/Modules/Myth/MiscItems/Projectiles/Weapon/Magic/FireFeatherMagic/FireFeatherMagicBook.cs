using Everglow.Myth.MagicWeaponsReplace.Projectiles;

namespace Everglow.Myth.MiscItems.Projectiles.Weapon.Magic.FireFeatherMagic;

internal class FireFeatherMagicBook : MagicBookProjectile
{
	public override string Texture => "Everglow/" + ModAsset.FireFeatherMagicPath;
	public override void SetDef()
	{
		ProjType = ModContent.ProjectileType<FireFeather>();
		DustType = ModContent.DustType<Dusts.FireFeather>();
		ItemType = ModContent.ItemType<Weapons.FireFeatherMagic>();
		MulStartPosByVelocity = 2f;
		UseGlow = false;
		GlowPath = "MiscItems/Weapons/FireFeatherMagic";
		effectColor = new Color(105, 75, 45, 100);
	}
	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;//书跟着玩家飞
		Projectile.spriteDirection = player.direction;
		Projectile.velocity *= 0;
		if (player.itemTime > 0 && player.HeldItem.type == ItemType)//检测手持物品
		{
			Projectile.timeLeft = player.itemTime + 60;
			if (Timer < 30)
				Timer++;
		}
		else
		{
			Timer--;
			if (Timer < 0)
				Projectile.Kill();
		}
		Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;//玩家动作

		player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.timeForVisualEffects / 18d) * 0.6 + 1.2) * -player.direction);
		Vector2 vTOMouse = Main.MouseWorld - player.Center;
		player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
		Projectile.rotation = player.fullRotation;
		SpecialAI();
		if (ProjType == -1)
			return;
		if (player.itemTime == player.itemTimeMax - 2 && player.HeldItem.type == ItemType)
		{
			Vector2 velocity = vTOMouse.SafeNormalize(Vector2.Zero) * player.HeldItem.shootSpeed;
			var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * MulStartPosByVelocity, velocity * MulVelocity, ProjType, (int)(player.HeldItem.damage * MulDamage), player.HeldItem.knockBack, player.whoAmI);
			p.CritChance = player.GetWeaponCrit(player.HeldItem);
			p.extraUpdates = 2;
		}
	}
}