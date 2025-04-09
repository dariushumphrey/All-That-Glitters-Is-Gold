using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Panda;

public class ReplevinScript : MonoBehaviour
{
    public int damage;
    //This value buffs enemy damage:
    //-Increasing this number adds a percentage of current damage onto itself
    //-Increasing damage allows this number to inflict more damage in return
    //-Note: The code that increases this number by difficulty is in the EnemyHealthScript.
    public float damagePercent = 10f;

    public int moveSpeed = 5;
    public int boostSpeed = 7;
    public float meleeRangeMin; //value required to register as a melee hit
    public float chargeRangeMin; //value required to register as a charge hit
    public float rotationStrength = 2f; //Governs turning speed of Enemy while Player is in range
    public float gapClose = 0f; //Governs speed of pounce; 0.07-0.1 is optimal
    public float pounceLimit = 1.5f; //Governs how far Enemy will pounce on position before timing out
    public float jumpLimit = 4f; //Governs how far Enemy will jump towards Player before timing out
    public float jumpForce = 5f;
    public float meleeAttackForce;
    public float chargeAttackForce;
    public float rangedAttackForce;
    public float chargeTimeout; //Time to wait before charging again
    public float punchTimeout; //Time to wait before pouncing again
    public float jumpTimeout; //Time to wait before jumping again
    public float airtimeShort = 2f;
    public float rangeATKMin;
    public float attackRate;
    public float agitationLimit;
    public bool amLeader;
    public bool amFollower;
    public bool amHunter;
    public Transform attackStartPoint;
    public Transform jumpCheck;
    public LineRenderer attackLine;
    public Material outRangeColor;
    public Material inRangeColor;
    public Material rangeHitColor;
    public List<GameObject> cluster = new List<GameObject>();
    public GameObject leader;
    private float gapCloseReset;
    private bool ramTimeout = false;
    private bool slamTimeout = false;
    private bool fireTimeout = false;
    private bool lockOn = false;

    internal NavMeshAgent self;
    private EnemyHealthScript enemy;
    private Vector3 focus;
    private Vector3 distance, lastKnownDistance;
    private Vector3 lastPlayerPosition;
    private LayerMask layer;

    private GameObject[] waypoint;
    private int waypointNext;
    private float accuracy = 5.0f;
    private GameObject player;
    internal bool playerFound = false;

    private float chargeReset;
    private float punchReset;
    private float jumpReset;
    private float airtimeReset;
    private float attackAgain;
    private float agitationTimer;
    private bool agitated;
    private bool canAttackAgain = true;
    private bool recorded = false;
    private bool fireSequence = false;

    // Start is called before the first frame update
    void Start()
    {
        layer = LayerMask.GetMask("Player");

        self = GetComponent<NavMeshAgent>();
        waypoint = GameObject.FindGameObjectsWithTag("Waypoint");
        waypointNext = Random.Range(0, waypoint.Length);
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<EnemyHealthScript>();
        playerFound = false;

        gapCloseReset = gapClose;
        chargeReset = chargeTimeout;
        punchReset = punchTimeout;
        jumpReset = jumpTimeout;
        airtimeReset = airtimeShort;
        agitationTimer = 0.0f;
        agitated = false;

        if(amLeader)
        {
            leader = gameObject;

            if (cluster.Count > 0)
            {
                for (int f = 0; f < cluster.Count; f++)
                {
                    cluster[f].GetComponent<ReplevinScript>().leader = gameObject;
                }
            }
        }  
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(self.velocity.magnitude);

        GrowingAgitation();
        ClusterManagement();
        CanSeePlayer();
        HaveIDied();

        if(jumpCheck != null)
        {
            AmIGrounded();
        }        
    }

    public void ClusterManagement()
    {
        for (int z = 0; z < cluster.Count; z++)
        {
            if (cluster[z].GetComponent<EnemyHealthScript>().healthCurrent <= 0)
            {
                cluster.RemoveAt(z);
            }
        }
    }

