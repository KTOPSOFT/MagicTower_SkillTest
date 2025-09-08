using UnityEngine;
using MagicalTower.Core;
using MagicalTower.Enemy;

namespace MagicalTower.Tower.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class HomingProjectile : ProjectileBase
    {
        [Tooltip("Angular lerp to smooth turning. 0 = instant, 1 = no turning."), Range(0f, 1f)]
        [SerializeField] private float smoothing = 0.1f;

        private EnemyBase target;

        /// <summary>
        /// Initialize homing projectile to chase a specific enemy.
        /// </summary>
        public void Initialize(EnemyBase target, float damage, float speed)
        {
            base.Initialize(damage, speed);
            this.target = target;
            // ensure collider is trigger for simpler collision handling
            var col = GetComponent<Collider>();
            if (col) col.isTrigger = true;
        }

        private void FixedUpdate()
        {
            if (!rb) return;

            if (target == null || !target.IsAlive)
            {
                // go straight if target lost
                rb.velocity = transform.forward * speed;
                return;
            }

            Vector3 dir = (target.transform.position - transform.position).normalized;
            if (dir.sqrMagnitude <= 0.0001f)
            {
                rb.velocity = transform.forward * speed;
                return;
            }

            // Smoothly rotate forward vector toward direction
            Vector3 newForward = Vector3.Slerp(transform.forward, dir, 1f - smoothing);
            rb.velocity = newForward * speed;
            transform.forward = newForward;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other) return;

            // Only damage enemies (tagged as "Enemy")
            if (!other.CompareTag("Enemy")) return;

            // Try to fetch IDamageable (works if Health implements it)
            var dmgTarget = other.GetComponent<IDamageable>();
            if (dmgTarget != null)
            {
                dmgTarget.TakeDamage(damage);
            }
            else
            {
                // Fallback: try Health
                var h = other.GetComponent<MagicalTower.Core.Health>();
                if (h != null) h.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}