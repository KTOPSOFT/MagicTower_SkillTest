using System.Collections.Generic;
using UnityEngine;
using MagicalTower.Core;


namespace MagicalTower.Tower
{
    public class TowerController : MonoBehaviour
    {
        [SerializeField] private Transform muzzle; // where projectiles spawn
        [SerializeField] private SpellLoadout loadout;
        [SerializeField] private float targetingRadius = 50f;


        public Transform Muzzle => muzzle ? muzzle : transform;


        public List<Enemy.EnemyBase> FindVisibleEnemies()
        {
            var results = new List<Enemy.EnemyBase>(Enemy.EnemyBase.All);
            results.RemoveAll(e => e == null || !e.IsAlive);


            // Keep only those visible by main camera
            var cam = Camera.main;
            if (!cam) return results;
            results.RemoveAll(e =>
            {
                var v = cam.WorldToViewportPoint(e.transform.position);
                return v.z <= 0f || v.x < 0f || v.x > 1f || v.y < 0f || v.y > 1f;
            });
            return results;
        }


        public Enemy.EnemyBase FindAnyEnemy()
        {
            foreach (var e in Enemy.EnemyBase.All)
                if (e && e.IsAlive) return e;
            return null;
        }


        private void Update()
        {
            if (!loadout) return;
            loadout.Tick(this);
        }
    }
}