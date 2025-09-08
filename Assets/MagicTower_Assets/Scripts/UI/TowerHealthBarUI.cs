using UnityEngine;
using UnityEngine.UI;
using MagicalTower.Core;

public class TowerHealthBarUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image fillImage;

    [Header("Tower Health")]
    [SerializeField] private Health towerHealth;

    private void Awake()
    {
        if (towerHealth != null)
        {
            towerHealth.OnChanged += UpdateHealthBar;
            UpdateHealthBar(towerHealth.Current, towerHealth.MaxHealth);
        }
        else
        {
            Debug.LogError("TowerHealthBarUI: Tower Health not assigned.");
        }
    }

    private void OnDestroy()
    {
        if (towerHealth != null)
            towerHealth.OnChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(float current, float max)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = current / max;
        }
    }
}
