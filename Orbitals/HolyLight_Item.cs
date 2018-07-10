﻿using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;

namespace Virtuous.Orbitals
{
    public class HolyLight_Item : OrbitalItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Circle of Protection");
            Tooltip.SetDefault("Holy lights surround you and increase life regeneration\nAligns with either magic or melee users");

            DisplayName.AddTranslation(GameCulture.Spanish, "Círculo Sagrado");
            Tooltip.AddTranslation(GameCulture.Spanish,
                "Luces santas te rodean y aumentan la regeneración de vida\nEl daño se alínea con magia o cuerpo a cuerpo");

            DisplayName.AddTranslation(GameCulture.Russian, "Круг Защиты");
            Tooltip.AddTranslation(GameCulture.Russian,
                "Святые огни окружают вас, увеличивая регенерацию здоровья\nПодходит воинам и магам");

            DisplayName.AddTranslation(GameCulture.Chinese, "圣光庇护");
            Tooltip.AddTranslation(GameCulture.Chinese,
                "圣光将围绕着你,提高生命再生\n更适合战士与法师使用");
        }


        public override void SetOrbitalDefaults()
        {
            type = OrbitalID.HolyLight;
            duration = 30 * 60;
            amount = 6;

            item.width = 30;
            item.height = 30;
            item.damage = 100;
            item.knockBack = 3f;
            item.mana = 60;
            item.rare = 8;
            item.value = Item.sellPrice(0, 40, 0, 0);
        }
    }
}
