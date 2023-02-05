using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2023;
using PartTimeKamikaze.KrakJam2023.UI;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class ResetLevel : MonoBehaviour {
        public void LoadMainMenu() {
            GameSystems.GetSystem<UISystem>().GetScreen<MainMenuScreen>().Show().Forget();
        }
    }
}
