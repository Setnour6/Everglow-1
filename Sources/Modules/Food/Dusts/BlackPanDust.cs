﻿namespace Everglow.Food.Dusts;

public class BlackPanDust : ModDust
{
	public override bool MidUpdate(Dust dust)
	{
		return true;
	}
	public override bool Update(Dust dust)
	{
		dust.rotation = dust.velocity.ToRotation();
		dust.scale *= 0.99f;
		float mulVelocity = 100f;
		Vector2 nextVelocity = Main.player[dust.color.G].Center - dust.position;
		if (nextVelocity.Length() >= Math.Sqrt(mulVelocity))
		{
			nextVelocity /= nextVelocity.Length() * nextVelocity.Length() / mulVelocity;

			dust.position += nextVelocity + Main.player[dust.color.G].velocity;
		}
		else
		{
			dust.position = Main.player[dust.color.G].Center;
			dust.active = false;
		}
		if (dust.scale < 0.05f)
			dust.active = false;
		return false;
	}
	public override void OnSpawn(Dust dust)
	{
		base.OnSpawn(dust);
	}
	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return new Color(255, 255, 255, 255);
	}
}
