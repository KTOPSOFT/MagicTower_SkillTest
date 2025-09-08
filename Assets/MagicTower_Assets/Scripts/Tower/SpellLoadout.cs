using UnityEngine;


namespace MagicalTower.Tower
{
    public class SpellLoadout : MonoBehaviour
    {
        [Header("Spell Definitions")]
        [SerializeField] private SpellDefinition fireball;
        [SerializeField] private SpellDefinition barrage;


        private SpellBase fireballSpell;
        private SpellBase barrageSpell;


        private void Awake()
        {
            if (fireball) fireballSpell = new FireballSpell(fireball);
            if (barrage) barrageSpell = new BarrageSpell(barrage);
        }


        public void Tick(TowerController tower)
        {
            fireballSpell?.Tick(tower);
            barrageSpell?.Tick(tower);
        }
    }
}