using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyPlayerController;
using MyCameraController;
using MyDrawerController;
using UnityEngine.SceneManagement;

namespace Cutscene
{
    public class level0 : MonoBehaviour
    {
        public bool blocked;
        public bool SafeOpen;
        [Header("Player Settings")]
        public GameObject Player;
        public GameObject Head;
        [Tooltip("PlayerTrack = green blocks")]
        public GameObject[] PlayerTrack;

        public GameObject[] TriggerObjects;
 
        [Header("Camera Settings")]
        public GameObject MainCamera;
        public GameObject FocusPoint;
        public GameObject[] FocusTrack;
        [Tooltip("PlayerTrack = red blocks")]
        public GameObject[] CameraTrack;

        [Header("Dialog Settings")]
        public Canvas UIDialog;
        public Image UICharater;
        public Text UITextLine;
        public Material[] UICharacters;

        [Header("Black Screen for fading")]
        public Canvas blackScreenCanvas;
        public Image blackScreen;

        private Animator animator;
        private Animator solveigAnimator;
        private Animator steffenAnimator;
        private Animator ayanaAnimator;
        private Animator sonjaAnimator;
        private playerController playerController;
        private cameraController cameraController;
        private draweropener Draweropener0;
        private draweropener Draweropener1;
        private draweropener Draweropener2;
        private bool once = true;
        private bool[] checker = { false, false, false };
        private bool[] zones = { false, false, false, false, false, false, false };
        private int levelState;
        private int levelPart;
        private int temp;
        private int UIimage;
        private float tempTime;
        private float pause;
        private Material[] faceTexture;
        private Material[] faceTexture2;
        private Vector3[] tempPositions = new Vector3[3];
        

        private void Awake()
        {
            Draweropener0 = TriggerObjects[5].GetComponent<draweropener>();
            Draweropener1 = TriggerObjects[6].GetComponent<draweropener>();
            Draweropener2 = TriggerObjects[7].GetComponent<draweropener>();
        }

        void Start()
        {
            // blocks user controls
            blocked = true;

            Dialog("", "", false);
            GetComponent<MeshRenderer>().enabled = false;
            FocusPoint.GetComponent<MeshRenderer>().enabled = false;
            animator = Player.GetComponent<Animator>();
            solveigAnimator = TriggerObjects[0].GetComponent<Animator>();
            steffenAnimator = TriggerObjects[21].GetComponent<Animator>();
            sonjaAnimator = TriggerObjects[23].GetComponent<Animator>();
            ayanaAnimator = TriggerObjects[22].GetComponent<Animator>();
            blackScreenCanvas.GetComponent<Canvas>().enabled = false;

            // rainbow
            TriggerObjects[1].transform.eulerAngles = new Vector3(-90.0f, -30.217f, -90.0f);
            TriggerObjects[1].GetComponent<MeshRenderer>().enabled = false;
            // Unikitty
            TriggerObjects[2].transform.eulerAngles = new Vector3(-180.0f, -149.783f, 0.0f);

            // hide player track points
            for (int i = 0; i < PlayerTrack.Length; i++)
            {
                PlayerTrack[i].GetComponent<MeshRenderer>().enabled = false;
            }

            // hide Camera track points
            for (int i = 0; i < CameraTrack.Length; i++)
            {
                CameraTrack[i].GetComponent<MeshRenderer>().enabled = false;
            }

            // hide Trigger Points (code note)
            for (int i = 0; i < 8; i++)
            {
                TriggerObjects[8 + i].GetComponent<MeshRenderer>().enabled = false;
            }
            
            //levelPart = 3;
        }
    
        void Dialog(string Message, string character, bool show)
        {
            if (show)
            {
                switch (character)
                {
                    case "player":
                        UIimage = 0;
                        break;
                    case "solveig":
                        UIimage = 1;
                        break;
                    case "ayana":
                        UIimage = 2;
                        break;
                    default:
                        UIimage = 0;
                        break;
                }

                UIDialog.GetComponent<Canvas>().enabled = true;
                UICharater.material = UICharacters[UIimage];
                UITextLine.text = Message;
            }
            else
            {
                UIDialog.GetComponent<Canvas>().enabled = false;
                UITextLine.text = "";
            }
        }

        void FadeToBlack()
        {
            blackScreenCanvas.GetComponent<Canvas>().enabled = true;
            blackScreen.color = Color.black;
            blackScreen.canvasRenderer.SetAlpha(0.0f);
            blackScreen.CrossFadeAlpha(1.0f, 1.5f, false);
        }

        void FadeFromBlack()
        {
            blackScreenCanvas.GetComponent<Canvas>().enabled = true;
            blackScreen.color = Color.black;
            blackScreen.canvasRenderer.SetAlpha(1.0f);
            blackScreen.CrossFadeAlpha(0.0f, 1.5f, false);
        }

