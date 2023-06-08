using Everglow.Myth.MagicWeaponsReplace.GlobalItems;
using Everglow.Myth.MiscItems.Projectiles.Weapon.Magic;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.MiscItems.Weapons;

public class FreezeFeatherMagic : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.damage = 41;
		Item.DamageType = DamageClass.Magic;
		Item.width = 28;
		Item.height = 30;
		Item.useTime = 24;
		Item.useAnimation = 24;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.knockBack = 1.6f;
		Item.value = 2406;
		Item.rare = ItemRarityID.LightRed;
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<FreezeFeather>(); 
		Item.shootSpeed = 8;
		Item.crit = 8;
		Item.mana = 15;
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.GetModPlayer<MagicBookPlayer>().MagicBookLevel == 1)
		{
			return false;
		}
		else
		{
			for (int k = 0; k < 3; k++)
			{
				Vector2 v2 = velocity.RotatedBy(Main.rand.NextFloat(-0.42f, 0.42f)) * Main.rand.NextFloat(0.9f, 1.1f);
				Projectile.NewProjectile(source, position + velocity * 2f, v2, type, damage, knockback, player.whoAmI, Main.rand.NextFloat(1f));
			}
			useSpeed = Item.useTime / 17f;
		}
		return false;
	}
	float useSpeed = -1f;
	public override void HoldItem(Player player)
	{
		if (player.GetModPlayer<MagicBookPlayer>().MagicBookLevel == 1)
		{
			if (useSpeed == -1)
			{
				useSpeed = Item.useTime / 17f;
			}
			Item.useTime = (int)(8 * useSpeed);
			Item.useAnimation = (int)(8 * useSpeed);
			Item.shootSpeed = 22f;
		}
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient<FeatherMagic>()
			.AddIngredient(ItemID.IceFeather, 3)
			.AddTile(TileID.CrystalBall)
			.Register();
	}
}
