using UnityEngine;

namespace _01_WaitForMouse
{
    public sealed class MousePolling : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Mouse Polling: I'm clicked!");
            }
        }
    }
}