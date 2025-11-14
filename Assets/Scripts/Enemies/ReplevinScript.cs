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

    public float meleeRangeCheck; //Distance required for Enemy to begin attack
    public float meleeRangeMin; //Melee attack range
    public float meleeAttackTimer = 0.8f; //Time to wait before committing a Melee attack
    public float meleeTimeout; //Time to wait before next melee
    public float meleeAttackForce;
    public float attackRate; //Rate of speed of which attacks occur

    [Header("Range Attack settings")]

    //This value buffs attack rate of Ranged enemies:
    //-Increasing this number adds a percentage of fire rate onto itself, allowing for faster firing.
    public float rangeAttackChange = 15f;
    public float rangeAttackRate;
    public float rangedAttackForce;
    public float rangeATKMin; //Distance required for Enemy to begin attack
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

    internal NavMeshAgent self;
    internal EnemyHealthScript enemy;
    private EnemyManagerScript manager;
    internal BossManagerScript boss; //For bosses only -- Used to spawn enemies when returning immune

    //distance - Describes length between Player and Enemy
    //lastKnownDistance - Describes last recorded length between Player and Enemy
    //lastPlayerPosition - Describes last recorded Player position
    //recoilPosition - Describes stagger position when hitting Lucent wall -- Bosses only
    private Vector3 distance, lastKnownDistance, lastPlayerPosition, recoilPosition;
    internal GameObject stunMechanic; //New Lucent Cluster game object

    private GameObject[] waypoint; //Array of waypoints
    private int waypointNext; //Number used to randomly choose next waypoint
    private float accuracy = 5.0f; //Goal length between Waypoint and Enemy
    private GameObject player; //Player game object

    internal int moveSpeedSlow; //Modified Enemy speed when Slowed
    internal int boostSpeedSlow; //Modified Enemy boost speed when Slowed
    internal int nmaAccelSlow; //Modified NavMeshAgent acceleration when Slowed
    internal int moveSpeedReset; //Holds starting Enemy speed
    internal int boostSpeedReset; //Holds starting Enemy boost speed
    internal int accelReset; //Holds starting NavMeshAgent acceleration

    private float meleeReset; //Holds starting Melee attack timer
    private float chargeReset; //Holds starting Charge attack timer
    private float buildupReset; //Holds starting Charge attack delay timer
    private float punchReset; //Holds starting Pounce attack timer
    private float jumpReset; //Holds starting Jump attack timer
    private float airtimeReset; //Holds starting Force jump timer
    private float attackAgain; //Holds starting attack rate
    private float gapCloseReset; //Holds starting Pounce attack speed
    private float berthJumpTimerReset; //Holds starting Berth Jump cluster timer
    private bool destinationSet = false; //Affirms location if true
    private bool gathered = false; //Affirms cluster has been spawned if true -- Bosses only
    private bool throwTarget = false; //Affirms Enemy can throw cluster at Player if true -- Bosses only
    private bool canAttackAgain = true; //Affirms Enemy can attack once more if true
    private bool recorded = false; //Affirms information has been saved for attack if true
    private bool attackLock = false; //Affirms Enemy is using Raycast for attack if true
    private bool ramTimeout = false; //Affirms Charge enemy has ended attack if true
    private bool slamTimeout = false; //Affirms Jump, Pounce Enemy has ended attack if true
    private bool lockOn = false; //Affirms Jump Enemy is lerping to Players if true
    private bool addWave = false; //Affirms Boss has spawned Enemies if true
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

        self = GetComponent<NavMeshAgent>();
        waypoint = GameObject.FindGameObjectsWithTag("Waypoint");
        waypointNext = Random.Range(0, waypoint.Length);
        player = GameObject.FindGameObjectWithTag("Player");
        enemy = GetComponent<EnemyHealthScript>();
        manager = FindObjectOfType<EnemyManagerScript>();

        if(amBoss)
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
            rangeAttackChange *= rangeAttackRate;
            rangeAttackRate -= rangeAttackChange;
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
                distance = transform.position - player.transform.position;
                attackAgain += Time.deltaTime;

                if (CanSeePlayer())
                {
                    self.SetDestination(player.transform.position);
                    self.speed = boostSpeed;

                    if (distance.magnitude <= meleeRangeCheck)
                    {
                        self.speed = moveSpeed / 2;
                        self.acceleration = enemyAcceleration / 2;

                        meleeAttackTimer -= Time.deltaTime;
                        if (meleeAttackTimer <= 0f)
                        {
                            attackLock = true;
                            self.speed = moveSpeed * 2;

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

                                            if (hit.collider.GetComponent<PlayerStatusScript>().counterplayCheat)
                                            {
                                                hit.collider.GetComponent<PlayerStatusScript>().counterplayFlag = true;                                               
                                            }

                                            attackLock = false;
                                            meleeAttackTimer = meleeReset;
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

                                            attackLock = false;
                                            meleeAttackTimer = meleeReset;           
                                        }
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

            distance = transform.position - player.transform.position;
            attackAgain += Time.deltaTime;

            Vector3 rayOrigin = attackStartPoint.transform.position;
            RaycastHit hit;  

            if(CanSeePlayer())
            {
                if(amSentry)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-distance, Vector3.up), rotationStrength);
                }

                else
                {
                    self.SetDestination(player.transform.position);
                }

                if (distance.magnitude <= rangeATKMin)
                {
                    if(!amSentry)
                    {
                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-distance, Vector3.up), rotationStrength);
                    }

                    if (Physics.Raycast(rayOrigin, attackStartPoint.transform.forward, out hit, rangeATKMin) && attackAgain >= rangeAttackRate)
                    {
                        if (hit.collider.tag == "Enemy")
                        {
                            //Debug.Log("Danger of Friendly Fire; Aborting!");
                            Task.current.Fail();
                        }

                        else
                        {
                            attackAgain = 0.0f;

                            GameObject projectile = Instantiate(rangeProjectile, attackStartPoint.transform.position, attackStartPoint.transform.rotation);
                            projectile.GetComponent<ProjectileScript>().damage = damage;
                            if (GetComponent<BerthScript>())
                            {
                                projectile.GetComponent<ProjectileScript>().berthFlag = true;
                            }

                            projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * rangedAttackForce);                      
                        }
                    }
                }            
            }

            else
            {
                if (amSentry)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(-distance, Vector3.up), rotationStrength);
                }

                else
                {
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

                if(AmIGrounded())
                {
                    airtimeShort -= Time.deltaTime;
                    if (airtimeShort <= 0f)
                    {
                        airtimeShort = airtimeReset;
                        if(lockOn)
                        {
                            lockOn = false;
                        }

                        recorded = false;
                    }
                }

                else
                {
                    airtimeShort = airtimeReset;

                    if(GetComponent<BerthScript>() && !lockOn)
                    {
                        berthJumpCarpetBombTimer -= Time.deltaTime;
                        if(berthJumpCarpetBombTimer <= 0f)
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
                        GameObject takeoff = Instantiate(jumpTakeoff, transform.position + Vector3.down, transform.rotation);

                    }
                }

                transform.position = Vector3.Lerp(transform.position, lastPlayerPosition, gapClose * Time.deltaTime);
                lastKnownDistance = lastPlayerPosition - transform.position;
                //Debug.Log(lastKnownDistance.magnitude + " | " + self.stoppingDistance);

                if (lastKnownDistance.magnitude <= pounceLimit)
                {
                    slamTimeout = true;
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
                gapClose = gapCloseReset;
                punchTimeout = punchReset;
                slamTimeout = false;
                canAttackAgain = true;
                recorded = false;
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
}