using UnityEngine;

public class trackingShot : MonoBehaviour
{
    public GameObject CamSpot;
    public GameObject[] TrackSpot;
    public float StartSpeed;
    public float[] SpeedChange;
    public float[] Pause;

    private float currSpeed;
    private int trackpoint = -1;
    
	void Start () {
        currSpeed = StartSpeed;
        Pause[0] = Pause[0] + Time.time;
    }

    // Update is called once per frame
    void FixedUpdate() {
        bool move = false;

        if (trackpoint + 1 < TrackSpot.Length) {
            if (Pause[trackpoint + 1] <= Time.time)
            {
                move = true;
            }
        }
        else
        {
            move = true;
        }

        if (move)
        {
            if (trackpoint == -1)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    TrackSpot[0].transform.position,
                    currSpeed * Time.deltaTime);
            }
            else if (trackpoint + 1 < TrackSpot.Length)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    TrackSpot[trackpoint + 1].transform.position,
                    currSpeed * Time.deltaTime);
            }
        }

        if (CamSpot != null)
        {
            transform.LookAt(CamSpot.transform.position);
        }

        if (trackpoint + 1 != TrackSpot.Length)
        {
            if (Vector3.Distance(transform.position, TrackSpot[trackpoint + 1].transform.position) < 0.1f) {
                trackpoint++;

                if (trackpoint + 1 < TrackSpot.Length)
                {
                    Pause[trackpoint + 1] = Pause[trackpoint + 1] + Time.time;
                }

                if (trackpoint < TrackSpot.Length)
                {
                    if (SpeedChange[trackpoint] != 0)
                    {
                        currSpeed = SpeedChange[trackpoint];
                    }
                }
            }
        }
    }     
}
