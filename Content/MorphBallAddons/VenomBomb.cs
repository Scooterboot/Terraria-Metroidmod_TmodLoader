﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.MorphBallAddons
{
	public class VenomBomb : ModMBWeapon
	{
		public override string ItemTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombItem";

		public override string TileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombTile";

		public override string ProjectileTexture => $"{Mod.Name}/Assets/Textures/MBAddons/VenomBomb/VenomBombProjectile";

		public override bool AddOnlyAddonItem => false;

		public override bool CanGenerateOnChozoStatue() => Common.Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues || (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3);

		public override double GenerationChance() => 1;

		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Venom Morph Ball Bombs");
			// ModProjectile.DisplayName.SetDefault("Venom Morph Ball Bomb");
			/* Tooltip.SetDefault("-Right click to set off a bomb\n" +
			"Inflicts enemies with Acid Venom"); */
			ItemNameLiteral = true;
		}
		public override void SetItemDefaults(Item item)
		{
			item.damage = 85;
			item.value = Item.buyPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.Lime;
		}

		public override void Kill(int timeLeft, ref int dustType, ref int dustType2, ref float dustScale, ref float dustScale2)
		{
			dustType = 171;
			dustType2 = 205;
			dustScale = 2.5f;
			dustScale2 = 2.5f;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			target.AddBuff(BuffID.Venom, 600);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddRecipeGroup(MBAddonLoader.BombsRecipeGroupID, 1)
				.AddIngredient(ItemID.ChlorophyteBar, 5)
				.AddIngredient(ItemID.VialofVenom, 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}
	}
}
