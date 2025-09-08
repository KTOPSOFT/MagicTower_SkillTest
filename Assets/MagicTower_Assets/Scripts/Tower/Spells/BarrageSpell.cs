using System.Collections.Generic;
using UnityEngine;
using MagicalTower.Enemy;


namespace MagicalTower.Tower
{
    public class BarrageSpell : SpellBase
    {
        public BarrageSpell(SpellDefinition def) : base(def) { }


        protected override bool TryCast(TowerController tower)
        {
            if (!def.projectilePrefab) return false;
            List<EnemyBase> visible = tower.FindVisibleEnemies();
            if (visible.Count == 0) return false;


            foreach (var e in visible)
            {
                if (!e) continue;
                var dir = (e.transform.position - tower.Muzzle.position).normalized;
                var go = Object.Instantiate(def.projectilePrefab, tower.Muzzle.position, Quaternion.LookRotation(dir));
                var proj = go.GetComponent<Projectiles.HomingProjectile>();
                if (proj)
                {
                    proj.Initialize(e, def.damage, def.projectileSpeed);
                }
            }
            return true;
        }
    }
}