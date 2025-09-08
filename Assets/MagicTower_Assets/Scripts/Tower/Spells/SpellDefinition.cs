using UnityEngine;


namespace MagicalTower.Tower
{
    [CreateAssetMenu(menuName = "MagicalTower/Spell Definition")]
    public class SpellDefinition : ScriptableObject
    {
        public string displayName = "Spell";
        public float cooldown = 1f;
        public float damage = 10f;
        public float projectileSpeed = 20f;
        public GameObject projectilePrefab;
        [TextArea] public string notes;
    }
}