﻿using Terraria;
using Terraria.ID;
using MetroidModPorted.Common.Players;
using MetroidModPorted.ID;

namespace MetroidModPorted.Content.SuitAddons
{
	public class VariaSuitV2Addon : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Item";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Tile";

		public override string ArmorTextureHead => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Helmet_Head";

		public override string ArmorTextureTorso => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Breastplate_Body";

		public override string ArmorTextureLegs => $"{Mod.Name}/Assets/Textures/SuitAddons/VariaSuitV2/VariaSuitV2Greaves_Legs";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Varia Suit V2");
			Tooltip.SetDefault("+15 defense\n" +
				"+30 overheat capacity\n" +
				"15% decreased overheat use\n" +
				"10% decreased Missile Charge Combo cost\n" +
				"10% increased hunter damage\n" +
				"7% increased hunter critical strike chance\n" +
				"80% increased underwater breathing\n" +
				"10% increased movement speed\n" +
				"Immunity to fire blocks" + "\n" +
				"Immunity to chill and freeze effects");
			AddonSlot = SuitAddonSlotID.Suit_Varia;
			ItemNameLiteral = false;
		}
		public override void SetItemDefaults(Item item)
		{
			item.value = Terraria.Item.buyPrice(0, 2, 10, 0);
			item.rare = ItemRarityID.Orange;
		}
		public override void OnUpdateArmorSet(Player player)
		{
			player.statDefense += 15;
			player.nightVision = true;
			player.fireWalk = true;
			player.buffImmune[BuffID.Chilled] = true;
			player.buffImmune[BuffID.Frozen] = true;
			player.moveSpeed += 0.10f;
			MPlayer mp = player.GetModPlayer<MPlayer>();
			HunterDamagePlayer.ModPlayer(player).HunterDamageMult += 0.10f;
			HunterDamagePlayer.ModPlayer(player).HunterCrit += 7;
			mp.maxOverheat += 30;
			mp.overheatCost -= 0.15f;
			mp.missileCost -= 0.10f;
			mp.visorGlow = true;
			mp.breathMult = 1.8f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient(SuitAddonLoader.GetAddon<VariaSuitAddon>().Item, 1)
				.AddRecipeGroup(MetroidModPorted.T2HMBarRecipeGroupID, 45)
				.AddIngredient<Items.Miscellaneous.KraidTissue>(30)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
