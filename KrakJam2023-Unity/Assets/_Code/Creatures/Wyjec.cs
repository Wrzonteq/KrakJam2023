using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class Wyjec : Enemy {
        [SerializeField] float magicRng;
        [SerializeField] int magicDmg;
        [SerializeField] protected float magicTime = 0f;


        protected bool magicAttacking;
        private void Update() {
            if (IsDead)
                return;
            if (!player)
            player = GameSystems.GetSystem<GameplaySystem>().PlayerInstance;
            if (player && !player.IsDead) {
                distanceToTarget = Vector2.Distance(player.Position, transform.position);
                if (player.Position.x > transform.position.x)
                    turnRight = true;
                else
                    turnRight = false;
                DecideAction();
                UpdateAttacking();
            } else
                StopAttacking();
            UpdateAnimation();
        }

        private void FixedUpdate() {
            if (IsDead)
                return;
            if (rigidbody.velocity.magnitude > maxVelocity)
                rigidbody.velocity /= rigidbody.velocity.magnitude / maxVelocity;
        }

        void DecideAction() {
            if (!meleeAttacking && !magicAttacking) {
                Debug.Log("22222222222");
                if (distanceToTarget < meleeRng)
                    TryHitPlayer();
                else if (distanceToTarget < magicRng)
                    TryMagicHitPlayer();
                else if (distanceToTarget < sightRng)
                    GoTo(player.Position);
            }
        }
        void TryMagicHitPlayer() {
            magicAttacking = true;
            endAttackTime = Time.time + magicTime;
        }

        public void ResolveMagicAttackNow() {
            if (Vector2.Distance(player.Position, transform.position) < meleeRng)
                player.GetComponent<Creature>().DealDamage(magicDmg);
        }

        protected void StopAttacking() {
            meleeAttacking = false;
            magicAttacking = false;
        }
        void UpdateAttacking() {
            if (meleeAttacking || magicAttacking)
                if (Time.time > endAttackTime)
                    StopAttacking();
        }

        void GoTo(Vector2 position) {
            rigidbody.AddForce(new Vector2(Mathf.Sign(position.x - gameObject.transform.position.x) * speed, 0f));
        }

        void UpdateAnimation() {
            animator.SetBool("isAttacking", meleeAttacking);
            animator.SetBool("isMagicAttacking", magicAttacking);
            animator.SetBool("isWalking", !meleeAttacking && !magicAttacking && rigidbody.velocity.magnitude > 0.1);
            if (turnRight)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            else
                transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
