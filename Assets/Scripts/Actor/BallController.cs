using UnityEngine;

namespace Actor
{
    public class BallController : MonoBehaviour
    {
        [Header("Launch Settings")]
        [SerializeField] private float launchSpeed = 12f;

        [Header("Bottom Wall Collider")]
        [SerializeField] private Collider bottomWallCollider;

        private Rigidbody rb;

        private Vector3 currentVelocity;

        private bool isLaunched = false;
        public bool hasEnteredPlayArea = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!isLaunched) return;

            CheckIfEnteredPlayArea();

            EnforceBallYPosition();
            SetCurrentVelocity();
            PreventInfiniteLoopBounce();
            MaintainConstantSpeed();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isLaunched && hasEnteredPlayArea && other == bottomWallCollider)
            {
                ResetBall();
            }
        }

        public void Launch(Vector3 launchDirection)
        {
            rb.linearVelocity = launchDirection * launchSpeed;

            isLaunched = true;
            hasEnteredPlayArea = false;
        }

        private void CheckIfEnteredPlayArea()
        {
            if (hasEnteredPlayArea || bottomWallCollider == null) return;
            
            float upperEdgeZ = bottomWallCollider.bounds.max.z;
            if (transform.position.z > upperEdgeZ)
            {
                hasEnteredPlayArea = true;
            }
        }

        private void EnforceBallYPosition()
        {
            Vector3 pos = transform.position;

            if (!Mathf.Approximately(pos.y, GameConstants.BALL_Y_POSITION))
            {
                pos.y = GameConstants.BALL_Y_POSITION;
                transform.position = pos;
            }
        }

        private void SetCurrentVelocity()
        {
            currentVelocity = rb.linearVelocity;
            currentVelocity.y = 0f;
        }

        private void PreventInfiniteLoopBounce()
        {
            if (Mathf.Abs(currentVelocity.x) < 0.15f && currentVelocity.z != 0)
            {
                currentVelocity.x = (currentVelocity.x >= 0 ? 0.2f : -0.2f);
            }


            if (Mathf.Abs(currentVelocity.z) < 0.15f && currentVelocity.x != 0)
            {
                currentVelocity.z = (currentVelocity.z >= 0 ? 0.2f : -0.2f);
            }
        }

        private void MaintainConstantSpeed()
        {
            rb.linearVelocity = currentVelocity.normalized * launchSpeed;
        }

        private void ResetBall()
        {
            isLaunched = false;
            hasEnteredPlayArea = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}