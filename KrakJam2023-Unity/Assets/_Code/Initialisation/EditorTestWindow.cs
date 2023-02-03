#if UNITY_EDITOR

using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace PartTimeKamikaze.KrakJam2023 {
    public class TestingWindow : EditorWindow {


        [MenuItem("KrakJam2023/Test Window", false, 0)]
        static void OpenTheWindow() {
            GetWindow<TestingWindow>("Testing Window");
        }

        void OnGUI() {
            if (!Application.isPlaying || !GameSystems.IsInitialised) {
                GUILayout.Label("Enter playmode to see tests.");
            } else {
                DrawWindow();
                DrawHotWaterHaxxes();
            }
        }

        void DrawWindow() {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Start enemies spawning")) {
                // GameSystems.GetSystem<EnemiesSystem>().Test();
            }
            GUILayout.EndHorizontal();
        }

        void DrawHotWaterHaxxes() {
            var gameplaySystem = GameSystems.GetSystem<GameplaySystem>();
            if (!gameplaySystem.IsInGameplay) {
                GUILayout.Label($"Start game to see haxxz");
                return;
            }

            GUILayout.BeginHorizontal();

            GUILayout.EndHorizontal();
        }


    }
}

#endif
