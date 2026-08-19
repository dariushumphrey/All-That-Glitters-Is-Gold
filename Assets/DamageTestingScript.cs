using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTestingScript : MonoBehaviour
{
    public int targetDiff = 1;
    public GameObject target;
    public GameObject dpsCanvas;
    public int totalDamage;
    public float damagePerSecond;
    public float dpsTimer = 0f;
    public float dpsTestEnd = 4f;
    public float dpsTextUpdateTimer = 0f;
    public float dpsTextUpdateGoal = 0.2f;
    public float testPauseTimer = 6f;
    public bool testStarted = false;
    public bool testTimeout = false;
    public bool indefiniteTest = false;
    public Text dpsText, totalDamageText, timerText, timeoutText, ttkText;

    internal float timerVisualValue = 0f;
    private int startingTargetHealthMax;
    private float startingTargetHealthPercent;
    internal float testPauseReset;
    private PlayerInventoryScript activeWeapon;
    private float timeBetweenShots = 0f;
    private float timeToKill = 0f;
    private bool reactivateSignal = false;
    // Start is called before the first frame update
    void Start()
    {
        activeWeapon = FindObjectOfType<PlayerInventoryScript>();

        startingTargetHealthMax = target.GetComponent<EnemyHealthScript>().healthMax;
        startingTargetHealthPercent = target.GetComponent<EnemyHealthScript>().healthPercent;

        damagePerSecond = 0f;
        totalDamage = 0;
        dpsTimer = 0f;
        timeToKill = 0f;

        timerVisualValue = dpsTestEnd;
        testPauseReset = testPauseTimer;

        dpsText.text = damagePerSecond.ToString("F0");
        totalDamageText.text = totalDamage.ToString();
        timerText.text = timerVisualValue.ToString("F1") + "s";
        ttkText.text = "";
        timeoutText.text = "Ready";        
    }

    // Update is called once per frame
    void Update()
    {
        if(testStarted)
        {
            timeoutText.text = "";

            dpsTimer += Time.deltaTime;
            dpsTextUpdateTimer += Time.deltaTime;

            if(!indefiniteTest)
            {
                timerVisualValue -= Time.deltaTime;
            }

            else
            {
                timerVisualValue += Time.deltaTime;
            }

            totalDamageText.text = totalDamage.ToString();
            timerText.text = timerVisualValue.ToString("F1") + "s ";

            if (dpsTimer > 0)
            {
                damagePerSecond = totalDamage / dpsTimer;
            }

            if(!indefiniteTest)
            {
                if (dpsTimer >= dpsTestEnd)
                {
                    dpsTimer = 0f;
                    dpsText.text = damagePerSecond.ToString("F0");
                    target.GetComponent<EnemyHealthScript>().healthCurrent = target.GetComponent<EnemyHealthScript>().healthMax;

                    timeoutText.text = "Timeout...";
                    testTimeout = true;
                    StartCoroutine(EndTestTimeout());

                    testStarted = false;
                }
            }

            else
            {
                testPauseTimer -= Time.deltaTime;
                if(testPauseTimer <= 0f)
                {
                    testPauseTimer = 0f;
                    timeoutText.text = "Ready";
                    testStarted = false;
                }
            }          

            if (dpsTextUpdateTimer >= dpsTextUpdateGoal)
            {
                dpsTextUpdateTimer = 0f;
                dpsText.text = damagePerSecond.ToString("F0");
            }

            if(timerVisualValue <= 0f && !indefiniteTest)
            {
                timerVisualValue = dpsTestEnd;
                timerText.text = timerVisualValue.ToString("F1") + "s";
            }

            if(target.GetComponent<EnemyHealthScript>().healthCurrent <= 0)
            {
                StartCoroutine(CalculateTimeToKill());
            }
        }

        if (Time.timeScale == 0)
        {
            if (dpsCanvas.gameObject.activeInHierarchy == true)
            {
                dpsCanvas.gameObject.SetActive(false);
                reactivateSignal = true;
            }
        }

        if(reactivateSignal)
        {
            if(Time.timeScale == 1)
            {
                dpsCanvas.gameObject.SetActive(true);
                reactivateSignal = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            dpsCanvas.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            dpsCanvas.gameObject.SetActive(false);
        }
    }

    public void ModifyTargetDifficulty()
    {
        target.GetComponent<EnemyHealthScript>().difficultyValue = targetDiff;
        target.GetComponent<EnemyHealthScript>().healthMax = startingTargetHealthMax;
        target.GetComponent<EnemyHealthScript>().healthPercent = startingTargetHealthPercent;
        target.GetComponent<EnemyHealthScript>().DifficultyMatch();
    }

    public void ModifyTestDuration(float newDuration)
    {
        if(newDuration == -1)
        {
            indefiniteTest = true;

            damagePerSecond = 0f;
            totalDamage = 0;
            timerVisualValue = 0f;
            timeToKill = 0f;

            dpsText.text = damagePerSecond.ToString("F0");
            totalDamageText.text = totalDamage.ToString();
            timerText.text = timerVisualValue.ToString("F1") + "s";
            ttkText.text = "";

        }

        else
        {
            if(indefiniteTest)
            {
                indefiniteTest = false;
            }

            damagePerSecond = 0f;
            totalDamage = 0;
            dpsTestEnd = newDuration;
            timerVisualValue = dpsTestEnd;
            timeToKill = 0f;

            dpsText.text = damagePerSecond.ToString("F0");
            totalDamageText.text = totalDamage.ToString();
            timerText.text = timerVisualValue.ToString("F1") + "s";
            ttkText.text = "";
        }
    }

    public IEnumerator CalculateTimeToKill()
    {
        yield return null;
        //Shots to kill is calculated in EnemyHealthScript

        float roundedFireRate = Mathf.Round((60f / activeWeapon.inventory[activeWeapon.selection].GetComponent<FirearmScript>().fireRate));
        timeBetweenShots = (60 / roundedFireRate);

        timeToKill = (target.GetComponent<EnemyHealthScript>().shotsToKill - 1) * timeBetweenShots;
        ttkText.text = target.GetComponent<EnemyHealthScript>().shotsToKill + " | " + timeToKill.ToString("F2") + "s";
    }

    IEnumerator EndTestTimeout()
    {
        yield return new WaitForSeconds(1.5f);
        testTimeout = false;
        timeoutText.text = "Ready";
    }
}
