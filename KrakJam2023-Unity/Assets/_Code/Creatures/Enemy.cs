using Cysharp.Threading.Tasks;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class Enemy : Creature {
        [SerializeField] Rigidbody2D rigidbody;
        [SerializeField] Animator animator;
        [SerializeField] int meleeDmg = 0;
        [SerializeField] float meleeRng = 0f;
        [SerializeField] float meleeTime = 0f;
        [SerializeField] float sightRng = 0f;
        [SerializeField] float speed = 0f;
        [SerializeField] float maxVelocity = 0f;

        bool meleeAttacking;
        float resolveAttackTime;
        float distanceToTarget;
        PlayerController player;


        protected override void Die() {
            animator.SetBool("IsDead", true);
            DestroyAfterDelay().Forget();
        }

        async UniTaskVoid DestroyAfterDelay() {
            //todo jakas animacja dezintegracji (shader graph) jesli zdazymy
            await UniTask.Delay(5000);
            Destroy(gameObject);
        }

        void TryHitPlayer() {
            meleeAttacking = true;
            resolveAttackTime = Time.time + meleeTime;
        }

        void DecideAction() {
            if (!meleeAttacking)
                if (distanceToTarget < meleeRng / 2)
                    TryHitPlayer();
                else if (distanceToTarget < sightRng)
                    GoTo(player.Position);
        }

        void UpdateAttacking() {
            if (meleeAttacking) {
                if (Time.time > resolveAttackTime) {
                    StopAttacking();
                }
            }
        }

        public void ResolveAttackNow() {
            if (Vector2.Distance(player.Position, transform.position) < meleeRng)
                player.GetComponent<Creature>().DealDamage(meleeDmg);
        }

        void UpdateAnimation() {
            animator.SetBool("isAttacking", meleeAttacking);
            animator.SetBool("isWalking", !meleeAttacking && rigidbody.velocity.magnitude > 0.1);
            if (rigidbody.velocity.x > 0)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            else if (rigidbody.velocity.x < 0)
                transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        void StopAttacking() {
            meleeAttacking = false;
        }

        void GoTo(Vector2 position) {
            rigidbody.AddForce(new Vector2(Mathf.Sign(position.x - gameObject.transform.position.x) * speed, 0f));
        }

        void Update() {
            if (IsDead)
                return;
            if(!player)
                player = GameSystems.GetSystem<GameplaySystem>().PlayerInstance;
            if (player && !player.IsDead) {
                distanceToTarget = Vector2.Distance(player.Position, transform.position);
                DecideAction();
                UpdateAttacking();
            } else
                StopAttacking();
            UpdateAnimation();
        }

        void FixedUpdate() {
            if (IsDead)
                return;
            if (Mathf.Abs(rigidbody.velocity.x) > maxVelocity)
                rigidbody.velocity = new Vector2(Mathf.Sign(rigidbody.velocity.x) * maxVelocity, rigidbody.velocity.y);
        }
    }
}
