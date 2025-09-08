using UnityEngine;
using MagicalTower.Enemy;


namespace MagicalTower.Tower
{
    public class FireballSpell : SpellBase
    {
        public FireballSpell(SpellDefinition def) : base(def) { }


        protected override bool TryCast(TowerController tower)
        {
            if (!def.projectilePrefab) return false;
            var target = tower.FindAnyEnemy();
            if (!target) return false;


            var dir = (target.transform.position - tower.Muzzle.position).normalized;
            var go = Object.Instantiate(def.projectilePrefab, tower.Muzzle.position, Quaternion.LookRotation(dir));
            var proj = go.GetComponent<Projectiles.FireballProjectile>();
            if (proj)
            {
                proj.Initialize(def.damage, def.projectileSpeed);
            }
            return true;
        }
    }
}