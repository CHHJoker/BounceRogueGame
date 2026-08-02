using UnityEngine;

namespace Actor.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputHandler playerInputHandler;

        private void Awake()
        {
            playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            if (playerInputHandler == null)
                return;

            if (playerInputHandler.IsInputPressing())
            {

            }

            if (playerInputHandler.IsInputReleasedThisFrame())
            {

            }
        }
    }
}