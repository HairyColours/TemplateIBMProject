using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class BotInfo : MonoBehaviour
{
    // Misc
    [Header("Misc Settings")]
    
    public int bThreatLevel;
    public int bBotCount;
    public int bRemainingBots;
    public GameController.Status bStatus;
    public LayerMask bObstacleLayer;
    public List<Component> bAbilitiesList;
    private bool bAbilityAdd;
    
    // Throwing
    [Header("Ranged Attack Settings")] 
    public float bFireRate = 0.5f;
    [HideInInspector]
    public Transform bShotStart;
    [HideInInspector]
    public float bProjectileSpeed;
    [HideInInspector]
    public float bNextFire;
    [HideInInspector]
    public Shooting bShooting;
    private GameObject bVisuals;

    // Patrol
    [Header("Patrol Settings")] 
    public GameObject bPaths;
    public bool bPointLoop;
    [HideInInspector]
    public bool bCreatePoints;
    [HideInInspector]
    public Transform[] bPatrol;
    [HideInInspector]
    public int bDestPoint;
    [HideInInspector]
    public bool bDirection;

    // Wander
    [Header("Wander Settings")] 
    [HideInInspector]
    public bool bRecentlyChase;
    public float bWanderRadius;
    public float bWanderTimer;
    public float bWanderDistance;
    public float bWanderJitter;
    [HideInInspector]
    public Vector3 bWanderTarget;
    [HideInInspector]
    public float bTimer;
    
    // LockOn
    [Header("Player Chase Settings")]
    public GameObject bPlayer;
    [HideInInspector]
    public float bViewRadius;
    public float bDefaultViewRadius;
    public float bSusViewRadius;
    [Range(0, 360)] 
    public float bViewAngle;
    [HideInInspector] 
    public float bViewAngleN;
    [HideInInspector]
    public float bViewAngleD;
    public float bDetectionTimer;
    public int bTimeBeforeDetect;
    [HideInInspector]
    public bool bEngaging;
    public float bSpeed;
    public float bRotationSpeed;
    [HideInInspector]
    public Vector3 bPreviousPos;
    [HideInInspector]
    public Vector3 bVelocity;
    public float bPredictionTime;
    public float bRecentChaseTimer;
    [HideInInspector]
    public float bRecentChaseTimerN;
    
    // Suspicious
    [Header("Suspicious Settings")]
    public float bSuspiciousRadius;
    public float bSuspiciousTimer;
    public int bSearchTime;
    [HideInInspector]
    public Vector3 bDebugLastKnownPos;
    [HideInInspector]
    public float bSusTimer;
    [HideInInspector]
    public bool bPlayerInView;

    private void Start()
    {
        // Misc
        bBotCount = BotCalc();
        bRemainingBots = BotCalc();
        bStatus = GameObject.Find("Main Camera").GetComponent<GameController>().PlayerStatus;
        //bObstacleLayer = LayerMask.NameToLayer("Obstacle");
        bObstacleLayer = LayerMask.GetMask("Obstacle");
        bAbilitiesList = new List<Component>();
        bAbilityAdd = false;
        // Ranged Attack
        bVisuals = gameObject.transform.GetChild(0).GetChild(1).gameObject;
        bProjectileSpeed = 1000.0f;
        bShooting = gameObject.AddComponent<Shooting>();
        bShooting.SetHost(bVisuals);
        bShooting.bulletSpeed = bProjectileSpeed;
        bShotStart = gameObject.transform.GetChild(0).GetChild(2);
        bNextFire = bFireRate;
        // Patrol
        bPaths = GameObject.Find("PatrolPaths");
        bDestPoint = 0;
        // Wander
        bTimer = bWanderTimer;
        // LockOn
        bViewRadius = bDefaultViewRadius;
        bDetectionTimer = 0;
        bRecentChaseTimerN = bRecentChaseTimer;
        // Suspicious
        bSusTimer = bSuspiciousTimer;
    }

    private void Update()
    {
        Debug.Log(bStatus);
        if (bDetectionTimer == 0) return;
        DateTime now = DateTime.Now;
        if (!bPlayerInView &&
            GetComponent<Perception>().sensedRecord[0].timeLastSensed < now.Subtract(new TimeSpan(0, 0, bSearchTime)))
        {
            bStatus = GameController.Status.SAFE;
            bDetectionTimer = 0;
            bViewRadius = bDefaultViewRadius;
        }
        
    }
    private void LateUpdate()
    {
        bRemainingBots = BotCalc();
        if (bAbilityAdd) return;
        bAbilitiesList.AddRange(GetComponents(typeof(Ability)));
        bAbilityAdd = true;
    }

    private static int BotCalc()
    {
        BotInfo[] botScripts = FindObjectsOfType<BotInfo>();
        List<GameObject> bots = botScripts.Select(t => t.transform.root.gameObject).ToList();
        return bots.Count;
    }
    
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, GetComponent<NavMeshAgent>().destination);
    }
}