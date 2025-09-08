using UnityEngine;
using MagicalTower.Core;

namespace MagicalTower.Tower.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class FireballProjectile : ProjectileBase
    {
        [Header("Explosion")]
        [SerializeField, Tooltip("Radius of the AOE explosion")] private float explosionRadius = 3f;
        [SerializeField, Tooltip("Layers considered to be enemies for explosion")] private LayerMask enemyMask;
        [SerializeField, Tooltip("Optional explosion force applied to rigidbodies hit")] private float explosionForce = 0f;

        protected override void Awake()
        {
            base.Awake();
            var col = GetComponent<Collider>();
            if (col) col.isTrigger = false; // collision-based
        }

        public override void Initialize(float damage, float speed)
        {
            base.Initialize(damage, speed);
            // initial velocity already set in base.Initialize
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Explode on first collision with anything
            Explode();
        }

        private void Explode()
        {
            // OverlapSphere with enemyMask so only hits layers we want (set in prefab)
            Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, enemyMask.value);

            foreach (var hit in hits)
            {
                if (!hit) continue;

                var dmg = hit.GetComponent<IDamageable>();
                if (dmg != null)
                {
                    dmg.TakeDamage(damage);
                }
                else
                {
                    var h = hit.GetComponent<Health>();
                    if (h != null) h.TakeDamage(damage);
                }

                // optional: apply physical force
                if (explosionForce > 0f)
                {
                    var rb = hit.attachedRigidbody;
                    if (rb != null)
                        rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
                }
            }

            // Optional VFX spawn here (particles, sound)
            Destroy(gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
#endif
    }
}