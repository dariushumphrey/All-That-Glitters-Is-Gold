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
    public float damagePercent = 10f;
    private int damageAdd; //Number used to increase Enemy damage

    [Header("General")]

    public int moveSpeed = 5;
    public int boostSpeed = 7;
    public int enemyAcceleration = 8;
    public float rotationStrength = 2f; //Governs turning speed of Enemy while Player is in range
    public bool amBoss; //Affirms Boss status if true
    public bool amSentry; //Affirms stationary, inactive Nav Mesh Agent if true
    public LayerMask contactOnly; //Permits raycast interaction only with specified Layers
    public Transform attackStartPoint; //Origin point of Enemy attack
    public GameObject stunningLucent; //Harmful Lucent Cluster game object

    [Header("Melee Attack settings")]

    public float meleeEngagementDistance; //Distance required for Enemy to setup attack
    public float meleeRangeCheck; //Distance required for Enemy to begin attack
    public float meleeRangeMin; //Melee attack range
    public float meleeAttackTimer = 0.8f; //Time to wait before committing a Melee attack
    public float meleeTimeout; //Time to wait before next melee
    public float meleeAttackForce;
    public float attackRate; //Rate of speed of which attacks occur

    [Header("Range Attack settings")]

    //This value buffs attack rate of Ranged enemies:
    //-Increasing this number adds a percentage of fire rate onto itself, allowing for faster firing.
    public float rangeAttackChange = 50f;
    public float rangeAttackRate;
    public float rangedAttackForce;
    public float rangeATKMin; //Distance required for Enemy to begin attack
    public float rangeEngagementDistance; //Distance required for Enemy to setup attack
    public int rangeAttackCount = 0; //Maximum range attacks to perform
    public float strafeDistance = 2f; //Extra distance extension for strafe action
    public float strafeLimit = 1f; //Extent of strafe goal before stopping
    public float strafeTimer = 3f; //Time delay before committing new strafe action
    public GameObject rangeProjectile; //Projectile game object

    [Header("Charge Attack settings")]

    public float chargeRangeCheck; //Distance required for Enemy to begin attack
    public float chargeRangeMin; //Charge attack range
    public float chargeLimit = 1f; //Governs how far Enemy will charge on position before timing out
    public float chargeOvershoot = 2f; //Governs how far Enemy will travel beyond the Player's last position
    public float chargeTimeout; //Time to wait before charging again
    public float chargeBuildup = 1f; //Time delay before next action -- Bosses only
    public float chargeAttackForce;

    [Header("Pounce Attack settings")]

    public float gapClose = 0f; //Governs speed of pounce; 0.07-0.1 is optimal  
    public float pounceLimit = 1.5f; //Governs how far Enemy will pounce on position before timing out
    public float punchTimeout; //Time to wait before pouncing again

    [Header("Jump Attack settings")]

    public float jumpLimit = 4f; //Governs how far Enemy will jump towards Player before timing out
    public float jumpForce = 5f; //Upwards jump force
    public float forwardForce = 4f; //Forward jump force
    public float jumpTimeout; //Time to wait before jumping again
    public float airtimeShort = 2f; //Forces another jump if grounded
    public float berthJumpCarpetBombTimer = 0.15f; //Harmful cluster drop rate -- Berth only
    public GameObject jumpTakeoff; //VFX used when jumping
    public Transform jumpCheck; //Transform used to determine if grounded

    private EnemyManagerScript manager;
    private Color attackTell = Color.white;
    private int materialIndex = 1; //Index used to find Material
    private Renderer subject;
    private Material[] materials;
    internal NavMeshAgent self;
    internal EnemyHealthScript enemy;
    internal BossManagerScript boss; //For bosses only -- Used to spawn enemies when returning immune

    //distance - Describes length between Player and Enemy
    //lastKnownDistance - Describes last recorded length between Player and Enemy
    //lastPlayerPosition - Describes last recorded Player position
    //recoilPosition - Describes stagger position when hitting Lucent wall -- Bosses only
    //strafePos - Holds position for Enemy to strafe towards
    //strafeCalc - Describes calculated strafing direction
    private Vector3 distance, lastKnownDistance, lastPlayerPosition, recoilPosition, strafePos, strafeCalc;
    private Vector3 lucentPrimed; //Describes length between Boss and Combustible Lucent -- Bosses only
    private GameObject primedLucent; //Lucent used to trigger damage phase -- Bosses only
    private GameObject[] combustibleLucent; //List of damage-mechanic Lucent -- Bosses only
    internal GameObject stunMechanic; //New Lucent Cluster game object

    public GameObject[] waypoint; //Array of waypoints
    private int waypointNext; //Number used to randomly choose next waypoint
    private float accuracy = 5.0f; //Goal length between Waypoint and Enemy
    private GameObject player; //Player game object

    internal int moveSpeedSlow; //Modified Enemy speed when Slowed
    internal int boostSpeedSlow; //Modified Enemy boost speed when Slowed
    internal int nmaAccelSlow; //Modified NavMeshAgent acceleration when Slowed
    internal int moveSpeedReset; //Holds starting Enemy speed
    internal int boostSpeedReset; //Holds starting Enemy boost speed
    internal int accelReset; //Holds starting NavMeshAgent acceleration

    private int burstCount = 0; //Total count of range attacks performed
    private int phaseTwoHealthThreshold;
    private float meleeReset; //Holds starting Melee attack timer
    private float chargeReset; //Holds starting Charge attack timer
    private float buildupReset; //Holds starting Charge attack delay timer
    private float punchReset; //Holds starting Pounce attack timer
    private float jumpReset; //Holds starting Jump attack timer
    private float airtimeReset; //Holds starting Force jump timer
    private float attackAgain; //Holds starting attack rate
    private float gapCloseReset; //Holds starting Pounce attack speed
    private float strafeReset; //Holds starting strafe time delay
    private float berthJumpTimerReset; //Holds starting Berth Jump cluster timer
    private float rangeCooldown; //Time to wait before next range attack
    private float meleeCooldown; //Time to wait before next melee attack
    private float playerDetected; //Timer spent seeing Player
    private float detectionLimit = 2f; //Goal time from seeing Player
    private float ambushTime = 0.75f; //Delay used to speed up lerp movement -- Bosses only
    internal bool destinationSet = false; //Affirms location if true
    private bool gathered = false; //Affirms cluster has been spawned if true -- Bosses only
    private bool throwTarget = false; //Affirms Enemy can throw cluster at Player if true -- Bosses only
    private bool canAttackAgain = true; //Affirms Enemy can attack once more if true
    private bool recorded = false; //Affirms information has been saved for attack if true
    private bool attackLock = false; //Affirms Enemy is using Raycast for attack if true
    private bool ramTimeout = false; //Affirms Charge enemy has ended attack if true
    internal bool slamTimeout = false; //Affirms Jump, Pounce Enemy has ended attack if true
    private bool rangeTimeout = false; //Affirms Range Enemy has ended attack if true
    private bool strafeRight, strafeLeft;
    private bool recoilBack = false;
    private bool phaseTwo = false;
    private bool lockOn = false; //Affirms Jump Enemy is lerping to Players if true
    internal bool addWave = false; //Affirms Boss has spawned Enemies if true
    internal bool interrupted = false; //Affirms Boss can be damaged if true

    // Start is called before the first frame update
    void Start()
    {
        moveSpeedReset = moveSpeed;
        boostSpeedReset = boostSpeed;
        accelReset = enemyAcceleration;
        gapCloseReset = gapClose;
        meleeReset = meleeAttackTimer;
        chargeReset = chargeTimeout;
        punchReset = punchTimeout;
        jumpReset = jumpTimeout;
        airtimeReset = airtimeShort;
        buildupReset = chargeBuildup;
        berthJumpTimerReset = berthJumpCarpetBombTimer;
        strafeReset = strafeTimer;
        meleeCooldown = meleeTimeout;

        self = GetComponent<NavMeshAgent>();
        waypoint = GameObject.FindGameObjectsWithTag("Waypoint");
        waypointNext = Random.Range(0, waypoint.Length);
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<EnemyHealthScript>();
        manager = FindObjectOfType<EnemyManagerScript>();

        subject = GetComponent<Renderer>();
        materials = subject.materials;

        if (amBoss)
        {
            boss = FindObjectOfType<BossManagerScript>();
        }

        AttackScaling();
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        HaveIDied();

        if(jumpCheck != null)
        {
            AmIGrounded();
        }        
    }

    /// <summary>
    /// Modifies Enemy damage, Ranged attack rate by Difficulty number
    /// </summary>
    private void AttackScaling()
    {
        if(enemy.difficultyValue >= 2)
        {
            damagePercent *= enemy.difficultyValue;
            damagePercent /= 100;
            damagePercent *= damage;
            damageAdd = (int)damagePercent;
            damage += damageAdd;

            rangeAttackChange *= enemy.difficultyValue;
            rangeAttackChange /= 100;
            rangeAttackChange *= rangeAttackCount;
            rangeAttackCount += (int)rangeAttackChange;
        }
    }

    /// <summary>
    /// Reports true if Player is found, false if not
    /// </summary>
    public bool CanSeePlayer()
    {
        RaycastHit rayInfo;
        Vector3 rayToTarget = player.transform.position - transform.position;
        //Debug.DrawRay(transform.position, rayToTarget);
        if(Physics.Raycast(transform.position, rayToTarget, out rayInfo, Mathf.Infinity, contactOnly))
        {
            if(rayInfo.transform.gameObject.tag == "Player")
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Reports true if Enemy is on ground, false if not
    /// </summary>
    public bool AmIGrounded()
    {
        //Debug.DrawRay(jumpCheck.transform.position + Vector3.up, Vector3.down * 1.3f, Color.red);
        RaycastHit impact;
        if(Physics.Raycast(jumpCheck.transform.position + Vector3.up, Vector3.down, out impact, 1.3f))
        {
            if(impact.point != null)
            {
                return true;
            }
           
        }

        return false;
    }

    /// <summary>
    /// Reports true if Enemy is alive, false if not
    /// </summary>
    public bool HaveIDied()
    {
        if(enemy.healthCurrent <= 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Controls Melee attack behaviors between Boss and non-Boss Enemies
    /// </summary>
    [Task]
    public void AttackMelee()
    {
        if(!HaveIDied())
        {
            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit;

            //This behavior dictates Melee combat for Bosses
            if(amBoss)
            {
                self.speed = boostSpeed;
                distance = transform.position - player.transform.position;
                attackAgain += Time.deltaTime;
                
                if(!destinationSet)
                {
                    waypointNext = Random.Range(0, waypoint.Length);
                    self.SetDestination(waypoint[waypointNext].transform.position);
                    destinationSet = true;
                }

                if (Vector3.Distance(waypoint[waypointNext].transform.position, self.transform.position) < accuracy)
                {
                    Vector3 waypointDistance = waypoint[waypointNext].transform.position - transform.position;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(waypointDistance, Vector3.up), rotationStrength);
                }

                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin))
                {

                    if (hit.collider.tag == "Hard Lucent" && !gathered)
                    {
                        stunMechanic = Instantiate(stunningLucent, attackStartPoint.transform.position + transform.forward, attackStartPoint.transform.rotation, gameObject.transform);
                        stunMechanic.name = stunningLucent.name;

                        if (stunMechanic.GetComponent<Rigidbody>())
                        {
                            Destroy(stunMechanic.GetComponent<Rigidbody>());
                        }

                        gathered = true;
                        throwTarget = true;
                    }
                }

                if (interrupted)
                {
                    addWave = true;
                    self.speed = 0;
                    meleeTimeout -= Time.deltaTime;
                    if (meleeTimeout <= 0f)
                    {
                        self.speed = moveSpeedReset;
                        destinationSet = false;
                        gathered = false;
                        recorded = false;
                        throwTarget = false;
                        interrupted = false;
                        enemy.isImmune = true;

                        if (boss != null && addWave == true)
                        {
                            if (enemy.healthCurrent >= 1)
                            {
                                boss.TriggerAdds();
                                addWave = false;
                            }

                            else
                            {
                                boss.isAlive = false;
                                addWave = false;
                            }
                        }

                        meleeTimeout = 4f;
                       
                    }
                }

                if (throwTarget && !interrupted)
                {
                    if(CanSeePlayer())
                    {
                        self.SetDestination(player.transform.position);
                        self.speed = moveSpeed;                  

                        if (distance.magnitude <= meleeRangeCheck)
                        {
                            if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeCheck) && !recorded && stunMechanic != null)
                            {
                                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-distance, Vector3.up), rotationStrength);

                                lastPlayerPosition = (hit.point - transform.position).normalized;
                                recorded = true;

                                if (stunMechanic.GetComponent<Rigidbody>() == null)
                                {
                                    stunMechanic.AddComponent<Rigidbody>();
                                }

                                stunMechanic.GetComponent<LucentScript>().shatterDelayTime = 0.6f;
                                stunMechanic.GetComponent<LucentScript>().threat = true;
                                stunMechanic.GetComponent<LucentScript>().StartCoroutine(stunMechanic.GetComponent<LucentScript>().Shatter());

                                stunMechanic.transform.parent = null;
                                stunMechanic.GetComponent<Rigidbody>().AddForce((lastPlayerPosition + Vector3.up) * jumpForce, ForceMode.Impulse);
                                stunMechanic.GetComponent<Rigidbody>().AddForce((transform.forward * forwardForce), ForceMode.Impulse);
                                stunMechanic = null;
                            }

                            self.speed = moveSpeed / 2;
                            self.acceleration = enemyAcceleration / 2;

                            meleeAttackTimer -= Time.deltaTime;
                            if (meleeAttackTimer <= 0f)
                            {
                                attackLock = true;
                                self.speed = boostSpeed;
                                self.acceleration = enemyAcceleration;

                                if (attackLock)
                                {
                                    meleeAttackTimer = 0f;
                                    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin))
                                    {

                                        if (hit.collider.tag == "Player" && attackAgain >= attackRate)
                                        {
                                            attackAgain = 0.0f;

                                            if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                                            {
                                                if (gameObject.GetComponent<DebuffScript>() == null)
                                                {
                                                    gameObject.AddComponent<DebuffScript>();
                                                }
                                            }

                                            hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                                            hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                                            //This code shoves the Player with particular force in their opposite direction.
                                            //This is a melee attack, shoving the player with less force, subtly offsetting the player upwards to distinguish it from a charge.
                                            Vector3 knockbackDir = -hit.collider.transform.forward;
                                            hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                                            manager.damageDealt += damage;

                                            attackLock = false;
                                            meleeAttackTimer = meleeReset;

                                            destinationSet = false;
                                            gathered = false;
                                            recorded = false;
                                            throwTarget = false;         
                                        }
                                    }
                                }

                            }
                        }

                        else
                        {
                            meleeAttackTimer = meleeReset;
                        }
                    }

                    else
                    {
                        self.speed = moveSpeed;
                        self.SetDestination(player.transform.position);
                        self.acceleration = accelReset;
                    }
                }
            }

            //This behavior dictates Melee combat for standard Enemies
            else
            {
                self.speed = moveSpeed;
                distance = player.transform.position - transform.position;
                //attackAgain += Time.deltaTime;

                if(CanSeePlayer())
                {
                    if(distance.magnitude <= meleeEngagementDistance)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                        meleeAttackTimer -= Time.deltaTime;
                        if (meleeAttackTimer <= 0f)
                        {
                            meleeAttackTimer = 0f;
                            attackLock = true;
                        }
                    }                   

                    if(!attackLock)
                    {
                        if (distance.magnitude <= meleeRangeCheck)
                        {                            
                            if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
                            {
                                if (hit.collider.tag == "Player")
                                {
                                    self.ResetPath();

                                    if(strafeRight)
                                    {
                                        strafeCalc = Vector3.Cross(distance, transform.up);
                                        strafePos = transform.position + strafeCalc * strafeDistance;
                                    }

                                    else if (strafeLeft)
                                    {
                                        strafeCalc = Vector3.Cross(distance, -transform.up);
                                        strafePos = transform.position + strafeCalc * strafeDistance;
                                    }

                                    self.SetDestination(strafePos);
                                    recorded = true;
                                }
                            }
                        }

                        else
                        {
                            strafePos = transform.position + distance * strafeDistance / 2;
                            self.SetDestination(strafePos);

                            int strafeAction = Random.Range(0, 2);
                            if (strafeAction == 0)
                            {
                                strafeRight = true;
                                strafeLeft = false;
                            }

                            else
                            {
                                strafeLeft = true;
                                strafeRight = false;
                            }

                            recorded = true;
                        }

                        lastKnownDistance = strafePos - transform.position;
                        //Debug.Log(lastKnownDistance.magnitude + " | " + strafeLimit);

                        //Enemy resets recorded state when within previous strafe position for a duration
                        if (lastKnownDistance.magnitude <= strafeLimit)
                        {
                            recorded = false;
                        }

                        //Debug.DrawRay(transform.position, distance, Color.red);
                        //Debug.DrawRay(transform.position, transform.up, Color.blue);
                        Debug.DrawLine(transform.position, strafePos, Color.white);
                    }

                    else
                    {
                        meleeTimeout -= Time.deltaTime;

                        if(meleeTimeout > 0f)
                        {
                            self.SetDestination(player.transform.position);

                            if(!GetComponent<BerthScript>())
                            {
                                subject.materials[materialIndex].color = attackTell;
                            }

                            if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, 1.25f))
                            {
                                if (hit.collider.tag == "Player")
                                {
                                    if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                                    {
                                        if (gameObject.GetComponent<DebuffScript>() == null)
                                        {
                                            gameObject.AddComponent<DebuffScript>();
                                        }

                                        if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                                        {
                                            hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                                        }
                                    }

                                    else
                                    {
                                        hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                                        hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                                        //This code shoves the Player with particular force in their opposite direction.
                                        //This is a melee attack, shoving the player with less force, subtly offsetting the player upwards to distinguish it from a charge.
                                        Vector3 knockbackDir = -hit.collider.transform.forward;
                                        hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                                        manager.damageDealt += damage;
                                    }

                                    //Selects a random duration to delay next attack, then initiates attack
                                    int randomTime = 0;

                                    float[] delays = { 5f, 7f, 10f };
                                    randomTime = Random.Range(0, delays.Length);
                                    meleeReset = delays[randomTime];

                                    meleeAttackTimer = meleeReset;
                                    if (recorded)
                                    {
                                        recorded = false;
                                    }

                                    if (!GetComponent<BerthScript>())
                                    {
                                        subject.materials[materialIndex].color = Color.red;
                                    }

                                    meleeTimeout = 0f;
                                    attackLock = false;
                                }
                            }
                        }
                        
                        else
                        {
                            //Selects a random duration to delay next attack, then initiates attack
                            int randomTime = 0;

                            float[] delays = { 5f, 7f, 10f };
                            randomTime = Random.Range(0, delays.Length);
                            meleeReset = delays[randomTime];

                            meleeAttackTimer = meleeReset;
                            if (recorded)
                            {
                                recorded = false;
                            }

                            if (!GetComponent<BerthScript>())
                            {
                                subject.materials[materialIndex].color = Color.red;
                            }

                            meleeTimeout = meleeCooldown;
                            attackLock = false;
                        }
                    }
                }

                else
                {
                    meleeAttackTimer = meleeReset;

                    recorded = false;
                    strafePos = Vector3.zero;

                    self.speed = moveSpeed;
                    self.SetDestination(player.transform.position);
                    self.acceleration = accelReset;
                }
            }          
        }

        else
        {
            self.enabled = false;
        }
        

        Task.current.Succeed();

    }

    /// <summary>
    /// Controls Range attack behaviors for Enemies
    /// </summary>
    [Task]
    public void AttackRange()
    {
        if(!HaveIDied())
        {
            self.speed = moveSpeed;
            
            if(amSentry)
            {
                self.enabled = false;
            }

            distance = player.transform.position - transform.position;
            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit;

            if (distance.magnitude <= rangeEngagementDistance && CanSeePlayer())
            {
                if (amSentry)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);
                }

                else
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, rangeATKMin) && !recorded)
                    {
                        if (hit.collider.tag == "Player")
                        {
                            if (strafeTimer != strafeReset)
                            {
                                strafeTimer = strafeReset;
                            } //Resets strafe timer

                            self.ResetPath();

                            int strafeAction = Random.Range(0, 4);
                            if(strafeAction == 0)
                            {
                                strafeCalc = Vector3.Cross(distance, transform.up);
                                strafePos = transform.position + strafeCalc * strafeDistance;
                            } //Strafes to the right

                            else if (strafeAction == 1)
                            {
                                strafeCalc = Vector3.Cross(distance, -transform.up);
                                strafePos = transform.position + strafeCalc * strafeDistance;
                            } //Strafes to the left

                            else if(strafeAction == 2)
                            {
                                strafePos = transform.position + distance * strafeDistance / 2;
                            } //Moves forward
                            
                            else
                            {
                                strafePos = transform.position - distance * strafeDistance;
                            } //Moves backwards

                            Vector3 strafeDirection = strafePos - transform.position;
                            if (!Physics.Raycast(rayOrigin, strafeDirection.normalized, out hit, strafeDirection.magnitude, contactOnly))
                            {
                                self.SetDestination(strafePos);
                                recorded = true;
                            }
                        }
                    }

                    lastKnownDistance = strafePos - transform.position;
                    //Debug.Log(lastKnownDistance.magnitude + " | " + strafeLimit);

                    //Enemy resets recorded state when within previous strafe position for a duration
                    if (lastKnownDistance.magnitude <= strafeLimit)
                    {
                        strafeTimer -= Time.deltaTime;
                        if (strafeTimer <= 0f)
                        {
                            recorded = false;
                        }
                    }

                    //Debug.DrawRay(transform.position, distance, Color.red);
                    //Debug.DrawRay(transform.position, transform.up, Color.blue);
                    //Debug.DrawLine(transform.position, strafePos, Color.white);
                  
                }

                if(!attackLock)
                {
                    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, rangeEngagementDistance, contactOnly))
                    {
                        if (hit.collider.tag == "Player")
                        {
                            //Selects a random duration to delay next attack, then initiates attack
                            int randomTime = 0;

                            float[] delays = { 1f, 1.25f, 1.5f, 2f };
                            randomTime = Random.Range(0, delays.Length);
                            rangeCooldown = delays[randomTime];

                            StartCoroutine(RangeAttackShot());
                            //burstAttack = true;
                            attackLock = true;
                        }

                        if (hit.collider.tag == "Enemy")
                        {
                            Task.current.Fail();
                        }
                    }
                }
                
                //Delays attack behavior until timer expires
                if(attackLock && rangeTimeout)
                {
                    attackAgain += Time.deltaTime;
                    if(attackAgain >= rangeCooldown)
                    {
                        attackAgain = 0.0f;
                        attackLock = false;
                        rangeTimeout = false;
                    }
                }
            }

            else
            {
                if (amSentry)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);
                }

                else
                {
                    recorded = false;
                    self.SetDestination(player.transform.position);
                }
            }
        }

        else
        {
            self.enabled = false;
        }     

        Task.current.Succeed();
    }

    /// <summary>
    /// Controls Charge attack behaviors between Boss and non-Boss Enemies
    /// </summary>
    [Task]
    public void AttackCharge()
    {
        if(!HaveIDied())
        {
            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit;

            //This behavior dictates Charge combat for Bosses
            if(amBoss)
            {
                distance = player.transform.position - transform.position;

                if(distance.magnitude <= chargeRangeCheck && CanSeePlayer())
                {
                    self.ResetPath();
                    self.speed = boostSpeed;
                    self.acceleration = enemyAcceleration;

                    //Debug.Log(attackLock + " | " + ramTimeout);
                    if(!attackLock && !ramTimeout)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                    }
           
                    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, chargeRangeCheck) && !recorded)
                    {
                        if (hit.collider.tag == "Player")
                        {
                            chargeBuildup -= Time.deltaTime;
                            if (chargeBuildup <= 0f)
                            {
                                chargeBuildup = buildupReset;
                                lastPlayerPosition = hit.point + distance * chargeOvershoot;
                                attackLock = true;

                                GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);
                                recorded = true;
                            }
                            //Debug.Log(lastPlayerPosition);
                            //Debug.Log(lastKnownDistance.magnitude);
                        }
                    }

                    Debug.DrawRay(transform.position, distance, Color.red);
                    Debug.DrawRay(transform.position, lastPlayerPosition, Color.green);
                    lastKnownDistance = lastPlayerPosition - transform.position;
                    //Debug.Log(lastKnownDistance.magnitude + " | " + self.stoppingDistance);

                    if(lastKnownDistance.magnitude <= chargeLimit)
                    {
                        self.ResetPath();
                        chargeBuildup = buildupReset;
                        attackLock = false;
                        ramTimeout = true;
                    }


                    if(interrupted)
                    {
                        Debug.DrawLine(transform.position, recoilPosition, Color.blue);
                        transform.position = Vector3.Lerp(transform.position, recoilPosition, gapClose * Time.deltaTime);
                    }

                    if (attackLock)
                    {
                        self.SetDestination(lastPlayerPosition);
                        if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, chargeRangeMin))
                        {

                            if (hit.collider.tag == "Hard Lucent")
                            {
                                recoilPosition = hit.point + (hit.normal * 5f);
                                self.ResetPath();
                                self.velocity = Vector3.zero;

                                interrupted = true;
                                chargeBuildup = buildupReset;

                                enemy.isImmune = false;
                                chargeTimeout *= 3;
                                addWave = true;
                                attackLock = false;
                                ramTimeout = true;
                            }

                            if (hit.collider.tag == "Player")
                            {
                                if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                                {
                                    if (gameObject.GetComponent<DebuffScript>() == null)
                                    {
                                        gameObject.AddComponent<DebuffScript>();
                                    }

                                    if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                                    {
                                        hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                                    }

                                    chargeBuildup = buildupReset;
                                    attackLock = false;
                                    ramTimeout = true;
                                }


                                else
                                {
                                    hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                                    hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                                    //This code shoves the Player with particular force in their opposite direction.
                                    //This is a charge attack, knocking the player with more force and keeping them grounded to distinguish it from a melee.
                                    Vector3 knockbackDir = -hit.collider.transform.forward;
                                    knockbackDir.y = 0;
                                    hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * chargeAttackForce);

                                    manager.damageDealt += damage;

                                    chargeBuildup = buildupReset;
                                    attackLock = false;
                                    ramTimeout = true;
                                }
                            }
                        }
                    }
                }

                else
                {
                    recorded = false;
                    self.SetDestination(player.transform.position);
                    self.speed = moveSpeed;

                }
            }

            //This behavior dictates Charge combat for standard Enemies
            else
            {
                self.speed = moveSpeed;
                distance = player.transform.position - transform.position;

                if (distance.magnitude <= chargeRangeCheck && CanSeePlayer())
                {
                    self.ResetPath();
                    self.speed = boostSpeed;
                    self.acceleration = enemyAcceleration * 2;

                    if (!recorded)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);
                    }

                    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, chargeRangeCheck) && !recorded)
                    {
                        if (hit.collider.tag == "Player")
                        {
                            lastPlayerPosition = hit.point + distance * chargeOvershoot;
                            recorded = true;
                            attackLock = true;
                            //Debug.Log(lastPlayerPosition);
                            //Debug.Log(lastKnownDistance.magnitude);
                        }
                    }

                    Debug.DrawLine(transform.position, player.transform.position, Color.red);
                    Debug.DrawLine(transform.position, lastPlayerPosition, Color.green);

                    //Debug.DrawRay(transform.position, distance, Color.red);
                    //Debug.DrawRay(transform.position, distance * chargeOvershoot, Color.green);

                    self.SetDestination(lastPlayerPosition);
                    lastKnownDistance = lastPlayerPosition - transform.position;
                    //Debug.Log(lastKnownDistance.magnitude + " | " + self.stoppingDistance);

                    if (lastKnownDistance.magnitude <= chargeLimit)
                    {
                        attackLock = false;
                        ramTimeout = true;
                    }

                    if (attackLock)
                    {
                        if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, chargeRangeMin))
                        {               

                            if (hit.collider.tag == "Player")
                            {
                                if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                                {
                                    if (gameObject.GetComponent<DebuffScript>() == null)
                                    {
                                        gameObject.AddComponent<DebuffScript>();
                                    }

                                    if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                                    {
                                        hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                                    }

                                    attackLock = false;
                                    ramTimeout = true;
                                }


                                else
                                {
                                    if (GetComponent<BerthScript>())
                                    {
                                        hit.collider.GetComponent<PlayerStatusScript>().playerHealth -= damage * GetComponent<BerthScript>().berthDamage;
                                        GetComponent<BerthScript>().Explode();
                                    }

                                    else
                                    {
                                        hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                                        hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                                        //This code shoves the Player with particular force in their opposite direction.
                                        //This is a charge attack, knocking the player with more force and keeping them grounded to distinguish it from a melee.
                                        Vector3 knockbackDir = -hit.collider.transform.forward;
                                        knockbackDir.y = 0;
                                        hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * chargeAttackForce);
                                    }

                                    manager.damageDealt += damage;

                                    attackLock = false;
                                    ramTimeout = true;
                                }
                            }
                        }
                    }
                }

                else
                {
                    recorded = false;
                    self.SetDestination(player.transform.position);
                    self.speed = boostSpeed;

                }
            }
        }

        else
        {
            self.enabled = false;
        }

        Task.current.Succeed();
    }

    /// <summary>
    /// Controls Jump attack behaviors for Enemies
    /// </summary>
    [Task]
    public void MajorAttackJump()
    {
        if (!HaveIDied())
        {
            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit, hitTheSequel;

            if (amBoss)
            {
                self.speed = boostSpeed;
                distance = player.transform.position - transform.position;
                if(primedLucent)
                {
                    lucentPrimed = (primedLucent.transform.position - transform.position).normalized;
                }

                if (self.enabled == true)
                {
                    self.ResetPath();
                    waypointNext = 0;

                    combustibleLucent = GameObject.FindGameObjectsWithTag("Combustible Lucent");
                    self.enabled = false;
                }

                if(!primedLucent)
                {
                    for(int o = 0; o < combustibleLucent.Length; o++)
                    {
                        if(combustibleLucent[o].GetComponent<CombustibleLucentScript>().primed)
                        {
                            primedLucent = combustibleLucent[o];
                        }
                    }
                }

                if(!attackLock)
                {
                    transform.position = Vector3.Lerp(transform.position, waypoint[waypointNext].transform.position, gapClose * Time.deltaTime);

                    if (player.GetComponent<PlayerInventoryScript>().inventory[player.GetComponent<PlayerInventoryScript>().selection].GetComponent<FirearmScript>().IsFiring())
                    {
                        jumpReset = 0f;
                    }

                    if (Vector3.Distance(waypoint[waypointNext].transform.position, transform.position) < accuracy)
                    {
                        if(primedLucent)
                        {
                            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lucentPrimed, Vector3.up), rotationStrength);
                        }

                        else if (waypointNext == waypoint.Length - 1)
                        {
                            Vector3 waypointDistance = waypoint[0].transform.position - transform.position;
                            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(waypointDistance, Vector3.up), rotationStrength);
                        }

                        else
                        {
                            Vector3 waypointDistance = waypoint[waypointNext + 1].transform.position - transform.position;
                            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(waypointDistance, Vector3.up), rotationStrength);
                        }
                        

                        jumpTimeout -= Time.deltaTime;

                        if (player.GetComponent<PlayerInventoryScript>().inventory[player.GetComponent<PlayerInventoryScript>().selection].GetComponent<FirearmScript>().IsFiring())
                        {
                            jumpTimeout = 0f;
                        }

                        if (jumpTimeout <= 0f)
                        {
                            if(primedLucent)
                            {
                                if (Vector3.Distance(primedLucent.transform.position, transform.position) <= meleeRangeMin)
                                {
                                    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin, contactOnly))
                                    {
                                        if(hit.collider.gameObject.CompareTag("Combustible Lucent"))
                                        {
                                            if (gameObject.GetComponent<Rigidbody>() == null)
                                            {
                                                gameObject.AddComponent<Rigidbody>();
                                                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                                            }

                                            gameObject.GetComponent<Rigidbody>().AddForce((lucentPrimed + Vector3.up) * jumpForce, ForceMode.Impulse);
                                            gameObject.GetComponent<Rigidbody>().AddForce((transform.forward * forwardForce), ForceMode.Impulse);
                                            GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);

                                            attackLock = true;
                                            if(jumpReset != detectionLimit)
                                            {
                                                jumpReset = detectionLimit;
                                            }
                                            jumpTimeout = jumpReset;
                                        }
                                    }                                                               
                                }

                                else
                                {
                                    if (waypointNext == waypoint.Length - 1)
                                    {
                                        waypointNext = 0;
                                    }

                                    else
                                    {
                                        waypointNext++;
                                    }

                                    if (playerDetected != 0f)
                                    {
                                        playerDetected = 0f;
                                    }

                                    jumpTimeout = ambushTime;

                                }
                            }
                        
                            else
                            {
                                if (CanSeePlayer() && distance.magnitude <= meleeRangeMin)
                                {
                                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                                    playerDetected += Time.deltaTime;
                                    if (playerDetected >= detectionLimit || 
                                        player.GetComponent<PlayerInventoryScript>().inventory[player.GetComponent<PlayerInventoryScript>().selection].GetComponent<FirearmScript>().IsFiring())
                                    {
                                        if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
                                        {
                                            if (hit.collider.tag == "Player")
                                            {
                                                lastPlayerPosition = (hit.point - transform.position).normalized;
                                                recorded = true;

                                                if (gameObject.GetComponent<Rigidbody>() == null)
                                                {
                                                    gameObject.AddComponent<Rigidbody>();
                                                    gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                                                }

                                                gameObject.GetComponent<Rigidbody>().AddForce((lastPlayerPosition + Vector3.up) * jumpForce, ForceMode.Impulse);
                                                gameObject.GetComponent<Rigidbody>().AddForce((transform.forward * forwardForce), ForceMode.Impulse);
                                                GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);

                                                attackLock = true;

                                            }
                                        }

                                        playerDetected = 0f;
                                        if (jumpReset != detectionLimit)
                                        {
                                            jumpReset = detectionLimit;
                                        }
                                        jumpTimeout = jumpReset;
                                    }
                                }

                                else
                                {
                                    if (waypointNext == waypoint.Length - 1)
                                    {
                                        waypointNext = 0;
                                    }

                                    else
                                    {
                                        waypointNext++;
                                    }

                                    if (playerDetected != 0f)
                                    {
                                        playerDetected = 0f;
                                    }

                                    jumpTimeout = jumpReset;
                                }
                            }                                                                            
                        }
                    }
                }

                else
                {
                    if (AmIGrounded())
                    {
                        subject.materials[materialIndex].color = Color.red;
                        airtimeShort -= Time.deltaTime;

                        if (airtimeShort <= 0f)
                        {
                            airtimeShort = airtimeReset;
                            if (lockOn)
                            {
                                lockOn = false;
                            }

                            recorded = false;
                            attackLock = false;

                            if (gameObject.GetComponent<Rigidbody>() != null)
                            {
                                Destroy(gameObject.GetComponent<Rigidbody>());
                            }
                        }
                    }

                    else
                    {
                        airtimeShort = airtimeReset;
                    }

                    if (primedLucent)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lucentPrimed, Vector3.up), rotationStrength);

                        if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, 2f))
                        {
                            if (hit.collider.tag == "Combustible Lucent")
                            {
                                if (hit.collider.GetComponent<CombustibleLucentScript>().primed)
                                {
                                    hit.collider.GetComponent<CombustibleLucentScript>().Combust();
                                    jumpTimeout *= 2;
                                    enemy.isImmune = false;
                                    addWave = true;
                                    slamTimeout = true;

                                    //if (gameObject.GetComponent<Rigidbody>() != null)
                                    //{
                                    //    Destroy(gameObject.GetComponent<Rigidbody>());
                                    //}
                                }
                            }
                        }
                    }

                    else
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                        if (!slamTimeout)
                        {
                            if (distance.magnitude <= jumpLimit)
                            {
                                lockOn = true;
                                subject.materials[materialIndex].color = attackTell;
                            }

                            if (lockOn && Time.timeScale == 1)
                            {
                                transform.position = Vector3.Lerp(transform.position, player.transform.position, gapClose / 60);

                                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, 2f))
                                {
                                    if (hit.collider.tag == "Player")
                                    {
                                        if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                                        {
                                            if (gameObject.GetComponent<DebuffScript>() == null)
                                            {
                                                gameObject.AddComponent<DebuffScript>();
                                            }

                                            if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                                            {
                                                hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                                            }

                                            if (gameObject.GetComponent<Rigidbody>() != null)
                                            {
                                                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                                                //Destroy(gameObject.GetComponent<Rigidbody>());
                                            }

                                            recorded = false;
                                            lockOn = false;
                                            slamTimeout = true;
                                        }

                                        else
                                        {
                                            hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                                            hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                                            Vector3 knockbackDir = transform.forward;
                                            knockbackDir.y = 0;
                                            hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                                            if (gameObject.GetComponent<Rigidbody>() != null)
                                            {
                                                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                                gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                                                //Destroy(gameObject.GetComponent<Rigidbody>());
                                            }

                                            manager.damageDealt += damage;

                                            recorded = false;
                                            lockOn = false;
                                            slamTimeout = true;
                                        }

                                        subject.materials[materialIndex].color = Color.red;
                                    }
                                }
                            }
                        }                  
                    }                  
                }
          
            } //This behavior dictates Jump combat for Bosses

            else
            {
                self.speed = moveSpeed;
                distance = player.transform.position - transform.position;

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
                            recorded = true;

                            if (gameObject.GetComponent<Rigidbody>() == null)
                            {
                                gameObject.AddComponent<Rigidbody>();
                                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                            }

                            gameObject.GetComponent<Rigidbody>().AddForce((lastPlayerPosition + Vector3.up) * jumpForce, ForceMode.Impulse);
                            gameObject.GetComponent<Rigidbody>().AddForce((transform.forward * forwardForce), ForceMode.Impulse);
                            GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);

                        }
                    }

                    if (AmIGrounded())
                    {
                        airtimeShort -= Time.deltaTime;

                        if (!GetComponent<BerthScript>())
                        {
                            subject.materials[materialIndex].color = Color.red;
                        }

                        if (airtimeShort <= 0f)
                        {
                            airtimeShort = airtimeReset;
                            if (lockOn)
                            {
                                lockOn = false;
                            }

                            recorded = false;
                        }
                    }

                    else
                    {
                        airtimeShort = airtimeReset;

                        if (GetComponent<BerthScript>() && !lockOn)
                        {
                            berthJumpCarpetBombTimer -= Time.deltaTime;
                            if (berthJumpCarpetBombTimer <= 0f)
                            {
                                berthJumpCarpetBombTimer = berthJumpTimerReset;

                                stunMechanic = Instantiate(stunningLucent, attackStartPoint.transform.position + (Vector3.down * 2f), Quaternion.identity);
                                stunMechanic.GetComponent<LucentScript>().threat = true;
                                stunMechanic.GetComponent<LucentScript>().shatterDelayTime = 2f;
                                stunMechanic.GetComponent<LucentScript>().StartCoroutine(stunMechanic.GetComponent<LucentScript>().Shatter());
                                stunMechanic.name = stunningLucent.name;
                            }
                        }
                    }

                    if (distance.magnitude <= jumpLimit)
                    {
                        lockOn = true;

                        if (!GetComponent<BerthScript>())
                        {
                            subject.materials[materialIndex].color = attackTell;
                        }
                    }

                    if (lockOn && Time.timeScale == 1)
                    {
                        transform.position = Vector3.Lerp(transform.position, player.transform.position, gapClose);

                        if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hitTheSequel, 2f))
                        {
                            if (hitTheSequel.collider.tag == "Player" && canAttackAgain)
                            {

                                if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                                {
                                    if (gameObject.GetComponent<DebuffScript>() == null)
                                    {
                                        gameObject.AddComponent<DebuffScript>();
                                    }

                                    if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                                    {
                                        hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                                    }

                                    if (gameObject.GetComponent<Rigidbody>() != null)
                                    {
                                        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                                    }

                                    slamTimeout = true;
                                    canAttackAgain = false;
                                }

                                else
                                {
                                    hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                                    hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                                    Vector3 knockbackDir = transform.forward;
                                    knockbackDir.y = 0;
                                    hitTheSequel.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                                    if (gameObject.GetComponent<Rigidbody>() != null)
                                    {
                                        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                                    }

                                    manager.damageDealt += damage;

                                    slamTimeout = true;
                                    canAttackAgain = false;
                                }
                            }
                        }
                    }
                }

                else
                {
                    if (self.enabled == false)
                    {
                        self.enabled = true;

                        if (gameObject.GetComponent<Rigidbody>() != null)
                        {
                            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                            Destroy(gameObject.GetComponent<Rigidbody>());
                        }

                    }

                    recorded = false;
                    airtimeShort = airtimeReset;
                    self.SetDestination(player.transform.position);
                }
            } //This behavior dictates Jump combat for standard Enemies           
        }

        else
        {
            self.enabled = false;
        }
             
        Task.current.Succeed();
    }

    /// <summary>
    /// Controls Pounce attack behaviors for Enemies
    /// </summary>
    [Task]
    public void MajorAttackPounce()
    {
        if(!HaveIDied())
        {
            
            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit, hitTheSequel;

            if(amBoss)
            {
                self.speed = moveSpeed;
                distance = player.transform.position - transform.position;

                if(!phaseTwo)
                {
                    if (!slamTimeout)
                    {
                        if (!destinationSet)
                        {
                            if (self.enabled == false)
                            {
                                self.enabled = true;
                            }

                            waypointNext = 0;
                            self.SetDestination(waypoint[waypointNext].transform.position);
                            destinationSet = true;
                            punchTimeout *= 2;
                        }

                        if (Vector3.Distance(waypoint[waypointNext].transform.position, transform.position) < 3.0f)
                        {
                            if (self.enabled == true)
                            {
                                self.ResetPath();
                                self.enabled = false;
                            }

                            if (waypointNext == waypoint.Length - 1)
                            {
                                Vector3 waypointDistance = waypoint[0].transform.position - transform.position;
                                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(waypointDistance, Vector3.up), rotationStrength);
                            }

                            else
                            {
                                Vector3 waypointDistance = waypoint[waypointNext + 1].transform.position - transform.position;
                                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(waypointDistance, Vector3.up), rotationStrength);
                            }

                            jumpTimeout -= Time.deltaTime;
                            if (jumpTimeout <= 0f)
                            {
                                if (waypointNext == waypoint.Length - 1)
                                {
                                    waypointNext = 0;
                                }

                                else
                                {
                                    waypointNext++;
                                }

                                Vector3 nextJump = (waypoint[waypointNext].transform.position - transform.position).normalized;

                                if (gameObject.GetComponent<Rigidbody>() == null)
                                {
                                    gameObject.AddComponent<Rigidbody>();
                                    gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                                }

                                gameObject.GetComponent<Rigidbody>().AddForce((nextJump + Vector3.up) * jumpForce, ForceMode.Impulse);
                                gameObject.GetComponent<Rigidbody>().AddForce((transform.forward * forwardForce), ForceMode.Impulse);
                                GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);

                                jumpTimeout = jumpReset;
                            }

                            if (AmIGrounded())
                            {
                                if (gameObject.GetComponent<Rigidbody>() != null)
                                {
                                    gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                                    gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                                }
                            }
                        }

                        if (CanSeePlayer() && distance.magnitude <= rangeEngagementDistance)
                        {
                            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                            if (!attackLock)
                            {
                                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, rangeEngagementDistance, contactOnly))
                                {
                                    if (hit.collider.tag == "Player")
                                    {
                                        //Selects a random duration to delay next attack, then initiates attack
                                        int randomTime = 0;

                                        float[] delays = { 0.25f, 0.5f, 0.75f, 1f };
                                        randomTime = Random.Range(0, delays.Length);
                                        rangeCooldown = delays[randomTime];

                                        StartCoroutine(RangeAttackShot());
                                        //burstAttack = true;
                                        attackLock = true;
                                    }

                                    if (hit.collider.tag == "Enemy")
                                    {
                                        Task.current.Fail();
                                    }
                                }
                            }

                            //Delays attack behavior until timer expires
                            if (attackLock && rangeTimeout)
                            {
                                attackAgain += Time.deltaTime;
                                if (attackAgain >= rangeCooldown)
                                {
                                    attackAgain = 0.0f;
                                    attackLock = false;
                                    rangeTimeout = false;
                                }
                            }
                        }
                    }
                }

                else
                {
                    if(CanSeePlayer())
                    {
                        if (distance.magnitude <= meleeEngagementDistance)
                        {
                            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                            meleeAttackTimer -= Time.deltaTime;
                            if (meleeAttackTimer <= 0f)
                            {
                                meleeAttackTimer = 0f;
                                attackLock = true;
                            }
                        }

                        if (!attackLock)
                        {
                            if (distance.magnitude <= meleeRangeCheck)
                            {
                                if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
                                {
                                    if (hit.collider.tag == "Player")
                                    {
                                        self.ResetPath();

                                        if (strafeRight)
                                        {
                                            strafeCalc = Vector3.Cross(distance, transform.up);
                                            strafePos = transform.position + strafeCalc * strafeDistance;
                                        }

                                        else if (strafeLeft)
                                        {
                                            strafeCalc = Vector3.Cross(distance, -transform.up);
                                            strafePos = transform.position + strafeCalc * strafeDistance;
                                        }

                                        self.SetDestination(strafePos);
                                        recorded = true;
                                    }
                                }
                            }

                            else
                            {
                                strafePos = transform.position + distance * strafeDistance / 2;
                                self.SetDestination(strafePos);

                                int strafeAction = Random.Range(0, 2);
                                if (strafeAction == 0)
                                {
                                    strafeRight = true;
                                    strafeLeft = false;
                                }

                                else
                                {
                                    strafeLeft = true;
                                    strafeRight = false;
                                }

                                recorded = true;
                            }

                            lastKnownDistance = strafePos - transform.position;
                            //Debug.Log(lastKnownDistance.magnitude + " | " + strafeLimit);

                            //Enemy resets recorded state when within previous strafe position for a duration
                            if (lastKnownDistance.magnitude <= strafeLimit)
                            {
                                recorded = false;
                            }

                            //Debug.DrawRay(transform.position, distance, Color.red);
                            //Debug.DrawRay(transform.position, transform.up, Color.blue);
                            Debug.DrawLine(transform.position, strafePos, Color.white);
                        }

                        else
                        {
                            if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
                            {
                                if (hit.collider.tag == "Player")
                                {
                                    lastPlayerPosition = hit.point;
                                    recorded = true;
                                    //Debug.Log(lastPlayerPosition);
                                    //Debug.Log(lastKnownDistance.magnitude);
                                    GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);

                                    if (!GetComponent<BerthScript>())
                                    {
                                        subject.materials[materialIndex].color = attackTell;
                                    }

                                }
                            }

                            transform.position = Vector3.Lerp(transform.position, lastPlayerPosition, gapClose * Time.deltaTime);
                            lastKnownDistance = lastPlayerPosition - transform.position;
                            //Debug.Log(lastKnownDistance.magnitude + " | " + self.stoppingDistance);

                            if (lastKnownDistance.magnitude <= pounceLimit)
                            {
                                slamTimeout = true;

                                if (!GetComponent<BerthScript>())
                                {
                                    subject.materials[materialIndex].color = Color.red;
                                }
                            }

                            if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hitTheSequel, 2f))
                            {
                                if (hitTheSequel.collider.tag == "Player" && canAttackAgain)
                                {
                                    if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                                    {
                                        if (gameObject.GetComponent<DebuffScript>() == null)
                                        {
                                            gameObject.AddComponent<DebuffScript>();
                                        }

                                        if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                                        {
                                            hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                                        }

                                        slamTimeout = true;
                                        canAttackAgain = false;

                                        if (!GetComponent<BerthScript>())
                                        {
                                            subject.materials[materialIndex].color = Color.red;
                                        }
                                    }

                                    else
                                    {
                                        hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                                        hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                                        Vector3 knockbackDir = transform.forward;
                                        knockbackDir.y = 0;
                                        hitTheSequel.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                                        manager.damageDealt += damage;

                                        slamTimeout = true;
                                        canAttackAgain = false;

                                        if (!GetComponent<BerthScript>())
                                        {
                                            subject.materials[materialIndex].color = Color.red;
                                        }
                                    }
                                }
                            }

                            //meleeTimeout -= Time.deltaTime;

                            //if (meleeTimeout > 0f)
                            //{
                            //    self.SetDestination(player.transform.position);

                            //    if (!GetComponent<BerthScript>())
                            //    {
                            //        subject.materials[materialIndex].color = attackTell;
                            //    }

                            //    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, 1.25f))
                            //    {
                            //        if (hit.collider.tag == "Player")
                            //        {
                            //            if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                            //            {
                            //                if (gameObject.GetComponent<DebuffScript>() == null)
                            //                {
                            //                    gameObject.AddComponent<DebuffScript>();
                            //                }

                            //                if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                            //                {
                            //                    hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                            //                }
                            //            }

                            //            else
                            //            {
                            //                hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                            //                hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                            //                //This code shoves the Player with particular force in their opposite direction.
                            //                //This is a melee attack, shoving the player with less force, subtly offsetting the player upwards to distinguish it from a charge.
                            //                Vector3 knockbackDir = -hit.collider.transform.forward;
                            //                hit.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                            //                manager.damageDealt += damage;
                            //            }

                            //            //Selects a random duration to delay next attack, then initiates attack
                            //            int randomTime = 0;

                            //            float[] delays = { 3f, 5f, 7f };
                            //            randomTime = Random.Range(0, delays.Length);
                            //            meleeReset = delays[randomTime];

                            //            meleeAttackTimer = meleeReset;
                            //            if (recorded)
                            //            {
                            //                recorded = false;
                            //            }

                            //            if (!GetComponent<BerthScript>())
                            //            {
                            //                subject.materials[materialIndex].color = Color.red;
                            //            }

                            //            meleeTimeout = meleeCooldown;
                            //            attackLock = false;
                            //        }
                            //    }
                            //}

                            //else
                            //{
                            //    //Selects a random duration to delay next attack, then initiates attack
                            //    int randomTime = 0;

                            //    float[] delays = { 3f, 5f, 7f };
                            //    randomTime = Random.Range(0, delays.Length);
                            //    meleeReset = delays[randomTime];

                            //    meleeAttackTimer = meleeReset;
                            //    if (recorded)
                            //    {
                            //        recorded = false;
                            //    }

                            //    if (!GetComponent<BerthScript>())
                            //    {
                            //        subject.materials[materialIndex].color = Color.red;
                            //    }

                            //    meleeTimeout = meleeCooldown;
                            //    attackLock = false;
                            //}
                        }
                    }

                    else
                    {
                        meleeAttackTimer = meleeReset;

                        recorded = false;
                        strafePos = Vector3.zero;

                        self.speed = moveSpeed;
                        self.SetDestination(player.transform.position);
                        self.acceleration = accelReset;
                    }                  
                }
                
                
                //if (distance.magnitude <= meleeRangeMin && CanSeePlayer())
                //{
                //    self.ResetPath();
                //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(distance, Vector3.up), rotationStrength);

                //    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, meleeRangeMin) && !recorded)
                //    {
                //        if (hit.collider.tag == "Player")
                //        {
                //            lastPlayerPosition = hit.point;
                //            recoilPosition = transform.position - (hit.point - transform.position) * strafeDistance;
                //            recorded = true;
                //            //Debug.Log(lastPlayerPosition);
                //            //Debug.Log(lastKnownDistance.magnitude);
                //            GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);

                //            if (!GetComponent<BerthScript>())
                //            {
                //                subject.materials[materialIndex].color = attackTell;
                //            }
                //        }
                //    }

                //    if(!recoilBack)
                //    {
                //        transform.position = Vector3.Lerp(transform.position, lastPlayerPosition, gapClose * Time.deltaTime);
                //        lastKnownDistance = lastPlayerPosition - transform.position;
                //    }

                //    else
                //    {
                //        //Debug.DrawLine(transform.position, -distance, Color.blue);
                //        transform.position = Vector3.Lerp(transform.position, recoilPosition, gapClose * Time.deltaTime);
                //        lastKnownDistance = transform.position - recoilPosition;
                //        Debug.Log(lastKnownDistance.magnitude + " | " + pounceLimit);
                //    }

                //    //transform.position = Vector3.Lerp(transform.position, lastPlayerPosition, gapClose * Time.deltaTime);
                //    //lastKnownDistance = lastPlayerPosition - transform.position;
                //    //Debug.Log(lastKnownDistance.magnitude + " | " + self.stoppingDistance);

                //    if (lastKnownDistance.magnitude <= pounceLimit)
                //    {
                //        slamTimeout = true;

                //        if (!GetComponent<BerthScript>())
                //        {
                //            subject.materials[materialIndex].color = Color.red;
                //        }
                //    }

                //    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hitTheSequel, 2f))
                //    {
                //        if (hitTheSequel.collider.tag == "Player" && canAttackAgain)
                //        {
                //            if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                //            {
                //                if (gameObject.GetComponent<DebuffScript>() == null)
                //                {
                //                    gameObject.AddComponent<DebuffScript>();
                //                }

                //                if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                //                {
                //                    hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                //                }

                //                slamTimeout = true;
                //                canAttackAgain = false;

                //                if (!GetComponent<BerthScript>())
                //                {
                //                    subject.materials[materialIndex].color = Color.red;
                //                }
                //            }

                //            else
                //            {
                //                hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                //                hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                //                Vector3 knockbackDir = transform.forward;
                //                knockbackDir.y = 0;
                //                hitTheSequel.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                //                manager.damageDealt += damage;

                //                slamTimeout = true;
                //                canAttackAgain = false;

                //                if (!GetComponent<BerthScript>())
                //                {
                //                    subject.materials[materialIndex].color = Color.red;
                //                }
                //            }
                //        }
                //    }
                //}

                //else
                //{
                //    if (self.enabled == false)
                //    {
                //        self.enabled = true;
                //    }

                //    recorded = false;
                //    recoilBack = false;
                //    self.SetDestination(player.transform.position);
                //}


            }

            else
            {
                self.speed = moveSpeed;
                distance = player.transform.position - transform.position;

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
                            GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);

                            if (!GetComponent<BerthScript>())
                            {
                                subject.materials[materialIndex].color = attackTell;
                            }

                        }
                    }

                    transform.position = Vector3.Lerp(transform.position, lastPlayerPosition, gapClose * Time.deltaTime);
                    lastKnownDistance = lastPlayerPosition - transform.position;
                    //Debug.Log(lastKnownDistance.magnitude + " | " + self.stoppingDistance);

                    if (lastKnownDistance.magnitude <= pounceLimit)
                    {
                        slamTimeout = true;

                        if (!GetComponent<BerthScript>())
                        {
                            subject.materials[materialIndex].color = Color.red;
                        }
                    }

                    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hitTheSequel, 2f))
                    {
                        if (hitTheSequel.collider.tag == "Player" && canAttackAgain)
                        {
                            if (hit.collider.GetComponent<PlayerStatusScript>().isInvincible)
                            {
                                if (gameObject.GetComponent<DebuffScript>() == null)
                                {
                                    gameObject.AddComponent<DebuffScript>();
                                }

                                if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                                {
                                    hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;
                                }

                                slamTimeout = true;
                                canAttackAgain = false;

                                if (!GetComponent<BerthScript>())
                                {
                                    subject.materials[materialIndex].color = Color.red;
                                }
                            }

                            else
                            {
                                hit.collider.GetComponent<PlayerStatusScript>().InflictDamage(damage);
                                hit.collider.GetComponent<PlayerStatusScript>().playerHit = true;

                                Vector3 knockbackDir = transform.forward;
                                knockbackDir.y = 0;
                                hitTheSequel.collider.GetComponent<Rigidbody>().AddForce(knockbackDir * meleeAttackForce);

                                manager.damageDealt += damage;

                                slamTimeout = true;
                                canAttackAgain = false;

                                if (!GetComponent<BerthScript>())
                                {
                                    subject.materials[materialIndex].color = Color.red;
                                }
                            }
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
            }

            Task.current.Succeed();
        }

        else
        {
            self.enabled = false;
        }
        
    }

    /// <summary>
    /// Delays return to Charge attack behavior
    /// </summary>
    [Task]
    public void RamTimeout()
    {
        if (ramTimeout == true)
        {
            self.speed = 0f;
            self.acceleration = 0f;

            chargeTimeout -= Time.deltaTime;
            if (chargeTimeout < 0f)
            {         
                chargeTimeout = chargeReset;
                self.speed = moveSpeed;
                self.acceleration = enemyAcceleration;

                if (amBoss)
                {
                    enemy.isImmune = true;
                    interrupted = false;

                    if(boss != null && addWave == true)
                    {
                        if(enemy.healthCurrent >= 1)
                        {
                            boss.TriggerAdds();
                            addWave = false;
                        }

                        else
                        {
                            boss.isAlive = false;
                            addWave = false;
                        }
                    }
                }

                ramTimeout = false;
                canAttackAgain = true;
                recorded = false;
            }
        }

        Task.current.Succeed();

    }

    /// <summary>
    /// Delays return to Jump attack behavior
    /// </summary>
    [Task]
    public void JumpTimeout()
    {
        if (slamTimeout == true)
        {
            if (!GetComponent<BerthScript>())
            {
                subject.materials[materialIndex].color = Color.red;
            }

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

                if (amBoss)
                {
                    if(!enemy.isImmune)
                    {
                        enemy.isImmune = true;
                        attackLock = false;
                        primedLucent = null;

                        if (gameObject.GetComponent<Rigidbody>() != null)
                        {
                            Destroy(gameObject.GetComponent<Rigidbody>());
                        }
                    }

                    if (boss != null && addWave == true)
                    {
                        if (enemy.healthCurrent >= 1)
                        {
                            boss.TriggerAdds();
                            addWave = false;
                        }

                        else
                        {
                            boss.isAlive = false;
                            addWave = false;
                        }
                    }
                }
            }
        }

        Task.current.Succeed();

    }

    /// <summary>
    /// Delays return to Pounce attack behavior
    /// </summary>
    [Task]
    public void PounceTimeout()
    {
        if (slamTimeout == true)
        {
            punchTimeout -= Time.deltaTime;
            gapClose = 0f;
            if (punchTimeout <= 0f)
            {
                if(amBoss)
                {
                    gapClose = gapCloseReset;
                    punchTimeout = punchReset;
                    slamTimeout = false;
                    attackLock = false;
                    canAttackAgain = true;

                    if (!enemy.isImmune && !phaseTwo)
                    {
                        enemy.isImmune = true;
                    }

                    if (boss != null && addWave == true)
                    {
                        if (enemy.healthCurrent >= 1)
                        {
                            if(enemy.healthCurrent < enemy.healthMax / 2 && !phaseTwo)
                            {
                                phaseTwo = true;
                                enemy.isImmune = false;
                                gameObject.AddComponent<BerthScript>();
                                gameObject.AddComponent<ColorLerpScript>();

                                gameObject.GetComponent<ColorLerpScript>().enemyUse = true;
                                gameObject.GetComponent<ColorLerpScript>().materialIndex = 1;
                                gameObject.GetComponent<ColorLerpScript>().colorOne = Color.red;
                                gameObject.GetComponent<ColorLerpScript>().colorTwo = Color.yellow;

                                if (self.enabled == false)
                                {
                                    self.enabled = true;
                                }
                            }

                            boss.TriggerAdds();
                            addWave = false;
                        }

                        else
                        {
                            boss.isAlive = false;
                            addWave = false;
                        }
                    }

                    //if (!recoilBack)
                    //{
                    //    recoilBack = true;
                    //}

                    //else
                    //{
                    //    recoilBack = false;
                    //    recorded = false;
                    //}
                }

                else
                {
                    gapClose = gapCloseReset;
                    punchTimeout = punchReset;
                    slamTimeout = false;
                    canAttackAgain = true;
                    recorded = false;
                }         
            }
        }

        Task.current.Succeed();

    }

    /// <summary>
    /// Creates Harmful Lucent cluster under airborne Jump Enemies
    /// </summary>
    public IEnumerator BerthJumpCarpetBomb()
    {
        yield return new WaitForSeconds(0.5f);
        stunMechanic = Instantiate(stunningLucent, attackStartPoint.transform.position + (Vector3.down * 2f), Quaternion.identity);
        stunMechanic.GetComponent<LucentScript>().threat = true;
        stunMechanic.GetComponent<LucentScript>().shatterDelayTime = 2f;
        stunMechanic.GetComponent<LucentScript>().StartCoroutine(stunMechanic.GetComponent<LucentScript>().Shatter());
        stunMechanic.name = stunningLucent.name;
    }

    /// <summary>
    /// Stops Boss-level Charge Enemy stunned movement
    /// </summary>
    public IEnumerator BossChargeStopLerp()
    {
        yield return new WaitForSeconds(0.5f);
        interrupted = false;
    }

    public IEnumerator RangeAttackShot()
    {
        if (!GetComponent<BerthScript>())
        {
            subject.materials[materialIndex].color = attackTell;
        }

        yield return new WaitForSeconds(rangeAttackRate);

        GameObject projectile = Instantiate(rangeProjectile, attackStartPoint.transform.position, attackStartPoint.transform.rotation);
        projectile.GetComponent<ProjectileScript>().damage = damage;
        if (GetComponent<BerthScript>())
        {
            projectile.GetComponent<ProjectileScript>().berthFlag = true;
        }

        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * (rangedAttackForce * distance.magnitude));

        burstCount++;
        if(burstCount != rangeAttackCount)
        {
            StartCoroutine(RangeAttackShot());
        }

        else
        {
            burstCount = 0;

            if (!GetComponent<BerthScript>())
            {
                subject.materials[materialIndex].color = Color.red;
            }

            rangeTimeout = true;
        }
    }
}