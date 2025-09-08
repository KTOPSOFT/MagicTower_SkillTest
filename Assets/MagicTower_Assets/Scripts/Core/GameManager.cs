using UnityEngine;
using UnityEngine.SceneManagement;


namespace MagicalTower.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Health towerHealth;
        [SerializeField] private Canvas gameOverCanvas;


        private void Start()
        {
            if (gameOverCanvas) gameOverCanvas.enabled = false;
            if (towerHealth) towerHealth.OnDied += OnTowerDied;
        }


        private void OnTowerDied()
        {
            Time.timeScale = 0f;
            if (gameOverCanvas) gameOverCanvas.enabled = true;
        }


        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}