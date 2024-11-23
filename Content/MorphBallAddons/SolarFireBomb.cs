﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class SolarFireBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarFireBomb/SolarFireBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarFireBomb/SolarFireBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/SolarFireBomb/SolarFireBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || NPC.downedAncientCultist;

		public override double GenerationChance() => 1;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Fire Morph Ball Bombs");
			// ModProjectile.DisplayName.SetDefault("Solar Fire Morph Ball Bomb");
			/* Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Burns foes with the fury of the sun"); */
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 200;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Red;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = DustID.OrangeTorch;
			dustType2 = DustID.SolarFlare;
			dustScale = 4f;
			dustScale2 = 2f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Daybreak, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.FragmentSolar, 5)
				.AddTile(TileID.LunarCraftingStation)
				.Register();
		}
	}
}
