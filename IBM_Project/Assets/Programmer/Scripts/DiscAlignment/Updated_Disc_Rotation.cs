using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;


//Left for Alignment minigame:
//Implement into and push git project
//Implement Lewis' Timer (via inheritance?)
//*Modify code to allow easier expansion (more or less discs can be easily implemented, preferably by setting public variable(s)
//Model Cynder objects with highlighted paths (e.g. Blender, 3DS Max etc) and implement them into Unity Project (as .obj files)
//*Modify rotation check to allow any 'location' for the paths to be aligned at (alternatively, could create a center disc which dosen't rotate, thus requiring a specific rotation)
//*Changes to how discs affect eachother? (e.g. moving center disc will rotate outline disc in opposite direction? - Discs alternate in rotation when a disc down their hirarchy is being rotated)
//- might be difficult to implement while allowing for expansion, considering as stetch goal
//Anything else I think of or other members of team (e.g. Designers) believe is worth implementing

//* = might take more time to implement


public class Updated_Disc_Rotation : MonoBehaviour
{

    public float[] rotationSpeed;
    private const int discs = 3;
    //public int test1;
    //private short ID; //1 for red (outline) 2 for green (middle) 3 for white (center)
    private short currentSelect;
    private bool selected;
    private bool[] numAligned;
    private bool debugWin;
    [SerializeField]
    GameObject disc1; //Red (outline)
    [SerializeField]
    GameObject disc2; //green (middle)
    [SerializeField]
    GameObject disc3; //White (center)

    //Gameobjects used to check alignment. Moves position when a combonation of discs are aligned.
    GameObject r1;
    GameObject r2;
    GameObject r3;

    Quaternion disc1StartRotation;
    Quaternion disc2StartRotation;
    Quaternion disc3StartRotation;

    Vector3 r1StartPosition;
    Vector3 r1StartRotation;

    //Commentted out code with '!REMOVE!' needs to be de-commented after testing in demo scene
    public GameObject pregameText;
    public bool showTutorial;

    public bool randomiseRotations;

    public GameObject timer;
    private ScoreSystem scoreSystemGameObject;

    public Minigame_Timer mTimer;

    private GameController gC;
    // Start is called before the first frame update

    public delegate void DelType1(bool discAlignmentReady);
    public static event DelType1 OnDiscAlignmentReady;

    Vector3 RotationToDegrees(Vector3 v3)
    {
        Vector3 rv = new Vector3(1, 1, 1);
        rv.x = v3.x * Mathf.Rad2Deg;
        rv.y = v3.y * Mathf.Rad2Deg;
        rv.z = v3.z * Mathf.Rad2Deg;
        return rv;
    }

