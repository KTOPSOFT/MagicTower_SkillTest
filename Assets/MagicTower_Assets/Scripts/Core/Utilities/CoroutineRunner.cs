using UnityEngine;
using System.Collections;

namespace MagicalTower.Core
{
    /// <summary>
    /// Utility singleton to run coroutines from non-MonoBehaviour classes.
    /// </summary>
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;
        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[CoroutineRunner]");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<CoroutineRunner>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Start a coroutine safely from any context.
        /// </summary>
        public static Coroutine Run(IEnumerator routine)
        {
            return Instance.StartCoroutine(routine);
        }

        /// <summary>
        /// Stop a coroutine started with Run.
        /// </summary>
        public static void Stop(Coroutine coroutine)
        {
            if (_instance != null && coroutine != null)
            {
                _instance.StopCoroutine(coroutine);
            }
        }
    }
}