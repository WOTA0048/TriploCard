using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gamePlayer;
    [SerializeField] private GameObject gameEnemy;
    [SerializeField] public GameSettings gameSettings;
    [SerializeField] public GameObject gameSettingsObject;
    [SerializeField] public RemovedCard removedCard;
    [SerializeField] private GameSaveData SaveDataRef;
    [SerializeField] private GameObject burgerButton;
    [SerializeField] private GameObject endgameButtons;
    [SerializeField] private GameObject backButton;
    [SerializeField] private PlayerController gamePlayerController;
    [SerializeField] private EnemyController gameEnemyController;

    [SerializeField] private TextMeshProUGUI announcerText;

    [SerializeField] private GameObject loadingScreenPanel;
    [SerializeField] private UnityEngine.UI.Image loadingBarProgress;

    [SerializeField] private GameObject startRoundButton;
    [SerializeField] private Sprite[] cardLockGraphicArray;

    [SerializeField] private Sprite[] winLosePrompt;

    [SerializeField] private GameObject confirmationPanel;

    [Header("Player Cards")]
    [SerializeField] private int gamePlayerCardOne;
    [SerializeField] private int gamePlayerCardTwo;
    [SerializeField] private int gamePlayerCardThree;
    [SerializeField] private int gamePlayerCardFour;
    [SerializeField] private int gamePlayerCardFive;
    [SerializeField] private int gamePlayerCardSix;

    [Header("Enemy Cards")]
    [SerializeField] public int gameEnemyCheatsRemaining;
    [SerializeField] public int gameEnemyCardRemaining;
    [SerializeField] public int gameEnemyCardOne;
    [SerializeField] public int gameEnemyCardTwo;
    [SerializeField] public int gameEnemyCardThree;
    [SerializeField] public int gameEnemyCardFour;
    [SerializeField] public int gameEnemyCardFive;
    [SerializeField] public int gameEnemyCardSix;

    [Header("Values")]
    [SerializeField] private int burgerButtonMode;
    [SerializeField] private string path;
    [SerializeField] public int highestCardToRemove;
    [SerializeField] public int randomCardToRemove;

    [SerializeField] private int roundResult;
    [SerializeField] private bool playerWins;
    [SerializeField] private bool enemyWins;
    [SerializeField] private bool roundDraw;
    [SerializeField] private bool skipResult;

    [SerializeField] private int gameDifficultyData;

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

    [Header("Audio")]
    [SerializeField] private GameObject gameAudio;
    [SerializeField] private AudioSource gameMusicAudioSource;
    [SerializeField] private AudioSource gameSFXAudioSource;
    [SerializeField] private AudioClip removeCardSFX;
    [SerializeField] private AudioClip roundWinSFX;
    [SerializeField] private AudioClip roundLoseDrawSFX;
    [SerializeField] private AudioClip[] gameFinishedMusic; //0 win 1 lose
    [SerializeField] private GameObject audioButton;
    [SerializeField] public Sprite[] soundOnOff;
    [SerializeField] public int soundToggle = 1;

    // Start is called before the first frame update
    void Start()
    {
        //GCPlayer = GameObject.FindGameObjectWithTag("Player");
        gamePlayerController = gamePlayer.GetComponent<PlayerController>();
        gameEnemyController = gameEnemy.GetComponent<EnemyController>();

        gameSettingsObject = GameObject.FindGameObjectWithTag("GameSettings");
        gameSettings = gameSettingsObject.GetComponent<GameSettings>();
        gamePlayerController.gameSettings = gameSettings;
        gameEnemyController.gameSettings = gameSettingsObject;
        
        SaveDataRef = new GameSaveData();
        //path = Application.persistentDataPath + "/SaveData.txt";

        if (soundToggle != gameSettings.soundOnOff)
        {
            //soundToggle = gameSettings.soundOnOff;
            soundOnOffSwitch();
        }

        gameDifficultyData = gameSettings.chosenLevel;
        gameEnemyController.enemyDifficultyLevel = gameDifficultyData;
    }

    void Update()
    {        
        if (gameSettings.GetComponent<GameSettings>().dataLoaded == true) //RESUME
        {
            loadData();
            gameSettings.GetComponent<GameSettings>().dataLoaded = false;
        }

        if (gamePlayerController.cardSetup[1].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite == null
            || gamePlayerController.cardSetup[2].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite == null)
        {
            //Unsure why this has to be done
            gamePlayerController.cardSetupper(0);
            gamePlayerController.cardSetupper(1);
            gamePlayerController.cardSetupper(2);
        }

        if (gameEnemyController.cardSetup[1].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite == null
            || gameEnemyController.cardSetup[2].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite == null)
        {
            //Unsure why this has to be done
            gameEnemyController.cardSetupper(0);
            gameEnemyController.cardSetupper(1);
            gameEnemyController.cardSetupper(2);
        }

        
    }

    IEnumerator loadLevelAsync()
    {
        loadingScreenPanel.SetActive(true);
        //selectedMenu(0);

        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync("MainMenu");


        while (loadingOperation.isDone == false)
        {
            loadingBarProgress.fillAmount = loadingOperation.progress;

            yield return new WaitForEndOfFrame();
        }
    }

    [ContextMenu("Tools/Write file")]
    static void writeString(string t)
    {
        //string path = "Assets/SaveData.txt";
        string path = "/SaveData.txt";

        if (File.Exists(Application.persistentDataPath + path) == false)
        {
            File.Create(Application.persistentDataPath + path);
        }
        //Write some text to the test.txt file
        //StreamWriter writer = new StreamWriter(Application.persistentDataPath, path, false);
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + path, false);
        writer.WriteLine(t);
        writer.Close();
        //Re-import the file to update the reference in the editor
        //Print the text from the file
    }

    [ContextMenu("Tools/Read file")]
    public GameSaveData readString()
    {
        //string path = "Assets/SaveData.txt";
        string path = "/SaveData.txt";
        //Read the text from directly from the test.txt file
        //Application.PersistentDataPath
        StreamReader reader = new StreamReader(Application.persistentDataPath + path);
        GameSaveData newSaveData = JsonUtility.FromJson<GameSaveData>(reader.ReadToEnd());

        gameDifficultyData = newSaveData.savedDifficultyData;
        gameEnemyCheatsRemaining = newSaveData.savedEnemyCheatsRemaining;

        gamePlayerCardOne = newSaveData.savedPlayerCardOne;
        gamePlayerCardTwo = newSaveData.savedPlayerCardTwo;
        gamePlayerCardThree = newSaveData.savedPlayerCardThree;
        gamePlayerCardFour = newSaveData.savedPlayerCardFour;
        gamePlayerCardFive = newSaveData.savedPlayerCardFive;
        gamePlayerCardSix = newSaveData.savedPlayerCardSix;

        gameEnemyCardOne = newSaveData.savedEnemyCardOne;
        gameEnemyCardTwo = newSaveData.savedEnemyCardTwo;
        gameEnemyCardThree = newSaveData.savedEnemyCardThree;
        gameEnemyCardFour = newSaveData.savedEnemyCardFour;
        gameEnemyCardFive = newSaveData.savedEnemyCardFive;
        gameEnemyCardSix = newSaveData.savedEnemyCardSix;

        gameEnemyController.enemyControllerCardRemaining[1] = newSaveData.savedEnemyCardOne;
        gameEnemyController.enemyControllerCardRemaining[2] = newSaveData.savedEnemyCardTwo;
        gameEnemyController.enemyControllerCardRemaining[3] = newSaveData.savedEnemyCardThree;
        gameEnemyController.enemyControllerCardRemaining[4] = newSaveData.savedEnemyCardFour;
        gameEnemyController.enemyControllerCardRemaining[5] = newSaveData.savedEnemyCardFive;
        gameEnemyController.enemyControllerCardRemaining[6] = newSaveData.savedEnemyCardSix;

        gameEnemyCardRemaining = gameEnemyCardOne + gameEnemyCardTwo + gameEnemyCardThree + gameEnemyCardFour + gameEnemyCardFive + gameEnemyCardSix;
        gameEnemyController.cardsRemaining = gameEnemyCardRemaining;

        //Debug.Log(newSaveData.savedPlayerCardOne + ": " + newSaveData.savedPlayerCardTwo + ": " + newSaveData.savedPlayerCardThree
             //+ ": " + newSaveData.savedPlayerCardFour + ": " + newSaveData.savedPlayerCardFive + ": " + newSaveData.savedPlayerCardSix);

        Debug.Log("ENEMY: " + newSaveData.savedEnemyCardOne + ": " + newSaveData.savedEnemyCardTwo + ": " + newSaveData.savedEnemyCardThree
            + ": " + newSaveData.savedEnemyCardFour + ": " + newSaveData.savedEnemyCardFive + ": " + newSaveData.savedEnemyCardSix);

        return newSaveData;
        reader.Close();
    }

    public void deleteData()
    {
        string path = "/SaveData.txt";

        if (File.Exists(Application.persistentDataPath + path))
        {
            File.Delete(Application.persistentDataPath + path);
        }
        
    }

    public void loadData()
    {
        readString();

        gamePlayerController.playerCardTeamColor = gameSettings.chosenCardColor;
        gamePlayerController.playerCardOne = gamePlayerCardOne;
        gamePlayerController.playerCardTwo = gamePlayerCardTwo;
        gamePlayerController.playerCardThree = gamePlayerCardThree;
        gamePlayerController.playerCardFour = gamePlayerCardFour;
        gamePlayerController.playerCardFive = gamePlayerCardFive;
        gamePlayerController.playerCardSix = gamePlayerCardSix;

        gameEnemyController.enemyControllerCardRemaining[1] = gameEnemyCardOne;
        gameEnemyController.enemyControllerCardRemaining[2] = gameEnemyCardTwo;
        gameEnemyController.enemyControllerCardRemaining[3] = gameEnemyCardThree;
        gameEnemyController.enemyControllerCardRemaining[4] = gameEnemyCardFour;
        gameEnemyController.enemyControllerCardRemaining[5] = gameEnemyCardFive;
        gameEnemyController.enemyControllerCardRemaining[6] = gameEnemyCardSix;

        gamePlayerController.loadedDataCardInfo();
        gameEnemyController.enemyCardRemainer();
        gameEnemyController.remainingCardUpdater();
        removedCard.cardRemovedCheckLoaded();

        gameEnemyController.enemyDifficultyLevel = gameDifficultyData;

        gameEnemyController.cheatMovesRemaining = gameEnemyCheatsRemaining; //CHEAT

        gameSettings.GetComponent<GameSettings>().dataLoaded = false;
        //loadingData = true;
    }

    public void saveData()
    {
        gamePlayerController.saveDataCardInfo();

        SaveDataRef.savedEnemyCardOne = gameEnemyController.enemyControllerCardRemaining[1];
        SaveDataRef.savedEnemyCardTwo = gameEnemyController.enemyControllerCardRemaining[2];
        SaveDataRef.savedEnemyCardThree = gameEnemyController.enemyControllerCardRemaining[3];
        SaveDataRef.savedEnemyCardFour = gameEnemyController.enemyControllerCardRemaining[4];
        SaveDataRef.savedEnemyCardFive = gameEnemyController.enemyControllerCardRemaining[5];
        SaveDataRef.savedEnemyCardSix = gameEnemyController.enemyControllerCardRemaining[6];

        SaveDataRef.savedPlayerCardOne = gamePlayerController.playerCardOne;
        SaveDataRef.savedPlayerCardTwo = gamePlayerController.playerCardTwo;
        SaveDataRef.savedPlayerCardThree = gamePlayerController.playerCardThree;
        SaveDataRef.savedPlayerCardFour = gamePlayerController.playerCardFour;
        SaveDataRef.savedPlayerCardFive = gamePlayerController.playerCardFive;
        SaveDataRef.savedPlayerCardSix = gamePlayerController.playerCardSix;

        SaveDataRef.savedDifficultyData = gameDifficultyData;
        SaveDataRef.savedEnemyCheatsRemaining = gameEnemyController.cheatMovesRemaining; //CHEAT

        //Debug.Log(SaveDataRef.savedPlayerCardOne + ": " + SaveDataRef.savedPlayerCardTwo + ": " + SaveDataRef.savedPlayerCardThree
             //+ ": " + SaveDataRef.savedPlayerCardFour + ": " + SaveDataRef.savedPlayerCardFive + ": " + SaveDataRef.savedPlayerCardSix);

        string t = JsonUtility.ToJson(SaveDataRef);

        writeString(t);

        //savingData = true;
        //print("SAVED");
        //savingData = false;

        //ReadString();
    }

    public void soundOnOffSwitch()
    {
        if (soundToggle == 1)
        {
            gameAudio.transform.GetChild(0).gameObject.SetActive(false);
            gameAudio.transform.GetChild(1).gameObject.SetActive(false);
            audioButton.GetComponent<UnityEngine.UI.Image>().sprite = soundOnOff[0];
            gameSettings.soundOnOff = 0;
            soundToggle = 0;
        }

        else if (soundToggle == 0)
        {
            gameAudio.transform.GetChild(0).gameObject.SetActive(true);
            gameAudio.transform.GetChild(1).gameObject.SetActive(true);
            audioButton.GetComponent<UnityEngine.UI.Image>().sprite = soundOnOff[1];
            gameSettings.soundOnOff = 1;
            soundToggle = 1;
        }
    }

    public void burgerButtonsUIController(int mode)
    {
        if (mode == 0)
        {
            burgerButtonMode = 0;
        }

        else if (mode == 1)
        {
            burgerButtonMode = 1;
        }

        burgerButtons();
    }

    public void burgerButtons()//mode 0 open, mode 1 close
    {
        if (burgerButtonMode == 0)
        {
            burgerButton.transform.GetChild(0).gameObject.SetActive(true);

            burgerButtonMode = 1;
        }

        else if (burgerButtonMode == 1)
        {
            burgerButton.transform.GetChild(0).gameObject.SetActive(false);

            burgerButtonMode = 0;
        }
    }

    public void subButtons(int menu)
    {
        if (menu == 0)
        {
            //guidePanel.SetActive(true);
            guideOutline.SetActive(true);
            backButton.SetActive(true);
        }

        else if (menu == 1)
        {
            //deleteData();

            if (endgameButtons.gameObject.activeInHierarchy == false) //if not endgame
            {
                confirmationPanel.gameObject.SetActive(true);

                //burgerButton.transform.GetChild(0).gameObject.SetActive(false);
                //gameFinished(1);
            }

            else //if endgame
            {
                endgameButtons.SetActive(false);
                StartCoroutine(loadLevelAsync());
            }
        }

        else if (menu == 2)
        {
            if (endgameButtons.gameObject.activeInHierarchy == false) //if not endgame
            {
                saveData();
            }

            endgameButtons.SetActive(false);

            StartCoroutine(loadLevelAsync());
        }

        else if (menu == 3)
        {
            //invite friends
        }
    }

    public void backMenu()
    {
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

        //backButton.SetActive(false);
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

    public void changeStartRoundButtonGraphic(int setGraphic)
    {
        if (gamePlayerController.cardOnPlayList.Count < 3 && setGraphic == 0)
        {
            startRoundButton.GetComponent<UnityEngine.UI.Image>().sprite = cardLockGraphicArray[0];
        }

        else if (gamePlayerController.cardOnPlayList.Count == 3 && setGraphic == 0 && playerWins == false && enemyWins == false)
        {
            startRoundButton.GetComponent<UnityEngine.UI.Image>().sprite = cardLockGraphicArray[1];
        }

        else if (setGraphic == 2)
        {
            startRoundButton.GetComponent<UnityEngine.UI.Image>().sprite = cardLockGraphicArray[2];
        }
    }

    public void startEnemysTurn() //PROBLEM HERE
    {
        //print("startEnemysTurn");

        if (gamePlayerController.cardOnPlayList.Count == 3 && playerWins == false && enemyWins == false && roundDraw == false)
        {
            /*
            gameEnemyController.enemyPlayCard(0, 0);
            gameEnemyController.enemyPlayCard(1, 0);
            gameEnemyController.enemyPlayCard(2, 1);*/

            gameEnemyController.enemyPlayCard();
            //gameEnemyController.enemyPlayCard();

            calculateResult(0);
        }

        else if (gamePlayerController.cardOnPlayList.Count == 3 && enemyWins == true)
        {
            //print("CLICKED");
            removeOpponentCard(1);
        }

        else if (gamePlayerController.cardOnPlayList.Count == 3 && playerWins == true)
        {
            //print("thisRan");

            //gameEnemyController.cardOnPlayGridRefTransform.GetChild(3).gameObject.SetActive(false);


            if (gamePlayerController.currentlyClickedCard == null)
            {
                announcerText.SetText("TAKE CARD!");
                announcerText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
            }

            else if (gamePlayerController.currentlyClickedCard != null)
            {
                gameEnemyController.GetComponent<EnemyController>().enemyCardRemovalMode(0);
                gameEnemyController.GetComponent<EnemyController>().enemyCardRemovedCheck(0);
            }
        }

        else if (gamePlayerController.cardOnPlayList.Count == 3 && roundDraw == true)
        {
            print("DRAW new round");
            removeOpponentCard(2);
        }

        else
        {
            announcerText.SetText("ADD CARDS!");
            announcerText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 0, 0, 255);
        }
    }

    public void calculateResult(int mode)//0 player win, 1 enemy win
    {

        gamePlayerController.GetComponent<PlayerController>().cardSetup[0].GetComponent<UnityEngine.UI.Button>().interactable = false;
        gamePlayerController.GetComponent<PlayerController>().cardSetup[1].GetComponent<UnityEngine.UI.Button>().interactable = false;
        gamePlayerController.GetComponent<PlayerController>().cardSetup[2].GetComponent<UnityEngine.UI.Button>().interactable = false;

        if (gamePlayerController.cardColorCombo == 3 && gameEnemyController.cardColorCombo == 2)
        {
            setWinnerText(0);
            removeOpponentCard(0);
        }

        else if (gamePlayerController.cardColorCombo == 2 && gameEnemyController.cardColorCombo == 3)
        {
            markCardToRemove(1);
            setWinnerText(1);
            changeStartRoundButtonGraphic(2);
        }

        else if (gamePlayerController.cardColorCombo == 2 && gameEnemyController.cardColorCombo == 1)
        {
            setWinnerText(0);
            removeOpponentCard(0);
        }

        else if (gamePlayerController.cardColorCombo == 1 && gameEnemyController.cardColorCombo == 2)
        {
            markCardToRemove(1);
            setWinnerText(1);
            changeStartRoundButtonGraphic(2);
        }

        else if (gamePlayerController.cardColorCombo == 1 && gameEnemyController.cardColorCombo == 3)
        {
            setWinnerText(0);
            removeOpponentCard(0);
        }

        else if (gamePlayerController.cardColorCombo == 3 && gameEnemyController.cardColorCombo == 1)
        {
            markCardToRemove(1);
            setWinnerText(1);
            changeStartRoundButtonGraphic(2);
        }

        else if (gamePlayerController.cardColorCombo == gameEnemyController.cardColorCombo) //SAME COLOR COMBO
        {
            if (gamePlayerController.cardPointTotal > gameEnemyController.cardPointTotal)
            {
                setWinnerText(0);
                removeOpponentCard(0);
            }

            else if (gamePlayerController.cardPointTotal < gameEnemyController.cardPointTotal)
            {
                markCardToRemove(1);
                setWinnerText(1);
                changeStartRoundButtonGraphic(2);
            }

            else if (gamePlayerController.cardPointTotal == gameEnemyController.cardPointTotal) //SAME COMBO AND TOTAL
            {
                setWinnerText(2);
                changeStartRoundButtonGraphic(2);
            }
        }

    }

    private void setWinnerText(int mode)//0 player, 1 enemy, 2 draw
    {
        if (mode == 0)
        {
            announcerText.SetText("PLAYER WINS");
            playerWins = true;

            gameSFXAudioSource.PlayOneShot(roundWinSFX, 1.25f);

            if (gamePlayerController.playerCardTeamColor == 0)
                announcerText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 165, 0, 255);

            else if (gamePlayerController.playerCardTeamColor == 1)
                announcerText.GetComponent<TextMeshProUGUI>().color = new Color32(165, 0, 255, 255);

            gameEnemyController.enemyLoseStreak++; //Times of enemy losses in a row before cheating
        }

        else if (mode == 1)
        {
            announcerText.SetText("ENEMY WINS");
            enemyWins = true;

            gameSFXAudioSource.PlayOneShot(roundLoseDrawSFX, 1.25f);

            if (gameEnemyController.enemyCardTeamColor == 0)
                announcerText.GetComponent<TextMeshProUGUI>().color = new Color32(255, 165, 0, 255);

            else if (gameEnemyController.enemyCardTeamColor == 1)
                announcerText.GetComponent<TextMeshProUGUI>().color = new Color32(165, 0, 255, 255);

            gameEnemyController.enemyLoseStreak = 0; //Reset
        }

        else if (mode == 2)
        {
            roundDraw = true;

            gameSFXAudioSource.PlayOneShot(roundLoseDrawSFX, 1.5f);

            announcerText.SetText("DRAW");
            announcerText.GetComponent<TextMeshProUGUI>().color = new Color32(0, 165, 0, 255);
        }
    }

    private void markCardToRemove(int mode)
    {
        if (mode == 1)
        {
            if (gameDifficultyData >= 2)
            {
                //int markHighestCard = 0;
                highestCardToRemove = 0;

                if (gamePlayerController.cardSetup[0].GetComponent<PlayerCard>().playerCardNo
                    >= gamePlayerController.cardSetup[1].GetComponent<PlayerCard>().playerCardNo
                    && gamePlayerController.cardSetup[0].GetComponent<PlayerCard>().playerCardNo
                    >= gamePlayerController.cardSetup[2].GetComponent<PlayerCard>().playerCardNo)
                {
                    highestCardToRemove = 0;
                }

                else if (gamePlayerController.cardSetup[1].GetComponent<PlayerCard>().playerCardNo
                    >= gamePlayerController.cardSetup[0].GetComponent<PlayerCard>().playerCardNo
                    && gamePlayerController.cardSetup[1].GetComponent<PlayerCard>().playerCardNo
                    >= gamePlayerController.cardSetup[2].GetComponent<PlayerCard>().playerCardNo)
                {
                    highestCardToRemove = 1;
                }

                else if (gamePlayerController.cardSetup[2].GetComponent<PlayerCard>().playerCardNo
                    >= gamePlayerController.cardSetup[0].GetComponent<PlayerCard>().playerCardNo
                    && gamePlayerController.cardSetup[2].GetComponent<PlayerCard>().playerCardNo
                    >= gamePlayerController.cardSetup[1].GetComponent<PlayerCard>().playerCardNo)
                {
                    highestCardToRemove = 2;
                }

                gamePlayerController.cardSetup[highestCardToRemove].transform.GetChild(3).gameObject.SetActive(true);
            }

            else if (gameDifficultyData == 1)
            {
                //int markHighestCard = 0;
                randomCardToRemove = UnityEngine.Random.Range(0, 3);

                gamePlayerController.cardSetup[randomCardToRemove].transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }

    private void removeOpponentCard(int mode)//0 playerwins, 1 enemywins
    {
        changeStartRoundButtonGraphic(2);

        if (mode == 0)
        {
            for (int i = 0; i < gameEnemyController.cardSetup.Length; i++)
            {
                gameEnemyController.cardSetup[i].GetComponent<EnemyCard>().enemyCardToRemove = true;
            }

            gameEnemyController.cardOnPlayGridRefTransform.GetChild(3).gameObject.SetActive(true);

            gamePlayerController.playerRemoveEnemyCard();

        }

        else if (mode == 1)
        {
            for (int i = 0; i < gameEnemyController.cardSetup.Length; i++)
            {
                gamePlayerController.cardSetup[i].GetComponent<PlayerCard>().playerCardToRemove = true;
            }

            //intRandomizer = Random.Range(0, 3);
            //gamePlayerController.transform.GetChild(3).gameObject.SetActive(true);

            //gameEnemyController.removeCardRandomizer = intRandomizer;
            gamePlayerController.cardSetup[0].transform.GetChild(3).gameObject.SetActive(false);
            gamePlayerController.cardSetup[1].transform.GetChild(3).gameObject.SetActive(false);
            gamePlayerController.cardSetup[2].transform.GetChild(3).gameObject.SetActive(false);

            gameEnemyController.enemyRemovePlayerCard(0);
        }

        else if (mode == 2)
        {
            gamePlayerController.playerRemoveEnemyCard();
            gameEnemyController.enemyRemovePlayerCard(1);
            newRound(0);
        }

    }

    public void newRound(int phase)
    {
        //print("NEW ROUND");
        changeStartRoundButtonGraphic(0);
        gameSFXAudioSource.PlayOneShot(removeCardSFX);

        gamePlayerController.GetComponent<PlayerController>().cardSetup[0].GetComponent<UnityEngine.UI.Button>().interactable = true;
        gamePlayerController.GetComponent<PlayerController>().cardSetup[1].GetComponent<UnityEngine.UI.Button>().interactable = true;
        gamePlayerController.GetComponent<PlayerController>().cardSetup[2].GetComponent<UnityEngine.UI.Button>().interactable = true;

        for (int i = 0; i < removedCard.removedCardGameObjects.Length; i++)
        {
            if (removedCard.removedCardGameObjects[i].transform.GetChild(3).gameObject.activeInHierarchy == true)
                removedCard.removedCardGameObjects[i].transform.GetChild(3).gameObject.SetActive(false);
        }

        if (phase == 0)
        {
            if (playerWins == true)
            {
                gameEnemyController.GetComponent<EnemyController>().enemyCardRemovedCheck(1);
                gamePlayerController.GetComponent<PlayerController>().playerCardRemovedCheck(1);
                playerWins = false;

            }

            else if (enemyWins == true)
            {
                gameEnemyController.GetComponent<EnemyController>().enemyCardRemovedCheck(1);
                gamePlayerController.GetComponent<PlayerController>().playerCardRemovedCheck(1);
                enemyWins = false;
            }

            else
            {
                gameEnemyController.GetComponent<EnemyController>().enemyCardRemovedCheck(1);
                gamePlayerController.GetComponent<PlayerController>().playerCardRemovedCheck(1);
                roundDraw = false;
            }
        }

        if (gamePlayerController.cardsRemaining < 3)//enemy wins
        {
            print("EnemyWins");
            gameFinished(1);
        }

        else if (gameEnemyController.cardsRemaining < 3)//player wins
        {
            print("PlayerWins");
            gameFinished(0);
        }
    }

    public void gameFinished(int mode)//0 player win, 1 enemy win
    {
        deleteData();
        endgameButtons.SetActive(true);

        if (mode == 0)
        {
            gameAudio.transform.GetChild(0).GetComponent<AudioSource>().clip = gameFinishedMusic[0];
            gameAudio.transform.GetChild(0).GetComponent<AudioSource>().enabled = false;
            gameAudio.transform.GetChild(0).GetComponent<AudioSource>().enabled = true;

            endgameButtons.transform.GetChild(0).transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = winLosePrompt[0];
        }

        else if (mode == 1)
        {
            gameAudio.transform.GetChild(0).GetComponent<AudioSource>().clip = gameFinishedMusic[1];
            gameAudio.transform.GetChild(0).GetComponent<AudioSource>().enabled = false;
            gameAudio.transform.GetChild(0).GetComponent<AudioSource>().enabled = true;

            endgameButtons.transform.GetChild(0).transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = winLosePrompt[1];
        }


    }
}
