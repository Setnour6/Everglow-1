using Everglow.Myth.Misc.Items.Ammos;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.Misc.Items.Weapons;

public class CloudGun : ModItem
{
	public override void SetDefaults()
	{
		Item.damage = 20;
		Item.DamageType = DamageClass.Ranged;
		Item.width = 42;
		Item.height = 46;
		Item.useTime = 30;
		Item.useAnimation = 30;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.knockBack = 5;
		Item.value = 700;
		Item.rare = ItemRarityID.Blue;
		Item.UseSound = SoundID.Item11;
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.CloudBall>();
		Item.shootSpeed = 8f;
		Item.useAmmo = ModContent.ItemType<CloudBall>();
		Item.crit = 16;
	}

	public override bool? UseItem(Player player)
	{
		return true;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		SoundEngine.PlaySound(SoundID.Item11);
		Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0);
		return false;
	}

	public override Vector2? HoldoutOffset()
	{
		return new Vector2(-24f, 0);
	}

	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient(ItemID.Cloud, 100)
			.AddTile(TileID.SkyMill)
			.Register();
	}
}