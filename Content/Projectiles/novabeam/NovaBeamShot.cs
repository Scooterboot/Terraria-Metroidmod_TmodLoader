using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace MetroidMod.Content.Projectiles.novabeam
{
	public class NovaBeamShot : MProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nova Beam Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.scale = 2f;
			Projectile.penetrate = 8;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;

			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 75;
		Color color = MetroidMod.novColor;
		public override void AI()
		{


			if (shot.Contains("ice"))
			{
				dustType = 135;
				color = MetroidMod.iceColor;
			}
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
				Projectile.tileCollide = false;
			}
			if (shot.Contains("wide") || (shot.Contains("wave")))
			{
				mProjectile.WaveBehavior(Projectile, !Projectile.Name.Contains("Wave"));
			}
			if (shot.Contains("wide") && !shot.Contains("wave"))
			{
				mProjectile.amplitude = 8f * Projectile.scale;
			}
			if (shot.Contains("wave") && !shot.Contains("wide"))
			{
				mProjectile.amplitude = 10f * Projectile.scale;
			}
			if (shot.Contains("wave") && shot.Contains("wide"))
			{
				mProjectile.amplitude = 16f * Projectile.scale;
			}

			if (Projectile.numUpdates == 0)
			{
				int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}

			Vector2 velocity = Projectile.position - Projectile.oldPos[0];
			if (Vector2.Distance(Projectile.position, Projectile.position + velocity) < Vector2.Distance(Projectile.position, Projectile.position + Projectile.velocity))
			{
				velocity = Projectile.velocity;
			}
			Projectile.rotation = (float)Math.Atan2(velocity.Y, velocity.X) + MathHelper.PiOver2;
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
			mProjectile.PlasmaDrawTrail(Projectile, Main.player[Projectile.owner], Main.spriteBatch);
			return false;
		}
	}

	public class WaveNovaBeamShot : NovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Wave Nova Beam Shot";
		}
	}

	public class IceNovaBeamShot : NovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Nova Beam Shot";
		}
	}

	public class IceWaveNovaBeamShot : WaveNovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Nova Beam Shot";
		}
	}

	public class IceWaveWideNovaBeamShot : WaveNovaBeamShot
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Ice Wave Wide Nova Beam Shot";
		}
	}
}
