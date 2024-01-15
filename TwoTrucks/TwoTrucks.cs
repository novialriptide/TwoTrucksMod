using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using Unity;
using UnityEngine;

namespace TwoTrucks
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class TwoTrucks : BaseUnityPlugin
    {
        public const string modGUID = "novialriptide.twotrucks";
        public const string modName = "Two Trucks";
        public const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        internal static TwoTrucks Instance;
        internal static AudioClip[] SoundFX;
        internal static AssetBundle Bundle;

        private ManualLogSource Logger;

        void Awake()
        {
            if (Instance is null)
                Instance = this;

            var BepInExLogSource = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            BepInExLogSource.LogMessage(modGUID + " has loaded succesfully.");

            Logger = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            harmony.PatchAll(typeof(TwoTrucksBase));
            string folderLocation = Instance.Info.Location;
            folderLocation = folderLocation.TrimEnd("TwoTrucksMod.dll".ToCharArray());
            SoundFX = new AudioClip[64];
            Bundle = AssetBundle.LoadFromFile("");
            if (!(Bundle is null))
            {
                SoundFX = Bundle.LoadAllAssets<AudioClip>();
                Logger.LogInfo("Loaded soundfx");
            }
            else
            {
                Logger.LogError("Unable to load soundfx");
            }
        }
    }

    [HarmonyPatch(typeof(GameNetcodeStuff.PlayerControllerB))]
    [HarmonyPatch("Update")]
    public class TwoTrucksBase
    {
        public static AudioClip TwoTrucksHavingSex;
        [HarmonyPostfix]
        static void Postfix(ref PlayerControllerB playerController, ref Vector3 serverPlayerPosition, ref AudioSource movementAudio)
        {
            if (playerController.causeOfDeath == CauseOfDeath.Crushing)
            {
                movementAudio.PlayOneShot(StartOfRound.Instance.alarmSFX);
            }
        }
    }
}
