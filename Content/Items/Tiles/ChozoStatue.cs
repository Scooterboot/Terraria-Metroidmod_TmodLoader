using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public abstract class ChozoStatueBuilder : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 30;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
		}

		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(ItemID.StoneBlock, 100)
				.AddTile(TileID.Anvils)
				.Register();
			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StoneBlock, 100);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();*/
		}
	}
	public class ChozoStatue : ChozoStatueBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Content.Tiles.ChozoStatue>();
		}
	}
	public class ChozoStatue2 : ChozoStatueBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Content.Tiles.ChozoStatue2>();
		}
	}
	public class ChozoStatue3 : ChozoStatueBuilder
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Item.createTile = ModContent.TileType<Content.Tiles.ChozoStatue3>();
		}
	}
}
