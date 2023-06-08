using Everglow.Myth.MagicWeaponsReplace.GlobalItems;
using Everglow.Myth.MagicWeaponsReplace.Projectiles.BookofSkulls;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Myth.MiscItems.Weapons;

public class FireFeatherMagic : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.damage = 36;
		Item.DamageType = DamageClass.Magic;
		Item.width = 28;
		Item.height = 30;
		Item.useTime = 17;
		Item.useAnimation = 17;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.knockBack = 8;
		Item.value = 2354;
		Item.rare = ItemRarityID.LightRed;
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.Magic.FireFeather>();
		Item.shootSpeed = 8;
		Item.crit = 16;
		Item.mana = 12;
	}
	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if(player.GetModPlayer<MagicBookPlayer>().MagicBookLevel == 1)
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
		}
	}
	public override void AddRecipes()
	{
		CreateRecipe()
			.AddIngredient<FeatherMagic>()
			.AddIngredient(ItemID.FireFeather, 3)
			.AddTile(TileID.CrystalBall)
			.Register();
	}
}
