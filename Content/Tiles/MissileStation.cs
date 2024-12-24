using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
//using MetroidMod.Content.Items;

namespace MetroidMod.Content.Tiles
{
	public class MissileStation : ModTile
	{
		readonly float rightclickRange = 50.0f;

		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileSolidTop[Type] = true;
			TileID.Sets.NotReallySolid[Type] = true;
			TileID.Sets.DrawsWalls[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			TileObjectData.newTile.UsesCustomCanPlace = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			// name.SetDefault("Missile Station");
			AddMapEntry(new Color(144, 15, 54), name);
		}

		public override bool Slope(int i, int j) { return false; }

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
		{
			return true;
		}
		public override void ModifySmartInteractCoords(ref int width, ref int height, ref int frameWidth, ref int frameHeight, ref int extraY)
		{
			width = 2;
			height = 2;
		}
		public override void MouseOver(int i, int j)
		{
			if (Main.LocalPlayer.Distance(TileCenter(i, j)) < rightclickRange)
			{
				Main.LocalPlayer.noThrow = 2;
				Main.LocalPlayer.cursorItemIconEnabled = true;
				Main.LocalPlayer.cursorItemIconID = ModContent.ItemType<Items.Tiles.MissileStation>();
			}
		}

		public override bool RightClick(int i, int j)
		{
			if (Main.LocalPlayer.Distance(TileCenter(i, j)) < rightclickRange && !MUtils.AnyBossesActive())
			{
				Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.MissileRecharge>(), 2);
				return true;
			}
			return false;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			new EntitySource_TileBreak(i, j); //Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, ModContent.ItemType<Items.Tiles.MissileStation>());
		}

		static Vector2 TileCenter(int x, int y)
		{
			Vector2 center = new Vector2(x * 16, y * 16);

			if (Main.tile[x, y].TileFrameX == 0)
			{
				center.X += 16;
			}
			if (Main.tile[x, y].TileFrameY == 0)
			{
				center.Y += 16;
			}

			return center;
		}
	}
}
