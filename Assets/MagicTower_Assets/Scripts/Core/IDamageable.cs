using UnityEngine;


namespace MagicalTower.Core
{
    public interface IDamageable
    {
        Transform Transform { get; }
        void TakeDamage(float amount);
        bool IsAlive { get; }
    }
}