        // Update is called once per frame
        void Update () {
            //Debug.Log(levelPart + " : " + levelState);
            TriggerObjects[17].GetComponent<MeshRenderer>().enabled = true;

	        switch (levelPart)
            {
                // intro
                case 0:
                    switch (levelState)
                    {
                        case 0:
                            // Player moves to entry                        
                            if (once)
                            {
                                temp = 0;
                                once = false;
                            }

                            // start walk animation
                            animator.SetFloat("speed", 0.5f);
                            
                            Player.transform.position = Vector3.MoveTowards(
                                Player.transform.position,
                                new Vector3(PlayerTrack[temp].transform.position.x, Player.transform.position.y, PlayerTrack[temp].transform.position.z),
                                0.5f * Time.deltaTime);

                            // Camera movement
                            if (temp >= 1) {
                                MainCamera.transform.position = Vector3.MoveTowards(
                                    MainCamera.transform.position,
                                    new Vector3(2.5f, MainCamera.transform.position.y, MainCamera.transform.position.z),
                                    0.5f * Time.deltaTime);
                            }
                            MainCamera.transform.LookAt(Player.transform.position + new Vector3(0.0f, 0.3f, 0.0f));
                            FocusPoint.transform.position = Head.transform.position;

                            if (Vector3.Distance(Player.transform.position, new Vector3(PlayerTrack[temp].transform.position.x, Player.transform.position.y, PlayerTrack[temp].transform.position.z)) < 0.1f && temp != PlayerTrack.Length)
                            {
                                temp++;
                            }

                            // next phase of cutscene
                            if (temp >= PlayerTrack.Length)
                            {
                                temp = 0;
                                once = true;
                                levelState = 1;
                            }
                            else
                            {
                                if (temp <= PlayerTrack.Length)
                                {
                                    Player.transform.LookAt(PlayerTrack[temp].transform.position);
                                    Player.transform.eulerAngles = new Vector3(0.0f, Player.transform.eulerAngles.y, 0.0f);
                                }
                                else
                                {
                                    Player.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);
                                }
                            }
                            break;
                        case 1:
                            // stop walk animation
                            animator.SetFloat("speed", 0.0f);
                            
                            // change face
                            faceTexture = Head.GetComponent<Renderer>().materials;
                            faceTexture[1].SetTextureOffset("_MainTex", new Vector2(0.0f, 0.5f));
                            faceTexture[2].SetTextureOffset("_MainTex", new Vector2(0.5f, 0.5f));
                            GetComponent<MeshRenderer>().materials = faceTexture;

                            if (pause == 0)
                            {
                                pause = Time.time;
                            }

                            if (pause + 1.0f <= Time.time)
                            {
                                pause = 0;
                                levelState = 2;
                            }
                            break;
                        case 2:
                            // Camera move to sleeping people
                            if (once)
                            {
                                temp = 0;
                                once = false;
                            }

                            //Camera focus
                            MainCamera.transform.LookAt(FocusPoint.transform.position);
                            
                            FocusPoint.transform.position = Vector3.MoveTowards(
                                FocusPoint.transform.position,
                                new Vector3(FocusTrack[temp].transform.position.x, 0.75f, FocusTrack[temp].transform.position.z),
                                5.0f * Time.deltaTime);

                            MainCamera.transform.position = Vector3.MoveTowards(
                                MainCamera.transform.position,
                                new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z),
                                1.0f * Time.deltaTime);
                                  
                            // next state
                            if (MainCamera.transform.position == new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z) &&
                                FocusPoint.transform.position == new Vector3(FocusTrack[temp].transform.position.x, 0.75f, FocusTrack[temp].transform.position.z))
                            {
                                if (pause == 0)
                                {
                                    pause = Time.time;
                                }

                                if (pause + 0.0f <= Time.time)
                                {
                                    pause = 0;
                                    once = true;
                                    levelState = 3;
                                }
                            }                            
                            break;
                        case 3:
                            // Camera move to sleeping people
                            if (once)
                            {
                                temp = 1;
                                once = false;
                            }

                            //Camera focus
                            MainCamera.transform.LookAt(FocusPoint.transform.position);

                            MainCamera.transform.position = Vector3.MoveTowards(
                                MainCamera.transform.position,
                                new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z),
                                1.0f * Time.deltaTime);

