using UnityEngine;
using UnityEngine.VFX;

namespace PartTimeKamikaze.KrakJam2023 {
    public class LightningVfx : MonoBehaviour {
        [SerializeField] VisualEffect vfx;

        public void Play() {
            vfx.gameObject.SetActive(true);
            vfx.Play();
        }

        public void Stop() {
            vfx.Stop();
        }

        public void SetLength(float length) {
            vfx.transform.localScale = new Vector3(1, 1, length);
        }
    }
}
