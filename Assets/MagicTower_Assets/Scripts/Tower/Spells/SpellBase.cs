using UnityEngine;


namespace MagicalTower.Tower
{
    public abstract class SpellBase
    {
        protected readonly SpellDefinition def;
        private float cooldownTimer;


        protected SpellBase(SpellDefinition def) { this.def = def; }


        public void Tick(TowerController tower)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                if (TryCast(tower)) cooldownTimer = def.cooldown;
            }
        }


        protected abstract bool TryCast(TowerController tower);
    }
}