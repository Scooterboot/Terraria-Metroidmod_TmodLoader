using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Miscellaneous
{
	[LegacyName("GravityGel")]
	public class GravityFlare : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gravity Flare");
			// Tooltip.SetDefault("'Totally breaking Newton's laws.'");
			ItemID.Sets.ItemNoGravity[Type] = true;
			Main.RegisterItemAnimation(Type, new DrawAnimationVertical(10, 2));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.maxStack = 9999;
			Item.width = 16;
			Item.height = 16;
			Item.value = 10000;
			Item.rare = ItemRarityID.Pink;

		}

	}
}
