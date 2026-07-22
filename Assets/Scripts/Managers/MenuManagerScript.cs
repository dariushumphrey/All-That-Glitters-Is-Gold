using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MenuManagerScript : MonoBehaviour
{
    //Configuration for use on Main Menu or Gameplay
    public enum Setting
    {
        Menu = 0, Game = 1
    }

    public Setting setting;

    [Header("Campaign Variables")]
    public Image caThumbnail; //caThumbnail - Image UI that displays Campaign level pictures
    public Sprite caLevelOne, caLevelTwo, caLevelThree, caLevelFour, caLevelFive; //Images of Level thumbnails
    public Text caDiffText, caLevelText, caObjectiveText, caCheckpointText; //Texts that displays difficulty number, level name, or Weapon focus
    public Slider caDifficulty, caLevel, caCheckpoint; //Sliders used to select Weapon type, level, or difficulty

    [Header("Viricide Variables")]
    public Image vcThumbnail; //vcThumbnail - Image UI that displays Viricide level pictures
    public Sprite vcLevelOne, vcLevelTwo, vcLevelThree; //Images of Level thumbnails
    public Text vcDiffText, vcLevelText, vcWepFocusText; //Texts that displays difficulty number, level name, or Weapon focus
    public Button vcButton; //Viricide navigation button
    public Slider vcDifficulty, vcLevel, vcWepFocus; //Sliders used to select Weapon type, level, or difficulty

    [Header("How To Play guide Variables")]
    public Image activeTab;
    public Image[] tabs;

    [Header("Controls page Variables")]
    public Image controllerLayout;
    public Image mnkLayout;

    [Header("Gameplay Settings page Variables")]
    public Slider ctrX;
    public Slider ctrY;
    public Slider mkX;
    public Slider mkY;
    public Text controllerX, controllerY, mouseX, mouseY;
    public Toggle aimToggle;

    private enum InputType { MNK, Controller }
    private InputType lastInput = InputType.MNK;

    private LevelManagerScript levelManager;
    private WeaponManagerScript weaponManager;

    private void Awake()
    {
        if(setting == Setting.Menu)
        {
            levelManager = FindObjectOfType<LevelManagerScript>();
            weaponManager = FindObjectOfType<WeaponManagerScript>();
        }

        activeTab = tabs[0];

    }

    // Start is called before the first frame update
    void Start()
    {
        if (setting == Setting.Menu)
        {
            Progression();
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if (setting == Setting.Menu)
        {
            //Changes Campaign/Viricide thumbnail image by slider value
            if (caLevel.value == 1)
            {
                caThumbnail.sprite = caLevelOne;
                caObjectiveText.text = "A vermin threat infests the Resplendent from the top down." + '\n' + 
                    "Objective: Find transit into the station's center.";

                if(PlayerPrefs.GetInt("lvl01Checkpoint") == 1)
                {
                    caCheckpoint.gameObject.SetActive(true);
                    caCheckpointText.gameObject.SetActive(true);
                }

                else
                {
                    caCheckpoint.gameObject.SetActive(false);
                    caCheckpointText.gameObject.SetActive(false);
                }
            }

            else if (caLevel.value == 2)
            {
                caThumbnail.sprite = caLevelTwo;
                caObjectiveText.text = "The crisis at hand lies beyond the current route." + '\n' +
                    "Objective: Stop the tram.";

                if (PlayerPrefs.GetInt("lvl02Checkpoint") == 1)
                {
                    caCheckpoint.gameObject.SetActive(true);
                    caCheckpointText.gameObject.SetActive(true);
                }

                else
                {
                    caCheckpoint.gameObject.SetActive(false);
                    caCheckpointText.gameObject.SetActive(false);
                }
            }

            else if (caLevel.value == 3)
            {
                caThumbnail.sprite = caLevelThree;
                caObjectiveText.text = "A hidden Replevin threat guards passage to ground zero." + '\n' +
                    "Objective: Kill the Replevin Ambuscade.";

                if (PlayerPrefs.GetInt("lvl03Checkpoint") == 1)
                {
                    caCheckpoint.gameObject.SetActive(true);
                    caCheckpointText.gameObject.SetActive(true);
                }

                else
                {
                    caCheckpoint.gameObject.SetActive(false);
                    caCheckpointText.gameObject.SetActive(false);
                }
            }

            else if (caLevel.value == 4)
            {
                caThumbnail.sprite = caLevelFour;
                caObjectiveText.text = "Miasmic hordes worsen as Lucent propagates outward." + '\n' +
                    "Objective: Approach the Replevin lair.";

                if (PlayerPrefs.GetInt("lvl04Checkpoint") == 1)
                {
                    caCheckpoint.gameObject.SetActive(true);
                    caCheckpointText.gameObject.SetActive(true);
                }

                else
                {
                    caCheckpoint.gameObject.SetActive(false);
                    caCheckpointText.gameObject.SetActive(false);
                }
            }

            else
            {
                caObjectiveText.text = "The apex of Replevin terror is found." + '\n' +
                    "Objective: Kill the Replevin Keystone.";
                caThumbnail.sprite = caLevelFive;

                if (PlayerPrefs.GetInt("lvl05Checkpoint") == 1)
                {
                    caCheckpoint.gameObject.SetActive(true);
                    caCheckpointText.gameObject.SetActive(true);
                }

                else
                {
                    caCheckpoint.gameObject.SetActive(false);
                    caCheckpointText.gameObject.SetActive(false);
                }
            }


            if (vcLevel.value == 6)
            {
                vcThumbnail.sprite = vcLevelOne;
            }

            else if (vcLevel.value == 7)
            {
                vcThumbnail.sprite = vcLevelTwo;
            }

            else
            {
                vcThumbnail.sprite = vcLevelThree;
            }

            //Changes Campaign/Viricide difficulty, level text by slider value
            caDiffText.text = "Difficulty: " + caDifficulty.value;
            vcDiffText.text = "Difficulty: " + vcDifficulty.value;

            caLevelText.text = "Level " + caLevel.value;
            vcLevelText.text = "Viricide: " + (vcLevel.value - 5);

            if(caCheckpoint.value == 1)
            {
                caCheckpointText.text = "Start - Checkpoint";
            }

            else
            {
                caCheckpointText.text = "Start - Fresh";
            }

            //Changes Weapon Focus text by slider value
            if(vcWepFocus.value == -1)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Random";
            }

            else if (vcWepFocus.value == 0)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Full Fire Rifles";
            }

            else if (vcWepFocus.value == 1)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Machine Guns";
            }

            else if (vcWepFocus.value == 2)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Pistols";
            }

            else if (vcWepFocus.value == 3)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Burst Fire Rifles";
            }

            else if (vcWepFocus.value == 4)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Shotguns";
            }

            else if (vcWepFocus.value == 5)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Single Fire Rifles";
            }

            else if (vcWepFocus.value == 6)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Submachine Guns";
            }

            else if (vcWepFocus.value == 7)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Grenade Launchers";
            }

            else if (vcWepFocus.value == 8)
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "Opening Shots";
            }

            else
            {
                vcWepFocusText.text = "Targeted Weapons: " + "\n" + "AMLRs";
            }

            controllerX.text = (ctrX.value / 10).ToString();
            controllerY.text = (ctrY.value / 10).ToString();
            mouseX.text = (mkX.value / 10).ToString();
            mouseY.text = (mkY.value / 10).ToString();
        }    
    }

    public void FormatGameplaySettings()
    {
        ctrX.value = 5;
        ctrY.value = 5;
        mkX.value = 10;
        mkY.value = 10;
        aimToggle.isOn = false;
    }

    public void SaveGameplaySettings()
    {
        if(aimToggle.isOn)
        {
            PlayerPrefs.SetInt("toggleAim", 1);
        }

        else
        {
            PlayerPrefs.SetInt("toggleAim", 0);
        }

        PlayerPrefs.SetFloat("aimControllerX", ctrX.value);
        PlayerPrefs.SetFloat("aimControllerY", ctrY.value);
        PlayerPrefs.SetFloat("aimMouseX", mkX.value);
        PlayerPrefs.SetFloat("aimMouseY", mkY.value);
    }

    /// <summary>
    /// Initializes Viricide game dependent on slider values and loads scene
    /// </summary>
    public void InitializeViricideGame()
    {
        levelManager.setting = LevelManagerScript.Setting.Viricide;
        levelManager.gameSettingState = (int)vcDifficulty.value;
        levelManager.level = (int)vcLevel.value;
        levelManager.weaponFocus = (int)vcWepFocus.value;

        if(levelManager.lvlProgressSaved)
        {
            levelManager.lvlProgressSaved = false;
        }

        //levelManager.LoadScene();
        levelManager.StartCoroutine(levelManager.LoadAsyncedSceneDelay());
    }

    /// <summary>
    /// Initializes Campaign game dependent on slider values and loads scene
    /// </summary>
    public void InitializeCampaignGame()
    {
        levelManager.setting = LevelManagerScript.Setting.Campaign;
        levelManager.gameSettingState = (int)caDifficulty.value;
        levelManager.level = (int)caLevel.value;
        levelManager.weaponFocus = -1;

        if (caCheckpoint.value == 1)
        {
            levelManager.lvlProgressSaved = true;
        }

        else
        {
            levelManager.lvlProgressSaved = false;
        }

        //levelManager.LoadScene();
        levelManager.StartCoroutine(levelManager.LoadAsyncedSceneDelay());
    }

    public void OpenPage(GameObject page)
    {
        page.gameObject.SetActive(true);
    }

    public void ClosePage(GameObject page)
    {
        page.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Configures slider max values by PlayerPref values
    /// Changes Viricide button interactivity by PlayerPref value
    /// </summary>
    public void Progression()
    {
        if (PlayerPrefs.GetInt("unlockLevel05") == 1)
        {
            caLevel.maxValue = 5;
        }

        else if (PlayerPrefs.GetInt("unlockLevel04") == 1)
        {
            caLevel.maxValue = 4;
        }

        else if (PlayerPrefs.GetInt("unlockLevel03") == 1)
        {
            caLevel.maxValue = 3;
        }

        else if(PlayerPrefs.GetInt("unlockLevel02") == 1)
        {
            caLevel.maxValue = 2;
        }

        else
        {
            caLevel.maxValue = 1;
        }

        if (PlayerPrefs.GetInt("unlockDifficulty5") == 1)
        {
            caDifficulty.maxValue = 5;
            vcDifficulty.maxValue = 5;
        }

        else
        {
            caDifficulty.maxValue = 4;
            vcDifficulty.maxValue = 4;
        }

        if (PlayerPrefs.GetInt("unlockViricide") == 1)
        {
            vcButton.interactable = true;
        }

        else
        {
            vcButton.interactable = false;
        }
    }

    /// <summary>
    /// For use with How to Play guide -- opens and closes tabs
    /// </summary>
    /// <param name="i"></param>
    public void OpenTab(int i)
    {
        activeTab.gameObject.SetActive(false);
        activeTab = null;

        activeTab = tabs[i];
        activeTab.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    private void OnDisable()
    {
        InputSystem.onActionChange -= OnActionChange;
    }

    /// <summary>
    /// Changes collection text based on last device input
    /// </summary>
    /// <param name="obj">Holds information of input state change</param>
    /// <param name="change">Represents logged device action</param>
    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionStarted || change == InputActionChange.ActionPerformed)
        {
            var action = (InputAction)obj;
            if (action.activeControl != null)
            {
                InputDevice current = action.activeControl.device;
                if (current is Gamepad)
                {
                    if (lastInput != InputType.Controller)
                    {
                        lastInput = InputType.Controller;
                        controllerLayout.gameObject.SetActive(true);
                        mnkLayout.gameObject.SetActive(false);
                    }
                }

                else if (current is Keyboard || current is Mouse)
                {
                    if (lastInput != InputType.MNK)
                    {
                        lastInput = InputType.MNK;
                        mnkLayout.gameObject.SetActive(true);
                        controllerLayout.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
