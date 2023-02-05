using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class AnimatorEventHandler : MonoBehaviour {
        void AttackResolveNow() {
            Debug.Log("1");
            GetComponentInParent<Enemy>().ResolveAttackNow();
        }

        void MagicAttackResolveNow() {
            GetComponentInParent<Wyjec>().ResolveMagicAttackNow();
            Debug.Log("2");
        }
    }
}
