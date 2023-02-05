using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PartTimeKamikaze.KrakJam2023 {
    public class KulkaMocy : MonoBehaviour {
        Vector3 velocity;
        void Update() {
            GetComponent<Rigidbody2D>().velocity = velocity;
        }

        private void Start() {
            velocity = (GameSystems.GetSystem<GameplaySystem>().PlayerInstance.Position - transform.position) * 20;
            DestroyAfterDelay();
        }

        async UniTaskVoid DestroyAfterDelay() {
            //todo jakas animacja dezintegracji (shader graph) jesli zdazymy
            await UniTask.Delay(100);
            Destroy(gameObject);
        }
    }
}
