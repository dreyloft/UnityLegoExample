using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro0 : MonoBehaviour {
    public GameObject Camera;
    public GameObject clockDelimiter;
    public GameObject clock759;
    public GameObject clock800;
    private Vector3 endpos = new Vector3(1.342f, 0.436f, 0.086f);
    private Vector3 startpos;
    private Vector3 rotation = new Vector3(-185.0f, 315.0f, 0.0f);
    private Vector3 pos;
    private float speed = 2.0f;
    private float angle = -185.0f;
    private bool stopped;
    private bool block;
    private int state = 0;
    private int clock = 795;
    public GameObject CamSpot;
    public AudioClip alarmclock;
    public GameObject Face;
    public GameObject FaceNew;
    private bool once;
    public GameObject note;
    private Renderer rend;
    private float pause;
    public GameObject sheet;
    public GameObject[] body;
    public GameObject player;
    public GameObject playerNew;
    private Material[] temp;
    private int step;

    void Start()
    {
        startpos = transform.position;
        transform.eulerAngles = rotation;
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(checkStopped());
        clock800.GetComponent<Renderer>().enabled = false;
        note.GetComponent<MeshRenderer>().enabled = false;
        playerNew.SetActive(false);
    }

    void Update()
    {
        if ((int)Time.time % 2 == 0)
        {
            clockDelimiter.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            clockDelimiter.GetComponent<Renderer>().enabled = false;
        }

        switch (state)
        {
            case 0:
                if (stopped && (Time.time >= 10.0f))
                {
                    if ((int)Time.time % 2 == 0 && block == false)
                    {
                        block = true;
                        clock++;
                    }
                    else
                    {
                        block = false;
                    }
                }

                if (clock == 800)
                {
                    clock759.GetComponent<Renderer>().enabled = false;
                    clock800.GetComponent<Renderer>().enabled = true;

                    if (!once)
                    {
                        //AudioSource.PlayClipAtPoint(alarmclock, clockDelimiter.transform.position);
                        once = true;
                    }
                }

                if (clock >= 850)
                {
                    GetComponent<MeshRenderer>().enabled = true;
                    transform.position = Vector3.MoveTowards(
                            transform.position,
                            endpos,
                            speed * Time.deltaTime);
                }

                if (transform.position == endpos)
                {
                    alarmclock.UnloadAudioData();
                    StartCoroutine(turn());
                    transform.eulerAngles = new Vector3(angle, rotation.y, rotation.z);
                }
                break;
            case 1:
                GetComponent<MeshRenderer>().enabled = true;
                transform.eulerAngles = new Vector3(angle, rotation.y, rotation.z);
                transform.position = Vector3.MoveTowards(
                        transform.position,
                        startpos,
                        0.3f * Time.deltaTime);
                break;
            case 2:
                GetComponent<MeshRenderer>().enabled = false;
                CamSpot.transform.position = Vector3.MoveTowards(
                    CamSpot.transform.position,
                    Face.transform.position - new Vector3(0.0f, 0.3f, 0.0f),
                    0.15f * Time.deltaTime);
                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    Face.transform.position - new Vector3(0.0f, -0.5f, 0.5f),
                    0.2f * Time.deltaTime);

                if (Camera.transform.position == Face.transform.position - new Vector3(0.0f, -0.5f, 0.5f) &&
                    CamSpot.transform.position == Face.transform.position - new Vector3(0.0f, 0.3f, 0.0f))
                {
                    state = 3;
                }
                break;
            case 3:
                transform.position = new Vector3(2.017f, 0.436f, -0.157f);
                transform.eulerAngles = new Vector3(-185, 315, 0);

                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    Face.transform.position - new Vector3(0.0f, -0.1f, 0.0f),
                    0.15f * Time.deltaTime);
                CamSpot.transform.position = new Vector3(1.869f, 0.729f, 0.072f);

                if (Camera.transform.position.y < CamSpot.transform.position.y)
                {
                    transform.eulerAngles = new Vector3(-159.125f, 328.454f, -89.013f);
                }

                if (Camera.transform.position == Face.transform.position - new Vector3(0.0f, -0.1f, 0.0f))
                {
                    state = 4;
                    once = false;
                }
                break;
            case 4:
                if (!once)
                {
                    GetComponent<MeshRenderer>().enabled = true;
                    note.GetComponent<MeshRenderer>().enabled = true;
                    transform.eulerAngles = new Vector3(-202, 314.5f, -28);
                    transform.position = new Vector3(2.017f, 0.0f, -0.186f);
                    pause = 0;
                    once = true;
                }

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(2.017f, 0.436f, -0.157f),
                    0.3f * Time.deltaTime);

                if (transform.position == new Vector3(2.017f, 0.436f, -0.157f))
                {
                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 2.0f <= Time.time)
                    {
                        once = false;
                        state = 5;
                    }
                }

                break;
            case 5:
                if (!once)
                {
                    pause = 0;
                    once = true;
                }

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(2.017f, -0.186f, -0.157f),
                    0.3f * Time.deltaTime);

                if (transform.position == new Vector3(2.017f, -0.186f, -0.157f))
                {
                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 0.0f <= Time.time)
                    {
                        once = false;
                        state = 6;
                    }
                }
                break;
            case 6:
                if (!once)
                {
                    temp = Face.GetComponent<Renderer>().materials;
                    temp[1].SetTextureOffset("_MainTex", new Vector2(0, 0));
                    temp[2].SetTextureOffset("_MainTex", new Vector2(0, 0));
                    GetComponent<MeshRenderer>().materials = temp;

                    GetComponent<MeshRenderer>().enabled = false;
                    note.GetComponent<MeshRenderer>().enabled = false;
                    pause = 0;
                    once = true;
                }

                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    new Vector3(1.95f, 1.0f, -0.1f),
                    0.5f * Time.deltaTime);
                CamSpot.transform.position = Vector3.MoveTowards(
                    CamSpot.transform.position,
                    Face.transform.position - new Vector3(-0.075f, -0.1f, 0.3f),
                    0.10f * Time.deltaTime);

                if (CamSpot.transform.position == Face.transform.position - new Vector3(-0.075f, -0.1f, 0.3f) &&
                    Camera.transform.position == new Vector3(1.95f, 1.0f, -0.1f))
                {
                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 2.0f <= Time.time)
                    {
                        once = false;
                        state = 7;
                    }
                }
                break;
            case 7:
                if (!once)
                {
                    pause = 0;
                    once = true;
                }

                sheet.transform.position = Vector3.MoveTowards(
                    sheet.transform.position,
                    new Vector3(sheet.transform.position.x, sheet.transform.position.y, -1.228f),
                    10 * Time.deltaTime);

                if (sheet.transform.position != new Vector3(sheet.transform.position.x, sheet.transform.position.y, -1.228f))
                {
                    for (int i = 0; i < body.Length; i++)
                    {
                        body[i].GetComponent<SkinnedMeshRenderer>().enabled = true;
                    }
                    sheet.transform.Rotate(Vector3.right * 120 * Time.deltaTime);
                }
                else
                {
                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 2.0f <= Time.time)
                    {
                        once = false;
                        state = 8;
                    }
                }
                break;
            case 8:
                if (!once)
                {
                    temp = FaceNew.GetComponent<Renderer>().materials;
                    temp[1].SetTextureOffset("_MainTex", new Vector2(0, 0));
                    temp[2].SetTextureOffset("_MainTex", new Vector2(0, 0));
                    GetComponent<MeshRenderer>().materials = temp;
                    pause = 0;
                    once = true;
                    playerNew.GetComponent<MeshRenderer>().enabled = true;
                    step = 0;
                }
                playerNew.SetActive(true);
                Destroy(player);

                switch (step)
                {
                    case 0:
                        Camera.transform.position = Vector3.MoveTowards(
                            Camera.transform.position,
                            new Vector3(3.5f, Camera.transform.position.y, Camera.transform.position.z),
                            0.75f * Time.deltaTime);

                        if (Camera.transform.position.x == 3.5f)
                        {
                            step = 1;
                        }
                        break;
                    case 1:
                        Camera.transform.position = Vector3.MoveTowards(
                            Camera.transform.position,
                            new Vector3(Camera.transform.position.x, Camera.transform.position.y, -3.5f),
                            0.75f * Time.deltaTime);

                        if (Camera.transform.position.z == -3.5f)
                        {
                            step = 2;
                        }
                        break;
                    case 2:
                        Camera.transform.position = Vector3.MoveTowards(
                            Camera.transform.position,
                            new Vector3(0.75f, Camera.transform.position.y, Camera.transform.position.z),
                            0.75f * Time.deltaTime);

                        if (Camera.transform.position.x == 0.75f)
                        {
                            step = 3;
                        }
                        break;
                }

                Debug.Log(playerNew.transform.rotation.eulerAngles.x);
                if (playerNew.transform.rotation.eulerAngles.x > 1)
                {
                    playerNew.transform.Rotate(Vector3.right * 40 * Time.deltaTime);
                }
                if (step == 3)
                {
                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 2.0f <= Time.time)
                    {
                        once = false;
                        state = 8;
                    }
                }
                break;
            case 9:
                playerNew.transform.position = new Vector3(0.65f, 0.475f, -0.175f);
                playerNew.transform.eulerAngles = new Vector3(0, -135, 0);

                if (pause == 0)
                {
                    pause = Time.time;
                }

                if (pause + 2.0f <= Time.time)
                {
                    once = false;
                    state = 8;
                }
                break;
        }
    }

    private IEnumerator checkStopped()
    {
        while (true)
        {
            Vector3 currPos = Camera.transform.position;
            if (pos == currPos)
            {
                stopped = true;
            }
            else
            {
                stopped = false;
            }
            yield return new WaitForSeconds(1.0f);
            pos = currPos;
        }
    }

    private IEnumerator turn()
    {
        while (angle >= -185.0f && angle < -175.0f)
        {
            angle = angle + 0.2f;
            yield return new WaitForSeconds(0.1f);
        }

        state = 1;

        while (angle >= -175.0f && angle < -85.0f)
        {
            angle = angle + 0.5f;
            yield return new WaitForSeconds(0.1f);
        }

        while (transform.position != startpos)
        {
            transform.position = Vector3.MoveTowards(
                            transform.position,
                            startpos,
                            0.25f * Time.deltaTime);
        }

        state = 2;
    }
}
