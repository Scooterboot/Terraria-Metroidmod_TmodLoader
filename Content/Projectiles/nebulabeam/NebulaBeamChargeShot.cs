using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace MetroidMod.Content.Projectiles.nebulabeam
{
	public class NebulaBeamChargeShot : MProjectile
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/wavebeam/WaveBeamV2ChargeShot";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nebula Beam Charge Shot");
			Main.projFrames[Projectile.type] = 2;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.scale = 2f;
			Projectile.tileCollide = false;

			mProjectile.amplitude = 10f * Projectile.scale;
			mProjectile.wavesPerSecond = 2f;
			mProjectile.delay = 4;
		}

		int dustType = 255;
		Color color = MetroidMod.waveColor2;
		float scale = 1f;
		public override void AI()
		{


			if (shot.Contains("stardust"))
			{
				dustType = 88;
				color = MetroidMod.iceColor;
				scale = 0.5f;
			}
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.PiOver2;
			Lighting.AddLight(Projectile.Center, color.R / 255f, color.G / 255f, color.B / 255f);

			if (Projectile.numUpdates == 0)
			{
				if (Main.projFrames[Projectile.type] > 1)
				{
					Projectile.frame++;
				}
			}
			if (Projectile.frame > 1)
			{
				Projectile.frame = 0;
			}

			mProjectile.WaveBehavior(Projectile);
			mProjectile.HomingBehavior(Projectile);

			int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, dustType, 0, 0, 100, default(Color), Projectile.scale * scale);
			Main.dust[dust].noGravity = true;
			if (shot.Contains("stardust"))
			{
				dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 87, 0, 0, 100, default(Color), Projectile.scale);
				Main.dust[dust].noGravity = true;
			}
		}
		public override void OnKill(int timeLeft)
		{
			mProjectile.Diffuse(Projectile, dustType);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			mProjectile.DrawCentered(Projectile, Main.spriteBatch);
			return false;
		}
	}

	public class StardustNebulaBeamChargeShot : NebulaBeamChargeShot
	{
		public override string Texture => $"{Mod.Name}/Content/Projectiles/wavebeam/IceWaveBeamV2ChargeShot";
		public override void SetDefaults()
		{
			base.SetDefaults();
			Projectile.Name = "Stardust Nebula Beam Charge Shot";
		}
	}
}
