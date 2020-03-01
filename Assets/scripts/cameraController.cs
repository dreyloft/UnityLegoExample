using UnityEngine;
using MyPlayerController;
using Cutscene;

namespace MyCameraController
{
    public class cameraController : MonoBehaviour
    {
        public GameObject playerObject;
        public GameObject levelDummie;
        public float cameraDistance;
        public float cameraHeight;
        public float distanceTolerance;
        public float manualRotationSpeed;
        public float currentAngleY;

        private bool blocked;
        private bool toleranceTrigger;
        private float currentDistance;
        private float hypotenuseDistance;
        private float cameraMovementSpeed;
        private playerController playerController;
        private level0 level0;

        void Awake()
        {
            playerController = playerObject.GetComponent<playerController>();
            level0 = levelDummie.GetComponent<level0>();
        }

        void Start()
        {
            hypotenuseDistance = Mathf.Sqrt(Mathf.Pow(cameraDistance, 2) + Mathf.Pow(cameraHeight, 2));
        }

        void FixedUpdate()
        {
            blocked = level0.blocked;

            if (!blocked)
            {
                if (playerController.run)
                {
                    cameraMovementSpeed = playerController.maxSpeed;
                }
                else
                {
                    cameraMovementSpeed = playerController.maxSpeed / 2.0f;
                }

                currentDistance = Vector3.Distance(playerObject.transform.position, transform.position);

                if ((currentDistance - distanceTolerance) > hypotenuseDistance || toleranceTrigger)
                {
                    transform.position += transform.forward * ((currentDistance / 3) * cameraMovementSpeed) * Time.deltaTime;
                    toleranceTrigger = true;

                    if (currentDistance <= hypotenuseDistance)
                    {
                        toleranceTrigger = false;
                    }
                }
                else if (currentDistance <= cameraHeight + distanceTolerance)
                {
                    if (currentDistance <= cameraHeight + 0.075f)
                    {
                        transform.position -= transform.forward * ((currentDistance / 2.0f) * cameraMovementSpeed) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position -= transform.forward * ((currentDistance / 3.0f) * cameraMovementSpeed) * Time.deltaTime;
                    }
                }

                // manual camera turn
                if (Input.GetKey("joystick button 4") || Input.GetKey(KeyCode.Q))
                {
                    transform.Translate(Vector3.right * manualRotationSpeed * Time.deltaTime);
                }

                if (Input.GetKey("joystick button 5") || Input.GetKey(KeyCode.E))
                {
                    transform.Translate(Vector3.right * -manualRotationSpeed * Time.deltaTime);
                }

                transform.position = new Vector3(transform.position.x, cameraHeight, transform.position.z);
                transform.LookAt(playerObject.transform.position + new Vector3(0.0f, playerObject.transform.lossyScale.y - 0.1f, 0.0f));
                currentAngleY = transform.eulerAngles.y;
            }
        }
    }
}
