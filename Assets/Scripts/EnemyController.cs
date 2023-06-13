using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [Header("Technical References")]
    public GameController enemyGameController;
    public GameObject Player;
    public GameObject cardPrefabRef;
    private EnemyCard enemyCardRefShort;
    public GameObject gameSettings;
    

    public GameObject cardOnPlayGridRef;
    public Transform cardOnPlayGridRefTransform;

    public CardPool enemyCardPool;

    [SerializeField] private RemovedCard removedCard;

    [Header("TMPro References")]
    [SerializeField] private TextMeshProUGUI cardsRemainingText;
    [SerializeField] private TextMeshProUGUI cardColorComboText;
    [SerializeField] private TextMeshProUGUI cardPointTotalText;

    [Header("Numbers and Counters")]
    [SerializeField] private bool firstTurn;
    public int enemyDifficultyLevel;
    [SerializeField] public int cheatMovesRemaining;
    [SerializeField] public int enemyLoseStreak;
    public int enemyCardTeamColor;//0 orange, 1 purple
    private int maxCard;
    [SerializeField] private int setCards;
    [SerializeField] public int cardsRemaining;
    private int cardColorGreen;
    private int cardColorBlue;
    private int cardColorRed;
    public int cardColorCombo;
    public int cardPointTotal;

    [Header("Checker")]
    public GameObject currentlyClickedCard;
    [SerializeField] private int cardRandomizer;
    [SerializeField] private int nextRandomizer;
    [SerializeField] private int cardComboRandomizer;
    [SerializeField] private int cardRandomizerPrevious;

    [Header("Audio")]
    [SerializeField] public AudioSource enemySFXAudioSource;
    [SerializeField] private AudioClip addCardSFX;
    [SerializeField] private AudioClip removeCardSFX;

    [Header("Arrays and Lists")]
    [SerializeField] public int[] enemyControllerCardRemaining;
    [SerializeField] public GameObject[] cardSetup;
    [SerializeField] public List<Sprite> cardBGFGTeamColorList = new List<Sprite>();
    [SerializeField] private List<GameObject> cardOnPlayList = new List<GameObject>();
    [SerializeField] private List<GameObject> enemyCardList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {/*
        enemyCardShow(false);

        if (gameSettings.GetComponent<GameSettings>().chosenLevel == 3)
            cheatMovesRemaining = 3;

        else if (gameSettings.GetComponent<GameSettings>().chosenLevel == 4)
            cheatMovesRemaining = 6;*/
    }

    void Update()
    {
        ///remainingCardUpdater();

        
    }

    private void enemyCardShow(bool mode)//true show, false hide
    {
        if (mode == true)
        {
            cardSetup[0].transform.GetChild(1).gameObject.SetActive(true);
            cardSetup[1].transform.GetChild(1).gameObject.SetActive(true);
            cardSetup[2].transform.GetChild(1).gameObject.SetActive(true);
        }

        else if (mode == false)
        {
            cardSetup[0].transform.GetChild(1).gameObject.SetActive(false);
            cardSetup[1].transform.GetChild(1).gameObject.SetActive(false);
            cardSetup[2].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void enemyInitialSetup()
    {

        enemyCardShow(false);

        /*
        cardOne5 = 5;
        cardTwo5 = 5;
        cardThree4 = 4;
        cardFour3 = 3;
        cardFive2 = 2;
        cardSix1 = 1;
        */

        enemyCardList = enemyCardPool.GetComponent<CardPool>().enemyAvailableCardList;

        for (int i = 0; i < cardSetup.Length; i++)
        {
            for (int l = 0; l < cardSetup[i].transform.childCount - 3; l++)//MODIFIED 1.2
            {
                cardSetup[i].transform.GetChild(l).GetComponent<UnityEngine.UI.Image>().sprite = cardBGFGTeamColorList[l];
            }

            if (cardSetup[i].GetComponent<EnemyCard>().enemyCardNo == 0)
                cardSetup[i].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = false;
        }

        if (gameSettings.GetComponent<GameSettings>().dataLoaded == false || System.IO.File.Exists(Application.persistentDataPath + "/SaveData.txt") == false)
        {
            enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining = 5;
            enemyCardList[1].GetComponent<EnemyCard>().thisCardRemaining = 5;
            enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining = 4;
            enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining = 3;
            enemyCardList[4].GetComponent<EnemyCard>().thisCardRemaining = 2;
            enemyCardList[5].GetComponent<EnemyCard>().thisCardRemaining = 1;
        }

        remainingCardUpdater();

        cardsRemainingText.SetText(cardsRemaining.ToString());

        

        firstTurn = true;

        //SET DIFFICULTY LEVEL
        enemyDifficultyLevel = gameSettings.GetComponent<GameSettings>().chosenLevel;

        //CHEAT MOVES
        if (enemyDifficultyLevel == 3)
            cheatMovesRemaining = 8;

        else if (enemyDifficultyLevel == 4)
            cheatMovesRemaining = 12;

    }

    public void cardSetupper(int setupNo)
    {
        //print("this ran " + setupNo);
        cardSetup[setupNo].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = cardBGFGTeamColorList[0];
        cardSetup[setupNo].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = cardBGFGTeamColorList[1];

        if (cardSetup[setupNo].GetComponent<EnemyCard>().enemyCardNo == 0)
            cardSetup[setupNo].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = false;
    }

    public void enemyCardRemainer()
    {
        for (int l = 0; l < enemyCardList.Count; l++)
        {
            enemyCardList[l].GetComponent<EnemyCard>().thisCardRemaining = enemyControllerCardRemaining[l + 1];
        }
    }

    public void remainingCardUpdater()
    {
        cardsRemaining = enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining + enemyCardList[1].GetComponent<EnemyCard>().thisCardRemaining
                            + enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining + enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining
                                + enemyCardList[4].GetComponent<EnemyCard>().thisCardRemaining + enemyCardList[5].GetComponent<EnemyCard>().thisCardRemaining;

        //print("ENEMY : " + cardsRemaining);
        cardsRemainingText.SetText(cardsRemaining.ToString());
        removedCard.removedCardSetRemainingCard();
    }

    //public void enemyPlayCard(int index, int mode)
    public void enemyPlayCard()//0 play card only, 1 && and return
    {
        /*
         * Level 1 : komputer bermain secara asal2an, tidak diprogram (mengaluarkan kartu secara random)

           Level 2 : komputer melakukan strategi sbb,
           -menjaga agar selalu ada 3 warna)
           -selalu mengeluarkan angka kecil terlebih dahulu 
           -bila mengeluarkan 1 warna, urutan prioritasnya : 111 (H), 112 (H), 122 (H), 222 (H), 333 (B), 334 (B), 344 (B), 444 (H). Sedapat mungkin jangan 556 (M)
           -bila mengeluarkan 2 warna, gunakan hijau dan biru. Urutan prioritasnya : 113, 114, 123, 124, 223, 224, 133, 134, 144, 233, 234, 244. Hindari menggunakan merah
           -bila mengeluarkan 3 warna, gunakan merah yg kecil dulu (5). Urutan prioritasnya : 135, 145, 235, 245, 136, 146, 236, 246
           -melihat permainan lawan, bila telah mengeluarkan jumlah warna sama 2x berturut2, diasumsikan pd babak berikutnya lawan tidak mengeluarkan jumlah warna yg sama lagi. Bila sebelumnya 1 dan 1, berikutnya akan 2 atau 3. Maka dipilih warna yg bisa mengalahkan 2 dan 3, yaitu 1 dan 2. 
           -saat "memakan" kartu lawan, pilih angka terbesar
           -saat mulai permainan, keluarkan kartu dari 1 atau 2 warna

           Level 3 : sama seperti level 2, tapi ada tambahan
           -sekali2 komputernya "curang", yaitu tahu kartu yg dikeluarkan lawan, lalu mengeluarkan kartu yg bisa mengalahkan lawan. Dalam satu permainan tindakan "curang" dilakukan 3x

           Level 4 : sama seperti level 2, tapi tindakan curang 6x
        */

        if (enemyDifficultyLevel == 1) //LEVEL 1
        {
            for (int index = 0; index < 3; index++)
            {
                cardRandomizer = UnityEngine.Random.Range(0, 6);

                if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                {
                    for (int k = 0; k <= 5; k++)
                    {
                        cardRandomizer = k;

                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                            break;
                    }
                }

                if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                {
                    enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                print("Index: " + index + " Randomizer: " + cardRandomizer + " Remain: " + enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining);
            }
        }

        else if (enemyDifficultyLevel >= 2) //LEVEL 2 3 4
        {
            if (firstTurn == true)
                cardComboRandomizer = UnityEngine.Random.Range(1, 3); //print("FIRST TURN");

            else if (firstTurn == false && cardsRemaining > 10)
                cardComboRandomizer = UnityEngine.Random.Range(1, 4);

            else if (firstTurn == false && cardsRemaining <= 10)
                cardComboRandomizer = UnityEngine.Random.Range(1, 4);

            int useCheat = UnityEngine.Random.Range(0, 3);
            
            /*
            if (enemyDifficultyLevel == 3)
                useCheat = UnityEngine.Random.Range(0, 3);

            else if (enemyDifficultyLevel == 4)
                useCheat = UnityEngine.Random.Range(0, 2);
            */

            if ((firstTurn == false && cheatMovesRemaining > 0 && useCheat == 0) || (enemyDifficultyLevel == 3 && enemyLoseStreak >= 2) || (enemyDifficultyLevel == 4 && enemyLoseStreak >= 1))
            //if ((firstTurn == false && cheatMovesRemaining > 0 && useCheat == 0) || enemyLoseStreak >= 2)
            {
                print("PLAYERCARDCOMBO" + Player.GetComponent<PlayerController>().cardColorCombo);
                switch (Player.GetComponent<PlayerController>().cardColorCombo)
                {
                    case 1: //ALL DIFF
                        cardComboRandomizer = 2; //COMBO 2
                        break;
                    case 2:
                        cardComboRandomizer = 1; //COMBO 1
                        break;
                    case 3: //ALL SAME
                        cardComboRandomizer = 3; //COMBO 3
                        break;
                }
                
                if ((enemyDifficultyLevel == 3 && enemyLoseStreak >= 2) || (enemyDifficultyLevel == 4 && enemyLoseStreak >= 1))
                    print("Lose Streak Free Cheat Used Level " + enemyDifficultyLevel);

                //if (enemyLoseStreak >= 2)
                //    print("Lose Streak Free Cheat Used Level " + enemyDifficultyLevel);

                else if (useCheat == 0)
                {
                    cheatMovesRemaining--;
                    print("Cheat Used, Remaining : " + cheatMovesRemaining);
                }
            }

            print("cardComboRandomize: " + cardComboRandomizer);

            if (cardComboRandomizer == 1) //ONE COLORS
            {
                for (int index = 0; index < 3; index++)
                {
                    if (enemyControllerCardRemaining[1] + enemyControllerCardRemaining[2] > 2)
                    {
                        int number = 0;

                        if (enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                number = i;

                                if (enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining > 0)
                                    break;
                            }
                        }

                        if (enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining > 0)
                        {
                            enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                            cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                            cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        }
                    }

                    else if (enemyControllerCardRemaining[1] + enemyControllerCardRemaining[2] <= 2
                        && (enemyControllerCardRemaining[3] + enemyControllerCardRemaining[4] > 2))
                    {
                        int number = 2;

                        if (enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                        {
                            for (int i = 2; i < 4; i++)
                            {
                                number = i;

                                if (enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining > 0)
                                    break;
                            }
                        }

                        if (enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining > 0)
                        {
                            enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                            cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                            cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        }
                    }

                    /*
                    else if (enemyControllerCardRemaining[2] + enemyControllerCardRemaining[3] <= 2
                        && (enemyControllerCardRemaining[4] + enemyControllerCardRemaining[5] > 2))
                    {
                        int number = UnityEngine.Random.Range(4, 6);

                        if (enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining > 0)
                        {
                            enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                            cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                            cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        }
                    }*/

                    /*
                    if (enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining > 1)
                    {
                        int number = 0;

                        enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }

                    else if (enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining < 2
                        && enemyCardList[1].GetComponent<EnemyCard>().thisCardRemaining > 1)
                    {
                        int number = 1;

                        enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }

                    else if (enemyCardList[1].GetComponent<EnemyCard>().thisCardRemaining < 4
                        && enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 2)
                    {
                        int number = 2;

                        enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }

                    else if (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining <= 3
                       && enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 2)
                    {
                        int number = 3;

                        enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }

                    else if (enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining <= 3
                       && enemyCardList[4].GetComponent<EnemyCard>().thisCardRemaining > 0)
                    {
                        int number = 4;

                        enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }

                    else if (enemyCardList[4].GetComponent<EnemyCard>().thisCardRemaining <= 0
                       && enemyCardList[5].GetComponent<EnemyCard>().thisCardRemaining > 0)
                    {
                        int number = 5;

                        enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }*/

                    else
                    {
                        print("Combo One Randomed");

                        cardRandomizer = UnityEngine.Random.Range(0, 4);

                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                cardRandomizer = i;

                                if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                                    break;
                            }
                        }

                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                        {
                            enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining--;

                            cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNo;
                            cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardColor;

                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        }
                    }
                }
            }

            else if (cardComboRandomizer == 2) //TWO COLORS
            {

                if (enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining > 1 && (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 0 
                    || enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 0)) //TWO CARD 1
                {
                    int number = 0;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[0].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[0].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[0].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[0].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                    //TWICE
                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[1].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[1].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    //TWICE

                    if (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 0)
                    {
                        int numberNext = 2;

                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[2].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[2].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }

                    else if (enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 0)
                    {
                        int numberNext = 3;

                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[2].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[2].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }
                }

                else if (enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining > 0 && (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 1 || enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 1)) //ONE CARD 1
                {
                    int number = 0;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[0].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[0].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[0].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[0].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                    if (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 1)
                    {
                        int numberNext = 2;

                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[1].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[1].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                        //TWICE
                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[2].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[2].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        //TWICE
                    }

                    else if (enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 1)
                    {
                        int numberNext = 3;

                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[1].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[1].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                        //TWICE
                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[2].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[2].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        //TWICE
                    }
                }

                else if (enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining <= 0 && enemyCardList[1].GetComponent<EnemyCard>().thisCardRemaining > 1 && (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 0 || enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 0)) //TWO CARD 2
                {
                    int number = 1;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[0].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[0].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[0].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[0].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                    //TWICE
                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[1].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[1].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    //TWICE

                    if (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 0)
                    {
                        int numberNext = 2;

                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[2].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[2].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }

                    else if (enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 0)
                    {
                        int numberNext = 3;

                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[2].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[2].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }
                }

                else if (enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining <= 0 && enemyCardList[1].GetComponent<EnemyCard>().thisCardRemaining > 0 && (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 1 || enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 1)) //ONE CARD 2
                {
                    int number = 1;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[0].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[0].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[0].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[0].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                    if (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 1)
                    {
                        int numberNext = 2;

                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[1].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[1].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                        //TWICE
                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[2].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[2].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        //TWICE
                    }

                    else if (enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 1)
                    {
                        int numberNext = 3;

                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[1].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[1].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[1].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                        //TWICE
                        enemyCardList[numberNext].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[2].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[2].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[numberNext].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[2].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        //TWICE
                    }
                }

                else
                {
                    print("Combo Two Randomed");

                    for (int index = 0; index < 3; index++)
                    {
                        cardRandomizer = UnityEngine.Random.Range(0, 6);

                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                        {
                            for (int i = 0; i <= 5; i++)
                            {
                                cardRandomizer = i;

                                if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                                    break;
                            }
                        }

                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                        {
                            enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining--;

                            cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNo;
                            cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardColor;

                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                        }
                    }
                }
            }


            else if (cardComboRandomizer == 3) //THREE COLORS
            {

                if (enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining > 0)
                {
                    int number = 0;
                    int index = 0;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                else if (enemyCardList[0].GetComponent<EnemyCard>().thisCardRemaining <= 0 && enemyCardList[1].GetComponent<EnemyCard>().thisCardRemaining > 0)
                {
                    int number = 1;
                    int index = 0;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                else
                {
                    int index = 0;

                    cardRandomizer = UnityEngine.Random.Range(0, 6);

                    if (cardSetup[index].GetComponent<EnemyCard>().enemyCardNo <= 0 || cardSetup[index].GetComponent<EnemyCard>().enemyCardColor == null)
                    {
                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                        {
                            for (int i = 0; i <= 5; i++)
                            {
                                cardRandomizer = i;

                                if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                                    break;
                            }
                        }

                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                        {
                            enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining--;

                            cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNo;
                            cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardColor;

                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                            print("Combo Three Randomed Index: " + index);
                        }
                    }
                }

                if (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining > 0)
                {
                    int number = 2;
                    int index = 1;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                else if (enemyCardList[2].GetComponent<EnemyCard>().thisCardRemaining <= 0 && enemyCardList[3].GetComponent<EnemyCard>().thisCardRemaining > 0)
                {
                    int number = 3;
                    int index = 1;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                else
                {
                    int index = 1;

                    cardRandomizer = UnityEngine.Random.Range(0, 6);

                    if (cardSetup[index].GetComponent<EnemyCard>().enemyCardNo <= 0 || cardSetup[index].GetComponent<EnemyCard>().enemyCardColor == null)
                    {
                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                        {
                            for (int i = 0; i <= 5; i++)
                            {
                                cardRandomizer = i;

                                if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                                    break;
                            }
                        }

                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                        {
                            enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining--;

                            cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNo;
                            cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardColor;

                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                            print("Combo Three Randomed Index: " + index);
                        }
                    }
                }

                if (enemyCardList[4].GetComponent<EnemyCard>().thisCardRemaining > 0)
                {
                    int number = 4;
                    int index = 2;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                else if (enemyCardList[4].GetComponent<EnemyCard>().thisCardRemaining <= 0 && enemyCardList[5].GetComponent<EnemyCard>().thisCardRemaining > 0)
                {
                    int number = 5;
                    int index = 2;

                    enemyCardList[number].GetComponent<EnemyCard>().thisCardRemaining--;

                    cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNo;
                    cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[number].GetComponent<EnemyCard>().enemyCardColor;

                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[number].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                    cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                }

                else
                {
                    int index = 2;

                    cardRandomizer = UnityEngine.Random.Range(0, 6);

                    if (cardSetup[index].GetComponent<EnemyCard>().enemyCardNo <= 0 || cardSetup[index].GetComponent<EnemyCard>().enemyCardColor == null)
                    {
                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                        {
                            for (int i = 0; i <= 5; i++)
                            {
                                cardRandomizer = i;

                                if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                                    break;
                            }
                        }

                        if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                        {
                            enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining--;

                            cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNo;
                            cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardColor;

                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                            cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                            print("Combo Three Randomed Index: " + index);
                        }
                    }
                }
            }

            else //FAILSAFE
            {
                for (int index = 0; index < 3; index++)
                {
                    cardRandomizer = UnityEngine.Random.Range(0, 6);

                    if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining <= 0)
                    {
                        for (int k = 0; k <= 5; k++)
                        {
                            cardRandomizer = k;

                            if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                                break;
                        }
                    }

                    if (enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining > 0)
                    {
                        enemyCardList[cardRandomizer].GetComponent<EnemyCard>().thisCardRemaining--;

                        cardSetup[index].GetComponent<EnemyCard>().enemyCardNo = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNo;
                        cardSetup[index].GetComponent<EnemyCard>().enemyCardColor = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardColor;

                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardList[cardRandomizer].GetComponent<EnemyCard>().enemyCardNumberGraphic;
                        cardSetup[index].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;
                    }
                }
            }
        }

        else if (enemyDifficultyLevel < 1 || enemyDifficultyLevel > 4)
        {
            print("DIFFICULTY LEVEL NOT FOUND");
        }
            

        cardPointTotaler(0);
        cardColorComparer(0);
        //Debug.Log("Maximum number of cards is already on play.");
        
        enemyCardShow(true);
    }

    public void cardColorComparer(int mode)//0 start, 1 reset
    {
        if (mode == 0)
        {
            for (int t = 0; t < cardSetup.Length; t++)
            {
                if (cardSetup[t].GetComponent<EnemyCard>().enemyCardColor == "G")
                {
                    cardColorGreen++;
                }

                if (cardSetup[t].GetComponent<EnemyCard>().enemyCardColor == "B")
                {
                    cardColorBlue++;
                }

                if (cardSetup[t].GetComponent<EnemyCard>().enemyCardColor == "R")
                {
                    cardColorRed++;
                }
            }

            //Take the color with most combo
            if (cardColorGreen > cardColorBlue && cardColorGreen > cardColorRed)
            {
                cardColorCombo = cardColorGreen;
            }

            else if (cardColorBlue > cardColorGreen && cardColorBlue > cardColorRed)
            {
                cardColorCombo = cardColorBlue;
            }

            else if (cardColorRed > cardColorGreen && cardColorRed > cardColorBlue)
            {
                cardColorCombo = cardColorRed;
            }

            else
            {
                cardColorCombo = 1;
            }

            //Take the color with most combo
        }

        else if (mode == 1)
        {
            cardColorGreen = 0;
            cardColorBlue = 0;
            cardColorRed = 0;
            cardColorCombo = 0;
        }

        //cardColorComboText.SetText(cardColorCombo.ToString());

        if (cardColorCombo == 1)
            cardColorComboText.SetText("3");

        else if (cardColorCombo == 2)
            cardColorComboText.SetText("2");

        else if (cardColorCombo == 3)
            cardColorComboText.SetText("1");

        else if (cardColorCombo == 0)
            cardColorComboText.SetText("0");
    }

    public void cardPointTotaler(int mode)//0 addition, 1 subtraction
    {
        if (mode == 0)
        {
            cardPointTotal += cardSetup[0].GetComponent<EnemyCard>().enemyCardNo;
            cardPointTotal += cardSetup[1].GetComponent<EnemyCard>().enemyCardNo;
            cardPointTotal += cardSetup[2].GetComponent<EnemyCard>().enemyCardNo;
        }

        else if (mode == 1)
        {
            cardPointTotal = 0;
        }

        cardPointTotalText.SetText(cardPointTotal.ToString());
    }

    public void enemyCardRemovalMode(int mode)//0 on, 1 off
    {
        for (int p = 0;p < cardSetup.Length;p++)
        {
            if (mode == 0)
            {
                cardSetup[p].GetComponent<EnemyCard>().enemyCardToRemove = true;
            }

            if (mode == 1)
            {
                cardSetup[p].GetComponent<EnemyCard>().enemyCardToRemove = false;
            }
        }
    }
    
    public void enemyCardRemovedCheck(int mode)//0 default, 1 player dont reset
    {
        firstTurn = false;

        for (int i = 0; i < cardSetup.Length; i++)
        {
            if (cardSetup[i].GetComponent<EnemyCard>().removedCard == false)
            {
                for (int l = 1; l <= 6; l++)//MUST BE <=
                {
                    if (cardSetup[i].GetComponent<EnemyCard>().enemyCardNo == l)
                    {
                        //print("RETURN >> " + (l));
                        enemyCardList[l - 1].GetComponent<EnemyCard>().thisCardRemaining++;
                    }
                    enemyControllerCardRemaining[l] = enemyCardList[l - 1].GetComponent<EnemyCard>().thisCardRemaining;
                }
            }
        }

        for (int h = 0; h < cardSetup.Length; h++)
        {
            cardSetup[h].GetComponent<EnemyCard>().removedCard = false;
            cardSetup[h].GetComponent<EnemyCard>().enemyCardToRemove = false;

            cardSetup[h].GetComponent<EnemyCard>().enemyCardColor = "";
            cardSetup[h].GetComponent<EnemyCard>().enemyCardNo = 0;
            //cardSetup[h].GetComponent<UnityEngine.UI.Image>().sprite = null;
            cardSetup[h].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = false;
            cardSetup[h].transform.GetChild(3).gameObject.SetActive(false);

            setCards = 0;
            cardPointTotaler(1);
            cardColorComparer(1);
        }

        removedCard.cardRemovedCheckUpdate();

        enemyCardShow(false);

        remainingCardUpdater();

        if (firstTurn == true)
            firstTurn = false;


        if (mode == 0)
        {
            enemyGameController.GetComponent<GameController>().newRound(0);
        }


    }

    public void enemyRemovePlayerCard(int mode)//0 win, other draw
    {
        if (mode == 0)
        {
            if (enemyDifficultyLevel >= 2)
            {
                int removeHighestCard = enemyGameController.highestCardToRemove;

                print("Player Highest Card : " + removeHighestCard + " | Card Value : " + Player.GetComponent<PlayerController>().cardSetup[removeHighestCard].GetComponent<PlayerCard>().playerCardNo);

                Player.GetComponent<PlayerController>().cardSetup[removeHighestCard].GetComponent<PlayerCard>().removedCard = true;

                switch (Player.GetComponent<PlayerController>().cardSetup[removeHighestCard].GetComponent<PlayerCard>().playerCardNo)
                {
                    case 1:
                        Player.GetComponent<PlayerController>().cardOne--;
                        break;
                    case 2:
                        Player.GetComponent<PlayerController>().cardTwo--;
                        break;
                    case 3:
                        Player.GetComponent<PlayerController>().cardThree--;
                        break;
                    case 4:
                        Player.GetComponent<PlayerController>().cardFour--;
                        break;
                    case 5:
                        Player.GetComponent<PlayerController>().cardFive--;
                        break;
                    default:
                        print("SWITCH ERROR ???");
                        break;
                }
            }

            else if (enemyDifficultyLevel == 1)
            {
                int removeRandomCard = enemyGameController.randomCardToRemove;

                Player.GetComponent<PlayerController>().cardSetup[removeRandomCard].GetComponent<PlayerCard>().removedCard = true;

                print("Remove Random Card : " + removeRandomCard);

                switch (Player.GetComponent<PlayerController>().cardSetup[removeRandomCard].GetComponent<PlayerCard>().playerCardNo)
                {
                    case 1:
                        Player.GetComponent<PlayerController>().cardOne--;
                        break;
                    case 2:
                        Player.GetComponent<PlayerController>().cardTwo--;
                        break;
                    case 3:
                        Player.GetComponent<PlayerController>().cardThree--;
                        break;
                    case 4:
                        Player.GetComponent<PlayerController>().cardFour--;
                        break;
                    case 5:
                        Player.GetComponent<PlayerController>().cardFive--;
                        break;
                    default:
                        print("SWITCH ERROR ???");
                        break;
                }
            }
        }

        enemyCardRemovedCheck(0);
    }

    public void cardRemover()
    {
        enemyCardList[UnityEngine.Random.Range(0, enemyCardList.Count)].GetComponent<EnemyCard>().removedCard = true;


        //cardRemovedCheck();
    }
}
