using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SR_PluginLoader
{
    /// <summary>
    /// Helper class for playing common sounds ingame.
    /// </summary>
    public static class Sound
    {
        private static Dictionary<SoundId, Func<SECTR_AudioCue>> SOUND_MAP = new Dictionary<SoundId, Func<SECTR_AudioCue>>()
        {
            { SoundId.POSITIVE, () => { return SRSingleton<GameContext>.Instance.UITemplates.purchasePersonalUpgradeCue; } },
            { SoundId.NEGATIVE, () => { return SRSingleton<GameContext>.Instance.UITemplates.errorCue; } },
            { SoundId.ERROR, () => { return SRSingleton<GameContext>.Instance.UITemplates.errorCue; } },
        };


        public static void Play(SoundId snd)
        {
            Sound.Play(snd, Vector3.zero);
        }

        public static void Play(SoundId snd, Vector3 pos)
        {
            //DebugHud.Log("[Sound] Playing: SoundId.{0}", snd.ToString());
            Func<SECTR_AudioCue> cue;
            if (SOUND_MAP.TryGetValue(snd, out cue))
            {
                SECTR_AudioSystem.Play(cue(), pos, false);
            }
            else
            {
                DebugHud.Log("[Sound] No sound listed for SoundId.{0}", snd.ToString());
            }
        }


        /// <summary>
        /// Plays a sound and sets a timed flag on a given object.
        /// This is useful for playing a sound in response to an object.
        /// Example:
        /// The player fires an object into a garden input that isnt a valid fruit/vegetable.
        /// The kiosk can use this function to play a negative sound for the item without it playing like 10 times due to the item still being in range of the input area!
        /// </summary>
        /// <param name="snd">The sound to play</param>
        /// <param name="obj">The GameObject to lock the sound so it doesn't play more then once.</param>
        /// <param name="lockSecs">Number of seconds before the sound can play again.</param>                   
        public static void PlayOnce(SoundId snd, GameObject obj=null, float lockSecs=1.0f)
        {
            TimedObjectFlag locker = obj.GetComponent<TimedObjectFlag>();
            if (locker == null) locker = obj.AddComponent<TimedObjectFlag>();
            string flag = String.Concat("soundlock_", snd.ToString());

            if (!locker.HasFlag(flag))
            {
                locker.SetFlag(flag, lockSecs);
                Sound.Play(snd);
            }
        }

    }


    public class SoundId : SafeEnum
    {
        public static SoundId NEGATIVE = new SoundId();
        public static SoundId POSITIVE = new SoundId();
        public static SoundId ERROR = new SoundId();
    }
}
