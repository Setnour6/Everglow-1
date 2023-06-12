using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords
{
	//TODO:翻译：红梅落\n产生方向随机的二级刺锋\n兼具美观与锋芒
	public class BlossomThorn : StabbingSwordItem
	{
		public override void SetDefaults()
		{
			Item.damage = 9;
			Item.knockBack = 0.97f;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(0, 2, 38, 0);
			Item.shoot = ModContent.ProjectileType<BlossomThorn_Pro>();
			PowerfulStabProj = 1;
			base.SetDefaults();
		}
		public override void AddRecipes()
		{
			CreateRecipe().
				AddIngredient(ItemID.CopperBar, 17).
				AddTile(TileID.Anvils).
				Register();
			base.AddRecipes();
		}
    }
}