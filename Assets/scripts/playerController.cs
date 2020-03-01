using UnityEngine;
using MyCameraController;
using Cutscene;

namespace MyPlayerController
{
    public class playerController : MonoBehaviour
    {
        public GameObject cameraObject;
        public GameObject levelDummie;
        public GameObject Head;
        public GameObject CameraFocusObject;
        public bool run;
        public float maxSpeed;
        public float yAngle;

        private bool blocked;
        private bool hitDirection;
        private float h;
        private float v;
        private float currentSpeed;
        private float resetTimer;
        private Animator animator;
        private cameraController cameraController;
        private level0 level0;
        public Vector3 direction;
        public Vector3 vectorRight;

        void Awake()
        {
            cameraController = cameraObject.GetComponent<cameraController>();
            level0 = levelDummie.GetComponent<level0>();
        }

        void Start()
        {
            animator = GetComponent<Animator>();
            transform.position = new Vector3(transform.position.x, transform.lossyScale.y / 2.0f - 0.1f, transform.position.z);
        }


        void FixedUpdate()
        {
            blocked = level0.blocked;

            if (!blocked) {
                h = Input.GetAxis("Horizontal");
                v = Input.GetAxis("Vertical");

                if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1") &&
                    !animator.GetCurrentAnimatorStateInfo(1).IsName("hit left") &&
                    !animator.GetCurrentAnimatorStateInfo(1).IsName("hit right"))
                {
                    if (hitDirection)
                    {
                        hitDirection = false;
                        animator.SetInteger("hit", 1);
                    }
                    else
                    {
                        hitDirection = true;
                        animator.SetInteger("hit", -1);
                    }

                    resetTimer = Time.time;
                }
                else
                {
                    if (resetTimer + 3.0f < Time.time)
                    {
                        hitDirection = true;
                    }
                    
                    animator.SetInteger("hit", 0);
                }

                if (h != 0.0f || v != 0.0f)
                {
                    direction = new Vector3(0.0f, (90 - Mathf.Atan2(v, h) * 180 / Mathf.PI) + cameraController.currentAngleY, 0.0f);
                    currentSpeed = Mathf.Clamp(new Vector2(h, v).sqrMagnitude, 0.0f, 0.5f);

                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey("joystick button 0"))
                    {
                        currentSpeed += 0.5f;
                        run = true;
                    }
                    else
                    {
                        run = false;
                    }
                }
                else
                {
                    currentSpeed = 0.0f;
                }

                CameraFocusObject.transform.position = Head.transform.position;
                animator.SetFloat("speed", currentSpeed);
                transform.eulerAngles = direction;
                transform.position += transform.forward * currentSpeed * maxSpeed * Time.deltaTime;
                yAngle = direction.y;
                vectorRight = transform.right;
            }
        }
    }
}
