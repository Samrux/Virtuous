using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using Virtuous.Dusts;

namespace Virtuous.Projectiles
{
    public class RainbowArrow : ModProjectile
    {
        private const int Lifespan = 6 * 60; // Total duration of the projetile
        private const int RainDelay = 50; // Time it takes for a special arrow effect to activate

        public int MaxArrowColor = 11;

        public enum ArrowMode
        {
            Normal, // Behave like regular arrows. The bow will shoot them faster
            White, // Are shot upward, don't affect enemies, and cause rain shots to spawn after a short time
            Rain,  // Spawned by white shots, fall from above and have no knockback
        }




        public ArrowMode Mode // Stored as ai[0]
        {
            get { return (ArrowMode)(int)projectile.ai[0]; }
            set { projectile.ai[0] = (int)value; }
        }

        private Color Color // Stored as ai[1]
        {
            get
            {
                if (Mode == ArrowMode.White) return new Color(255, 255, 255, 0);

                int alpha = 120;
                switch ((int)projectile.ai[1])
                {
                    case  0: return new Color(000, 050, 255, alpha); // Blue
                    case  1: return new Color(000, 125, 255, alpha); // Sky
                    case  2: return new Color(000, 255, 255, alpha); // Cyan
                    case  3: return new Color(000, 255, 110, alpha); // Aquamarine
                    case  4: return new Color(000, 255, 000, alpha); // Green
                    case  5: return new Color(110, 255, 000, alpha); // Lime
                    case  6: return new Color(240, 240, 000, alpha); // Yellow
                    case  7: return new Color(255, 100, 000, alpha); // Orange
                    case  8: return new Color(255, 000, 000, alpha); // Red
                    case  9: return new Color(255, 000, 150, alpha); // Fuchsia
                    case 10: return new Color(150, 000, 255, alpha); // Purple
                    case 11: return new Color(090, 020, 255, alpha); // Violet
                    default: return Color.Black;
                }
            }
        }

        public int ColorId { set { projectile.ai[1] = value; } }




        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rainbow Arrow");
            DisplayName.AddTranslation(GameCulture.Spanish, "Flecha Iris");
        }


        public override void SetDefaults()
        {
            projectile.width = 15;
            projectile.height = 15;
            projectile.friendly = true;
            projectile.arrow = true;
            projectile.alpha = 0;
            projectile.timeLeft = Lifespan;
            projectile.ranged = true;
        }

        private void SpawnRainbowDust(bool Burst=false)
        {
            int type=0;
            float scale=0;
            Vector2 velocity = Vector2.Zero;

            switch(Mode)
            {
                case ArrowMode.Normal:
                    type = 16; //Cloud
                    if(Burst) scale = 1.2f;
                    else scale = 0.8f;
                    break;

                case ArrowMode.White:
                    type = 16; //Cloud
                    scale = 1.3f;
                    break;

                case ArrowMode.Rain:
                    type = mod.DustType<RainbowDust>();
                    scale = Burst ? 1.5f : 1.2f; //Bigger size if it's a burst of dust
                    if (!Burst) velocity = new Vector2(Main.rand.NextFloat(-0.25f, +0.25f), projectile.velocity.Length()/2); //X is random, Y follows the arrow
                    break;
            }

            int dustAmount = Burst ? 10 : 1;
            for (int i = 0; i < dustAmount; i++)
            {
                Dust newDust = Dust.NewDustDirect(projectile.Center, 0, 0, type, 0f, 0f, /*Alpha*/0, Color, scale);
                if (velocity != Vector2.Zero) newDust.velocity = velocity; //If a special velocity was set, apply it
                if (Mode == ArrowMode.Rain) newDust.position.Y -= projectile.height/2; //Trails behind
            }
        }


        public override void AI()
        {
            //On projectile spawn
            if(projectile.timeLeft == Lifespan) 
            {
                switch(Mode)
                {
                    case ArrowMode.White:
                        projectile.scale *= 1.4f;
                        projectile.friendly = false; //Doesn't affect enemies
                        break;

                    case ArrowMode.Rain:
                        SpawnRainbowDust(true);
                        break;
                }

                projectile.rotation = projectile.velocity.ToRotation() + 90.ToRadians(); //Rotates the sprite according to its velocity. Adds 90 degrees because of the sprite
            }

            //On every tick
            SpawnRainbowDust();
            float l = 0.7f; //Light intensity
            Lighting.AddLight(projectile.Center, l*Color.R/255, l*Color.G/255, l*Color.B/255);

            //Special arrow effect
            if(Mode == ArrowMode.White && projectile.timeLeft == Lifespan - RainDelay && projectile.owner == Main.myPlayer) 
            {
                const int ArrowAmount = 30;
                float arrowSpacing = projectile.width * 6f;
                int nextColor = Main.rand.Next(12); //Starts the rainbow of arrows at a random color
                for(int i = 1; i <= ArrowAmount; i++)
                {
                    Vector2 position = projectile.Center;
                    position.X += -ArrowAmount*arrowSpacing/2 - arrowSpacing/2 + i*arrowSpacing; //Makes a row of arrows spaced evenly according to the arrow amount and the defined space between them
                    position.Y += 120; //Distance below the original arrow
                    if (!Main.tile[(int)position.X/16,(int)position.Y/16].active() || !Main.tile[(int)position.X/16,(int)position.Y/16].nactive()) //Only spawns if it's open space
                    {
                        var proj = Projectile.NewProjectileDirect(
                            position, new Vector2(0, projectile.velocity.Length()), mod.ProjectileType<RainbowArrow>(),
                            projectile.damage, 0, projectile.owner);
                        var arrow = proj.modProjectile as RainbowArrow;
                        arrow.Mode = ArrowMode.Rain;
                        arrow.ColorId = nextColor;
                    }
                    //Cycles through colors as the rainbow progresses
                    if (nextColor < MaxArrowColor) nextColor++;
                    else nextColor = 0;
                }
                projectile.Kill();
            }

        }

        public override void Kill(int timeLeft)
        {
            SpawnRainbowDust(true);
        }

        public override Color? GetAlpha(Color newColor) //Sets the custom color
        {
            return Color;
        }

    }
}