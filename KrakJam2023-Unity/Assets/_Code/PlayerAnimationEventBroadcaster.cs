using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class PlayerAnimationEventBroadcaster : MonoBehaviour {
        public void FireMissile() {
            Debug.Log($"FIRE");
        }

        public void HitMelee() {
            Debug.Log($"BOINK");
        }
    }
}