    public void GrowingAgitation()
    {
        if(playerFound)
        {
            agitationTimer += Time.deltaTime;
            //Debug.Log(agitationTimer);

            if (agitationTimer >= agitationLimit)
            {
                agitated = true;
                agitationTimer = 0.0f;

            }
        }
    }
   
    public bool CanSeePlayer()
    {
        RaycastHit rayInfo;
        Vector3 rayToTarget = player.transform.position - transform.position;
        Debug.DrawRay(transform.position, rayToTarget);
        if(Physics.Raycast(transform.position, rayToTarget, out rayInfo, Mathf.Infinity, layer))
        {
            if(rayInfo.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }

    public bool AmIGrounded()
    {
        Debug.DrawRay(jumpCheck.transform.position, -jumpCheck.transform.up);
        RaycastHit impact;
        if(Physics.Raycast(jumpCheck.transform.position, -jumpCheck.transform.up, out impact, 1f))
        {
            if(impact.point != null)
            {
                return true;
            }
           
        }

        return false;
    }

    public bool HaveIDied()
    {
        if(enemy.healthCurrent <= 0)
        {
            return true;
        }

        return false;
    }

    [Task]
    bool Monitor(float turnAngle)
    {
        Vector3 p = transform.position + Quaternion.AngleAxis(turnAngle, Vector3.up) * transform.forward;
        focus = p;
        return true;
    }

    [Task]
    public void LookAtFocus()
    {
        Vector3 direction = focus - transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);

        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("angle={0}", Vector3.Angle(transform.forward, direction));
        }

        if (Vector3.Angle(transform.forward, direction) < 5.0f)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void VisitWaypoint()
    {      
        self.SetDestination(waypoint[waypointNext].transform.position);

        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        }

        if (self.remainingDistance <= self.stoppingDistance && !self.pathPending)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void FollowLeader()
    {
        if(amFollower)
        {
            focus = leader.transform.position;
            self.SetDestination(focus);
        }

        if (Task.isInspected)
        {
            Task.current.debugInfo = string.Format("t={0:0.00}", Time.time);
        }

        if (self.remainingDistance <= self.stoppingDistance && !self.pathPending)
        {
            Task.current.Succeed();
        }

    }

    [Task]
    public void PlayerLockOn()
    {
        playerFound = true;
        focus = player.transform.position;
        Task.current.Succeed();
    }

    [Task]
    public void PlayerSeek()
    {
        self.SetDestination(focus);
        Task.current.Succeed();
    }

    [Task]
    public bool IsPlayerClose()
    {
        distance = transform.position - player.transform.position;
        if(distance.magnitude <= meleeRangeMin)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    [Task]
    public bool IsPlayerFar()
    {
        distance = transform.position - player.transform.position;
        if (distance.magnitude >= rangeATKMin)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    [Task]
    public void AttackMelee()
    {
        if(!HaveIDied())
        {
            self.speed = moveSpeed;
            self.SetDestination(player.transform.position);
            distance = transform.position - player.transform.position;
            attackAgain += Time.deltaTime;

            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit;

            //attackLine.SetPosition(0, attackStartPoint.position);
            //attackLine.SetPosition(1, rayOrigin + (attackStartPoint.transform.forward * meleeRangeMin));
            //attackLine.material = inRangeColor;

            if (distance.magnitude <= meleeRangeMin && attackAgain >= attackRate)
            {
                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin))
                {
                    //attackLine.SetPosition(1, hit.point);
                    //attackLine.material = rangeHitColor;

                    if (hit.collider.tag == "Player" && attackAgain >= attackRate)
                    {
                        attackAgain = 0.0f;
                        hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                        hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                        //This code shoves the Player with particular force in their opposite direction.
                        //This is a melee attack, shoving the player with less force, subtly offsetting the player upwards to distinguish it from a charge.
                        Vector3 knockbackDir = -hit.collider.transform.forward;
                        //knockbackDir.y = 0;
                        hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                        //if (GetComponent<EnemyFollowerScript>() != null)
                        //{
                        //    if (GetComponent<EnemyFollowerScript>().leader != null && GetComponent<EnemyFollowerScript>().leader.GetComponent<EnemyHealthScript>().healthCurrent > 0)
                        //    {
                        //        GetComponent<EnemyFollowerScript>().leader.Pursuit();
                        //    }

                        //    GetComponent<EnemyFollowerScript>().ChasePlayer();
                        //}              
                    }
                }


            }

            else
            {
                //attackLine.material = inRangeColor;
            }
        }

        else
        {
            self.enabled = false;

            attackLine.SetPosition(0, attackStartPoint.position);
            attackLine.SetPosition(1, attackStartPoint.position);
        }
        

        Task.current.Succeed();

    }

    [Task]
    public void AttackRange()
    {
        if(!HaveIDied())
        {
            self.speed = moveSpeed;

            if (self.enabled != true)
            {
                self.enabled = true;
                self.SetDestination(player.transform.position);
            }

            else
            {
                self.SetDestination(player.transform.position);
            }

            distance = transform.position - player.transform.position;
            attackAgain += Time.deltaTime;

            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit;

            //attackLine.SetPosition(0, attackStartPoint.position);
            //attackLine.SetPosition(1, rayOrigin + (attackStartPoint.transform.forward * rangeATKMin));
            //attackLine.material = outRangeColor;

            transform.LookAt(player.transform.position);

            if (distance.magnitude <= rangeATKMin && CanSeePlayer())
            {
                self.enabled = false;

                //attackLine.SetPosition(1, player.transform.position);
                //attackLine.material = inRangeColor;

                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, rangeATKMin) && attackAgain >= attackRate)
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        //attackLine.SetPosition(1, hit.collider.transform.position);
                        Debug.Log("Danger of Friendly Fire; Aborting!");
                        Task.current.Fail();
                    }

                    else
                    {
                        attackAgain = 0.0f;

                        //attackLine.SetPosition(1, player.transform.position);
                        //attackLine.material = rangeHitColor;

                        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        projectile.transform.position = attackStartPoint.transform.position;
                        projectile.transform.rotation = attackStartPoint.transform.rotation;
                        projectile.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        projectile.name = "Projectile";
                        projectile.tag = "Projectile";

                        projectile.GetComponent<SphereCollider>().isTrigger = true;
                        projectile.AddComponent<Rigidbody>();
                        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * rangedAttackForce);

                        projectile.AddComponent<ProjectileScript>();
                        projectile.GetComponent<ProjectileScript>().damage = damage;
                    }
                }
            }

            else
            {
                if (self.enabled != true)
                {
                    self.enabled = true;
                    self.SetDestination(player.transform.position);
                }
            }
        }

        else
        {
            self.enabled = false;

            attackLine.SetPosition(0, attackStartPoint.position);
            attackLine.SetPosition(1, attackStartPoint.position);
        }     

        Task.current.Succeed();
    }

    [Task]
    public void AttackCharge()
    {
        if(!HaveIDied())
        {
            self.speed = boostSpeed;

            //Vector3 chargeVector = transform.position - player.transform.position;

            //GameObject help = new GameObject();
            //help = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //help.transform.position = player.transform.position - chargeVector;

            self.SetDestination(player.transform.position);
            //Debug.DrawRay(player.transform.position - chargeVector, chargeVector, Color.blue);

            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit;

            //attackLine.SetPosition(0, attackStartPoint.position);
            //attackLine.SetPosition(1, rayOrigin + (attackStartPoint.transform.forward * chargeRangeMin));
            //attackLine.material = inRangeColor;

            if (distance.magnitude <= chargeRangeMin)
            {
                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, chargeRangeMin))
                {
                    //attackLine.SetPosition(1, hit.point);
                    //attackLine.material = rangeHitColor;

                    if (hit.collider.tag == "Player" && canAttackAgain)
                    {
                        //attackLine.SetPosition(1, hit.point);
                        //attackLine.material = rangeHitColor;

                        hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                        hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                        //This code shoves the Player with particular force in their opposite direction.
                        //This is a charge attack, knocking the player with more force and keeping them grounded to distinguish it from a melee.
                        Vector3 knockbackDir = -hit.collider.transform.forward;
                        knockbackDir.y = 0;
                        hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * chargeAttackForce);

                        //if (GetComponent<EnemyFollowerScript>() != null)
                        //{
                        //    if (GetComponent<EnemyFollowerScript>().leader != null && GetComponent<EnemyFollowerScript>().leader.GetComponent<EnemyHealthScript>().healthCurrent > 0)
                        //    {
                        //        GetComponent<EnemyFollowerScript>().leader.Pursuit();
                        //    }

                        //    GetComponent<EnemyFollowerScript>().ChasePlayer();
                        //}

                        self.speed = moveSpeed;
                        agitated = false;
                        ramTimeout = true;
                        canAttackAgain = false;
                    }
                }
            }

        }

        else
        {
            self.enabled = false;

            attackLine.SetPosition(0, attackStartPoint.position);
            attackLine.SetPosition(1, attackStartPoint.position);
        }

        Task.current.Succeed();
    }

    [Task]
    public void MajorAttackJump()
    {
        if (!HaveIDied())
        {
            self.speed = moveSpeed;
            distance = player.transform.position - transform.position;
            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit, hitTheSequel;

            if (distance.magnitude <= meleeRangeMin && CanSeePlayer())
            {
                if (self.enabled == true)
                {
                    self.ResetPath();
                    self.enabled = false;
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
                {
                    if (hit.collider.tag == "Player")
                    {
                        lastPlayerPosition = (hit.point - transform.position).normalized;
                        //lastKnownDistance = lastPlayerPosition - transform.position;
                        recorded = true;

                        gameObject.GetComponent<Rigidbody>().AddForce((lastPlayerPosition + Vector3.up) * jumpForce, ForceMode.Impulse);
                        //Debug.Log(lastPlayerPosition);
                        //Debug.Log(lastKnownDistance.magnitude);
                        //slamTimeout = true;
                    }

                }

                if (AmIGrounded())
                {
                    airtimeShort -= Time.deltaTime;
                    if (airtimeShort <= 0f)
                    {
                        airtimeShort = airtimeReset;
                        recorded = false;
                    }
                }

                else
                {
                    airtimeShort = airtimeReset;
                }

                //transform.position = Vector3.Lerp(transform.position, lastPlayerPosition, gapClose);
                //lastKnownDistance = lastPlayerPosition - transform.position;
                //Debug.Log(lastKnownDistance.magnitude + " | " + self.stoppingDistance);
                //Debug.Log(distance.magnitude + " | " + jumpLimit);

                if (distance.magnitude <= jumpLimit)
                {
                    //self.speed = 0;
                    lockOn = true;
                }

                if (lockOn)
                {
                    transform.position = Vector3.Lerp(transform.position, player.transform.position, gapClose);
                    //slamTimeout = true;
                }

                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hitTheSequel, 2f))
                {
                    if (hitTheSequel.collider.tag == "Player" && canAttackAgain)
                    {
                        Vector3 knockbackDir = transform.forward;
                        knockbackDir.y = 0;
                        hitTheSequel.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                        hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                        hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                        slamTimeout = true;
                        canAttackAgain = false;
                    }
                }
            }

            else
            {
                if (self.enabled == false)
                {
                    self.enabled = true;
                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                }

                recorded = false;
                self.SetDestination(player.transform.position);
            }

            //attackLine.SetPosition(0, attackStartPoint.position);
            //attackLine.SetPosition(1, rayOrigin + (attackStartPoint.transform.forward * meleeRangeMin));
            //attackLine.material = inRangeColor;
        }

        else
        {
            self.enabled = false;
        }
             
        Task.current.Succeed();
    }

    [Task]
    public void MajorAttackPounce()
    {
        if(!HaveIDied())
        {
            self.speed = moveSpeed;
            distance = player.transform.position - transform.position;
            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit, hitTheSequel;

            if (distance.magnitude <= meleeRangeMin && CanSeePlayer())
            {
                self.ResetPath();
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
                {
                    if (hit.collider.tag == "Player")
                    {
                        lastPlayerPosition = hit.point;
                        recorded = true;
                        //Debug.Log(lastPlayerPosition);
                        //Debug.Log(lastKnownDistance.magnitude);
                    }
                }

                transform.position = Vector3.Lerp(transform.position, lastPlayerPosition, gapClose * Time.deltaTime);
                lastKnownDistance = lastPlayerPosition - transform.position;
                Debug.Log(lastKnownDistance.magnitude + " | " + self.stoppingDistance);

                if (lastKnownDistance.magnitude <= pounceLimit)
                {
                    slamTimeout = true;
                }

                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hitTheSequel, 2f))
                {
                    if (hitTheSequel.collider.tag == "Player" && canAttackAgain)
                    {
                        hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                        hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                        Vector3 knockbackDir = transform.forward;
                        knockbackDir.y = 0;
                        hitTheSequel.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                        slamTimeout = true;
                        canAttackAgain = false;
                    }
                }
            }

            else
            {
                if (self.enabled == false)
                {
                    self.enabled = true;
                }

                recorded = false;
                self.SetDestination(player.transform.position);
            }

            //attackLine.SetPosition(0, attackStartPoint.position);
            //attackLine.SetPosition(1, rayOrigin + (attackStartPoint.transform.forward * meleeRangeMin));
            //attackLine.material = inRangeColor;

            Task.current.Succeed();
        }

        else
        {
            self.enabled = false;
        }
        
    }  

    [Task]
    public void RamTimeout()
    {
        if (ramTimeout == true)
        {
            self.speed = 0;
            chargeTimeout -= Time.deltaTime;
            if (chargeTimeout < 0f)
            {
                chargeTimeout = chargeReset;
                self.speed = moveSpeed;
                ramTimeout = false;
                canAttackAgain = true;
            }
        }

        Task.current.Succeed();

    }  

    [Task]
    public void JumpTimeout()
    {
        if (slamTimeout == true)
        {
            gapClose = 0f;
            airtimeShort = airtimeReset;
            jumpTimeout -= Time.deltaTime;
            if (jumpTimeout <= 0f)
            {
                gapClose = gapCloseReset;
                jumpTimeout = jumpReset;
                slamTimeout = false;
                canAttackAgain = true;
                recorded = false;
                lockOn = false;
            }
        }

        Task.current.Succeed();

    }

    [Task]
    public void PounceTimeout()
    {
        if (slamTimeout == true)
        {
            punchTimeout -= Time.deltaTime;
            gapClose = 0f;
            if (punchTimeout <= 0f)
            {
                gapClose = gapCloseReset;
                punchTimeout = punchReset;
                slamTimeout = false;
                canAttackAgain = true;
                recorded = false;
            }
        }

        Task.current.Succeed();

    }

    [Task]
    public void Retreat()
    {
        distance = player.transform.position - transform.position;
        self.SetDestination(transform.position - distance);
        if(distance.magnitude > meleeRangeMin)
        {

        }

        Task.current.Succeed();
    }   

    [Task]
    public void ChooseNextWaypoint()
    {
        if (waypoint.Length == 0)
        {
            return;
        }

        if (Vector3.Distance(waypoint[waypointNext].transform.position, self.transform.position) < accuracy)
        {
            waypointNext = Random.Range(0, waypoint.Length);
        }

        Task.current.Succeed();
    }  
    
    [Task]
    public bool PlayerFound()
    {
        if(!playerFound)
        {
            return false;
        }

        else
        {          
            return true;
        }
    }

    [Task]
    public void AlertMyCluster()
    {
        if(playerFound)
        {
            if (amLeader && cluster.Count > 0)
            {
                for (int f = 0; f < cluster.Count; f++)
                {
                    cluster[f].GetComponent<ReplevinScript>().playerFound = true;
                    cluster[f].GetComponent<ReplevinScript>().focus = player.transform.position;
                }
            }
        }
        
        Task.current.Succeed();
    }

    [Task]
    public bool AmILeading()
    {
        if (!amLeader)
        {
            return false;
        }

        else
        {          
            return true;
        }
    }

    [Task]
    public bool AmIFollowing()
    {
        if (!amFollower)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    [Task]
    public bool AmIHunting()
    {
        if (!amHunter)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    [Task]
    public bool Agitated()
    {
        if(agitated)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

}
