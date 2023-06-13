using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPool : MonoBehaviour
{
    //public ScriptableCharacterData SOCustomizationData;
    [SerializeField] private ScriptableAllCards allCardList;
    [SerializeField] public ScriptableCardBGFG allCardTeamColorList;

    //public string poolCardColor;

    public GameObject gameSettings;
    public GameController gameController;

    private int cardCount;
    public int maxCard = 20;
    public int loadedRemainingCard;

    [Header("Player Side")]
    [SerializeField] private GameObject playerCardRef;
    private PlayerCard PPlayerCardRefShort;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject playerCardOnPlayGridRef;
    [SerializeField] private GameObject playerCardGridRef;

    [Header("Enemy Side")]
    [SerializeField] private GameObject enemyCardRef;
    private EnemyCard EEnemyCardRefShort;
    [SerializeField] private int enemyTeamColorRandomizer;

    [SerializeField] private EnemyController enemyController;
    [SerializeField] private GameObject enemyCardOnPlayGridRef;
    [SerializeField] private GameObject enemyCardGridRef;

    public List<GameObject> defaultCardList = new List<GameObject>();
    public List<GameObject> playerAvailableCardList = new List<GameObject>();

    public List<GameObject> enemyAvailableCardList = new List<GameObject>();


    // Start is called before the first frame update
    void Awake()
    {
        //GetThenSetCardColor
        gameSettings = GameObject.FindGameObjectWithTag("GameSettings");
        enemyController.gameSettings = gameSettings;

        if (gameSettings.GetComponent<GameSettings>().chosenCardColor == 2)
            gameSettings.GetComponent<GameSettings>().chosenCardColor = Random.Range(0, 2);

        playerController.playerCardTeamColor = gameSettings.GetComponent<GameSettings>().chosenCardColor;

        PPlayerCardRefShort = playerCardRef.GetComponent<PlayerCard>();
        EEnemyCardRefShort = enemyCardRef.GetComponent<EnemyCard>();

        playerController.cardBGFGTeamColorList.Add(allCardTeamColorList.cardBackgroundList[playerController.playerCardTeamColor]);
        playerController.cardBGFGTeamColorList.Add(allCardTeamColorList.cardForegroundList[playerController.playerCardTeamColor]);

        if (playerController.playerCardTeamColor == 0)
            enemyController.enemyCardTeamColor = 1;

        else if (playerController.playerCardTeamColor == 1)
            enemyController.enemyCardTeamColor = 0;
        

        enemyController.cardBGFGTeamColorList.Add(allCardTeamColorList.cardBackgroundList[enemyController.enemyCardTeamColor]);
        enemyController.cardBGFGTeamColorList.Add(allCardTeamColorList.cardForegroundList[enemyController.enemyCardTeamColor]);
        
        
        cardSpawner();
        EEnemyCardReferences();
    }

    private void PPlayerCardReferences(int value, int list)
    {
        //print("Add" + value); 
        PPlayerCardRefShort.Player = gameObject;
        PPlayerCardRefShort.playerCardNo = value;
        PPlayerCardRefShort.playerCardPlayerController = playerController;

        PPlayerCardRefShort.playerCardBackground = allCardTeamColorList.cardBackgroundList[playerController.playerCardTeamColor];
        PPlayerCardRefShort.playerCardForeground = allCardTeamColorList.cardForegroundList[playerController.playerCardTeamColor];
        PPlayerCardRefShort.playerCardNumberGraphic = allCardList.scriptableObjectIndividualCard[value].scriptableCardGraphic;

        /*
        PPlayerCardRefShort.playerCardBackground = allCardTeamColorList.cardBackgroundList[playerController.playerCardTeamColor];
        PPlayerCardRefShort.playerCardForeground = allCardTeamColorList.cardForegroundList[playerController.playerCardTeamColor];
        */


        //Color Adder//
        if (value == 1 || value == 2)
        {
            PPlayerCardRefShort.playerCardColor = "G";
            //EEnemyCardRefShort.enemyCardColor = "G";
        }

        else if (value == 3 || value == 4)
        {
            PPlayerCardRefShort.playerCardColor = "B";
            //EEnemyCardRefShort.enemyCardColor = "B";
        }

        else if (value == 5 || value == 6)
        {
            PPlayerCardRefShort.playerCardColor = "R";
            //EEnemyCardRefShort.enemyCardColor = "R";
        }

        GameObject playerCards = Instantiate(playerCardRef, playerCardGridRef.transform);

        //All Cards List WITHOUT COPIES//
        if (list == 0)
        {
            defaultCardList.Add(playerCards);
            playerCards.SetActive(false);

            //enemyCards.SetActive(false);
        }

        //All Cards List INCLUDING COPIES//
        else if (list == 1)
        {
            playerAvailableCardList.Add(playerCards);
        }

        if (cardCount == maxCard)
            value = 0;
    }

    private void EEnemyCardReferences()
    {
        if (playerController.playerCardTeamColor == 0)
        {
            enemyController.enemyCardTeamColor = 1;
        }

        else if (playerController.playerCardTeamColor == 0)
        {
            enemyController.enemyCardTeamColor = 0;
        }


        for (int i = 1; i <= 6; i++)
        {
            EEnemyCardRefShort.enemyCardNo = i;
            EEnemyCardRefShort.Enemy = gameObject;
            EEnemyCardRefShort.enemyCardEnemyController = enemyController;
            EEnemyCardRefShort.enemyCardNumberGraphic = allCardList.scriptableObjectIndividualCard[i].scriptableCardGraphic;

            EEnemyCardRefShort.enemyCardBackground = allCardTeamColorList.cardBackgroundList[playerController.playerCardTeamColor];
            EEnemyCardRefShort.enemyCardForeground = allCardTeamColorList.cardForegroundList[playerController.playerCardTeamColor];
            EEnemyCardRefShort.enemyCardNumberGraphic = allCardList.scriptableObjectIndividualCard[i].scriptableCardGraphic;

            if (i == 1 || i == 2)
                EEnemyCardRefShort.enemyCardColor = "G";

            else if (i == 3 || i == 4)
                EEnemyCardRefShort.enemyCardColor = "B";

            else if (i == 5 || i == 6)
                EEnemyCardRefShort.enemyCardColor = "R";

            EEnemyCardRefShort.enemyCardBackground = allCardTeamColorList.cardBackgroundList[enemyController.enemyCardTeamColor];
            EEnemyCardRefShort.enemyCardForeground = allCardTeamColorList.cardForegroundList[enemyController.enemyCardTeamColor];

            GameObject enemyCards = Instantiate(enemyCardRef, enemyCardGridRef.transform);
            enemyAvailableCardList.Add(enemyCards);
        }
    }


    private void cardSpawner()
    {

        for (cardCount = 0; cardCount < maxCard; cardCount++)
        {
            if (cardCount < 5)
            {
                PPlayerCardReferences(1, 1);
                continue;
            }

            if (cardCount < 10)
            {
                PPlayerCardReferences(2, 1);
                continue;
            }

            if (cardCount < 14)
            {
                PPlayerCardReferences(3, 1);
                continue;
            }

            if (cardCount < 15)
            {
                PPlayerCardReferences(6, 1);
                continue;
            }

            if (cardCount < 18)
            {
                PPlayerCardReferences(4, 1);
                continue;
            }

            if (cardCount < 20)
            {
                PPlayerCardReferences(5, 1);
                continue;
            }
        }


        for (int i = 1; i <= 6; i++)
        {
            PPlayerCardReferences(i, 0);
        }
    }

    private void cardDestroyer()
    {
        for (cardCount = 0; cardCount < maxCard; cardCount++)
        {
            playerAvailableCardList[cardCount].SetActive(false);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
