﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Virtuous.Projectiles;

namespace Virtuous.Items
{
    public class EtherSlit : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ether Slit");
            Tooltip.SetDefault(
                "Right Click for a barrage attack\nBarrage swords have higher speed, critical chance and stick duration");

            DisplayName.AddTranslation(GameCulture.Spanish, "Éter");
            Tooltip.AddTranslation(GameCulture.Spanish,
                "Haz Click Derecho para un ataque concentrado de mayor potencia");

            DisplayName.AddTranslation(GameCulture.Russian, "Небесная Скважина");
            Tooltip.AddTranslation(GameCulture.Russian,
                "ПКМ для стихийной атаки\nУ стихийных клинков повышены скорость, шанс критического удара и время застревания");

            DisplayName.AddTranslation(GameCulture.Chinese, "苍穹裂痕");
            Tooltip.AddTranslation(GameCulture.Chinese,
                "右键释放空灵剑诀\n空灵剑诀拥有更高的攻速,暴击率及持续时间");
        }


        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 28;
            item.shoot = mod.ProjectileType<ProjSummonedSword>();
            item.UseSound = mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/Slash");
            item.damage = 250;
            item.knockBack = 7f;
            item.mana = 7;
            item.magic = true;
            item.noMelee = true;
            item.autoReuse = true;
            item.noUseGraphic = true;
            item.rare = 10;
            item.value = Item.sellPrice(0, 50, 0, 0);

            // Replaced in SetUseStats
            item.useStyle = 5;
            item.useTime = 10;
            item.useAnimation = item.useTime;
            item.shootSpeed = 16f;
            item.crit = 15;
        }
        

        public override bool AltFunctionUse(Player player) => true;


        private void SetUseStats(Player player)
        {
            // Left Click
            if (player.altFunctionUse != 2)
            {
                item.useStyle = 5;
                item.useTime = 10;
                item.useAnimation = item.useTime;
                item.shootSpeed = 16f;
                item.crit = 15;
            }
            // Right Click
            else
            {
                item.useStyle = 3;
                item.useTime = 7;
                item.useAnimation = item.useTime;
                item.shootSpeed = 20f;
                item.crit = 30;
            }
        }
        

        public override bool CanUseItem(Player player)
        {
            SetUseStats(player); // Sets stats before use

            return base.CanUseItem(player);
        }


        public override void GetWeaponDamage(Player player, ref int damage)
        {
            SetUseStats(player); // Always displays left-click stats
            Tools.HandleAltUseAnimation(player);

            base.GetWeaponDamage(player, ref damage);
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < 20; i++) // Makes 20 attempts at finding a projectile position that the player can reach
            {
                position = player.MountedCenter + Main.rand.NextVector2(20, 60);
                if (Collision.CanHit(player.MountedCenter, 0, 0, position, 0, 0)) break;
            }

            // Right click: Horizontal, in the direction the player is facing
            // Left click: Middlepoint between straight from the player to the cursor and straight from the sword to the cursor
            Vector2 velocity = player.altFunctionUse == 2
                ? new Vector2(player.direction * item.shootSpeed, 0)
                : Vector2.Lerp(Main.MouseWorld - player.Center, Main.MouseWorld - position, 0.5f).OfLength(item.shootSpeed);

            Projectile.NewProjectile(position, velocity, type, damage, knockBack, player.whoAmI);
            return false;
        }


        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SkyFracture);
            recipe.AddIngredient(ItemID.LunarBar, 10);
            recipe.AddIngredient(ItemID.Ectoplasm, 20);
            recipe.AddIngredient(ItemID.BlackLens);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