    private void OnEnable()
    {
        gC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        scoreSystemGameObject = GameObject.FindGameObjectWithTag("ScoreSystem").GetComponent<ScoreSystem>();


        currentSelect = 1;
        //Timer.SetActive(true);
        //Timer.GetComponent<MeshRenderer>().enabled = true;
        //Debug.Log("start test");

        numAligned = new bool[3];

        numAligned[0] = false;
        numAligned[1] = false;
        numAligned[2] = false;

        r1 = GameObject.Find("R1");
        r2 = GameObject.Find("R2");
        r3 = GameObject.Find("R3");

        r1.transform.position = new Vector3(-5, 15, 0);
        r2.transform.position = new Vector3(0, 15, 0);
        r3.transform.position = new Vector3(5, 15, 0);

        disc1StartRotation = disc1.transform.rotation;
        disc2StartRotation = disc2.transform.rotation;
        disc3StartRotation = disc3.transform.rotation;

        int randDirection = 0;
        randDirection = UnityEngine.Random.Range(-1, 2);

        if (randomiseRotations == true)
        {
            //Disc1.transform.rotation.eulerAngles.x = UnityEngine.Random.Range(0.0f, 360.0f);
            //Disc1.transform.Rotate(new Vector3(Disc1.transform.rotation.x, UnityEngine.Random.Range(1.0f, 360.0f), Disc1.transform.rotation.z));
            for (int i = 0; i < UnityEngine.Random.Range(30, 360); i++)
            {
                currentSelect = (short)UnityEngine.Random.Range(1, 4);
                //Debug.Log("Direction: " + randDirection + " currentSelect: " + currentSelect + "," + currentSelect * 5);
                RotateDisc(1.0f * (currentSelect * 5));
            }

            //Disc1.transform.Rotate(new Vector3(0, (rotationSpeed[0] * direction) * Time.deltaTime, 0));
        }
        else
        {
            //Disc1startRotation = Disc1.transform.rotation;
            //Disc2startRotation = Disc2.transform.rotation;
            //Disc3startRotation = Disc3.transform.rotation;
        }



        currentSelect = 1;
        ColourUpdate();
        debugWin = false;

        //R1startPosition = R1.transform.position;
        //R1startRotation = R1.transform.eulerAngles;
        //Debug.Log(R1startPosition + "," + R1startRotation);
        timer.SetActive(true);
        timer.GetComponent<TextMeshProUGUI>().enabled = false;
        GameObject.Find("TutorialBackground").GetComponent<MeshRenderer>().enabled = showTutorial;
        pregameText.SetActive(showTutorial);
    }
    void OnDisable()
    {
        pregameText.SetActive(false);
        pregameText.GetComponent<TextMeshProUGUI>().enabled = true;
    }

    void OnValidate()
    {
        //if (rotationSpeed.Length != 3)
        if (rotationSpeed.Length != discs)
        {
            //Debug.LogError("Oi! What you doing?! The rotationSpeed length should be set to " + DISCS + "! (Because there should be exactly " + DISCS + " discs)");
            Array.Resize(ref rotationSpeed, discs);
        }
    }

