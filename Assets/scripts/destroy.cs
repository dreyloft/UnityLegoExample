using UnityEngine;
using MyPlayerController;

namespace destroyables
{
    public class destroy : MonoBehaviour
    {
        public GameObject[] spawnObjects;
        public Material[] materials;
        public int[] numberOfSpawns;
        public float[] posDiffY;
        public float[] posDiffXZ;
        public int health;
        public GameObject playerObject;
        
        private bool isTouching;
        private bool animate;
        private float lastHitTime;
        private float speed = 10.0f;
        private float amount = 10.0f;
        
        private int shakeCounter;
        private Vector3 startAngle;
        private Vector3 startPosition;
        private Vector3 playerRight;

        private playerController playerController;

        void Awake()
        {
            playerController = playerObject.GetComponent<playerController>();
        }

        private void Start()
        {
            lastHitTime = Time.time;

            for (int i = 0; i < spawnObjects.Length; i++)
            {
                if (numberOfSpawns[i] == 0)
                {
                    numberOfSpawns[i] = 1;
                }
            }

            startPosition = transform.position;
            startAngle = transform.eulerAngles;
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1"))
            {
                if (isTouching && Time.time > lastHitTime + 0.1f)
                {
                    health = health - 1;
                    lastHitTime = Time.time;
                    animate = true;
                    playerRight = playerController.vectorRight;
                }
            }

            if (animate && health >= 1.0f)
            {
                float shaker = amount * Mathf.Sin((Time.time - lastHitTime) * speed);
                
                transform.RotateAround(
                    new Vector3(
                        transform.position.x, 0.0f, transform.position.z),
                        playerRight,
                        Mathf.Sin(Time.time * 20));

                if (shaker <= 0.5f && shaker >= -0.5f)
                {
                    shakeCounter++;
                }

                if (shakeCounter > 1.0f)
                {
                    animate = false;
                    shakeCounter = 0;
                    transform.eulerAngles = startAngle;
                    transform.position = startPosition;
                }
            }

            if (health <= 0)
            {
                // Spawn new collectable objects
                for (int i = 0; i < spawnObjects.Length; i++)
                {
                    for (int j = 0; j < numberOfSpawns[i]; j++)
                    {
                        GameObject Spawn = Instantiate(
                            spawnObjects[i],
                            new Vector3(transform.position.x + Random.Range(-posDiffXZ[i], posDiffXZ[i]),
                                        transform.lossyScale.y / 2 + posDiffY[i],
                                        transform.position.z + Random.Range(-posDiffXZ[i], posDiffXZ[i])),
                            transform.rotation);
                        Spawn.GetComponent<Renderer>().material = materials[i];
                    }
                }

                // Destroy object
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            isTouching = true;
        }

        private void OnTriggerStay(Collider other)
        {
            isTouching = true;
        }


        private void OnTriggerExit(Collider other)
        {
            isTouching = false;
        }
    }
}
