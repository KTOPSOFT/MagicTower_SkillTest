using MagicalTower.Tower;
using UnityEngine;
using UnityEngine.UI;

public class SpellDebugUI : MonoBehaviour
{
    [Header("Spell SO")]
    [SerializeField] private SpellDefinition fireballSO;

    [Header("UI References")]
    [SerializeField] private Slider damageSlider;
    [SerializeField] private Slider cooldownSlider;
    [SerializeField] private Slider speedSlider;

    private SpellDefinition runtimeSpell;

    private void Start()
    {
        if (fireballSO == null) return;

        // Create a runtime copy so we don't modify the asset permanently
        runtimeSpell = ScriptableObject.Instantiate(fireballSO);

        // Initialize sliders with current values
        if (damageSlider != null) damageSlider.value = runtimeSpell.damage;
        if (cooldownSlider != null) cooldownSlider.value = runtimeSpell.cooldown;
        if (speedSlider != null) speedSlider.value = runtimeSpell.projectileSpeed;

        // Subscribe slider events to update runtime values
        if (damageSlider != null) damageSlider.onValueChanged.AddListener(v => runtimeSpell.damage = v);
        if (cooldownSlider != null) cooldownSlider.onValueChanged.AddListener(v => runtimeSpell.cooldown = v);
        if (speedSlider != null) speedSlider.onValueChanged.AddListener(v => runtimeSpell.projectileSpeed = v);
    }

    // Call this when casting a spell
    public SpellDefinition GetRuntimeSpell()
    {
        return runtimeSpell;
    }
}