    void ColourUpdate()
    {
        switch (currentSelect)
        {
            
            case 1:
                disc2.GetComponent<Renderer>().material.color = new Color(0.5f, 1.0f, 0.5f);
                disc3.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
                disc1.GetComponent<Renderer>().material.color = Color.black;
                //Debug.Log(currentSelect + " 1~ " + transform.rotation.eulerAngles.x + " 2~ " + transform.rotation.eulerAngles.x + " 3~ " + transform.rotation.eulerAngles.x);
                break;
            case 2:
                disc1.GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.5f);
                disc3.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
                disc2.GetComponent<Renderer>().material.color = Color.black;
                //GetComponent<Renderer>().material.color = new Color(0.5f, 1.0f, 0.5f);
                //Debug.Log(currentSelect + " 1~ " + transform.rotation.eulerAngles.x + " 2~ " + transform.rotation.eulerAngles.x + " 3~ " + transform.rotation.eulerAngles.x);
                break;
            case 3:
                //GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
                disc1.GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.5f);
                disc2.GetComponent<Renderer>().material.color = new Color(0.5f, 1.0f, 0.5f);
                disc3.GetComponent<Renderer>().material.color = Color.black;
                //Debug.Log(currentSelect + " 1~ " + transform.rotation.eulerAngles.x + " 2~ " + transform.rotation.eulerAngles.x + " 3~ " + transform.rotation.eulerAngles.x);
                break;
            
        }
    }

    void RotateDisc(float direction)
    {
        //Debug.Log("Direction: " + direction + " currentSelect: " + currentSelect + " rotationSpeeds for 1: " + rotationSpeed[0] + " 2:" + rotationSpeed[1] + " 3:" + rotationSpeed[2]);
        switch (currentSelect)
        {
            case 1:
                //Debug.Log("case 1");
                disc1.transform.Rotate(new Vector3(0, (rotationSpeed[0] * direction) * Time.deltaTime, 0));
                break;
            case 2:
                //Debug.Log("case 2");
                disc1.transform.Rotate(new Vector3(0, (rotationSpeed[0] * direction) * Time.deltaTime, 0));
                disc2.transform.Rotate(new Vector3(0, (rotationSpeed[1] * direction) * Time.deltaTime, 0));
                break;
            case 3:
                //Debug.Log("case 3");
                disc1.transform.Rotate(new Vector3(0, (rotationSpeed[0] * direction) * Time.deltaTime, 0));
                disc2.transform.Rotate(new Vector3(0, (rotationSpeed[1] * direction) * Time.deltaTime, 0));
                disc3.transform.Rotate(new Vector3(0, (rotationSpeed[2] * direction) * Time.deltaTime, 0));
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(mTimer.timer);
        //GetComponent<Renderer>().material.color = Color.grey;

        //Selected = true;
        //GetComponent<Renderer>().material.color = Color.black;
        ////////Debug.Log("Disc1: " + (int)Disc1.transform.rotation.eulerAngles.x + " | Disc2: " + (int)Disc2.transform.rotation.eulerAngles.x + " | Disc3: " + (int)Disc3.transform.rotation.eulerAngles.x + " | currentSelect: " + currentSelect);

        //Only for debugging purposes before implementing custom game objects
        //My aim is to make this system easily expandable if more/less discs were to be added

        if (Input.GetKeyDown(KeyCode.S) && currentSelect != 3)
        {
            currentSelect += 1;
            ColourUpdate();
        }
        if (Input.GetKeyDown(KeyCode.W) && currentSelect != 1)
        {
            currentSelect -= 1;
            ColourUpdate();
        }

        /*if (Input.GetKeyDown(KeyCode.N))
        {
            Timer.SetActive(true);
            Timer.GetComponent<TextMeshProUGUI>().enabled = true;
            OnDiscAlignmentReady(true);
            //Debug.Log("DISC ALIGNMENT!!!");
        }*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            if (GameObject.Find("TutorialBackground").GetComponent<MeshRenderer>().enabled == true)
            {
                GameObject.Find("TutorialBackground").GetComponent<MeshRenderer>().enabled = false;
                //Timer.SetActive(true);
                //Timer.GetComponent<MeshRenderer>().enabled = true;
                pregameText.GetComponent<TextMeshProUGUI>().enabled = false;
                //OnDiscAlignmentReady(true);
                //Timer.SetActive(true);
                timer.GetComponent<TextMeshProUGUI>().enabled = true;
                OnDiscAlignmentReady(true);
                //Debug.Log("If Timer isn't showing, press 'N'");
            }
            

            if (randomiseRotations == true)
            {
                int randDirection = 0;
                randDirection = UnityEngine.Random.Range(-1, 2);

                for (int i = 0; i < UnityEngine.Random.Range(30, 360); i++)
                {
                    currentSelect = (short)UnityEngine.Random.Range(1, 4);
                    //Debug.Log(randDirection);
                    //Debug.Log("Direction: " + randDirection + " currentSelect: " + currentSelect + "," + currentSelect * 5);
                    RotateDisc(1.0f * (currentSelect * 5));
                    currentSelect = 1;
                    ColourUpdate();
                }
                currentSelect = 1;
            }
            else
            {
                disc1.transform.rotation = disc1StartRotation;
                disc2.transform.rotation = disc2StartRotation;
                disc3.transform.rotation = disc3StartRotation;
            }
        }

        //Changed rotation input from Q and E to A and D to keep consistency with player controls
        if (Input.GetKey(KeyCode.D))
        {
            RotateDisc(1.0f);
            //transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
            //if (currentSelect == 2) { ... }
        }

        if (Input.GetKey(KeyCode.A))
        {
            RotateDisc(-1.0f);
            //transform.Rotate(new Vector3(0, -rotationSpeed * Time.deltaTime, 0));
        }


        //switch (currentSelect)
        //{
        //    case 1:
        //        Disc1.GetComponent<Renderer>().material.color = Color.black;
        //        break;
        //    case 2:
        //        Disc2.GetComponent<Renderer>().material.color = Color.black;
        //        break;
        //    case 3:
        //        Disc3.GetComponent<Renderer>().material.color = Color.black;
        //        break;
        //}
        //Debug.Log(currentSelect + " | " + transform.rotation.eulerAngles + " | " + numAligned[0] + "," + numAligned[1] + "," + numAligned[2]);
        //Debug.Log(transform.rotation.eulerAngles.x + " " + transform.rotation.eulerAngles.y + " " + transform.rotation.eulerAngles.z);

        //Selected = false;
        //switch (ID)
        //{
        //    case 1:
        //        GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.5f);
        //        //Debug.Log(currentSelect + " 1~ " + transform.rotation.eulerAngles.x + " 2~ " + transform.rotation.eulerAngles.x + " 3~ " + transform.rotation.eulerAngles.x);
        //        break;
        //    case 2:
        //        GetComponent<Renderer>().material.color = new Color(0.5f, 1.0f, 0.5f);
        //        //Debug.Log(currentSelect + " 1~ " + transform.rotation.eulerAngles.x + " 2~ " + transform.rotation.eulerAngles.x + " 3~ " + transform.rotation.eulerAngles.x);
        //        break;
        //    case 3:
        //        GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f);
        //        //Debug.Log(currentSelect + " 1~ " + transform.rotation.eulerAngles.x + " 2~ " + transform.rotation.eulerAngles.x + " 3~ " + transform.rotation.eulerAngles.x);
        //        break;
        //}




        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Debug.Log("R Pressed");
            //R1.transform.position = R1startPosition;
            //R1.transform.eulerAngles = R1startRotation;


            Debug.Log("R1.transform.rotation | startRotationR1 " + Disc1.transform.rotation + " | " + Disc1startRotation);
            //transform.rotation = Quaternion.Euler(0, -90, 90); //Default rotation: 0, -90, 90
            Disc1.transform.rotation = Disc1startRotation;
            Disc2.transform.rotation = Disc2startRotation;
            Disc3.transform.rotation = Disc3startRotation;
            Debug.Log("[Updated] R1.transform.rotation | startRotationR1 " + Disc1.transform.rotation + " | " + Disc1startRotation);
        }
        */



        //Testing example, to be set when paths align
        //1 - 85 to 105
        //2 - 80 to 110
        //3 - 70 to 120
        //if (transform.rotation.eulerAngles.x >= 85.0f && transform.rotation.eulerAngles.x <= 105.0f && ID == 1)
        //If ID1.transform.rotation == ID2.transform.rotation with 10 degree leniance (5 degrees for going over or under target rotation)

        //Debug.Log("Disc1: " + (int)Disc1.transform.rotation.eulerAngles.x + "," + (int)Disc1.transform.eulerAngles.y + "," + (int)Disc1.transform.eulerAngles.z);
        //Debug.Log("Disc2: " + (int)Disc2.transform.rotation.eulerAngles.x + "," + (int)Disc2.transform.eulerAngles.y + "," + (int)Disc2.transform.eulerAngles.z);
        //Debug.Log("Disc3: " + (int)Disc3.transform.rotation.eulerAngles.x + "," + (int)Disc3.transform.eulerAngles.y + "," + (int)Disc3.transform.eulerAngles.z);

        Debug.Log("Disc1 Rotations xyz: " + disc1.transform.rotation.eulerAngles.x + "," + disc1.transform.rotation.eulerAngles.y + "," + disc1.transform.rotation.eulerAngles.z);
        Debug.Log("Disc2 Rotations xyz: " + disc2.transform.rotation.eulerAngles.x + "," + disc2.transform.rotation.eulerAngles.y + "," + disc2.transform.rotation.eulerAngles.z);
        Debug.Log("Disc3 Rotations xyz: " + disc3.transform.rotation.eulerAngles.x + "," + disc3.transform.rotation.eulerAngles.y + "," + disc3.transform.rotation.eulerAngles.z);

        //Disc1 and Disc2
        if ((disc1.transform.rotation.eulerAngles.x >= disc2.transform.rotation.eulerAngles.x - 5.0f && disc1.transform.rotation.eulerAngles.x <= disc2.transform.rotation.eulerAngles.x + 5.0f 
            && disc1.transform.rotation.eulerAngles.y >= disc2.transform.rotation.eulerAngles.y - 5.0f && disc1.transform.rotation.eulerAngles.y <= disc2.transform.rotation.eulerAngles.y + 5.0f
            && disc1.transform.rotation.eulerAngles.z >= disc2.transform.rotation.eulerAngles.z - 5.0f && disc1.transform.rotation.eulerAngles.z <= disc2.transform.rotation.eulerAngles.z + 5.0f) || debugWin == true || (disc1.transform.rotation.eulerAngles.x - disc2.transform.rotation.eulerAngles.x) > 340.0f)
        {
            numAligned[0] = true;
            //Debug.Log((int)Disc1.transform.rotation.eulerAngles.x + " , " + (int)Disc2.transform.rotation.eulerAngles.x + " [Discs 1 and 2 are aligned]");
            r1.transform.position = new Vector3(-5, 10, 0);
            ////Disc1.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            numAligned[0] = false;
            r1.transform.position = new Vector3(-5, 15, 0);
        }

        //Disc1 and Disc3
        if ((disc1.transform.rotation.eulerAngles.x >= disc3.transform.rotation.eulerAngles.x - 5.0f && disc1.transform.rotation.eulerAngles.x <= disc3.transform.rotation.eulerAngles.x + 5.0f
            && disc1.transform.rotation.eulerAngles.y >= disc3.transform.rotation.eulerAngles.y - 5.0f && disc1.transform.rotation.eulerAngles.y <= disc3.transform.rotation.eulerAngles.y + 5.0f
            && disc1.transform.rotation.eulerAngles.z >= disc3.transform.rotation.eulerAngles.z - 5.0f && disc1.transform.rotation.eulerAngles.z <= disc3.transform.rotation.eulerAngles.z + 5.0f) || debugWin == true || (disc1.transform.rotation.eulerAngles.x - disc3.transform.rotation.eulerAngles.x) > 340.0f)
        {
            numAligned[1] = true;
            //Debug.Log((int)Disc1.transform.rotation.eulerAngles.x + " , " + (int)Disc3.transform.rotation.eulerAngles.x + " [Discs 1 and 3 are aligned]");
            r2.transform.position = new Vector3(0, 10, 0);
            ////Disc3.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            numAligned[1] = false;
            r2.transform.position = new Vector3(-5, 15, 0);
        }

        //Disc2 and Disc3
        if ((disc2.transform.rotation.eulerAngles.x >= disc3.transform.rotation.eulerAngles.x - 5.0f && disc2.transform.rotation.eulerAngles.x <= disc3.transform.rotation.eulerAngles.x + 5.0f
            && disc2.transform.rotation.eulerAngles.y >= disc3.transform.rotation.eulerAngles.y - 5.0f && disc2.transform.rotation.eulerAngles.y <= disc3.transform.rotation.eulerAngles.y + 5.0f
            && disc2.transform.rotation.eulerAngles.z >= disc3.transform.rotation.eulerAngles.z - 5.0f && disc2.transform.rotation.eulerAngles.z <= disc3.transform.rotation.eulerAngles.z + 5.0f) || debugWin == true || (disc1.transform.rotation.eulerAngles.x - disc2.transform.rotation.eulerAngles.x) > 340.0f)
        {
            numAligned[2] = true;
            //Debug.Log((int)Disc1.transform.rotation.eulerAngles.x + " , " + (int)Disc3.transform.rotation.eulerAngles.x + " [Discs 2 and 3 are aligned]");
            r3.transform.position = new Vector3(5, 10, 0);
            ////Disc2.GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            numAligned[2] = false;
            r3.transform.position = new Vector3(5, 15, 0);
        }

        //Disc2 and Disc3

        /*
        if ((Disc1.transform.rotation.eulerAngles.x >= Disc2.transform.rotation.eulerAngles.x - 10.0f && Disc1.transform.rotation.eulerAngles.x <= Disc2.transform.rotation.eulerAngles.x + 10.0f) || debugWin == true)
        {
            numAligned[0] = true; //numAligned[ID - 1] = true; //numAligned[0] = true
            Debug.Log((int)Disc1.transform.rotation.eulerAngles.x + " , " + (int)Disc2.transform.rotation.eulerAngles.x + " [Disc 1 is in target position]");
            R1.transform.position = new Vector3(-5, 10, 0);
        }
        else
        {
            if (ID == 1) { numAligned[ID - 1] = false; R1.transform.position = new Vector3(-5, 15, 0); Debug.Log((int)Disc1.transform.rotation.eulerAngles.x + " , " + (int)Disc2.transform.rotation.eulerAngles.x); }
        }

        //if (transform.rotation.eulerAngles.x >= 80.0f && transform.rotation.eulerAngles.x <= 110.0f && ID == 2)
        //Disc1 and Disc3
        if ((Disc1.transform.rotation.eulerAngles.x >= Disc3.transform.rotation.eulerAngles.x - 10.0f && Disc1.transform.rotation.eulerAngles.x <= Disc3.transform.rotation.eulerAngles.x + 10.0f) || debugWin == true)
        {
            numAligned[1] = true;
            //numAligned[ID - 1] = true; //numAligned[1] = true
            Debug.Log("Disc 2 is in target position");
            R2.transform.position = new Vector3(0, 10, 0);
        }
        else
        {
            if (ID == 2) { numAligned[0] = true;  R2.transform.position = new Vector3(0, 15, 0); }
            //numAligned[ID - 1] = false;
        }
        //Disc2 and Disc3
        //if (transform.rotation.eulerAngles.x >= 70.0f && transform.rotation.eulerAngles.x <= 120.0f && ID == 3)
        if ((Disc2.transform.rotation.eulerAngles.x >= Disc3.transform.rotation.eulerAngles.x - 10.0f && Disc2.transform.rotation.eulerAngles.x <= Disc3.transform.rotation.eulerAngles.x + 10.0f) || debugWin == true)
        {
            numAligned[ID - 1] = true; //numAligned[2] = true
            Debug.Log("Disc 3 is in target position");
            R3.transform.position = new Vector3(5, 10, 0);
        }
        else
        {
            if (ID == 3) { numAligned[ID - 1] = false; R3.transform.position = new Vector3(5, 15, 0); }
        }
        */
        //Debug.Log(currentSelect);

        //Debug.Log("Discs: " + Disc1.transform.position + "," + Disc2.transform.position + "," + Disc3.transform.position + " | " + "Rs: " + R1.transform.position + "," + R2.transform.position + "," + R3.transform.position);

        //if (Input.GetKeyDown(KeyCode.M))
        //{
            if (r1.transform.position == new Vector3(-5, 10, 0) && r2.transform.position == new Vector3(0, 10, 0) && r3.transform.position == new Vector3(5, 10, 0))// || debugWin == true)
            {
            //Debug.Log(mTimer.timer + "!!!!!!!!!");
            timer.SetActive(false);
            timer.GetComponent<TextMeshProUGUI>().enabled = false;
            OnDiscAlignmentReady(false);
            gC.mC.completedDoor = true;

            gC.inMinigame = false;
            scoreSystemGameObject.SendMessage("CompletedMinigame", new Vector2(2, mTimer.timer)); //2 = DiscAlignment minigame NEED TO REPLACE 10, ACTING AS PLACEHOLDER!!!
            Debug.Log("Disc Complete!!!");
            //Debug.Log("You Win!");
        }
        //}

        //if (Input.GetKeyDown("p"))
        //{
        //    R1.transform.position = new Vector3(-5, 10, 0);
        //    R2.transform.position = new Vector3(0, 10, 0);
        //    R3.transform.position = new Vector3(5, 10, 0);
        //    //Disc1 = Disc 3; Disc2 = Disc3

        //    debugWin = true;
        //}

        //GetComponent<Renderer>().material.color = Color.grey;
    }
}
