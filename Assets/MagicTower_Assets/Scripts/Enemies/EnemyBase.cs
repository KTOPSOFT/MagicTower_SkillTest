using System.Collections.Generic;
using UnityEngine;
using MagicalTower.Core;

namespace MagicalTower.Enemy
{
    [RequireComponent(typeof(Health))]
    [DisallowMultipleComponent]
    public class EnemyBase : MonoBehaviour
    {
        // Global registry of alive enemy instances
        public static readonly HashSet<EnemyBase> All = new HashSet<EnemyBase>();

        [Header("Stats")]
        [SerializeField] private EnemyStats stats;

        [Header("Movement / Combat")]
        [SerializeField, Tooltip("Distance to tower at which enemy starts dealing damage.")]
        private float towerContactRadius = 1.5f;

        [Header("Optional Health Bar")]
        [SerializeField] private GameObject healthBarPrefab;
        private GameObject healthBarInstance;

        private Transform tower;
        private Health health;
        private bool isDealingDamage;

        public bool IsAlive => health && health.IsAlive;
        public EnemyStats Stats => stats;

        private void OnEnable()
        {
            All.Add(this);
        }

        private void OnDisable()
        {
            All.Remove(this);
        }

        private void Awake()
        {
            health = GetComponent<Health>();

            if (stats != null)
            {
                health.ResetHealth(stats.maxHealth);
                transform.localScale = stats.visualScale;
            }
            else
            {
                Debug.LogWarning($"{name}: EnemyStats not assigned.", this);
            }

            var towerGo = GameObject.FindGameObjectWithTag("Tower");
            if (towerGo) tower = towerGo.transform;
            else Debug.LogWarning($"{name}: Could not find object with tag 'Tower'.", this);

            // Subscribe to death event
            if (health != null)
                health.OnDied += Die;

            // Spawn optional health bar
            if (healthBarPrefab != null)
            {
                healthBarInstance = Instantiate(healthBarPrefab);
                var bar = healthBarInstance.GetComponent<MagicalTower.UI.WorldspaceHealthBar>();
                if (bar != null) bar.AttachTo(health);
            }
        }

        private void Update()
        {
            if (!IsAlive || tower == null) return;

            Vector3 toTower = tower.position - transform.position;
            float dist = toTower.magnitude;

            // Move toward tower if not in contact radius
            if (dist > towerContactRadius)
            {
                transform.position += toTower.normalized * (stats.moveSpeed * Time.deltaTime);
                if (toTower.sqrMagnitude > 0.001f)
                    transform.rotation = Quaternion.LookRotation(toTower);
                isDealingDamage = false;
            }
            else
            {
                // Deal DPS to tower
                if (!isDealingDamage) isDealingDamage = true;

                var towerHealth = tower.GetComponent<Health>();
                if (towerHealth != null)
                    towerHealth.TakeDamage(stats.dpsToTower * Time.deltaTime);
            }
        }

        private void Die()
        {
            // Destroy health bar if exists
            if (healthBarInstance != null)
                Destroy(healthBarInstance);

            // Optional: spawn death VFX or sound here

            // Destroy enemy GameObject
            Destroy(gameObject);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, towerContactRadius);
        }
    }
}
