﻿using System;
using System.Linq;
using MetroidMod.Common.Players;
using MetroidMod.Content.Buffs;
using MetroidMod.Content.NPCs.Mobs.Metroid;
using Microsoft.Xna.Framework;
using rail;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Common.GlobalNPCs
{
	public class MGlobalNPC : GlobalNPC
	{
		public override bool InstancePerEntity => true;

		internal bool checkedForFreezability = false;

		public bool stunned = false;
		public bool unstunned = true;
		public bool froze = false;
		public bool unfroze = true;
		private int oldDmg = 0;

		private int oldDir = 1;
		private int oldSprDir = 1;
		public float speedDecrease = 0.8f;

		public override void ResetEffects(NPC npc)
		{
			froze = false;
			stunned = false;
		}

		public override bool PreAI(NPC npc)
		{
			if (!checkedForFreezability)
			{
				bool isSkeletronArm = (npc.aiStyle == 12 || (npc.aiStyle >= 33 && npc.aiStyle <= 36));
				bool canFreeze = !npc.dontTakeDamage && !npc.boss && npc.lifeMax < 3000 && !isSkeletronArm && npc.type != NPCID.SnowmanGangsta && npc.type != NPCID.MisterStabby && npc.type != NPCID.SnowBalla && npc.type != NPCID.None3 && npc.aiStyle != 6 && !npc.buffImmune[44];

				if (!canFreeze)
				{
					npc.buffImmune[ModContent.BuffType<IceFreeze>()] = true;
					npc.buffImmune[ModContent.BuffType<InstantFreeze>()] = true;
					npc.buffImmune[ModContent.BuffType<ParalyzerStun>()] = true;
				}
				checkedForFreezability = true;
			}
			if (froze || stunned)
			{
				if (speedDecrease <= 0 && npc.type != ModContent.NPCType<LarvalMetroid>() && !MetroidMod.Instance.FrozenStandOnNPCs.Contains(npc.type))
				{
					npc.damage = 0;
					npc.frame.Y = 0;
					npc.velocity.X = 0;
					if (npc.noGravity)
					{
						npc.velocity.Y = 0;
					}
					npc.direction = 0;//oldDir;
					npc.spriteDirection = oldSprDir;
					unfroze = false;
					return false;
				}
				else
				{
					oldDir = npc.direction;
					oldSprDir = npc.spriteDirection;
				}
			}
			else
			{
				speedDecrease = 0.8f;
				if (!unfroze)
				{
					npc.TargetClosest(true);
					npc.direction = oldDir;
					npc.damage = oldDmg;
					unfroze = true;
				}
				else
				{
					oldDmg = npc.damage;
					oldDir = npc.direction;
					oldSprDir = npc.spriteDirection;
				}
			}
			return base.PreAI(npc);
		}
		public override void PostAI(NPC npc)
		{
			if (froze && speedDecrease > 0 && npc.type != ModContent.NPCType<LarvalMetroid>() && !MetroidMod.Instance.FrozenStandOnNPCs.Contains(npc.type))
			{
				npc.velocity.X -= npc.velocity.X * (1 - speedDecrease);
				if (npc.noGravity)
				{
					npc.velocity.Y -= npc.velocity.Y * (1 - speedDecrease);
				}
			}

			base.PostAI(npc);
		}
		public override void DrawEffects(NPC npc, ref Color drawColor)
		{
			if (froze)
			{
				drawColor = Lighting.GetColor((int)npc.position.X / 16, (int)npc.position.Y / 16, new Color(0, 144, 255));
			}
			if (stunned)
			{
				drawColor = Lighting.GetColor((int)npc.position.X / 16, (int)npc.position.Y / 16, new Color(255, 255, 0));
			}
		}

		public override bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot) => target.TryGetModPlayer(out Players.MPlayer mp) && !(mp.screwAttack && mp.somersault);

		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			MPlayer mp = player.GetModPlayer<MPlayer>();
			if (mp.PrimeHunter)
			{
				spawnRate = (int)(spawnRate * 0.75); //it's truly bizarre how this has to be an inverse ~Dr
			}
			//base.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			//Player player = Main.player[Player.FindClosest(npc.position, npc.width, npc.height)];
			//MPlayer mp = player.GetModPlayer<MPlayer>();

			/*bool flag = false;

			for (int i = 0; i < player.inventory.Length; i++)
			{
				if (player.inventory[i].type == ModContent.ItemType<Content.Items.Weapons.MissileLauncher>())
				{
					flag = true;
					break;
				}
			}

			if (flag)
			{
				if (npc.type != NPCID.MotherSlime && npc.type != NPCID.CorruptSlime && npc.type != NPCID.Slimer && npc.lifeMax > 1 && npc.damage > 0)
				{
					//if (Main.rand.Next(5) <= 1)
						//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MissilePickup"), 1 + Main.rand.Next(5));
					//else if (Main.rand.Next(10) == 0)
						//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MissilePickup"), 10 + Main.rand.Next(16));
				}
			}*/
			if (npc.type != NPCID.MotherSlime && npc.type != NPCID.CorruptSlime && npc.type != NPCID.Slimer && npc.lifeMax > 1 && npc.damage > 0)
			{
				npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.MissileCondition(), ModContent.ItemType<Content.Items.Miscellaneous.MissilePickup>(), 6, 1, 6, 1));
				if (npc.boss)
				{
					npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.EnergyCondition(), ModContent.ItemType<Content.Items.Miscellaneous.EnergyPickup>(), 3, (int)Math.Min(5f * (float)npc.lifeMax / 10f, 255f), (int)Math.Min(25f * (float)npc.lifeMax / 10f, 255f)));
				}
				else
				{
					npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.EnergyCondition(), ModContent.ItemType<Content.Items.Miscellaneous.EnergyPickup>(), 5, 5, 25));
				}
				npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.UniversalAmmoCondition(), ModContent.ItemType<Content.Items.Miscellaneous.UAPickup>(), 5, 5, 40));
					//.OnFailedRoll(ItemDropRule.ByCondition(new ItemDropRules.Conditions.UniversalAmmoCondition(), ModContent.ItemType<Content.Items.Miscellaneous.UAPickup>(), 5, 10, 10), true);
			}
			/*if (mp.reserveTanks > 0 && mp.reserveHearts < mp.reserveTanks && player.statLife >= player.statLifeMax2)
			{
				//if (npc.type != NPCID.MotherSlime && npc.type != NPCID.CorruptSlime && npc.type != NPCID.Slimer && npc.lifeMax > 1 && npc.damage > 0 && Main.rand.Next(12) == 0)
					//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Heart, 1);
			}*/
			if (npc.type != NPCID.MotherSlime && npc.type != NPCID.CorruptSlime && npc.type != NPCID.Slimer && npc.lifeMax > 1 && npc.damage > 0)
			{
				npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.ReserveHearts(), ItemID.Heart, 13));
			}


			if (npc.type == NPCID.WallofFlesh)
			{
				npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Accessories.HunterEmblem>()));
				//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HunterEmblem"), 1);
			}
			if (npc.type == NPCID.IceQueen)
			{
				//npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Content.Items.Miscellaneous.FrozenCore>())); //everyone wanted this rebalanced --Dr
				//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FrozenCore"), 1);
			}
			if (npc.type == NPCID.GoblinSummoner)
			{
				npcLoot.Add(ItemDropRule.Common(MBAddonLoader.GetAddon<Content.MorphBallAddons.ShadowflameBomb>().ItemType, 6));
				//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ShadowflameBombAddon"));
			}
			//if (npc.type == NPCID.Pumpking && Main.pumpkinMoon && NPC.waveNumber >= 7)
			if (npc.type == NPCID.Pumpking)
			{
				npcLoot.Add(ItemDropRule.ByCondition(new ItemDropRules.Conditions.PumpkingBombDrop(), MBAddonLoader.GetAddon<Content.MorphBallAddons.PumpkinBomb>().ItemType));
				/*int wave = NPC.waveNumber - 6;
				if (Main.expertMode)
				{
					wave += 5;
				}
				int chance = (int)MathHelper.Max(16 - wave, 1);
				if (Main.rand.NextBool(chance))
				{
					//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PumpkinBombAddon"));
				}*/
			}
			if (npc.type == NPCID.DD2Betsy)
			{
				npcLoot.Add(ItemDropRule.Common(MBAddonLoader.GetAddon<Content.MorphBallAddons.BetsyBomb>().ItemType));
				//Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BetsyBombAddon"));
			}
		}
	}
}
