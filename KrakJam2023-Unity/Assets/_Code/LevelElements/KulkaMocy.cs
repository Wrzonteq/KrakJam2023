using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace PartTimeKamikaze.KrakJam2023 {
    public class KulkaMocy : MonoBehaviour {
        Vector3 velocity;
        [SerializeField] int magicDmg;
        void Update() {
            GetComponent<Rigidbody2D>().velocity = velocity;
        }

        private void Start() {
            velocity = ((GameSystems.GetSystem<GameplaySystem>().PlayerInstance.Position + new Vector3(0f,1f,0f)) - transform.position) * 10;
            DestroyAfterDelay().Forget();
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.CompareTag("Player")) {
                collision.GetComponent<Creature>().DealDamage(magicDmg);
                Destroy(gameObject);
            }
        }

        async UniTaskVoid DestroyAfterDelay() {
            //todo jakas animacja dezintegracji (shader graph) jesli zdazymy
            await UniTask.Delay(600);
            Destroy(gameObject);
        }
    }
}
