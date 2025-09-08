using UnityEngine;
using TMPro;


namespace MagicalTower.UI
{
    public class DamageNumber : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        [SerializeField] private float floatSpeed = 2f;
        [SerializeField] private float duration = 1f;


        public void Initialize(float amount)
        {
            if (text) text.text = amount.ToString("F0");
            Destroy(gameObject, duration);
        }


        private void Update()
        {
            transform.position += Vector3.up * (floatSpeed * Time.deltaTime);
            transform.LookAt(Camera.main.transform);
        }
    }
}