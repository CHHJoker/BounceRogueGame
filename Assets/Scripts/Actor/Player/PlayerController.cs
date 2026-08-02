using UnityEngine;

namespace Actor.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TargetIndicator targetIndicator;

        private PlayerInputHandler playerInputHandler;

        private Vector3 startPos;
        private Vector3 targetPos;
        private Vector3 launchDirection;

        private void Awake()
        {
            playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            if (playerInputHandler == null
                || targetIndicator == null)
                return;

            if (playerInputHandler.IsInputPressing())
            {
                UpdateAiming();
            }

            if (playerInputHandler.IsInputReleasedThisFrame())
            {

            }
        }

        private void UpdateAiming()
        {
            Vector2? mouseScreenPos = playerInputHandler.GetMouseScreenPos();

            if (mouseScreenPos.HasValue)
            {
                targetIndicator.MoveTargetPoint(mouseScreenPos.Value);
                CalculateLaunchDirection();
                targetIndicator.UpdateAimLine(startPos, targetPos, launchDirection);
            }
        }

        private void CalculateLaunchDirection()
        {
            startPos = transform.position;
            startPos.y = GameConstants.GROUND_Y_POSITION;

            targetPos = targetIndicator.CurrentTargetPointPosition;
            targetPos.y = GameConstants.GROUND_Y_POSITION;

            launchDirection = (targetPos - startPos).normalized;
        }
    }
}