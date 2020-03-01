using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class introArm : MonoBehaviour
{
    public GameObject Camera;
    public GameObject Viewport;

    public GameObject player;
    public GameObject Head;
    public GameObject[] disabledBodyParts;

    public GameObject glassBrick;
    public GameObject spawnPoint;
    public Material glassMaterial;
    public Material[] UICharacters;

    public GameObject[] clockObj;
    public GameObject[] bedsheet;

    public AudioClip[] sounds;

    public Canvas UIDialog;
    public Image UICharater;
    public Text UITextLine;

    private bool checkStoppedRunning;
    private bool stopped;
    private bool block;
    private bool once;
    private bool DoIt = true;
    private int subStep;
    private int state;
    private int clock;
    private float angle = -185.0f;
    private float pause;
    private float speed = 1.0f;
    
    private Vector3 destVector;
    private Vector3 endpos = new Vector3(1.342f, 0.436f, 0.086f);
    private Vector3 startpos;
    private Vector3 rotation = new Vector3(-185.0f, 315.0f, 0.0f);
    private Vector3 prevPos;
    private Vector3 originalCamPos;
    private Vector3 originalViewPortPos;

    private Animator animator;
    private Material[] temp;
    private Renderer rend;
    private AssetBundle myLoadedAssetBundle;

    void Start()
    {
        animator = player.GetComponent<Animator>();
        startpos = transform.position;
        transform.eulerAngles = rotation;
        GetComponent<MeshRenderer>().enabled = false;
        clockObj[1].GetComponent<Renderer>().enabled = false;
        Dialog("", -1, false);
    }


    private IEnumerator checkStopped()
    {
        checkStoppedRunning = true;

        while (!stopped)
        {
            Vector3 currPos = Camera.transform.position;
            if (prevPos == currPos)
            {
                stopped = true;
            }
            else
            {
                stopped = false;
            }
            yield return new WaitForSeconds(1.0f);
            prevPos = currPos;
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


    void Dialog(string Message, int character, bool show)
    {
        if (show)
        {
            UIDialog.GetComponent<Canvas>().enabled = true;
            UICharater.material = UICharacters[character];
            UITextLine.text = Message;
        }
        else
        {
            UIDialog.GetComponent<Canvas>().enabled = false;
            UITextLine.text = "";
        }
    }


    void Update()
    {
        // clock
        if ((int)Time.time % 2 == 0)
        {
            clockObj[2].GetComponent<Renderer>().enabled = true;
        }
        else
        {
            clockObj[2].GetComponent<Renderer>().enabled = false;
        }
        
        // states
        switch (state)
        {
            case 0:
                if (!checkStoppedRunning && !stopped)
                {
                    StartCoroutine(checkStopped());
                }

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

                if (clock >= 5)
                {
                    clockObj[0].GetComponent<Renderer>().enabled = false;
                    clockObj[1].GetComponent<Renderer>().enabled = true;

                    Dialog("*Schrilles Läuten*", 0, true);

                    if (!once)
                    {
                        //AudioSource.PlayClipAtPoint(sounds[0], clockObj[2].transform.position);
                        once = true;
                    }
                }

                if (clock >= 45)
                {
                    GetComponent<MeshRenderer>().enabled = true;
                    transform.position = Vector3.MoveTowards(
                            transform.position,
                            endpos,
                            2 * Time.deltaTime);
                }

                if (transform.position == endpos)
                {
                    sounds[0].UnloadAudioData();
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
                Dialog("", -1, false);
                GetComponent<MeshRenderer>().enabled = false;

                Viewport.transform.position = Vector3.MoveTowards(
                    Viewport.transform.position,
                    Head.transform.position - new Vector3(0.0f, 0.3f, -0.4f),
                    0.15f * Time.deltaTime);

                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    Head.transform.position - new Vector3(0.05f, 0.1f, -0.3f),
                    0.2f * Time.deltaTime);

                if (Camera.transform.position == Head.transform.position - new Vector3(0.05f, 0.1f, -0.3f) &&
                    Viewport.transform.position == Head.transform.position - new Vector3(0.0f, 0.3f, -0.4f))
                {
                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 1.0f <= Time.time)
                    {
                        state = 3;
                        once = false;
                    }
                }
                break;
            case 3:
                if (!once)
                {
                    //hide fake arm
                    GetComponent<MeshRenderer>().enabled = false;
                    once = true;
                    pause = 0;

                    temp = Head.GetComponent<Renderer>().materials;
                    temp[2].SetTextureOffset("_MainTex", new Vector2(0.5f, -0.25f));
                    GetComponent<MeshRenderer>().materials = temp;

                    Dialog("Hmpf ... So früh aufstehen ...", 1, true);
                }

                destVector = new Vector3(-0.5f, 1, -1.0f);
                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    destVector,
                    0.3f * Time.deltaTime);

                // show all bodyparts (arms)
                for (int i = 0; i < disabledBodyParts.Length; i++)
                {
                    disabledBodyParts[i].SetActive(true);
                }

                //bedsheet move to feet
                bedsheet[0].transform.position = Vector3.MoveTowards(
                    bedsheet[0].transform.position,
                    bedsheet[3].transform.position - new Vector3(0, 0, 0.475f),
                    (0.20f) * Time.deltaTime);

                bedsheet[1].transform.position = Vector3.MoveTowards(
                    bedsheet[1].transform.position,
                    bedsheet[3].transform.position - new Vector3(0, 0, 0.215f),
                    (0.15f) * Time.deltaTime);

                bedsheet[2].transform.position = Vector3.MoveTowards(
                    bedsheet[2].transform.position,
                    bedsheet[3].transform.position - new Vector3(0, 0, 0.1f),
                    (0.10f) * Time.deltaTime);

                //bedsheet back? all right!
                if (bedsheet[0].transform.position == bedsheet[3].transform.position - new Vector3(0, 0, 0.475f) &&
                    bedsheet[1].transform.position == bedsheet[3].transform.position - new Vector3(0, 0, 0.215f) &&
                    bedsheet[2].transform.position == bedsheet[3].transform.position - new Vector3(0, 0, 0.1f))
                {
                    state = 4;
                    once = false;
                }
                break;
            case 4:
                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    destVector,
                    0.3f * Time.deltaTime);

                animator.SetBool("standup", true);
                
                switch (subStep)
                {
                    case 0:
                        Vector3 dest = new Vector3(1.796f, 0.474f, -0.165f);

                        player.transform.position = Vector3.MoveTowards(
                            player.transform.position,
                            dest,
                            0.1f * Time.deltaTime);

                        if (player.transform.position == dest)
                        {
                            subStep = 1;
                        }
                        break;
                    case 1:
                        dest = new Vector3(1.341f, 0.474f, -0.165f);

                        player.transform.position = Vector3.MoveTowards(
                            player.transform.position,
                            dest,
                            0.15f * Time.deltaTime);

                        if (player.transform.position == dest)
                        {
                            subStep = 2;
                        }
                        break;
                }

                if (subStep == 2 && !animator.GetCurrentAnimatorStateInfo(0).IsName("standup"))
                {
                    //Dialog("", -1,false);
                    state = 5;
                    subStep = 0;
                    pause = 0;
                }
                break;
            case 5:
                /*
                float delay = 5.0f;

                Vector3[] walkline = new[] {
                    new Vector3( 0.88f, player.transform.position.y, -0.165f),
                    new Vector3( 0.50f, player.transform.position.y,  0.000f),
                    new Vector3( 0.45f, player.transform.position.y,  1.215f),
                    new Vector3( 0.45f, player.transform.position.y,  1.215f),
                    new Vector3( 0.35f, player.transform.position.y,  0.375f),
                    new Vector3(-0.60f, player.transform.position.y, -0.875f)
                };

                if ((subStep + 1) <= walkline.Length - 1)
                {
                    if (pause + delay <= Time.time)
                    {
                        player.transform.LookAt(walkline[subStep + 1]);
                    }
                }
                else
                {
                    player.transform.LookAt(new Vector3(-0.929f, 0.482f, -0.931f));
                }

                if (player.transform.position == walkline[subStep] && subStep < walkline.Length)
                {
                    subStep++;
                }

                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    walkline[subStep],
                    speed * Time.deltaTime);
                */
                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    new Vector3(-0.60f, player.transform.position.y, -0.875f),
                    speed * Time.deltaTime);

                animator.SetFloat("walkspeed", 0.5f);
                Dialog("Aber, was ist das?", 1, true);

                Viewport.transform.position = Vector3.MoveTowards(
                    Viewport.transform.position,
                    Head.transform.position,
                    Time.deltaTime);
                /*
                if (player.transform.position == walkline[2] && !once)
                {
                    pause = Time.time;
                    speed = 0;
                    animator.SetFloat("walkspeed", 0.0f);
                    player.transform.LookAt(walkline[subStep]);
                    Dialog("Au... Regal...", true);
                    once = true;
                }

                if (pause + delay <= Time.time)
                {
                    if (once)
                    {
                        Dialog("Was ist das?", true);
                    }
                    speed = 0.65f;
                    animator.SetFloat("walkspeed", 0.5f);
                }

                if (subStep >= 3)
                {
                    Camera.transform.position = Vector3.MoveTowards(
                        Camera.transform.position,
                        new Vector3(-1.0f, 1.275f, -1.45f),
                        0.5f * Time.deltaTime);
                }
                */
                player.transform.LookAt(new Vector3(-0.60f, player.transform.position.y, -0.875f));

                //if (player.transform.position == walkline[walkline.Length - 1])
                if(player.transform.position == new Vector3(-0.60f, player.transform.position.y, -0.875f))
                {
                    animator.SetFloat("walkspeed", 0.0f);
                    state = 6;
                    once = false;
                }
                break;
            case 6:
                if (!once)
                {
                    subStep = 0;
                    once = true;
                    pause = 0;
                }

                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    new Vector3(-1.0f, 1.0f, -0.9f),
                    1.0f * Time.deltaTime);

                Viewport.transform.position = Vector3.MoveTowards(
                    Viewport.transform.position,
                    new Vector3(-1.087f, 0.482f, -0.931f),
                    1.0f * Time.deltaTime);

                if (Viewport.transform.position == new Vector3(-1.087f, 0.482f, -0.931f) &&
                    Camera.transform.position == new Vector3(-1.0f, 1.0f, -0.9f))
                {
                    if (pause == 0)
                    {
                        Dialog("Achja, heute fängt mein neuer Job bei Meelogic an!", 1,true);
                        pause = Time.time;
                    }

                    if (pause + 3.0f <= Time.time)
                    {
                        once = false;
                        state = 7;
                    }
                }
                break;
            case 7:
                if (!once)
                {
                    once = true;
                    pause = 0;
                    animator.SetBool("fun", true);
                }

                temp = Head.GetComponent<Renderer>().materials;
                temp[1].SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
                temp[2].SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
                GetComponent<MeshRenderer>().materials = temp;

                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    new Vector3(-1.30f, 1.0f, -1.0f),
                    0.25f * Time.deltaTime);

                Viewport.transform.position = Vector3.MoveTowards(
                    Viewport.transform.position,
                    Head.transform.position - new Vector3(0.0f, 0.3f, 0.0f),
                    1.0f * Time.deltaTime);

                animator.SetBool("fun", true);

                if (Viewport.transform.position == Head.transform.position - new Vector3(0.0f, 0.3f, 0.0f) &&
                    Camera.transform.position == new Vector3(-1.30f, 1.0f, -1.0f))
                {
                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 3.0f <= Time.time)
                    {
                        animator.SetFloat("walkspeed", 0.0f);
                        once = false;
                        state = 8;
                    }
                }
                break;
            case 8:
                if (!once)
                {
                    Dialog("Da beeile ich mich lieber etwas.", 1,true);
                    once = true;
                    pause = 0;
                }

                if (pause == 0)
                {
                    pause = Time.time;
                }

                if (pause + 3.0f <= Time.time)
                {
                    animator.SetBool("fun", false);
                    once = false;
                    state = 9;
                    subStep = 0;
                }
                break;
            case 9:
                if (!once)
                {
                    once = true;
                    pause = 0;
                }

                switch (subStep) {
                    case 0:
                        Camera.transform.position = Vector3.MoveTowards(
                        Camera.transform.position,
                        new Vector3(0.63f, 1.0f, -2.23f),
                        1.25f * Time.deltaTime);

                        if (Camera.transform.position == new Vector3(0.63f, 1.0f, -2.23f))
                        {
                            subStep = 1;
                        }
                        break;
                    case 1:
                        Camera.transform.position = Vector3.MoveTowards(
                            Camera.transform.position,
                            new Vector3(0.63f, 1.0f, -3.75f),
                            1.25f * Time.deltaTime);

                        if (Camera.transform.position == new Vector3(0.63f, 1.0f, -3.75f))
                        {
                            subStep = 2;
                        }

                        Dialog("", -1, false);
                        break;
                }
                
                Viewport.transform.position = Vector3.MoveTowards(
                    Viewport.transform.position,
                    new Vector3(-7.5f, -1.5f, -4.0f),
                    2.0f * Time.deltaTime);

                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    new Vector3(-0.6f, 0.474f, 0.65f),
                    Time.deltaTime);
                
                if (subStep == 2 &&
                    Viewport.transform.position == new Vector3(-7.5f, -1.5f, -4.0f) &&
                    player.transform.position == new Vector3(-0.6f, 0.474f, 0.65f))
                {
                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 3.0f <= Time.time)
                    {
                        once = false;
                        state = 10;
                    }
                }
                break;
            case 10:
                if (!once)
                {
                    once = true;
                    pause = 0;

                    player.transform.position = new Vector3(-7.16f, -0.88f, -2.257f);
                    player.transform.LookAt(spawnPoint.transform.position);
                    player.transform.eulerAngles = new Vector3(0.0f, player.transform.eulerAngles.y, 0.0f);

                    temp = Head.GetComponent<Renderer>().materials;
                    temp[1].SetTextureOffset("_MainTex", new Vector2(0.0f, -0.25f));
                    GetComponent<MeshRenderer>().materials = temp;
                    Dialog("Ah Treppe!", 1, true);
                }

                animator.SetBool("falling", true);

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("falling")) {
                    player.transform.position = Vector3.MoveTowards(
                        player.transform.position,
                        new Vector3(
                            player.transform.position.x,
                            player.transform.position.y,
                            -3.659f),
                        2.5f * Time.deltaTime);

                    if (DoIt)
                    {
                        for (int i = 0; i < 75; i++)
                        {
                            float posDiffXZ = 1.0f;

                            GameObject Spawn = Instantiate(
                                glassBrick,
                                new Vector3(spawnPoint.transform.position.x + Random.Range(0.0f, posDiffXZ),
                                            spawnPoint.transform.position.y,
                                            spawnPoint.transform.position.z + Random.Range(-posDiffXZ, posDiffXZ)),
                                spawnPoint.transform.rotation);

                            Spawn.GetComponent<Renderer>().material = glassMaterial;
                            Material myMaterial = Spawn.GetComponent<MeshRenderer>().material;
                            myMaterial.SetInt("_ZWrite", 1);
                            if (i > 73)
                            {
                                DoIt = false;
                            }
                        }
                    }
                }
                
                if (player.transform.position.z == -3.659f)
                {
                    animator.SetBool("falling", false);

                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 3.0f <= Time.time)
                    {
                        state = 11;
                        subStep = 0;
                        once = false;
                        Dialog("", -1, false);
                        once = false;
                    }
                }
                break;
            case 11:
                if (!once)
                {
                    once = true;
                    pause = 0;
                }

                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    new Vector3(-6.6f, -1.5f, -4.0f),
                    5.0f * Time.deltaTime);

                Viewport.transform.position = player.transform.position - new Vector3(0.0f, 0.25f, 0.3f);

                if (Camera.transform.position == new Vector3(-6.6f, -1.5f, -4.0f))
                {
                    Dialog("Och nö ...", 1,true);

                    if (pause == 0)
                    {
                        pause = Time.time;
                    }

                    if (pause + 3.0f <= Time.time)
                    {
                        Dialog("", -1, false);
                        once = false;
                        state = 12;
                    }
                }
                break;
            case 12:
                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    new Vector3(player.transform.position.x,
                        -3.44f, 
                        player.transform.position.z),
                    3.0f * Time.deltaTime);

                if(player.transform.position.y == -3.44f)
                {
                    state = 13;
                }
                break;
            case 13:
                if (!once)
                {
                    once = true;
                    pause = Time.time;
                    originalCamPos = Camera.transform.position;
                    originalViewPortPos = Viewport.transform.position;
                }
                
                if (pause + 0.25f <= Time.time)
                {
                    Camera.transform.position = originalCamPos;
                    Viewport.transform.position = originalViewPortPos;
                    once = false;
                    state = 14;
                    Dialog("Aua ...", 1,true);
                }
                else
                {
                    float range = 0.1f;

                    Vector3 randomVector = new Vector3(
                            Random.Range(-range, range),
                            Random.Range(-range, range),
                            Random.Range(-range, range));

                    Camera.transform.position = originalCamPos + randomVector;
                    Viewport.transform.position = originalViewPortPos + randomVector;
                }
                break;
            case 14:
                if (!once)
                {
                    once = true;
                    pause = Time.time;
                }

                if (pause + 3.0f <= Time.time)
                {
                    once = false;
                    state = 15;
                    Dialog("", -1,false);
                }
                break;
            case 15:
                Viewport.transform.position = Vector3.MoveTowards(
                    Viewport.transform.position,
                    originalViewPortPos + new Vector3(0.0f, -2.0f, 0.0f),
                    0.5f * Time.deltaTime);

                Camera.transform.position = Vector3.MoveTowards(
                    Camera.transform.position,
                    originalCamPos + new Vector3(0.0f, 0.0f, -1.0f),
                    0.5f * Time.deltaTime);

                if (Camera.transform.position == originalCamPos + new Vector3(0.0f, 0.0f, -1.0f) &&
                    Viewport.transform.position == originalViewPortPos + new Vector3(0.0f, -2.0f, 0.0f))
                {
                    state = 16;
                }
                break;
            case 16:
                if (!once)
                {
                    once = true;
                    pause = Time.time;

                    temp = Head.GetComponent<Renderer>().materials;
                    temp[1].SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
                    GetComponent<MeshRenderer>().materials = temp;

                    Dialog("Jetzt aber los!", 1,true);
                }

                animator.SetBool("falling", false);
                animator.SetBool("turn", true);

                if (pause + 1.0f <= Time.time)
                {
                    once = false;
                    state = 17;
                }
                break;
            case 17:
                if (!once)
                {
                    once = true;
                    pause = Time.time;
                }

                animator.SetBool("turn", true);

                if (pause + 1.0f <= Time.time)
                {
                    once = false;
                    state = 18;
                }
                break;
            case 18:
                animator.SetFloat("walkspeed", 1.0f);

                player.transform.position = Vector3.MoveTowards(
                    player.transform.position,
                    player.transform.position + new Vector3(-1.0f, 0.0f, 0.0f),
                    1.5f * Time.deltaTime);

                player.transform.eulerAngles = new Vector3(0.0f, -90.0f, 0.0f); 

                if (player.transform.position.x <= -15.0f)
                {
                    Dialog("", -1, false);
                    SceneManager.LoadScene("office");
                }
                break;
        }
    }
}
