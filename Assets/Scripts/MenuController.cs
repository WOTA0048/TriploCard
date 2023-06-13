using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEngine.UIElements;

public class MenuController : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject mainMenuPanel;
    public GameObject selectionsPanel;
    public GameObject loadingScreenPanel;
    public GameObject confirmationPanel;

    [Header("References")]
    public GameObject gameSettingsObject;
    public GameSettings gameSettings;

    public GameObject startButton;
    public GameObject backButton;
    public GameObject resumeButton;
    public GameObject Banner;

    public GameObject menuAudio;
    public GameObject audioButton;
    
    public GameObject checkMark;
    public UnityEngine.UI.Image loadingBarProgress;
    public TextMeshProUGUI colorText;

    public TextMeshProUGUI directoryText;

    public Sprite[] soundOnOff;
    public int soundToggle = 1;

    [Header("Game Settings")]
    public GameObject orangeColor;
    public GameObject purpleColor;
    public GameObject randomColor;
    public string nextScene;
    [SerializeField] private int chosenColor;

    [Header("Credits Pages")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private int creditsCurrentPage;
    [SerializeField] private GameObject[] creditsPageArray;

    [Header("Guide Pages")]
    [SerializeField] private GameObject guideOutline;
    [SerializeField] private GameObject guidePanel;
    [SerializeField] private GameObject theGuide;
    [SerializeField] private GameObject textIndonesia;
    [SerializeField] private GameObject textEnglish;
    [SerializeField] private int guideCurrentLanguage;
    [SerializeField] private int guideCurrentPage;
    [SerializeField] private Sprite[] guideIDNPageArray;
    [SerializeField] private Sprite[] guideENGPageArray;

    void Awake()
    {
        
    }

    void Update()
    {
        resumeButtonActivatorCheck();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (backButton.activeSelf == false && loadingScreenPanel.activeSelf == false)
                confirmationPanel.transform.GetChild(0).gameObject.SetActive(true);

            else if (backButton.activeSelf == true)
                backMenu();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameSettings);
        gameSettingsObject = GameObject.FindGameObjectWithTag("GameSettings");
        gameSettings = gameSettingsObject.GetComponent<GameSettings>();

        chosenColor = 2;
        colorText.color = new Color32(255, 255, 255, 255);
        randomColor.transform.GetChild(1).gameObject.SetActive(true);
        purpleColor.transform.GetChild(1).gameObject.SetActive(false);
        orangeColor.transform.GetChild(1).gameObject.SetActive(false);

        mainMenuPanel.SetActive(true);

        directoryText.SetText(Application.persistentDataPath);

        resumeButtonActivatorCheck();


        if (soundToggle != gameSettings.soundOnOff)
        {
            soundOnOffSwitch();
        }
        //soundOnOffSwitch();
    }


    IEnumerator loadLevelAsync()
    {
        loadingScreenPanel.SetActive(true);
        selectedMenu(0);

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(nextScene);

        while (loadingOperation.isDone == false)
        {
            loadingBarProgress.fillAmount = loadingOperation.progress;

            yield return new WaitForEndOfFrame();
        }
    }

    public void soundOnOffSwitch()
    {
        if (soundToggle == 1)
        {
            menuAudio.transform.GetChild(0).gameObject.SetActive(false);
            menuAudio.transform.GetChild(1).gameObject.SetActive(false);
            audioButton.GetComponent<UnityEngine.UI.Image>().sprite = soundOnOff[1];
            gameSettings.soundOnOff = 0;
            soundToggle = 0;
        }

        else if (soundToggle == 0)
        {
            menuAudio.transform.GetChild(0).gameObject.SetActive(true);
            menuAudio.transform.GetChild(1).gameObject.SetActive(true);
            audioButton.GetComponent<UnityEngine.UI.Image>().sprite = soundOnOff[0];
            gameSettings.soundOnOff = 1;
            soundToggle = 1;
        }
    }

    private void resumeButtonActivatorCheck()
    {
        if (gameSettings.dataLoadAvailable == false)
        {
            resumeButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

        else if (gameSettings.dataLoadAvailable == true)
        {
            resumeButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }

    public void quitGame(int confirm)
    {
        if (confirm == 0) //NO
        {
            confirmationPanel.transform.GetChild(0).gameObject.SetActive(false);
            mainMenuPanel.SetActive(true);
        }

        else if (confirm == 1) //YES
        {
            print("QUITGAME");
            Application.Quit();
        }
    }
    public void resumeGame(int confirm)
    {
        if (confirm == 0) //STARTOVER
        {
            confirmationPanel.transform.GetChild(1).gameObject.SetActive(false);
            mainMenuPanel.SetActive(false);
            
            selectionsPanel.SetActive(true);
            backButton.SetActive(true);
        }

        else if (confirm == 1) //RESUME
        {
            gameSettings.dataLoaded = true;
        }
    }


    public void backMenu()
    {
        resumeButtonActivatorCheck();

        selectionsPanel.SetActive(false);

        creditsPanel.SetActive(false);
        creditsButtons(2);

        if (guideOutline.activeInHierarchy == true && guidePanel.activeInHierarchy == false)
        {
            guideOutline.SetActive(false);
            backButton.SetActive(false);
        }

        else if (guidePanel.activeInHierarchy == true && guidePanel.activeInHierarchy == true)
        {
            guidePanel.SetActive(false);
            backButton.SetActive(true);
        }

        else if (guideOutline.activeInHierarchy == false && guidePanel.activeInHierarchy == false)
        { 
            backButton.SetActive(false);
        }

        //--  

        mainMenuPanel.SetActive(true);
        
    }

    public void selectedMenu(int menu)//1 start game
    {
        //backButton.SetActive(true);

        if (menu == 0)//loading
        {
            backButton.SetActive(false);
            selectionsPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
        }

        else if (menu == 1) //start game > level color panel
        {
            mainMenuPanel.SetActive(false);
            

            if (resumeButton.GetComponent<UnityEngine.UI.Button>().interactable == false)
            {
                selectionsPanel.SetActive(true);
                backButton.SetActive(true);
            }

            else if (resumeButton.GetComponent<UnityEngine.UI.Button>().interactable == true)
                confirmationPanel.transform.GetChild(1).gameObject.SetActive(true);
        }

        else if (menu == 2) //resume game
        {
            gameSettings.dataLoaded = true;
        }

        else if (menu == 3) //rules
        {
            backButton.SetActive(true);
            showGuide();
        }

        else if (menu == 4) //credits
        {
            backButton.SetActive(true);
            showCredits();   
        }

        else if (menu == 5) //exit
        {
            mainMenuPanel.SetActive(false);
            
            confirmationPanel.transform.GetChild(0).gameObject.SetActive(true);
        }

    }

    public void chooseDifficulty(int level)
    {
        gameSettings.chosenLevel = level;
    }

    public void showGuide()
    {
        if (guidePanel.gameObject.activeInHierarchy == false)
        {
            guideOutline.SetActive(true);
            //guidePanel.SetActive(true);
        }

        else
        {
            guideOutline.SetActive(false);
            guideOutline.SetActive(false);
        }
    }

    public void showCredits()
    {
        if (creditsPanel.gameObject.activeInHierarchy == false)
        {
            creditsPanel.SetActive(true);
            creditsPageArray[0].SetActive(true);
        }

        else
            creditsPanel.SetActive(false);
    }

    public void creditsButtons(int changePage)
    {
        //print("Change Credits Page");

        if (changePage == 0)
        {
            if (creditsCurrentPage == 0)
            {
                creditsCurrentPage = 2;
                creditsPageArray[0].SetActive(false);
                creditsPageArray[1].SetActive(false);
                creditsPageArray[2].SetActive(true);
            }

            else if (creditsCurrentPage == 1)
            {
                creditsCurrentPage = 0;
                creditsPageArray[1].SetActive(false);
                creditsPageArray[2].SetActive(false);
                creditsPageArray[0].SetActive(true);
            }

            else if (creditsCurrentPage == 2)
            {
                creditsCurrentPage = 1;
                creditsPageArray[0].SetActive(false);
                creditsPageArray[2].SetActive(false);
                creditsPageArray[1].SetActive(true);
            }
        }

        else if (changePage == 1)
        {
            if (creditsCurrentPage == 0)
            {
                creditsCurrentPage = 1;
                creditsPageArray[0].SetActive(false);
                creditsPageArray[2].SetActive(false);
                creditsPageArray[1].SetActive(true);
            }

            else if (creditsCurrentPage == 1)
            {
                creditsCurrentPage = 2;
                creditsPageArray[0].SetActive(false);
                creditsPageArray[1].SetActive(false);
                creditsPageArray[2].SetActive(true);
            }

            else if (creditsCurrentPage == 2)
            {
                creditsCurrentPage = 0;
                creditsPageArray[1].SetActive(false);
                creditsPageArray[2].SetActive(false);
                creditsPageArray[0].SetActive(true);
            }
        }

        else if (changePage == 2)//reset
        {
            creditsCurrentPage = 0;
            creditsPageArray[0].SetActive(false);
            creditsPageArray[1].SetActive(false);
            creditsPageArray[2].SetActive(false);
        }
    }

    public void setGuidePage(int setPage)
    {
        guideCurrentPage = setPage;
    }

    public void guideButtons(int changePage)
    {
        if (changePage == 0)
        {
            if (guideCurrentPage > 0)//more than min
                guideCurrentPage--;

            else if (guideCurrentPage <= 0)//previous page from first
            {
                guideCurrentPage = guideIDNPageArray.Length - 1; //length is 15 so it is actually (- 1 = 14)
            }  
        }

        else if (changePage == 1)
        {
            if (guideCurrentPage < guideIDNPageArray.Length - 1)//less than max
                guideCurrentPage++;

            else if (guideCurrentPage >= guideIDNPageArray.Length - 1)//back to beginning
                guideCurrentPage = 0;
        }

        if (guideCurrentLanguage == 0)
            theGuide.GetComponent<UnityEngine.UI.Image>().sprite = guideIDNPageArray[guideCurrentPage];

        else if (guideCurrentLanguage == 1)
            theGuide.GetComponent<UnityEngine.UI.Image>().sprite = guideENGPageArray[guideCurrentPage];
    }

    public void guideLanguageButtons()
    {
        if (guideCurrentLanguage == 0)
        {
            guideCurrentLanguage = 1;

            textEnglish.SetActive(true);
            textIndonesia.SetActive(false);

            theGuide.GetComponent<UnityEngine.UI.Image>().sprite = guideENGPageArray[guideCurrentPage];
        }

        else if (guideCurrentLanguage == 1)
        {
            guideCurrentLanguage = 0;

            textEnglish.SetActive(false);
            textIndonesia.SetActive(true);

            theGuide.GetComponent<UnityEngine.UI.Image>().sprite = guideIDNPageArray[guideCurrentPage];
        }

    }

    public void switchScene(string sceneName)
    {
        nextScene = sceneName;

        //loadingScreenPanel.SetActive(true);

        StartCoroutine(loadLevelAsync());
    }

    public void thisColorChosen(int Color)//
    {
        chosenColor = Color;

        if (Color == 0)
        {
            randomColor.transform.GetChild(1).gameObject.SetActive(false);
            purpleColor.transform.GetChild(1).gameObject.SetActive(false);
            orangeColor.transform.GetChild(1).gameObject.SetActive(true);
            
            colorText.color = new Color32 (255, 90, 45, 255);
        }

        else if (Color == 1)
        {
            randomColor.transform.GetChild(1).gameObject.SetActive(false);
            orangeColor.transform.GetChild(1).gameObject.SetActive(false);
            purpleColor.transform.GetChild(1).gameObject.SetActive(true);
            colorText.color = new Color32(130, 90, 175, 255);
        }

        else if (Color == 2)
        {
            orangeColor.transform.GetChild(1).gameObject.SetActive(false);
            purpleColor.transform.GetChild(1).gameObject.SetActive(false);
            randomColor.transform.GetChild(1).gameObject.SetActive(true);
            colorText.color = new Color32(255, 255, 255, 255);
        }

        gameSettings.chosenCardColor = chosenColor;
    }

}
