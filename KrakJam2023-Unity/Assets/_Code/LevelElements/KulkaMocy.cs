using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class KulkaMocy : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {
            GetComponent<Rigidbody2D>().velocity = (GameSystems.GetSystem<GameplaySystem>().PlayerInstance.Position - transform.position) * 10;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            Destroy(gameObject);
        }
    }
}
