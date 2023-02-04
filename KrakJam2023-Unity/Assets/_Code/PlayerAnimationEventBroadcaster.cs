using System;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class PlayerAnimationEventBroadcaster : MonoBehaviour {
        public Action callBackOnFire;
        public Action callBackOnBoink;

        public void FireMissile() {
            Debug.Log($"FIRE");
            callBackOnFire?.Invoke();
        }

        public void HitMelee() {
            Debug.Log($"BOINK");
            callBackOnBoink?.Invoke();
        }
    }
}
