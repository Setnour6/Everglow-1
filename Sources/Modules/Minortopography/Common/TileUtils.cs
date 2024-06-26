namespace Everglow.Minortopography.Common;

public class TileUtils
{
	/// <summary>
	/// 以[x,y]为左上顶点放置大件连续物块,此类物块必须是18x18(不算分隔线就16x16)一帧的
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static void PlaceFrameImportantTiles(int x, int y, int width, int height, int type, int xStartAt = 0, int yStartAt = 0)
	{
		if (x > Main.maxTilesX - width || x < 0 || y > Main.maxTilesY - height || y < 0)
			return;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Tile tile = Main.tile[x + i, y + j];
				tile.TileType = (ushort)type;
				tile.TileFrameX = (short)(i * 18 + xStartAt);
				tile.TileFrameY = (short)(j * 18 + yStartAt);
				tile.HasTile = true;
			}
		}
	}
}