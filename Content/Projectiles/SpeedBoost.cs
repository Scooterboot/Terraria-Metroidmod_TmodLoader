using System;
using MetroidMod.Common.Configs;
using MetroidMod.Common.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using tModPorter;

namespace MetroidMod.Content.Projectiles
{
	public class SpeedBoost : ModProjectile
	{
		private int SpeedSound = 0;
		public ActiveSound activeSound;
		public SoundEffectInstance soundInstance;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Speed Booster");
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.aiStyle = 0;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;//Projectile.melee = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 9000;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 7;
			Projectile.alpha = 255;
		}
		public override void AI()
		{

			Player P = Main.player[Projectile.owner];
			Projectile.position.X = P.Center.X - Projectile.width / 2;
			Projectile.position.Y = P.Center.Y - Projectile.height / 2;

			if (!MConfigItems.Instance.muteSpeedBooster)
			{
				SpeedSound++;
				if (SpeedSound == 4)
				{
					if (SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.SpeedBoosterStartup, P.position), out activeSound))
					{
						soundInstance = activeSound.Sound;
					}

				}
				if (soundInstance != null && SpeedSound == 82)
				{
					soundInstance.Stop();
					if (SoundEngine.TryGetActiveSound(SoundEngine.PlaySound(Sounds.Items.Weapons.SpeedBoosterLoop, P.position), out activeSound))
					{
						soundInstance = activeSound.Sound;

						SpeedSound = 68;
					}
				}
			}
			MPlayer mp = P.GetModPlayer<MPlayer>();
			if (mp.ballstate || !mp.speedBoosting || mp.SMoveEffect > 0)
			{
				if (soundInstance != null)
				{
					soundInstance.Stop(true);
				}
				Projectile.Kill();
			}
			foreach (Projectile Pr in Main.projectile) if (Pr != null)
				{
					if (Pr.active && (Pr.type == ModContent.ProjectileType<ShineSpark>() || Pr.type == ModContent.ProjectileType<SpeedBall>()))
					{
						if (soundInstance != null)
						{
							soundInstance.Stop(true);
						}
						Projectile.Kill();
						return;
					}
				}
			Lighting.AddLight((int)((float)Projectile.Center.X / 16f), (int)((float)(Projectile.Center.Y) / 16f), 0, 0.75f, 1f);
			float rotation = (float)Math.Atan2(P.position.Y - P.shadowPos[0].Y, P.position.X - P.shadowPos[0].X);
			float rotation1 = rotation + ((float)Math.PI / 2);
			float rotation2 = rotation - ((float)Math.PI / 2);
			Vector2 vect = P.Center - new Vector2(4, 4) + rotation.ToRotationVector2() * 24f;
			Vector2 vel = P.position - mp.oldPosition;
			Vector2 vel1 = rotation1.ToRotationVector2() * Math.Abs(P.velocity.X);
			Vector2 vel2 = rotation2.ToRotationVector2() * Math.Abs(P.velocity.X);
			int num20 = Dust.NewDust(vect - vel1, 1, 1, 67, vel1.X + vel.X, vel1.Y + vel.Y, 100, default(Color), 2f);
			Main.dust[num20].noGravity = true;
			int num21 = Dust.NewDust(vect - vel2, 1, 1, 67, vel2.X + vel.X, vel2.Y + vel.Y, 100, default(Color), 2f);
			Main.dust[num21].noGravity = true;

			if (activeSound != null)
			{
				activeSound.Position = P.Center;
			}
		}
		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.FinalDamage.Flat = (int)(target.damage * 1.5f);
		}
	}
}
