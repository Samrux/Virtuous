﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Virtuous.Projectiles;

namespace Virtuous.Items
{
    public class Fireplower : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireplower");
            Tooltip.SetDefault("50% chance to not consume gel\nLeaves burning gel on the ground");

            DisplayName.AddTranslation(GameCulture.Spanish, "Arrasallamas");
            Tooltip.AddTranslation(GameCulture.Spanish, "50% probabilidad de no consumir gel\nDeja restos de gel ardiente");

            DisplayName.AddTranslation(GameCulture.Russian, "Огнеплюв");
            Tooltip.AddTranslation(GameCulture.Russian, "50% не потратить гель\nОставляет горящий гель на земле");

            DisplayName.AddTranslation(GameCulture.Chinese, "烈焰之星");
            Tooltip.AddTranslation(GameCulture.Chinese, "50%几率不消耗凝胶\n在地上留下徐徐花瓣");
        }


        public override void SetDefaults()
        {
            item.width = 50;
            item.height = 22;
            item.useStyle = 5;
            item.autoReuse = true;
            item.useAnimation = 30;
            item.useTime = item.useAnimation / 2;
            item.shoot = ProjectileID.Flames;
            item.useAmmo = AmmoID.Gel;
            item.UseSound = SoundID.Item34;
            item.damage = 120;
            item.knockBack = 2f;
            item.shootSpeed = 7f;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 15, 0, 0);
            item.rare = 8;
            item.ranged = true;
        }


        public override bool ConsumeAmmo(Player player) => Main.rand.OneIn(2);

        //public override Vector2? HoldoutOffset() => new Vector2(10, 0);


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            const float shootArc = Tools.FullCircle / 10f;
            const int fireAmount = 9;

            for (int i = 0; i < fireAmount; i++)
            {
                // Creates the arc by going back and forth - this way it will not look lopsided in low graphics settings
                var velocity = new Vector2(speedX, speedY).RotatedBy(shootArc/2f * i/fireAmount * (i % 2 == 0 ? 1 : -1));
                Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
            }

            int gelAmount = Main.rand.Next(1, 4);
            var gelPosition = position + new Vector2(speedX, speedY).OfLength(item.width); // Tip of the nozzle
            if (!Collision.CanHit(player.Center, 0, 0, gelPosition, 0, 0)) gelPosition = position; // So that it doesn't go through walls

            for (int i = 0; i < gelAmount; i++)
            {
                var gelVelocity = new Vector2(speedX, speedY)
                    .RotatedBy(shootArc * Main.rand.NextFloat(-1, +1)) // Random inside the fire arc
                    * Main.rand.NextFloat(0, 2); // Random power

                Projectile.NewProjectile(gelPosition, gelVelocity, mod.ProjectileType<ProjGelFire>(), damage, 0, player.whoAmI);
            }

            if (Main.rand.OneIn(1000)) Main.NewText("Burn, baby! Burn!", Color.Orange);
            return false;
        }


        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Flamethrower);
            recipe.AddIngredient(ItemID.FireFeather);
            recipe.AddIngredient(ItemID.LunarTabletFragment, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}