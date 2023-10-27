using Everglow.Yggdrasil.Common;
using Everglow.Yggdrasil.YggdrasilTown.Tiles;
using Everglow.Yggdrasil.YggdrasilTown.Tiles.LampWood;
using static Everglow.Yggdrasil.WorldGeneration.YggdrasilWorldGeneration;
namespace Everglow.Yggdrasil.YggdrasilTown.Items;

public class BoneAndPlatformCreate : ModItem
{
	public override void SetDefaults()
	{
		Item.width = 30;
		Item.height = 30;
		Item.useTurn = true;
		Item.useAnimation = 4;
		Item.useTime = 4;
		Item.autoReuse = false;
		Item.useStyle = ItemUseStyleID.Swing;
	}
	public override bool CanUseItem(Player player)
	{
		int x0 = (int)(Main.MouseWorld.X / 16);
		int y0 = (int)(Main.MouseWorld.Y / 16);
		if(player.direction == -1)
		{
			x0 -= 60;
		}
		PlaceFrameImportantTiles(x0, y0, 60, 1, ModContent.TileType<BoneAndPlatform_tile>(),0, 0);
		BoneAndPlatform_background scene = new BoneAndPlatform_background { position = new Vector2(x0, y0 - 14) * 16, Active = true, originTile = new Point(x0, y0), originType = ModContent.TileType<BoneAndPlatform_tile>(), direction = player.direction };
		Ins.VFXManager.Add(scene);
		BoneAndPlatform_foreground scene2 = new BoneAndPlatform_foreground { position = new Vector2(x0, y0 - 14) * 16, Active = true, originTile = new Point(x0, y0), originType = ModContent.TileType<BoneAndPlatform_tile>(), direction = player.direction };
		Ins.VFXManager.Add(scene2);
		return false;
	}
	public override bool? UseItem(Player player)
	{
		return base.UseItem(player);
	}
}
