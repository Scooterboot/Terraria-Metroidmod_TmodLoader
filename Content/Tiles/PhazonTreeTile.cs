using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Metadata;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Tiles
{
	public class PhazonTreeTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileDungeon[Type] = true;
			TileID.Sets.SwaysInWindBasic[Type] = true;
			TileMaterials.SetForTileId(Type, TileMaterials._materialsByName["Plant"]); // Make this tile interact with golf balls in the same way other plants do

			DustType = 87;
			MinPick = 65;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(50, 160, 115));
		}
	}
}
