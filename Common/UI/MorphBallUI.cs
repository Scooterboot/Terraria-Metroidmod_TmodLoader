﻿using System;
using MetroidMod.Common.Configs;
using MetroidMod.Common.GlobalItems;
using MetroidMod.Content.Items.Accessories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace MetroidMod.Common.UI
{
	public class MorphBallUI : UIState
	{
		public static bool Visible => Main.LocalPlayer.miscEquips[3].type == ModContent.ItemType<MorphBall>() && Main.EquipPage == 2;

		private MorphBallPanel morphBallPanel;

		public override void OnInitialize()
		{
			morphBallPanel = new MorphBallPanel();
			morphBallPanel.Initialize();

			Append(morphBallPanel);
		}
	}

	public class MorphBallPanel : DragableUIPanel
	{
		private Texture2D panelTexture;

		public MorphBallItemBox[] ballSlots;

		public UIText[] textSlots;

		public Rectangle DrawRectangle => new((int)Left.Pixels, (int)Top.Pixels, (int)Width.Pixels, (int)Height.Pixels);

		public Vector2[] itemBoxPositionValues = new Vector2[MetroidMod.ballSlotAmount]
		{
			new Vector2(26, 86),
			new Vector2(182, 86),
			new Vector2(184, 22),
			new Vector2(26, 22),
			new Vector2(104, 86)
		};

		public override void OnInitialize()
		{
			panelTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/MorphBall_Border", AssetRequestMode.ImmediateLoad).Value;

			SetPadding(0);
			Width.Pixels = panelTexture.Width;
			Height.Pixels = panelTexture.Height;
			Left.Pixels = Main.screenWidth - Width.Pixels - 180;
			Top.Pixels = 240;

			Append(new MorphBallFrame());
			Append(new MorphBallLines());

			ballSlots = new MorphBallItemBox[ 5/*MetroidMod.beamSlotAmount*/];
			/*
			textSlots = new UIText[MetroidMod.beamSlotAmount];
			for (int i = 0; i < MetroidMod.beamSlotAmount; ++i)
			{

				textSlots[i] = new UIText("0", Main.screenHeight / 1080f);
				textSlots[i].SetText(MBAddonLoader.GetAddonSlotName(i));
				textSlots[i].Top.Pixels = itemBoxPositionValues[i].Y + 44;
				textSlots[i].Left.Pixels = itemBoxPositionValues[i].X - 22;
				textSlots[i].IsWrapped = true;
				textSlots[i].Width.Pixels = 88;
				textSlots[i].Height.Pixels = 22;

				Append(textSlots[i]);
			}
			*/
			for (int i = 0; i < 5 /*MetroidMod.beamSlotAmount*/; ++i)
			{
				ballSlots[i] = new MorphBallItemBox();
				ballSlots[i].Top.Pixels = itemBoxPositionValues[i].Y;
				ballSlots[i].Left.Pixels = itemBoxPositionValues[i].X;
				ballSlots[i].morphBallSlotType = i;
				ballSlots[i].SetCondition();

				Append(ballSlots[i]);
			}
		}

		public override void Update(GameTime gameTime)
		{
			enabled = MConfigClient.Instance.MorphBall.enabled;
			if (!enabled && MConfigClient.Instance.MorphBall.auto)
			{
				Left.Pixels = Main.screenWidth - Width.Pixels - 180;
				Top.Pixels = 240;
				if (!Main.mapFullscreen && Main.mapStyle == 1)
					Top.Pixels += Math.Min(256, Main.screenHeight - Main.instance.RecommendedEquipmentAreaPushUp);
			}

			base.Update(gameTime);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(panelTexture, DrawRectangle, Color.White);
		}
	}

	public class MorphBallItemBox : UIPanel
	{
		private Texture2D itemBoxTexture;

		public Condition condition;

		public int morphBallSlotType;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public delegate bool Condition(Item item);
		public override void OnInitialize()
		{
			itemBoxTexture = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/ItemBox", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = itemBoxTexture.Width; Height.Pixels = itemBoxTexture.Height;
			OnLeftClick += ItemBoxClick;
		}

		public override void Update(GameTime gameTime)
		{
			// Ignore mouse input.
			if (base.IsMouseHovering)
				Main.LocalPlayer.mouseInterface = true;
		}

		public void SetCondition()
		{
			condition = delegate (Item addonItem) {
				if (addonItem.ModItem != null)// && addonItem.ModItem.Mod == MetroidMod.Instance)
				{
					//MGlobalItem mItem = addonItem.GetGlobalItem<MGlobalItem>();
					if (addonItem.GetGlobalItem<MGlobalItem>().AddonType != AddonType.MorphBall || !MBAddonLoader.TryGetAddon(addonItem, out ModMBAddon mbAddon)) { return false; }
					return (addonItem.type <= 0 || mbAddon.AddonSlot == morphBallSlotType);
				}
				return (addonItem.type <= 0);// || (addonItem.ModItem != null && addonItem.ModItem.Mod == MetroidMod.Instance));
			};
		}

		// Clicking functionality.
		private void ItemBoxClick(UIMouseEvent evt, UIElement e)
		{
			// No failsafe. Should maybe be implemented?
			if (Main.LocalPlayer.controlUseItem || Main.LocalPlayer.controlUseTile) { return; }

			MorphBall morphBallTarget = Main.LocalPlayer.miscEquips[3].ModItem as MorphBall;

			if (morphBallTarget.ballMods[morphBallSlotType] != null && !morphBallTarget.ballMods[morphBallSlotType].IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					SoundEngine.PlaySound(SoundID.Grab);
					Main.mouseItem = morphBallTarget.ballMods[morphBallSlotType].Clone();

					morphBallTarget.ballMods[morphBallSlotType].TurnToAir();
				}
				else if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);

					Item tempBoxItem = morphBallTarget.ballMods[morphBallSlotType].Clone();
					Item tempMouseItem = Main.mouseItem.Clone();

					morphBallTarget.ballMods[morphBallSlotType] = tempMouseItem;
					Main.mouseItem = tempBoxItem;
				}
			}
			else if (!Main.mouseItem.IsAir)
			{
				if (condition == null || (condition != null && condition(Main.mouseItem)))
				{
					SoundEngine.PlaySound(SoundID.Grab);
					morphBallTarget.ballMods[morphBallSlotType] = Main.mouseItem.Clone();
					Main.mouseItem.TurnToAir();
				}
			}
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//base.DrawSelf(spriteBatch);
			Item target = Main.LocalPlayer.miscEquips[3];
			if (target == null || target.type != ModContent.ItemType<MorphBall>()) { return; }
			MorphBall morphBallTarget = (MorphBall)target.ModItem;

			spriteBatch.Draw(itemBoxTexture, DrawRectangle, Color.White);

			// Item drawing.
			if (morphBallTarget == null || morphBallTarget.ballMods == null || morphBallTarget.ballMods[morphBallSlotType].IsAir) { return; }

			Color itemColor = morphBallTarget.ballMods[morphBallSlotType].GetAlpha(Color.White);
			Texture2D itemTexture = Terraria.GameContent.TextureAssets.Item[morphBallTarget.ballMods[morphBallSlotType].type].Value;
			CalculatedStyle innerDimensions = GetDimensions();

			if (IsMouseHovering)
			{
				Main.hoverItemName = morphBallTarget.ballMods[morphBallSlotType].Name;
				Main.HoverItem = morphBallTarget.ballMods[morphBallSlotType].Clone();
			}

			var frame = Main.itemAnimations[morphBallTarget.ballMods[morphBallSlotType].type] != null
						? Main.itemAnimations[morphBallTarget.ballMods[morphBallSlotType].type].GetFrame(itemTexture)
						: itemTexture.Frame(1, 1, 0, 0);

			float drawScale = 1f;
			if ((float)frame.Width > innerDimensions.Width || (float)frame.Height > innerDimensions.Width)
			{
				if (frame.Width > frame.Height)
					drawScale = innerDimensions.Width / (float)frame.Width;
				else
					drawScale = innerDimensions.Width / (float)frame.Height;
			}

			var unreflectedScale = drawScale;
			var tmpcolor = Color.White;

			ItemSlot.GetItemLight(ref tmpcolor, ref drawScale, morphBallTarget.ballMods[morphBallSlotType].type);

			Vector2 drawPosition = new Vector2(innerDimensions.X, innerDimensions.Y);

			drawPosition.X += (float)innerDimensions.Width * 1f / 2f - (float)frame.Width * drawScale / 2f;
			drawPosition.Y += (float)innerDimensions.Height * 1f / 2f - (float)frame.Height * drawScale / 2f;

			spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
				Vector2.Zero, drawScale, SpriteEffects.None, 0f);

			if (morphBallTarget.ballMods[morphBallSlotType].color != default(Color))
			{
				spriteBatch.Draw(itemTexture, drawPosition, new Rectangle?(frame), itemColor, 0f,
					Vector2.Zero, drawScale, SpriteEffects.None, 0f);
			}
		}
	}

	/*
	 * The classes in the following section do not have any functionality besides visual aesthetics.
	 */
	public class MorphBallFrame : UIPanel
	{
		private Texture2D morphBallFrame;

		public Rectangle DrawRectangle => new((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			morphBallFrame = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/MorphBall_Frame", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = morphBallFrame.Width;
			Height.Pixels = morphBallFrame.Height;

			// Hardcoded position values.
			Top.Pixels = 34;
			Left.Pixels = 108;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(morphBallFrame, DrawRectangle, Color.White);
		}
	}
	public class MorphBallLines : UIPanel
	{
		private Texture2D morphBallLines;

		public Rectangle DrawRectangle => new Rectangle((int)(Parent.Left.Pixels + Left.Pixels), (int)(Parent.Top.Pixels + Top.Pixels), (int)Width.Pixels, (int)Height.Pixels);

		public override void OnInitialize()
		{
			morphBallLines = ModContent.Request<Texture2D>("MetroidMod/Assets/Textures/UI/MorphBall_Lines", AssetRequestMode.ImmediateLoad).Value;

			Width.Pixels = morphBallLines.Width;
			Height.Pixels = morphBallLines.Height;

			// Hardcoded position values.
			Top.Pixels = 0;
			Left.Pixels = 0;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(morphBallLines, DrawRectangle, Color.White);
		}
	}
}
