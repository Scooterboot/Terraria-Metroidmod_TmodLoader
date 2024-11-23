﻿using System;
using MetroidMod.Common.Players;
using MetroidMod.Content.Items.Tiles;
using MetroidMod.Content.Tiles.ItemTile;
using MetroidMod.ID;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.SuitAddons
{
	public class EnergyTank : ModSuitAddon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/SuitAddons/EnergyTank/EnergyTankTile";

		public override bool AddOnlyAddonItem => false;

		//public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || NPC.downedBoss2;

		public override double GenerationChance() => 4;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Energy Tank");
			// Tooltip.SetDefault("Grants the user an extra tank of energy.");
			ItemNameLiteral = true;
			SacrificeTotal = 14;
			ItemID.Sets.ShimmerTransformToItem[ItemType] = ModContent.ItemType<UAExpansion>();
			AddonSlot = SuitAddonSlotID.Tanks_Energy;
			TileID.Sets.FriendlyFairyCanLureTo[TileType] = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.width = 16;
			item.height = 11;
			item.maxStack = 14;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.rare = ItemRarityID.Green;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Items.Miscellaneous.EnergyShard>(4)
				.AddIngredient<Items.Miscellaneous.ChoziteBar>(1)
				.AddRecipeGroup(MetroidMod.EvilBarRecipeGroupID, 1)
				.AddRecipeGroup(MetroidMod.EvilMaterialRecipeGroupID, 10)
				.AddTile(TileID.Anvils)
				.Register();
		}
		public override void OnUpdateArmorSet(Player player, int stack)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			mp.EnergyTanks = stack;
		}
	}
}
