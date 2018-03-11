﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;

namespace Virtuous
{
    /*
     * This class contains many tools, mostly shorthands for Terraria utils and some constant keywords.
     * They help me feel like my code is clean, understandable and more personal.
     */

    public static class Tools
    {
        //Constants

        public const float FullCircle = 2 * (float)Math.PI;
        public const float RevolutionPerSecond = FullCircle / 60;
        public const float GoldenRatio = 1.61803398875f;


        //Game objects

        public static Vector2 SpriteCenter(this NPC npc) => npc.Center + new Vector2(0, npc.gfxOffY);
        public static Vector2 SpriteCenter(this Player player) => player.Center + new Vector2(0, player.gfxOffY);
        public static Vector2 MountedSpriteCenter(this Player player) => player.MountedCenter + new Vector2(0, player.gfxOffY);

        public static string InternalName(this Item item) => Lang.GetItemName(item.type).Key.Split('.').Last();
        public static string InternalName(this NPC npc) => Lang.GetNPCName(npc.type).Key.Split('.').Last();
        public static string InternalName(this Projectile proj) => Lang.GetProjectileName(proj.type).Key.Split('.').Last();

        public static bool InternalNameContains(this Item item, params string[] values) => item.InternalName().Contains_IgnoreCase(values);
        public static bool InternalNameContains(this NPC npc, params string[] values) => npc.InternalName().Contains_IgnoreCase(values);
        public static bool InternalNameContains(this Projectile proj, params string[] values) => proj.InternalName().Contains_IgnoreCase(values);

        public static void ResizeProjectile(int projIndex, int newWidth, int newHeight, bool changeDrawPos = false) //Changes the size of the hitbox while keeping its center
        {
            Projectile projectile = Main.projectile[projIndex];

            if (changeDrawPos)
            {
                projectile.modProjectile.drawOffsetX += (newWidth - projectile.width) / 2;
                projectile.modProjectile.drawOriginOffsetY += (newHeight - projectile.height) / 2;
            }
            
            projectile.position += new Vector2(projectile.width / 2, projectile.height / 2);
            projectile.width = newWidth;
            projectile.height = newHeight;
            projectile.position -= new Vector2(projectile.width / 2, projectile.height / 2);
        }

        public static void HandleAltUseAnimation(Player player) //A trick to stop the bugged 1-tick delay between consecutive right-click uses of a weapon
        {
            if (player.altFunctionUse == 2)
            {
                if (PlayerInput.Triggers.JustReleased.MouseRight) //Stops the animation manually
                {
                    player.itemAnimation = 0;
                }
                else if (player.itemAnimation == 1) //Doesn't let the hand return to resting position
                {
                    player.altFunctionUse = 1;
                    player.controlUseItem = true;
                }
            }
        }


        //Vectors

        public static Vector2 Normalized(this Vector2 vector) //Shorthand for SafeNormalize
        {
            return vector.SafeNormalize(Vector2.UnitX);
        }

        public static Vector2 OfLength(this Vector2 vector, float length) //Returns a vector of the same direction but with the provided absolute length
        {
            return vector.Normalized() * length;
        }

        public static Vector2 Perpendicular(this Vector2 oldVector, float? length = null, bool clockwise = true) //Returns a vector perpendicular to the original
        {
            Vector2 vector = new Vector2(oldVector.Y, -oldVector.X);
            if (!clockwise) vector *= new Vector2(-1, -1);
            if (length != null) vector = vector.OfLength((float)length);
            return vector;
        }


        //String

        public static string If(this string text, bool condition) => condition ? text : ""; //Conditional strings to help with complex text concatenation
        public static string Unless(this string text, bool condition) => text.If(!condition);

        public static bool Contains(this string text, params string[] values) //Contains any of the values provided as arguments
        {
            for (int i = 0; i < values.Length; i++) if (text.Contains(values[i])) return true;
            return false;
        }
        public static bool Contains_IgnoreCase(this string text, params string[] values)
        {
            for (int i = 0; i < values.Length; i++) values[i] = values[i].ToUpper();
            return text.ToUpper().Contains(values);
        }

        //Random

        public static int RandomInt(int min, int inclusiveMax)
        {
            return Main.rand.Next(min, inclusiveMax + 1);
        }
        public static int RandomInt(int max)
        {
            return Main.rand.Next(max);
        }

        public static float RandomFloat(float min, float max)
        {
            return (float)Main.rand.NextDouble() * (max - min) + min;
        }
        public static float RandomFloat(float max = 1)
        {
            return (float)Main.rand.NextDouble() * max;
        }

        public static Vector2 RandomDirection(float length = 1)
        {
            return Vector2.UnitY.RotatedByRandom(FullCircle).OfLength(length);
        }

        public static bool OneIn(int integer) //Returns true with a 1/<integer> chance
        {
            return RandomInt(integer) == 0;
        }

        public static bool CoinFlip() //Returns true half the time
        {
            return Main.rand.NextDouble() < 0.5;
        }


        //Circles

        public static float ToRadians(this int deg) //Returns the given degrees in radians
        {
            return (float)deg * (float)Math.PI / 180f;
        }
        public static float ToRadians(this float deg)
        {
            return deg * (float)Math.PI / 180f;
        }

        public static float ToDegrees(this float rad) //Returns the given radians in degrees
        {
            return rad * 180f / (float)Math.PI;
        }
    }
}
