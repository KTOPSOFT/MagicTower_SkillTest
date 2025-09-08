using UnityEngine;

namespace MagicalTower.Enemy
{
    [CreateAssetMenu(menuName = "MagicalTower/Enemy Stats")]
    public class EnemyStats : ScriptableObject
    {
        [Header("Core")]
        public float maxHealth = 30f;
        public float moveSpeed = 2.5f;

        [Header("Combat")]
        [Tooltip("Damage per second dealt to the tower while touching it.")]
        public float dpsToTower = 4f;

        [Header("Visual")]
        public Vector3 visualScale = Vector3.one;
    }
}