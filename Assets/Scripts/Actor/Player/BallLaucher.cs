using UnityEngine;

namespace Actor.Player
{
    public class BallLauncher : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private BallController ball;

        public void LaunchBall(Vector3 launchDirection)
        {
            if (launchDirection == Vector3.zero)
            {
                launchDirection = Vector3.forward;
            }

            ball.Launch(launchDirection);
        }
    }
}

