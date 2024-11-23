using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.plasmabeamred
{
	public class PlasmaBeamRedShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Plasma Beam Red Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;

			mProjectile.wavesPerSecond = 1f;
			mProjectile.delay = 3;
		}

		int dustType = 6;
		Color color = MetroidMod.plaRedColor;
		public override void AI()
		{


			if (shot.Contains("ice"))
			{
				dustType = 135;
				color = MetroidMod.iceColor;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);
			if (Projectile.numUpdates == 0)
			{
				Projectile.frame++;
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}

			if (shot.Contains("wave"))
			{
				Projectile.Name += "Wave";
				Projectile.tileCollide = false;
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (!shot.Contains("spazer") && shot.Contains("wave"))
			{
				mProjectile.amplitude = 8f * Projectile.scale;
			}
			if (shot.Contains("spazer") && !shot.Contains("wave"))
			{
				mProjectile.amplitude = 7.5f * Projectile.scale;
				mProjectile.wavesPerSecond = 2f;
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (shot.Contains("spazer") && shot.Contains("wave"))
			{
				mProjectile.amplitude = 12f * Projectile.scale;
				mProjectile.wavesPerSecond = 1f;
			}

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}

		public override void OnKill(int timeLeft)
		{
			mProjectile.DustyDeath(Projectile, dustType);
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return new Color((int)lightColor.R, (int)lightColor.G, (int)lightColor.B, 25);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch, 4);
			return false;
		}
	}

	public class IcePlasmaBeamRedShot : PlasmaBeamRedShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/wavebeam/IceWaveBeamV2Shot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Plasma Beam Red Shot";
		}
	}
}
