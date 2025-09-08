using UnityEngine;
using MagicalTower.Core;

namespace MagicalTower.Tower.Projectiles
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ProjectileBase : MonoBehaviour
    {
        [Header("Projectile")]
        [SerializeField] protected float lifetime = 5f;

        protected float damage;
        protected float speed;
        protected Rigidbody rb;

        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
            // default settings: no gravity unless designer enabled it in prefab
            if (rb) rb.useGravity = false;
        }

        /// <summary>
        /// Initialize the projectile with damage/speed. Derived classes may override to accept extra args.
        /// </summary>
        public virtual void Initialize(float damage, float speed)
        {
            this.damage = damage;
            this.speed = speed;
            if (rb)
            {
                // set velocity along forward
                rb.velocity = transform.forward * speed;
            }
            Destroy(gameObject, lifetime);
        }
    }
}