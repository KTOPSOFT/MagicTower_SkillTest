using UnityEngine;
using UnityEngine.UI;
using MagicalTower.Core;

namespace MagicalTower.UI
{
    /// <summary>
    /// Worldspace health bar that follows a Health component target in world.
    /// Attach this to the health bar prefab (with a Canvas + Image).
    /// </summary>
    public class WorldspaceHealthBar : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Health targetHealth;
        [SerializeField] private Image fillImage;
        [SerializeField] private Vector3 worldOffset = new Vector3(0, 2f, 0);

        private Camera cam;

        private void Start()
        {
            cam = Camera.main;
            if (!cam) Debug.LogWarning("WorldspaceHealthBar: No main camera found.");
        }

        private void LateUpdate()
        {
            if (!targetHealth || !fillImage) return;

            // Update fill
            float ratio = targetHealth.Current / targetHealth.MaxHealth;
            fillImage.fillAmount = Mathf.Clamp01(ratio);

            // Position above target
            if (targetHealth)
            {
                transform.position = targetHealth.transform.position + worldOffset;
            }

            // Face camera
            if (cam)
            {
                transform.forward = cam.transform.forward;
            }
        }

        public void AttachTo(Health newTarget)
        {
            targetHealth = newTarget;
        }
    }
}