using HarmonyLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace MGT_BestSettings
{
    public class MgtBestSettings : MelonMod
    {
    }

    [HarmonyPatch(typeof(Menu_DevGame), nameof(Menu_DevGame.BUTTON_AutoDesignSettings))]
    public static class PatchMenuDevGameAutoDesignSettings
    {
        private static void Postfix(ref Menu_DevGame __instance, ref genres ___genres_)
        {
            var genreId = __instance.g_GameMainGenre;

            __instance.g_GameAP_Gameplay = Mathf.RoundToInt(___genres_.genres_GAMEPLAY[genreId] / 5f);
            __instance.g_GameAP_Grafik = Mathf.RoundToInt(___genres_.genres_GRAPHIC[genreId] / 5f);
            __instance.g_GameAP_Sound = Mathf.RoundToInt(___genres_.genres_SOUND[genreId] / 5f);
            __instance.g_GameAP_Technik = Mathf.RoundToInt(___genres_.genres_CONTROL[genreId] / 5f);

            __instance.uiObjects[97].GetComponent<Slider>().value = (float)__instance.g_GameAP_Gameplay;
            __instance.uiObjects[98].GetComponent<Slider>().value = (float)__instance.g_GameAP_Grafik;
            __instance.uiObjects[99].GetComponent<Slider>().value = (float)__instance.g_GameAP_Sound;
            __instance.uiObjects[100].GetComponent<Slider>().value = (float)__instance.g_GameAP_Technik;

            // UpdateGesamtArbeitsprioritaet is a private function,
            // so we just call a public function that calls UpdateGesamtArbeitsprioritaet in background
            __instance.SetAP_Technik();
        }
    }

    [HarmonyPatch(typeof(genres), nameof(genres.GetFocusKnown))]
    public static class PatchGenresGetFocusKnown
    {
        private static bool Prefix(ref bool __result)
        {
            __result = true;

            return false;
        }
    }

    [HarmonyPatch(typeof(genres), nameof(genres.GetAlignKnown))]
    public static class PatchGenresGetAlignKnown
    {
        private static bool Prefix(ref bool __result)
        {
            __result = true;

            return false;
        }
    }

    [HarmonyPatch(typeof(genres), nameof(genres.GetFocusTested))]
    public static class PatchGenresGetFocusTested
    {
        private static bool Prefix(ref bool __result)
        {
            __result = true;

            return false;
        }
    }

    [HarmonyPatch(typeof(genres), nameof(genres.GetAlignTested))]
    public static class PatchGenresGetAlignTested
    {
        private static bool Prefix(ref bool __result)
        {
            __result = true;

            return false;
        }
    }

    [HarmonyPatch(typeof(Menu_DevGame_Theme), "FitGenre")]
    public static class PatchMenuDevGameThemeFitGenre
    {
        private static bool Prefix(
            ref int __result,
            ref themes ___themes_,
            ref GUI_Main ___guiMain_,
            ref Menu_DevGame ___mDevGame_,
            ref Menu_Dev_AddonDo ___mDevAddon_,
            ref Menu_Dev_MMOAddon ___mDevMMOAddon_,
            int theme_
        )
        {
            var genre = -1;
            if (___guiMain_.uiObjects[56].activeSelf) genre = ___mDevGame_.g_GameMainGenre;
            if (___guiMain_.uiObjects[193].activeSelf) genre = ___mDevAddon_.gS_.maingenre;
            if (___guiMain_.uiObjects[247].activeSelf) genre = ___mDevMMOAddon_.gS_.maingenre;

            if (genre != -1)
                __result = ___themes_.IsThemesFitWithGenre(theme_, genre) ? 1 : -1;
            else
                __result = 0;

            return false;
        }
    }

    [HarmonyPatch(typeof(Menu_DevGame_Zielgruppe), "FitTargetGroup")]
    public static class PatchMenuDevGameZielgruppeFitTargetGroup
    {
        private static bool Prefix(ref int __result, ref genres ___genres_, ref Menu_DevGame ___mDevGame_, int targetGroup_)
        {
            __result = ___genres_.IsTargetGroup(___mDevGame_.g_GameMainGenre, targetGroup_) ? 1 : -1;

            return false;
        }
    }
}