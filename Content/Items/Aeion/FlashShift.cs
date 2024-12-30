using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Aeion
{
	public class FlashShift : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = 100000;
			Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MPlayer>().hasFlashShift = true;
			player.GetModPlayer<MPlayer>().flashShiftColor = new Color(0f, 1f, 1f);
		}

	}
	public class FlashShift2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = 200000;
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<MPlayer>().hasFlashShift = true;
			player.GetModPlayer<MPlayer>().allowVerticalFlashShift = true;
			player.GetModPlayer<MPlayer>().flashShiftColor = new Color(1f, 0f, 0.3f);
		}

	}
}
