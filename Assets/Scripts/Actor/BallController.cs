using UnityEngine;

namespace Actor
{
    public class BallController : MonoBehaviour
    {
        [Header("Launch Settings")]
        [SerializeField] private float launchSpeed = 12f;

        private Rigidbody rb;

        private Vector3 currentVelocity;

        private bool isLaunched = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!isLaunched) return;

            EnforceBallYPosition();
            SetCurrentVelocity();
            PreventInfiniteLoopBounce();
            MaintainConstantSpeed();
        }

        public void Launch(Vector3 launchDirection)
        {
            rb.linearVelocity = launchDirection * launchSpeed;

            isLaunched = true;
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
    }
}