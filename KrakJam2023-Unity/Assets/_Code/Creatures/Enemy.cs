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
        float endAttackTime;
        float distanceToTarget;
        PlayerController player;
        bool turnRight;


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
            endAttackTime = Time.time + meleeTime;
        }

        void DecideAction() {
            if (!meleeAttacking)
                if (distanceToTarget < meleeRng)
                    TryHitPlayer();
                else if (distanceToTarget < sightRng)
                    GoTo(player.Position);
        }

        void UpdateAttacking() {
            if (meleeAttacking) {
                if (Time.time > endAttackTime) {
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
            if (turnRight)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            else
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
                if (player.transform.position.x > transform.position.x)
                    turnRight = true;
                else
                    turnRight = false;
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
