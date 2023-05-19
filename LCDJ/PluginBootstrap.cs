using System;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Il2CppInterop.Runtime.Injection;
using UnityEngine;
using Array = System.Array;

namespace LCDJ
{
    public class PluginBootstrap : MonoBehaviour
    {
        public static PluginBootstrap Instance;
        
        internal static void Setup()
        {
            ClassInjector.RegisterTypeInIl2Cpp<PluginBootstrap>();

            GameObject obj = new(MyPluginInfo.PLUGIN_GUID + "bootstrap");
            DontDestroyOnLoad(obj);
            obj.hideFlags |= HideFlags.HideAndDontSave;
            Instance = obj.AddComponent<PluginBootstrap>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F6))
            {
                foreach (var (key, value) in RuntimeManager.Instance.loadedBanks)
                {
                    Plugin.PluginLog.LogWarning($"{key}: {value}");

                    GetEventList(value.Bank, out var events);
                    foreach (var eventDescription in events)
                    {
                        eventDescription.getPath(out var path);
                        if (path.StartsWith("event:/") && !path.Contains(":/Voice") && !path.Contains(":/SFX"))
                        {
                            Plugin.PluginLog.LogWarning(path);
                        }
                    }
                }
                SoundGenerator._currentBGM = RuntimeManager.CreateInstance("event:/BGM/LimbusCompany_BGM_Event_3_5");
                SingletonBehavior<SoundManager>.Instance.ChangeBGM(SoundGenerator._currentBGM);
            }
        }

        private RESULT GetEventList(Bank bank, out EventDescription[] array)
        {
            array = null;
            var eventCount = FMOD_Studio_Bank_GetEventCount(bank.handle, out var count1);
            if (eventCount != RESULT.OK)
                return eventCount;
            if (count1 == 0)
            {
                array = Array.Empty<EventDescription>();
                return eventCount;
            }
            var array1 = new IntPtr[count1];
            var eventList = FMOD_Studio_Bank_GetEventList(bank.handle, array1, count1, out var count2);
            if (eventList != RESULT.OK)
                return eventList;
            if (count2 > count1)
                count2 = count1;
            array = new EventDescription[count2];
            for (var index = 0; index < count2; ++index)
                array[index].handle = array1[index];
            return RESULT.OK;
        }
        
        [DllImport("fmodstudio")]
        private static extern RESULT FMOD_Studio_Bank_GetEventCount(IntPtr bank, out int count);
        
        [DllImport("fmodstudio")]
        private static extern RESULT FMOD_Studio_Bank_GetEventList(
            IntPtr bank,
            IntPtr[] array,
            int capacity,
            out int count);
    }
}