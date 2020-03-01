using UnityEngine;

namespace MyDrawerController
{
    public class draweropener : MonoBehaviour
    {
        public Vector3 endPos;
        public float speed;
        public int id;

        public bool open;
        private Vector3 startPos;
        private float openTime;

        // Use this for initialization
        void Start()
        {
            startPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (open)
            {
                transform.position = Vector3.MoveTowards(transform.position, endPos, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            }

            if (openTime + 5.0f < Time.time)
            {
                open = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown("joystick button 1"))
            {
                openTime = Time.time;
                open = true;
            }
        }
    }
}
