using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class StaffLightningController : MonoBehaviour {
        [SerializeField] Transform staffTop;
        [SerializeField] LightningVfx vfx;
        [SerializeField] float duration;

        LightningVfx instance;
        Transform target;
        bool isPlaying;


        public void ShootTarget(Transform newTarget) {
            Debug.Log($"Shoot at {newTarget.gameObject.name}");
            target = newTarget;
            PlayAndStopAfterDuration().Forget();
        }

        async UniTaskVoid PlayAndStopAfterDuration() {
            isPlaying = true;
            instance = Instantiate(vfx);
            instance.Play();
            await UniTask.Delay((int)(duration * 1000));
            instance.Stop();
            Destroy(instance.gameObject);
            isPlaying = false;
        }

        void Update() {
            if (!isPlaying)
                return;
            if (!instance)
                return;
            var fromStaffToTarget = target.position - staffTop.position;
            var angle = Vector3.Angle(fromStaffToTarget, Vector3.right);
            if (angle < 0)
                angle = 360 - angle;
            var midpoint = staffTop.position + fromStaffToTarget / 2;
            instance.transform.position = midpoint;
            instance.transform.eulerAngles = new Vector3(0, 0, 90 * angle);
            instance.SetLength(fromStaffToTarget.magnitude);
        }
    }
}
