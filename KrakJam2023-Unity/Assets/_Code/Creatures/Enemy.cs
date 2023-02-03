using UnityEngine;

namespace PartTimeKamikaze.KrakJam2023 {
    public class Enemy : Creature {
        [SerializeField] int meleeDmg = 0;
        [SerializeField] float meleeRng = 0f;
        [SerializeField] float sightRng = 10f;

        protected IEnemyBrain brain; //trzeba przypisac to pole, np. w klasie dziedziczacej albo dodac mu [SerializeField] i zrobic prefaby z mozgami i podpinac w edytorze

        private bool busy;


        protected override void Die() {
            Destroy(this.gameObject);
        }

        private void TryHitPlayer() {

        }

        private void DecideAction() {
        
        }

        private void GoTo(Vector2 position) {

        }

        void Update() {
            //AI
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
