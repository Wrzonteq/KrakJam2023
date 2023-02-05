using PartTimeKamikaze.KrakJam2023;
using UnityEngine;

public class LevelEnd : MonoBehaviour {
    [SerializeField] Transform marchew;

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            Debug.Log("WIN");
            GameSystems.GetSystem<InputSystem>().DisableInput();
            GameSystems.GetSystem<CameraSystem>().FocusOnMe(marchew, 10).Forget();
            //todo fade to black
            //todo display win screen
            
            // todo switch input when win screen is shown
            // GameSystems.GetSystem<InputSystem>().SwitchToInterfaceInput();
        }
    }
}
