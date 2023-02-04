using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class Enemy : Creature {
        [SerializeField] int meleeDmg = 0;
        [SerializeField] float meleeRng = 0f;
        [SerializeField] float meleeTime = 0f;
        [SerializeField] float sightRng = 0f;
        [SerializeField] float speed = 0f;
        [SerializeField] float maxVelocity = 0f;
        [SerializeField] Animator anim;


        protected IEnemyBrain brain; //trzeba przypisac to pole, np. w klasie dziedziczacej albo dodac mu [SerializeField] i zrobic prefaby z mozgami i podpinac w edytorze

        protected Rigidbody2D rb;
        protected bool isRigidBody = false;
        protected float direction;

        private bool meleeAttacking = false;
        private float resolveAttackTime;

        private GameObject target;
        private float distanceToTarget;

        private bool isAnimator = false;



        protected override void Die() {
            Destroy(this.gameObject);
        }

        private void TryHitPlayer() {
            meleeAttacking = true;
            resolveAttackTime = Time.time + meleeTime;
        }

        private void DecideAction() {
            if (!meleeAttacking)
                if (distanceToTarget < meleeRng / 2)
                    TryHitPlayer();
                else if (distanceToTarget < sightRng)
                    GoTo(target.transform.position);
        }

        private void UpdateAttacking() {
            if (meleeAttacking) {
                if (Time.time > resolveAttackTime) {
                    StopAttacking();
                }
            }
        }

        public void ResolveAttackNow() {
            if (Vector2.Distance(target.transform.position, transform.position) < meleeRng)
                target.GetComponent<Creature>().DealDamage(meleeDmg);
        }

        private void UpdateAnimation() {
            if (isAnimator) {
                anim.SetBool("isAttacking", meleeAttacking);
                anim.SetBool("isWalking", !meleeAttacking && rb.velocity.magnitude > 0.1);
            }
        }

        private void StopAttacking() {
            meleeAttacking = false;
        }

        private void GoTo(Vector2 position) {
            if (isRigidBody)
                rb.AddForce(new Vector2(Mathf.Sign(position.x - gameObject.transform.position.x) * speed, 0f));
        }

        void Start() {
            isRigidBody = gameObject.TryGetComponent<Rigidbody2D>(out rb);
            isAnimator = anim != null;
            base.Start();
        }

        private void Update() {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target != null) {
                distanceToTarget = Vector2.Distance(target.transform.position, transform.position);
                DecideAction();
                UpdateAttacking();
            } else
                StopAttacking();
            UpdateAnimation();
        }

        void FixedUpdate() {
            if (rb.velocity.x > maxVelocity) {
                rb.velocity = new Vector2( Mathf.Sign(rb.velocity.x) * maxVelocity, rb.velocity.y);
            }
        }
    }

    public interface IEnemyBrain {
        //przykladowe metody
        bool CanMove();
        bool CanAttack();
        bool IsPlayerInRange();
        void Move(Vector2 position, float timeDelta);
    }
}
