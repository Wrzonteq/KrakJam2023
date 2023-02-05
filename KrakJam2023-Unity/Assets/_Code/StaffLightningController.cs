using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class StaffLightningController : MonoBehaviour {
        [SerializeField] Transform staffTop;
        [SerializeField] LightningVfx vfx;
        [SerializeField] float duration;

        LightningVfx currentInstance;
        Transform target;
        bool isPlaying;


        public void ShootTarget(Transform newTarget, bool stronk) {
            Debug.Log($"Shoot at {newTarget.gameObject.name}");
            target = newTarget;
            isPlaying = true;
            var newInstance = Instantiate(vfx);
            currentInstance = newInstance;
            currentInstance.Play();
            DestroyAfterDuration(newInstance).Forget();
            if(stronk)
                currentInstance.SetWidth(1.5f);
        }

        async UniTaskVoid DestroyAfterDuration(LightningVfx instance) {
            await UniTask.Delay((int)(duration * 1000));
            instance.Stop();
            Destroy(instance.gameObject);
            isPlaying = false;
        }


        void Update() {
            if (!isPlaying)
                return;
            if (!currentInstance)
                return;
            var fromStaffToTarget = target.position - staffTop.position;
            var rotation = Quaternion.LookRotation(fromStaffToTarget.normalized);
            var midpoint = staffTop.position + fromStaffToTarget / 2;
            currentInstance.transform.position = midpoint;
            var instanceRotation = currentInstance.transform.rotation;
            instanceRotation.z = rotation.z;
            currentInstance.transform.rotation = instanceRotation;
            currentInstance.SetLength(fromStaffToTarget.magnitude);
        }
    }
}
