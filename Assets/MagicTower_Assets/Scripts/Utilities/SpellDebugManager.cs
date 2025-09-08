#if UNITY_EDITOR
using MagicalTower.Tower;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

namespace MagicalTower.UI
{
    public class SpellDebugManager : MonoBehaviour
    {
        [Header("Assign all spells here")]
        public List<SpellDefinition> spells = new List<SpellDefinition>();

        [Header("UI Prefab Setup")]
        public GameObject spellPanelPrefab; // prefab with TMP UI elements
        public Transform container;         // where panels spawn

        [Header("Hotkey")]
        public KeyCode toggleKey = KeyCode.H;

        private void Start()
        {
            if (container) container.gameObject.SetActive(false); // start hidden

            foreach (var spell in spells)
            {
                CreateSpellPanel(spell);
            }
        }

        private void Update()
        {
            if (container && Input.GetKeyDown(toggleKey))
            {
                container.gameObject.SetActive(!container.gameObject.activeSelf);
            }
        }

        private void CreateSpellPanel(SpellDefinition spell)
        {
            if (spellPanelPrefab == null || container == null) return;

            GameObject panel = Instantiate(spellPanelPrefab, container);
            panel.name = spell.displayName + "_Panel";

            // Find TMP UI elements
            TMP_Text title = panel.transform.Find("Title").GetComponent<TMP_Text>();
            TMP_InputField dmg = panel.transform.Find("DamageInput").GetComponent<TMP_InputField>();
            TMP_InputField cd = panel.transform.Find("CooldownInput").GetComponent<TMP_InputField>();
            TMP_InputField spd = panel.transform.Find("SpeedInput").GetComponent<TMP_InputField>();
            Button apply = panel.transform.Find("ApplyButton").GetComponent<Button>();

            // Fill initial values
            if (title) title.text = spell.displayName;
            if (dmg) dmg.text = spell.damage.ToString();
            if (cd) cd.text = spell.cooldown.ToString();
            if (spd) spd.text = spell.projectileSpeed.ToString();

            // Apply button logic
            if (apply)
            {
                apply.onClick.AddListener(() =>
                {
                    if (float.TryParse(dmg.text, out float newDmg)) spell.damage = newDmg;
                    if (float.TryParse(cd.text, out float newCd)) spell.cooldown = newCd;
                    if (float.TryParse(spd.text, out float newSpd)) spell.projectileSpeed = newSpd;

                    Save(spell);
                    Debug.Log($"Updated {spell.displayName}: dmg={spell.damage}, cd={spell.cooldown}, spd={spell.projectileSpeed}");
                });
            }
        }

        private void Save(SpellDefinition spell)
        {
            EditorUtility.SetDirty(spell);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
