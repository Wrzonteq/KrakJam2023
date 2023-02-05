using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PartTimeKamikaze.KrakJam2023 {
    public class AnimatorEventHandler : MonoBehaviour {
        void AttackResolveNow() {
            GetComponentInParent<Enemy>().ResolveAttackNow();
        }

        void MagicAttackResolveNow() {
            GetComponentInParent<Wyjec>().ResolveMagicAttackNow();
        }
    }
}
