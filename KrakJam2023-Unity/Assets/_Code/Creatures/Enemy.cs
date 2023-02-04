using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class Enemy : Creature {
        [SerializeField] int meleeDmg = 0;
        [SerializeField] float meleeRng = 0f;
        [SerializeField] float meleeTime = 0f;
        [SerializeField] float sightRng = 0f;
        [SerializeField] float speed = 0f;
        [SerializeField] float maxVelocity = 0f;


        protected IEnemyBrain brain; //trzeba przypisac to pole, np. w klasie dziedziczacej albo dodac mu [SerializeField] i zrobic prefaby z mozgami i podpinac w edytorze

        protected Rigidbody2D rb;
        protected bool isRigidBody = false;
        protected float direction;

        private bool meleeAttacking = false;
        private float resolveAttackTime;
        //private bool busy = false; //u�ywane przy ataku, po rozpocz�ciu ataku musi go zako�czy� zanim zrobi co� innego

        private GameObject target;
        private float distanceToTarget;

        private bool isRenderer = false;
        private SpriteRenderer renderer;



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
                    if (Vector2.Distance(target.transform.position, transform.position) < meleeRng)
                        target.GetComponent<Creature>().DealDamage(meleeDmg);
                    StopAttacking();
                }
            }
        }

        private void UpdateAnimation() {
            if (isRenderer)
                if (meleeAttacking)
                    renderer.color = Color.red;
                else
                    renderer.color = Color.white;
        }

        private void StopAttacking() {
            meleeAttacking = false;
        }

        private void GoTo(Vector2 position) {
            if (isRigidBody) {
                rb.AddForce(new Vector2(Mathf.Sign(position.x - gameObject.transform.position.x) * speed, 0f));
                //rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, 0, maxVelocity), rb.velocity.y);
                Vector3.ClampMagnitude(rb.velocity, maxVelocity);
            }
        }

        void Start() {
            isRigidBody = gameObject.TryGetComponent<Rigidbody2D>(out rb);
            isRenderer = gameObject.TryGetComponent<SpriteRenderer>(out renderer);
            base.Start();
        }

        void FixedUpdate() {
            //AI
            target = GameObject.FindGameObjectWithTag("Player");
            if (target != null) {
                distanceToTarget = Vector2.Distance(target.transform.position, transform.position);
                DecideAction();
                UpdateAttacking();
            } else
                StopAttacking();
            UpdateAnimation();
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
