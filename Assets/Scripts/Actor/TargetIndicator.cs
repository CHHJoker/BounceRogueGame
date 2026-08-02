using UnityEngine;

namespace Actor
{
    public class TargetIndicator : MonoBehaviour
    {
        [SerializeField] private Transform targetPoint;
        [SerializeField] private LineRenderer aimLine;

        [Header("Movement Smoothness (Optional)")]
        [SerializeField] private bool useSmoothing = false;
        [SerializeField] private float smoothSpeed = 15f;

        private Camera mainCamera;
        public Vector3 CurrentTargetPointPosition { get; private set; }

        private Plane stageGroundPlane;
        private float maxLineLength = 15f;

        private void Awake()
        {
            if (targetPoint == null || aimLine == null)
            {
                return;
            }

            mainCamera = Camera.main;

            stageGroundPlane = new Plane(Vector3.up, new Vector3(0f, GameConstants.GROUND_Y_POSITION, 0f));
        }


        public void MoveTargetPoint(Vector2 mouseScreenPos)
        {
            Ray ray = mainCamera.ScreenPointToRay(mouseScreenPos);

            if (stageGroundPlane.Raycast(ray, out float enterDistance))
            {
                Vector3 worldHitPoint = ray.GetPoint(enterDistance);
                worldHitPoint.y = GameConstants.GROUND_Y_POSITION;

                CurrentTargetPointPosition = worldHitPoint;

                if (useSmoothing)
                {
                    targetPoint.position = Vector3.Lerp(
                        targetPoint.position,
                        worldHitPoint,
                        Time.deltaTime * smoothSpeed
                    );
                }
                else
                {
                    targetPoint.position = worldHitPoint;
                }
            }
        }

        public void UpdateAimLine(Vector3 startPos, Vector3 targetPos, Vector3 launchDirection)
        {
            float distanceToTarget = Vector3.Distance(startPos, targetPos);
            float lineLength = Mathf.Min(distanceToTarget, maxLineLength);
            Vector3 endPos = startPos + launchDirection * lineLength;

            aimLine.SetPosition(0, startPos);
            aimLine.SetPosition(1, endPos);
        }
    }
}