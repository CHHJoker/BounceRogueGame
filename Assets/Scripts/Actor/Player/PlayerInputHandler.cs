using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public bool IsInputPressing()
        {
            bool isMousePressed = Mouse.current != null && Mouse.current.leftButton.isPressed;
            bool isTouchPressed = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed;

            return isMousePressed || isTouchPressed;
        }

        public bool IsInputReleasedThisFrame()
        {
            bool mouseReleased = Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame;
            bool touchReleased = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasReleasedThisFrame;

            return mouseReleased || touchReleased;
        }

        public Vector2? GetMouseScreenPos()
        {
            if (Mouse.current == null) return null;

            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            return mouseScreenPos;

        }
    }
}