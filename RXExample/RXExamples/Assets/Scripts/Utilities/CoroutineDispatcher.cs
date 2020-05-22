using UnityEngine;

namespace Utilities
{
    public sealed class CoroutineDispatcher : MonoBehaviour
    {
        public static CoroutineDispatcher Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            Instance = this;
        }
    }
}