                            // next state
                            if (MainCamera.transform.position == new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z) &&
                                FocusPoint.transform.position == new Vector3(FocusTrack[temp-1].transform.position.x, 0.75f, FocusTrack[temp-1].transform.position.z))
                            {
                                if (pause == 0)
                                {
                                    pause = Time.time;
                                }

                                if (pause + 0.0f <= Time.time)
                                {
                                    pause = 0;
                                    once = true;
                                    levelState = 4;
                                }
                            }
                            break;
                        case 4:
                            // Camera move to sleeping people
                            if (once)
                            {
                                temp = 2;
                                once = false;
                            }

                            //Camera focus
                            MainCamera.transform.LookAt(FocusPoint.transform.position);

                            MainCamera.transform.position = Vector3.MoveTowards(
                                MainCamera.transform.position,
                                new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z),
                                1.0f * Time.deltaTime);

                            FocusPoint.transform.position = Vector3.MoveTowards(
                                FocusPoint.transform.position,
                                new Vector3(FocusTrack[temp - 1].transform.position.x, 0.75f, FocusTrack[temp - 1].transform.position.z),
                                5.0f * Time.deltaTime);

                            // next state
                            if (MainCamera.transform.position == new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z) &&
                                FocusPoint.transform.position == new Vector3(FocusTrack[temp - 1].transform.position.x, 0.75f, FocusTrack[temp - 1].transform.position.z))
                            {
                                if (pause == 0)
                                {
                                    pause = Time.time;
                                }

                                if (pause + 3.5f <= Time.time)
                                {
                                    pause = 0;
                                    once = true;
                                    levelState = 5;
                                }
                            }
                            break;
                        case 5:
                            // Camera move to sleeping people
                            if (once)
                            {
                                temp = 3;
                                once = false;
                            }

                            //Camera focus
                            MainCamera.transform.LookAt(FocusPoint.transform.position);

                            MainCamera.transform.position = Vector3.MoveTowards(
                                MainCamera.transform.position,
                                new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z),
                                1.0f * Time.deltaTime);

                            FocusPoint.transform.position = Vector3.MoveTowards(
                                FocusPoint.transform.position,
                                new Vector3(FocusTrack[temp].transform.position.x, 0.75f, FocusTrack[temp].transform.position.z),
                                5.0f * Time.deltaTime);

                            // next state
                            if (MainCamera.transform.position == new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z) &&
                                FocusPoint.transform.position == new Vector3(FocusTrack[temp].transform.position.x, 0.75f, FocusTrack[temp].transform.position.z))
                            {
                                if (pause == 0)
                                {
                                    pause = Time.time;
                                }

                                if (pause + 1.0f <= Time.time)
                                {
                                    pause = 0;
                                    once = true;
                                    levelState = 6;
                                }
                            }
                            break;
                        case 6:
                            // Camera move to sleeping people
                            if (once)
                            {
                                temp = 4;
                                once = false;
                            }

                            //Camera focus
                            MainCamera.transform.LookAt(FocusPoint.transform.position);

                            MainCamera.transform.position = Vector3.MoveTowards(
                                MainCamera.transform.position,
                                new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z),
                                1.0f * Time.deltaTime);

                            FocusPoint.transform.position = Vector3.MoveTowards(
                                FocusPoint.transform.position,
                                new Vector3(FocusTrack[temp].transform.position.x, 0.75f, FocusTrack[temp].transform.position.z),
                                5.0f * Time.deltaTime);

                            // next state
                            if (MainCamera.transform.position == new Vector3(CameraTrack[temp].transform.position.x, 0.75f, CameraTrack[temp].transform.position.z) &&
                                FocusPoint.transform.position == new Vector3(FocusTrack[temp].transform.position.x, 0.75f, FocusTrack[temp].transform.position.z))
                            {
                                if (pause == 0)
                                {
                                    pause = Time.time;
                                }

                                if (pause + 0.0f <= Time.time)
                                {
                                    pause = 0;
                                    once = true;
                                    levelState = 7;
                                }
                            }
                            break;
                        case 7:
                            MainCamera.transform.LookAt(FocusPoint.transform.position);

                            MainCamera.transform.position = Vector3.MoveTowards(
                                MainCamera.transform.position,
                                new Vector3(CameraTrack[temp].transform.position.x, 2.5f, CameraTrack[temp].transform.position.z),
                                1.0f * Time.deltaTime);

                            if (MainCamera.transform.position == new Vector3(CameraTrack[temp].transform.position.x, 2.5f, CameraTrack[temp].transform.position.z))
                            {
                                levelState = 8;
                            }
                            break;
                        case 8:
                            // self monolog
                            if (once)
                            {
                                tempTime = Time.time;
                                temp = 0;
                                once = false;
                            }

                            string[] Text = {
                                "player","Was ist denn hier los?" ,
                                "player","Irgendjemand muss hier doch wach sein." ,
                                "player","Ich muss mich auf die Suche machen!"
                            };
                        
                            if (tempTime + 3.5f <= Time.time)
                            {
                                temp = temp + 2;
                                tempTime = Time.time;
                            }

                            if (temp == Text.Length)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 9;
                                Dialog("", "", false);
                                once = true;
                            }
                            else
                            {
                                Dialog(Text[temp + 1], Text[temp], true);
                            }
                            break;
                        case 9:
                            // Destroy unused Objects
                            for (int i = 0; i < PlayerTrack.Length; i++)
                            {
                                Destroy(PlayerTrack[i]);
                            }
                            for (int i = 0; i < CameraTrack.Length; i++)
                            {
                                Destroy(CameraTrack[i]);
                            }

                            levelState = 10;
                            break;
                        case 10:
                            // reset face texure to normal
                            faceTexture[1].SetTextureOffset("_MainTex", new Vector2(0.0f, 0.25f));
                            faceTexture[2].SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
                            GetComponent<MeshRenderer>().materials = faceTexture;
                            
                            // reset user controls
                            blocked = false;
                            levelState = 11;
                            break;
                        case 11:
                            // check for Solveig
                            //Debug.Log(Vector3.Distance(Player.transform.position, TriggerObjects[0].transform.position));

                            if(Vector3.Distance(Player.transform.position, TriggerObjects[0].transform.position) < 0.5f)
                            {
                                levelState = 0;
                                levelPart = 1;
                            }
                            break;
                    }
                    break;
                case 1:
                    switch (levelState)
                    {
                        case 0:
                            // waiting for Solveig found
                            blocked = false;

                            if (Vector3.Distance(Player.transform.position, TriggerObjects[0].transform.position) < 1.5f)
                            {
                                tempPositions[0] = MainCamera.transform.position;
                                tempPositions[1] = TriggerObjects[0].transform.position;
                                tempPositions[2] = Player.transform.position;
                                levelState = 1;
                            }
                            break;
                        case 1:
                            //setup positions
                            blocked = true;
                            // set camera
                            MainCamera.transform.position = new Vector3(-7.36f, 1.32f, -2.26f);
                            MainCamera.transform.eulerAngles = new Vector3(24.625f, -135.0f, 0.0f);

                            // set solveig
                            TriggerObjects[0].transform.position = new Vector3(-8.501f, 0.41f, -3.122f);
                            TriggerObjects[0].transform.eulerAngles = new Vector3(0.0f, 30.0f, 0.0f);
                            solveigAnimator.SetBool("Happy", true);

                            // set player
                            Player.transform.position = TriggerObjects[0].transform.position + new Vector3(-0.1f, 0.0f, 0.7f);
                            Player.transform.LookAt(TriggerObjects[0].transform.position);

                            animator.SetFloat("speed", 0.0f);
                            levelState = 2;
                            break;
                        case 2:
                            // solveig player dialog
                            Player.transform.LookAt(TriggerObjects[0].transform.position);

                            if (once)
                            {
                                tempTime = Time.time;
                                temp = 0;
                                once = false;
                                TriggerObjects[1].transform.eulerAngles = new Vector3(0.0f, -30.217f, -90.0f);
                            }

                            TriggerObjects[1].GetComponent<MeshRenderer>().enabled = true;
                            if (TriggerObjects[1].transform.eulerAngles.x < 180.0f)
                            {
                                TriggerObjects[1].transform.Rotate(Vector3.up, 25.0f * Time.deltaTime);
                            }

                            string[] Text = {
                                "solveig","Hallo und Willkommen bei Meelogic!" ,
                                "solveig","Ich bin Solveig und freu mich ja so jemand neues hier begrüßen zu können, besonders an so einem schönen Tag!" ,
                                "player","Ähm... Hallo, äh ja ich freu mich auch aber irgendwie scheint hier im Büro niemand so richtig wach zu sein?" ,
                                "solveig","Oh diese Informatiker!\n Aber das ist kein Problem da fehlt nur der Weckruf des Kaffees!",
                                "solveig","Suche einfach die Kaffeemaschine und koche ein Tasse das sollte Schwung in die Bude bringen!",
                                "solveig","Die Kaffeemühle steht direkt daneben."
                            };

                            if (tempTime + 3.5f <= Time.time)
                            {
                                temp = temp + 2;
                                tempTime = Time.time;
                            }

                            if (temp == Text.Length)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 3;
                                Dialog("", "", false);
                                once = true;
                            }
                            else
                            {
                                Dialog(Text[temp + 1], Text[temp], true);
                            }
                            break;
                        case 3:
                            // reset positions
                            MainCamera.transform.position = tempPositions[0];
                            TriggerObjects[0].transform.position = tempPositions[1];
                            Player.transform.position = tempPositions[2];

                            solveigAnimator.SetBool("Happy", false);
                            TriggerObjects[0].transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

                            Player.transform.eulerAngles = new Vector3(0.0f, 90.0f, 0.0f);

                            TriggerObjects[1].GetComponent<MeshRenderer>().enabled = false;

                            blocked = false;

                            levelState = 4;
                            break;
                        case 4:
                            if (Vector3.Distance(Player.transform.position, TriggerObjects[3].transform.position) < 1.5f)
                            {
                                tempPositions[0] = MainCamera.transform.position;
                                blocked = true;
                                temp = 0;
                                levelState = 5;
                            }
                            break;
                        case 5:
                            // no coffee available
                            if (once)
                            {
                                tempTime = Time.time;
                                once = false;
                            }

                            string[] Text2 = {
                                "player","",
                                "player","Hmm die Kaffeemühle ist leer." ,
                                "player","Ich sollte mal schauen ob ich neuen Kaffee finde."
                            };
                            animator.SetFloat("speed", 0.0f);

                            FocusPoint.transform.position = Vector3.MoveTowards(
                                FocusPoint.transform.position,
                                TriggerObjects[3].transform.position + new Vector3(0.0f, -0.1f, 0.0f),
                                0.5f * Time.deltaTime);
                            MainCamera.transform.LookAt(FocusPoint.transform.position);

                            MainCamera.transform.position = Vector3.MoveTowards(
                                MainCamera.transform.position,
                                new Vector3(3.5f, 1.4f, 1.0f),
                                1.0f * Time.deltaTime);

                            if (FocusPoint.transform.position == TriggerObjects[3].transform.position + new Vector3(0.0f, -0.1f, 0.0f) &&
                                MainCamera.transform.position == new Vector3(3.5f, 1.4f, 1.0f))
                            {
                                if (tempTime + 3.5f <= Time.time)
                                {
                                    temp = temp + 2;
                                    tempTime = Time.time;
                                }

                                if (temp == Text2.Length)
                                {
                                    temp = 0;
                                    tempTime = 0;
                                    levelState = 6;
                                    Dialog("", "", false);
                                    once = true;
                                }
                                else
                                {
                                    Dialog(Text2[temp + 1], Text2[temp], true);
                                }
                            }
                            break;
                        case 6:
                            // reset camera smoothly
                            MainCamera.transform.position = Vector3.MoveTowards(
                                MainCamera.transform.position,
                                tempPositions[0],
                                1.0f * Time.deltaTime);

                            FocusPoint.transform.position = Vector3.MoveTowards(
                                FocusPoint.transform.position,
                                Head.transform.position,
                                0.5f * Time.deltaTime);

                            MainCamera.transform.LookAt(FocusPoint.transform.position);

                            if (MainCamera.transform.position == tempPositions[0] &&
                                FocusPoint.transform.position == Head.transform.position)
                            {
                                blocked = false;
                                levelState = 7;
                            }
                            break;
                        case 7:
                            // check if all drawers became opened
                            if (Draweropener0.open)
                            {
                                checker[0] = true;
                            }

                            if (Draweropener1.open)
                            {
                                checker[1] = true;
                            }

                            if (Draweropener2.open)
                            {
                                checker[2] = true;
                            }
                            
                            if (checker[0] && checker[1] && checker[2])
                            {
                                levelState = 8;
                            }
                            break;
                        case 8:
                            // self monolog 2
                            if (once)
                            {
                                tempTime = Time.time;
                                once = false;
                            }

                            Dialog("Sieht schlecht aus da sollte ich noch mal nachfragen.", "player", true);

                            if (tempTime + 5.0f <= Time.time)
                            {
                                tempTime = 0;
                                Dialog("", "", false);
                                once = true;

                                levelPart = 2;
                                levelState = 0;
                            }
                            break;
                    }
                    break;
                case 2:
                    switch (levelState)
                    {
                        case 0:
                            // save positions
                            if (Vector3.Distance(Player.transform.position, TriggerObjects[0].transform.position) < 1.5f)
                            {
                                tempPositions[0] = MainCamera.transform.position;
                                tempPositions[1] = TriggerObjects[0].transform.position;
                                tempPositions[2] = Player.transform.position;
                                levelState = 1;
                            }
                            break;
                        case 1:
                            //setup positions
                            blocked = true;
                            // set camera
                            MainCamera.transform.position = new Vector3(-7.36f, 1.32f, -2.26f);
                            MainCamera.transform.eulerAngles = new Vector3(24.625f, -135.0f, 0.0f);

                            // set solveig
                            TriggerObjects[0].transform.position = new Vector3(-8.501f, 0.41f, -3.122f);
                            TriggerObjects[0].transform.eulerAngles = new Vector3(0.0f, 30.0f, 0.0f);
                            solveigAnimator.SetBool("Stop", true);

                            // set player
                            Player.transform.position = TriggerObjects[0].transform.position + new Vector3(-0.1f, 0.0f, 0.7f);
                            Player.transform.LookAt(TriggerObjects[0].transform.position);

                            animator.SetFloat("speed", 0.0f);
                            levelState = 2;
                            break;
                        case 2:
                            // solveig player dialog 2
                            Player.transform.LookAt(TriggerObjects[0].transform.position);

                            if (once)
                            {
                                tempTime = Time.time;
                                temp = 0;
                                once = false;
                                tempTime = 0;
                                TriggerObjects[1].transform.eulerAngles = new Vector3(180.0f, -30.217f, -90.0f);
                                TriggerObjects[1].GetComponent<MeshRenderer>().enabled = true;
                            }
                            
                            string[] Text = {
                                "player","",
                                "player","Hey, leider ist der Kaffee leer und hab in den Schubladen keinen neuen gefunden." ,
                                "solveig","Oh ja, den Kaffee mussten wir in den Safe einschließen.",
                                "solveig","Das mussten wir machen nachdem einmal ein paar Leute etwas länger gearbeitet haben und müde wurden.\n" +
                                "Da haben die angefangen den Kaffee pur zu futtern... mit gewissen Auswirkungen...",
                                "solveig","Aber jetzt sind sie zum Glück alle wieder normal...\nAußer Steffen mit seinen Eichhörnchen das kann nicht normal sein..."
                            };

                            if (temp == 8)
                            {
                                solveigAnimator.SetBool("Stop", false);
                                solveigAnimator.SetBool("Sad", true);
                            }
                            else
                            {
                                solveigAnimator.SetBool("Stop", true);
                                solveigAnimator.SetBool("Sad", false);
                            }

                            if (tempTime + 3.5f <= Time.time)
                            {
                                temp = temp + 2;
                                tempTime = Time.time;
                            }

                            if (temp == Text.Length)
                            {
                                if (tempTime == 0)
                                {
                                    tempTime = Time.time;
                                    FadeToBlack();
                                    TriggerObjects[1].GetComponent<MeshRenderer>().enabled = true;
                                }

                                if (tempTime + 1.5f < Time.time)
                                {
                                    temp = 0;
                                    tempTime = 0;
                                    levelState = 3;
                                    Dialog("", "", false);
                                    once = true;
                                }
                            }
                            else
                            {
                                Dialog(Text[temp + 1], Text[temp], true);
                            }
                            break;
                        case 3:
                            if (once)
                            {
                                tempTime = 0;
                                FadeFromBlack();
                                once = false;
                            }

                            // set position in front of safe
                            MainCamera.transform.position = new Vector3(-1.53f, 0.91f, -7.72f);
                            MainCamera.transform.eulerAngles = new Vector3(10.0f, -15.0f, 0.0f);

                            TriggerObjects[0].transform.position = new Vector3(-2.13f, 0.405f, -6.66f);
                            TriggerObjects[0].transform.eulerAngles = new Vector3(0.0f, 155.0f, 0.0f);
                            solveigAnimator.SetBool("stop", true);

                            Player.transform.position = TriggerObjects[0].transform.position + new Vector3(0.8f, 0.0f, 0.0f);
                            Player.transform.eulerAngles = new Vector3(0.0f, -125.0f, 0.0f);
                            animator.SetFloat("speed", 0.0f);

                            FocusPoint.transform.position = new Vector3(-1.83f, 0.9938904f, -6.734f);

                            if (tempTime == 0)
                            {
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                                tempTime = Time.time;
                            }

                            if (tempTime + 1.5f < Time.time) {
                                tempTime = 0;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = false;
                                levelState = 4;
                            }
                            break;
                        case 4:
                            // solveig player dialog 2
                            if (once)
                            {
                                tempTime = Time.time;
                                temp = 0;
                                once = false;
                            }

                            string[] Text2 = {
                                "solveig","",
                                "solveig","Oh Nein!\nDer Code für den Safe wurde geändert!",
                                "solveig","Ich weiß leider nicht wer das gemacht hat.",
                                "player","Kein Problem ich werde mal nachsehen ob ich eine Notiz finde."
                            };

                            if (tempTime + 3.5f <= Time.time)
                            {
                                temp = temp + 2;
                                tempTime = Time.time;
                            }

                            if (temp == Text2.Length)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 5;
                                Dialog("", "", false);
                                once = true;
                            }
                            else
                            {
                                Dialog(Text2[temp + 1], Text2[temp], true);
                            }
                            break;
                        case 5:
                            // reset
                            solveigAnimator.SetBool("Stop", true);
                            blocked = false;
                            tempTime = 0;
                            levelState = 6;
                            break;
                        case 6:
                            // adam, stefan, steffen, eva, max, meeting, bernd;
                            int check = 0;
                            int pos = 0;
                            solveigAnimator.SetBool("Stop", true);
                            TriggerObjects[1].GetComponent<MeshRenderer>().enabled = false;

                            // check distance to get last unchecked zone
                            for (int i = 0; i < zones.Length; i++)
                            {
                                if (Vector3.Distance(TriggerObjects[9 + i].transform.position, Player.transform.position) < 2.0f)
                                {
                                    zones[i] = true;
                                }
                            }
                            
                            // check for last zone not check                                    
                            for (int i = 0; i < zones.Length; i++)
                            {
                                if (!zones[i])
                                {
                                    check++;
                                    pos = i;
                                }
                            }
                            
                            // if only one zone left place code note
                            if (check == 1)
                            {
                                switch (pos)
                                {
                                    case 0:
                                        TriggerObjects[8].transform.position = new Vector3(0.21f, 0.479f, 19.341f);
                                        TriggerObjects[8].transform.eulerAngles = new Vector3(-90.0f, 135.0f, 0.0f);
                                        TriggerObjects[8].GetComponent<MeshRenderer>().enabled = true;
                                        levelState = 7;
                                        break;
                                    case 1:
                                        TriggerObjects[8].transform.position = new Vector3(-7.518f, 0.479f, 18.08f);
                                        TriggerObjects[8].transform.eulerAngles = new Vector3(-90.0f, -135.0f, 0.0f);
                                        TriggerObjects[8].GetComponent<MeshRenderer>().enabled = true;
                                        levelState = 7;
                                        break;
                                    case 2:
                                        TriggerObjects[8].transform.position = new Vector3(-1.447f, 0.479f, 17.684f);
                                        TriggerObjects[8].transform.eulerAngles = new Vector3(-90.0f, -135.0f, 0.0f);
                                        TriggerObjects[8].GetComponent<MeshRenderer>().enabled = true;
                                        levelState = 7;
                                        break;
                                    case 3:
                                        TriggerObjects[8].transform.position = new Vector3(-8.147f, 0.479f, 10.992f);
                                        TriggerObjects[8].transform.eulerAngles = new Vector3(-90.0f, 135.0f, 0.0f);
                                        TriggerObjects[8].GetComponent<MeshRenderer>().enabled = true;
                                        levelState = 7;
                                        break;
                                    case 4:
                                        TriggerObjects[8].transform.position = new Vector3(-6.837f, 0.479f, 7.825f);
                                        TriggerObjects[8].transform.eulerAngles = new Vector3(-90.0f, 0.0f, 0.0f);
                                        TriggerObjects[8].GetComponent<MeshRenderer>().enabled = true;
                                        levelState = 7;
                                        break;
                                    case 5:
                                        TriggerObjects[8].transform.position = new Vector3(-8.0f, 0.479f, 1.452f);
                                        TriggerObjects[8].transform.eulerAngles = new Vector3(-90.0f, 135.0f, 0.0f);
                                        TriggerObjects[8].GetComponent<MeshRenderer>().enabled = true;
                                        levelState = 7;
                                        break;
                                    case 6:
                                        TriggerObjects[8].transform.position = new Vector3(2.992f, 0.48f, -9.202f);
                                        TriggerObjects[8].transform.eulerAngles = new Vector3(-90.0f, -45.0f, 0.0f);
                                        TriggerObjects[8].GetComponent<MeshRenderer>().enabled = true;
                                        levelState = 7;
                                        break;
                                }
                            }
                            break;
                        case 7:
                            // do actual level / find new safe code
                            if (Vector3.Distance(TriggerObjects[8].transform.position, Player.transform.position) < 1.5f)
                            {
                                blocked = true;
                                
                                // self monolog
                                if (once)
                                {
                                    tempTime = Time.time;
                                    temp = 0;
                                    once = false;
                                }

                                // set camera and focuspoint
                                FocusPoint.transform.position = Vector3.MoveTowards(
                                    FocusPoint.transform.position,
                                    TriggerObjects[8].transform.position,
                                    2.5f * Time.deltaTime);

                                MainCamera.transform.position = Vector3.MoveTowards(
                                    MainCamera.transform.position,
                                    TriggerObjects[8].transform.position + new Vector3(-0.1f, 0.75f, 0.0f),
                                    2.5f * Time.deltaTime);

                                MainCamera.transform.LookAt(FocusPoint.transform.position);

                                string[] Text3 = {
                                "player","Ah! Da ist ja der Code!" ,
                                "player","1 - 2 - 3 - 4 - 5 - 6?" ,
                                "player","Was für ein blöder Code ist das denn?"
                                };

                                if (tempTime + 2.5f <= Time.time)
                                {
                                    temp = temp + 2;
                                    tempTime = Time.time;
                                }

                                if (temp == Text3.Length)
                                {
                                    temp = 0;
                                    tempTime = 0;
                                    levelState = 8;
                                    Dialog("", "", false);
                                    once = true;
                                }
                                else
                                {
                                    Dialog(Text3[temp + 1], Text3[temp], true);
                                }
                                break;
                            }
                            break;
                        case 8:
                            if (tempTime == 0)
                            {
                                FadeToBlack();
                                tempTime = Time.time;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                            }

                            if (tempTime + 1.5f < Time.time)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 9;
                                once = true;
                            }
                            break;
                        case 9:
                            if (once)
                            {
                                tempTime = 0;
                                FadeFromBlack();
                                once = false;
                            }

                            // set position in front of safe
                            MainCamera.transform.position = new Vector3(-1.53f, 0.91f, -7.72f);
                            MainCamera.transform.eulerAngles = new Vector3(10.0f, -15.0f, 0.0f);

                            TriggerObjects[0].transform.position = new Vector3(-2.13f, 0.405f, -6.66f);
                            TriggerObjects[0].transform.eulerAngles = new Vector3(0.0f, 155.0f, 0.0f);
                            //solveigAnimator.SetBool("stop", true);

                            Player.transform.position = TriggerObjects[0].transform.position + new Vector3(0.8f, 0.0f, 0.0f);
                            Player.transform.eulerAngles = new Vector3(0.0f, -125.0f, 0.0f);
                            animator.SetFloat("speed", 0.0f);

                            FocusPoint.transform.position = new Vector3(-1.83f, 0.9938904f, -6.734f);

                            if (tempTime == 0)
                            {
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                                tempTime = Time.time;
                            }

                            if (tempTime + 1.5f < Time.time)
                            {
                                tempTime = 0;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = false;
                                levelState = 0;
                                levelPart = 3;
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (levelState)
                    {
                        case 0:
                            // solveig player dialog 3
                            if (once)
                            {
                                levelState = 1;
                                tempTime = Time.time;
                                temp = 0;
                                once = false;
                            }

                            string[] Text2 = {
                                "player","",
                                "player","Ich habe den Code gefunden.",
                                "solveig","Super! 1 - 2 - 3 - 4 - 5 - 6?\nWas für ein Zufall das ist auch mein Windows Passwort!"
                            };

                            if (tempTime + 3.5f <= Time.time)
                            {
                                temp = temp + 2;
                                tempTime = Time.time;
                            }

                            if (temp == Text2.Length)
                            {
                                // change face
                                faceTexture = Head.GetComponent<Renderer>().materials;
                                faceTexture[2].SetTextureOffset("_MainTex", new Vector2(0.5f, 0.5f));
                                GetComponent<MeshRenderer>().materials = faceTexture;

                                temp = 0;
                                tempTime = 0;
                                levelState = 1;
                                Dialog("", "", false);
                                once = true;
                            }
                            else
                            {
                                Dialog(Text2[temp + 1], Text2[temp], true);
                            }
                            break;
                        case 1:
                            // spawn coffee
                            for (int i = 0; i < 200; i++)
                            {
                                Spawn = Instantiate(
                                    TriggerObjects[17],
                                    TriggerObjects[16].transform.position,
                                    transform.rotation);
                            }

                            levelState = 2;
                            break;
                        case 2:
                            // open safe
                            if (once)
                            {
                                tempTime = Time.time;
                                temp = 0;
                                once = false;
                            }

                            SafeOpen = true;

                            if (tempTime + 4.5f < Time.time)
                            {
                                tempTime = 0;
                                levelState = 3;
                            }
                            break;
                        case 3:
                            // solveig monolog
                            if (once)
                            {
                                tempTime = Time.time;
                                once = false;
                            }

                            solveigAnimator.SetBool("Happy", true);
                            Dialog("Jetzt kann es richtig los gehen!", "solveig", true);

                            if (tempTime + 3.0f <= Time.time)
                            {
                                tempTime = 0;
                                Dialog("", "", false);
                                once = true;
                                
                                levelState = 4;
                            }
                            break;
                        case 4:
                            if (once)
                            {
                                FadeToBlack();
                                tempTime = Time.time;
                                once = false;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                            }

                            if (tempTime + 1.5f < Time.time)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 5;
                                once = true;
                            }
                            break;
                        case 5:
                            if (once)
                            {
                                tempTime = 0;
                                FadeFromBlack();
                                once = false;
                            }

                            // set Camera position to coffee maker
                            FocusPoint.transform.position = TriggerObjects[3].transform.position + new Vector3(0.0f, -0.1f, 0.0f);

                            MainCamera.transform.LookAt(FocusPoint.transform.position);
                            MainCamera.transform.position = new Vector3(3.5f, 1.4f, 1.0f);
                            
                            if (tempTime == 0)
                            {
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                                tempTime = Time.time;
                            }

                            if (tempTime + 2.5f < Time.time)
                            {
                                tempTime = 0;
                                once = true;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = false;
                                levelState = 6;
                            }
                            break;
                        case 6:
                            if (once)
                            {
                                tempTime = Time.time;
                                once = false;
                            }

                            TriggerObjects[18].SetActive(true);
                            
                            if (tempTime + 2.5f < Time.time)
                            {
                                tempTime = 0;
                                once = true;
                                levelState = 7;
                            }
                            break;
                        case 7:
                            if (once)
                            {
                                FadeToBlack();
                                tempTime = Time.time;
                                once = false;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                            }

                            if (tempTime + 1.5f < Time.time)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 8;
                                once = true;
                            }
                            break;
                        case 8:
                            if (once)
                            {
                                tempTime = 0;
                                FadeFromBlack();
                                once = false;
                            }

                            // set Camera position to coffee maker
                            FocusPoint.transform.position = TriggerObjects[19].transform.position + new Vector3(0.0f, -0.3f, 0.0f);

                            MainCamera.transform.LookAt(FocusPoint.transform.position);
                            MainCamera.transform.position = TriggerObjects[19].transform.position + new Vector3(-0.3f, 0.3f, -0.70f);

                            TriggerObjects[20].GetComponent<MeshRenderer>().enabled = false;

                            if (tempTime == 0)
                            {
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                                tempTime = Time.time;
                            }

                            if (tempTime + 1.5f < Time.time)
                            {
                                tempTime = 0;
                                once = true;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = false;
                                levelState = 9;
                            }
                            break;
                        case 9:
                            if (once)
                            {
                                once = false;
                                tempTime = Time.time;
                                temp = 1;
                            }

                            Vector2 tempVec;

                            if (Time.time - tempTime < 0.5f / temp)
                            {
                                tempVec = new Vector2(0.0f, 0.0f);
                            }
                            else
                            {
                                tempVec = new Vector2(0.0f, -0.25f);
                            }

                            if (Time.time - tempTime > 1.0f / temp)
                            {
                                tempTime = Time.time;
                                temp++;
                            }

                            faceTexture2 = TriggerObjects[19].GetComponent<Renderer>().materials;
                            faceTexture2[2].SetTextureOffset("_MainTex", tempVec);
                            TriggerObjects[20].GetComponent<MeshRenderer>().materials = faceTexture2;

                            if (temp > 5)
                            {
                                once = true;
                                tempTime = 0;
                                temp = 0;

                                faceTexture2 = TriggerObjects[19].GetComponent<Renderer>().materials;
                                faceTexture2[2].SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
                                TriggerObjects[20].GetComponent<MeshRenderer>().materials = faceTexture2;

                                levelState = 10;
                            }
                            break;
                        case 10:
                            if (once)
                            {
                                once = false;
                                tempTime = Time.time;
                            }

                            FocusPoint.transform.position = Vector3.MoveTowards(
                                FocusPoint.transform.position,
                                TriggerObjects[21].transform.position + new Vector3(0.0f, -0.3f, 0.0f),
                                2.0f * Time.deltaTime);

                            MainCamera.transform.LookAt(FocusPoint.transform.position);
                            
                            steffenAnimator.SetBool("wakeup", true);

                            if (tempTime + 3.0f < Time.time)
                            {
                                tempTime = 0;
                                once = true;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = false;
                                levelState = 11;
                            }
                            break;
                        case 11:
                            if (once)
                            {
                                FadeToBlack();
                                tempTime = Time.time;
                                once = false;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                            }

                            if (tempTime + 1.5f < Time.time)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 12;
                                once = true;
                            }
                            break;
                        case 12:
                            TriggerObjects[22].transform.position = new Vector3(3.638f, 0.402f, 1.728f);
                            TriggerObjects[22].transform.eulerAngles = new Vector3(0.0f, 328.0f, 0.0f);
                            ayanaAnimator.SetBool("drink", true);

                            TriggerObjects[23].transform.position = new Vector3(3.75f, 0.402f, 2.788f);
                            TriggerObjects[23].transform.eulerAngles = new Vector3(0.0f, 270.0f, 0.0f);
                            sonjaAnimator.SetBool("stop", true);

                            levelState = 13;
                            break;
                        case 13:
                            if (once)
                            {
                                tempTime = 0;
                                FadeFromBlack();
                                once = false;
                                tempTime = Time.time;
                            }

                            // set Camera position to kitchen
                            FocusPoint.transform.position = new Vector3(3.861f, 0.527f, 1.799f);
                            MainCamera.transform.position = new Vector3(2.1f, 1.82f, 4.03f);
                            MainCamera.transform.LookAt(FocusPoint.transform.position);
                            
                            if (tempTime + 4.5f < Time.time)
                            {
                                tempTime = 0;
                                once = true;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = false;
                                levelState = 14;
                            }
                            break;
                        case 14:
                            // self monolog ayana
                            if (once)
                            {
                                tempTime = Time.time;
                                once = false;
                            }

                            Dialog("Ah! Kaffee!.", "ayana", true);

                            if (tempTime + 5.0f <= Time.time)
                            {
                                tempTime = 0;
                                Dialog("", "", false);
                                once = true;
                                levelState = 15;
                            }
                            break;
                        case 15:
                            if (once)
                            {
                                FadeToBlack();
                                tempTime = Time.time;
                                once = false;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                            }
                            if (tempTime + 1.5f < Time.time)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 16;
                                once = true;
                            }
                            break;
                        case 16:
                            // set camera
                            MainCamera.transform.position = new Vector3(-7.36f, 1.32f, -2.26f);
                            MainCamera.transform.eulerAngles = new Vector3(24.625f, -135.0f, 0.0f);

                            // set solveig
                            TriggerObjects[0].transform.position = new Vector3(-8.501f, 0.41f, -3.122f);
                            TriggerObjects[0].transform.eulerAngles = new Vector3(0.0f, 30.0f, 0.0f);
                            solveigAnimator.SetBool("Happy", true);

                            // set player
                            Player.transform.position = TriggerObjects[0].transform.position + new Vector3(-0.1f, 0.0f, 0.7f);
                            Player.transform.LookAt(TriggerObjects[0].transform.position);

                            // set focus
                            FocusPoint.transform.position = TriggerObjects[0].transform.position;

                            animator.SetFloat("speed", 0.0f);

                            levelState = 17;
                            break;
                        case 17:
                            // solveig player dialog
                            Player.transform.LookAt(TriggerObjects[0].transform.position);
                            solveigAnimator.SetBool("Happy", true);

                            if (once)
                            {
                                FadeFromBlack();
                                tempTime = Time.time;
                                temp = 0;
                                once = false;
                                TriggerObjects[1].transform.eulerAngles = new Vector3(0.0f, -30.217f, -90.0f);
                                TriggerObjects[2].transform.eulerAngles = new Vector3(130.0f, 149.783f, 0.0f);
                            }

                            TriggerObjects[1].transform.eulerAngles = new Vector3(180.0f, -30.217f, -90.0f);
                            TriggerObjects[1].GetComponent<MeshRenderer>().enabled = true;

                            TriggerObjects[2].transform.Rotate(Vector3.right, -75.0f * Time.deltaTime);

                            string[] Text = {
                                "solveig","Hurra! Alle Leute haben ihren Kaffee und jetzt geht es richtig los!"
                            };

                            if (tempTime + 3.5f <= Time.time)
                            {
                                temp = temp + 2;
                                tempTime = Time.time;
                            }

                            if (temp == Text.Length)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 18;
                                Dialog("", "", false);
                                once = true;
                            }
                            else
                            {
                                Dialog(Text[temp + 1], Text[temp], true);
                            }
                            break;
                        case 18:
                            if (once)
                            {
                                tempTime = Time.time;
                                once = false;
                            }

                            TriggerObjects[2].transform.Rotate(Vector3.right, -75.0f * Time.deltaTime);
                            if (tempTime + 5.5f < Time.time)
                            {
                                temp = 0;
                                tempTime = 0;
                                levelState = 19;
                                once = true;
                            }
                            break;
                        case 19:
                            if (once)
                            {
                                FadeToBlack();
                                tempTime = Time.time;
                                once = false;
                                blackScreenCanvas.GetComponent<Canvas>().enabled = true;
                            }

                            TriggerObjects[2].transform.Rotate(Vector3.right, -75.0f * Time.deltaTime);
                            if (tempTime + 1.5f < Time.time)
                            {
                                tempTime = 0;
                                levelState = 20;
                                once = true;
                            }
                            break;
                        case 20:                            
                            //SceneManager.LoadScene("");
                            break;
                    }
                    break;
            }
	    }
        GameObject Spawn;
    }
}
