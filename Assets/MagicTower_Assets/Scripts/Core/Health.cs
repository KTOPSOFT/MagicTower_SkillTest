using UnityEngine;


namespace MagicalTower.Core
{
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        public float MaxHealth => maxHealth;
        public float Current = 100;
        public bool IsAlive => Current > 0f;
        public Transform Transform => transform;


        public System.Action<float, float> OnChanged; // (current, max)
        public System.Action OnDied;


        private void Awake()
        {
            Current = maxHealth;
        }


        public void ResetHealth(float newMax)
        {
            maxHealth = newMax;
            Current = newMax;
            OnChanged?.Invoke(Current, maxHealth);
        }


        public void TakeDamage(float amount)
        {
            if (!IsAlive) return;
            Current -= amount;
            OnChanged?.Invoke(Current, maxHealth);
            if (Current <= 0f)
            {
                Current = 0f;
                OnDied?.Invoke();
            }
        }
    }
}