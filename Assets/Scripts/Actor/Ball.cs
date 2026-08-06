using System.Collections;
using UnityEngine;

namespace Actor
{
    public class Ball : MonoBehaviour
    {
        [Header("Launch Settings")]
        [SerializeField] private float launchSpeed = 12f;
        [SerializeField] private float returnSpeed = 15f;

        [Header("Bottom Wall Collider")]
        [SerializeField] private Collider bottomWallCollider;

        private Rigidbody rb;

        private Vector3 currentVelocity;
        private Vector3 returnPosition;

        private bool isLaunched = false;
        public bool hasEnteredPlayArea = false;
        private bool isReturning = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (!isLaunched || isReturning) return;

            CheckIfEnteredPlayArea();

            EnforceBallYPosition();
            SetCurrentVelocity();
            PreventInfiniteLoopBounce();
            MaintainConstantSpeed();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isLaunched 
                && hasEnteredPlayArea
                && !isReturning
                && other == bottomWallCollider)
            {
                StartCoroutine(ReturnToTargetRoutine());
            }
        }

        public void Launch(Vector3 launchDirection, Vector3 returnPos)
        {
            if (isReturning) return;

            returnPosition = returnPos;
            rb.linearVelocity = launchDirection * launchSpeed;

            isLaunched = true;
            hasEnteredPlayArea = false;
            isReturning = false;
        }

        private void CheckIfEnteredPlayArea()
        {
            if (hasEnteredPlayArea || bottomWallCollider == null)
                return;
            
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

        private IEnumerator ReturnToTargetRoutine()
        {
            isReturning = true;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            while (Vector3.Distance(transform.position, returnPosition) > 0.05f)
            {
                Vector3 pos = returnPosition;
                pos.y = GameConstants.BALL_Y_POSITION;

                transform.position = Vector3.MoveTowards(transform.position, pos, returnSpeed * Time.deltaTime);
                yield return null;
            }

            Vector3 finalPos = returnPosition;
            finalPos.y = GameConstants.BALL_Y_POSITION;
            transform.position = finalPos;

            rb.isKinematic = false;
            isLaunched = false;
            hasEnteredPlayArea = false;
            isReturning = false;
        }
    }
}