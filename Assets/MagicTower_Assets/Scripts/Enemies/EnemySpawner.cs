using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicalTower.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [System.Serializable]
        public class SpawnEntry
        {
            public GameObject enemyPrefab;
            [Tooltip("Spawns per second. 0.5 means one spawn every 2 seconds.")]
            public float spawnsPerSecond = 0.5f;
        }

        [System.Serializable]
        public class SpawnPhase
        {
            public string name = "Phase";
            public float duration = 30f;
            public List<SpawnEntry> entries = new List<SpawnEntry>();
        }

        [Header("Spawn Layout")]
        [SerializeField] private List<SpawnPhase> phases = new List<SpawnPhase>();
        [SerializeField, Tooltip("Distance from tower where enemies spawn (ring).")] private float spawnRadius = 40f;
        [SerializeField] private Transform tower;

        private void Start()
        {
            if (!tower)
            {
                var t = GameObject.FindGameObjectWithTag("Tower");
                if (t) tower = t.transform;
            }

            if (!tower) Debug.LogWarning("EnemySpawner: Tower transform not assigned and no object with tag 'Tower' found.");

            if (phases == null || phases.Count == 0)
            {
                Debug.LogWarning("EnemySpawner: No phases configured. Add at least one SpawnPhase.");
                return;
            }

            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            Vector3 GetCenter() => tower ? tower.position : transform.position;

            foreach (var phase in phases)
            {
                float t = 0f;

                // accumulator approach to handle fractional spawn rates
                var accum = new Dictionary<SpawnEntry, float>();
                foreach (var e in phase.entries) accum[e] = 0f;

                while (t < phase.duration)
                {
                    float dt = Time.deltaTime;
                    t += dt;

                    foreach (var e in phase.entries)
                    {
                        if (e == null || e.enemyPrefab == null || e.spawnsPerSecond <= 0f) continue;

                        accum[e] += e.spawnsPerSecond * dt;
                        while (accum[e] >= 1f)
                        {
                            Spawn(e.enemyPrefab, GetCenter(), spawnRadius);
                            accum[e] -= 1f;
                        }
                    }

                    yield return null;
                }
            }
        }

        private void Spawn(GameObject prefab, Vector3 center, float radius)
        {
            if (!prefab) return;
            // spawn somewhere on a horizontal ring around center
            Vector3 dir = Random.onUnitSphere;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.001f) dir = Vector3.forward;
            dir.Normalize();

            Vector3 pos = center + dir * radius;

            // Optional small random offset so they don't spawn perfectly on ring
            pos += new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f));

            Instantiate(prefab, pos, Quaternion.identity);
        }

#if UNITY_EDITOR
        // Helper to draw spawn ring in editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            if (tower) Gizmos.DrawWireSphere(tower.position, spawnRadius);
            else Gizmos.DrawWireSphere(transform.position, spawnRadius);
        }
#endif
    }
}