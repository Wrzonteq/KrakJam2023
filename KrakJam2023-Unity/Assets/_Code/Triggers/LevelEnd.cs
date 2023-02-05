using System;
using Cysharp.Threading.Tasks;
using PartTimeKamikaze.KrakJam2023;
using PartTimeKamikaze.KrakJam2023.UI;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    [SerializeField] Transform marchew;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("WIn Condition");
            collision.GetComponent<PlayerMovementController>().BlockMovement();
            GameSystems.GetSystem<InputSystem>().DisableInput();
            GameSystems.GetSystem<CameraSystem>().FocusOnMe(marchew, 10, WinGame).Forget();
        }
    }

    public void WinGame() {
        GameSystems.GetSystem<UISystem>().GetScreen<WinScreen>().Show().Forget();
        GameSystems.GetSystem<InputSystem>().SwitchToInterfaceInput();

        return;
    }
}
