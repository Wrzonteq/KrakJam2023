using System;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    [CreateAssetMenu(fileName = "GameStateData", menuName = "KrakJam2023/GameStateAsset")]
    public class GameStateDataAsset : ScriptableObject {
        public GameStage stage;

        public int completedLevelsCount;

        //tutaj bedziemy tez trzymac info o tym ile mamy "amunicji" w danym momencie etc.
    }

    [Serializable]
    public class EmotionLevelState {
        public bool gateUnlocked;
        public bool negativeMemoryCollected;
        public bool positiveMemoryCollected;
        public bool insanityStarted;
        public bool insanitySurvived;

        public bool CanStartInsanity => negativeMemoryCollected && positiveMemoryCollected;
    }

    public enum Emotion {
        Fear = 0,
        Sadness = 1,
        Anger = 2,
        Loneliness = 3,
        Despair = 4,
    }

    public enum GameStage {
        Unset = -1,
        Sanity,
        Insanity
    }
}
