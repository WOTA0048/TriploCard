using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/*
public class cardGrid : MonoBehaviour
{
    public List<GameObject> cardList = new List<GameObject>();
}
*/

public class PlayerController : MonoBehaviour
{
    [Header("Technical References")]
    public GameObject playerGameController;
    [SerializeField] private GameObject Enemy;
    public GameObject cardPrefabRef;
    private PlayerCard playerCardRefShort;

    public GameObject cardOnPlayGridRef;
    public Transform cardOnPlayGridRefTransform;

    public CardPool playerCardPool;
    public GameSettings gameSettings;

    [Header("TMPro References")]
    [SerializeField] private TextMeshProUGUI cardsRemainingText;
    [SerializeField] private TextMeshProUGUI cardColorComboText;
    [SerializeField] private TextMeshProUGUI cardPointTotalText;

    [Header("Numbers and Counters")]
    public int playerCardTeamColor;//0 orange, 1 purple
    private int maxCard;
    public int cardsRemaining;
    private int cardColorGreen;
    private int cardColorBlue;
    private int cardColorRed;
    public int cardColorCombo;
    public int cardPointTotal;

    [Header("Cards")]
    [SerializeField] public int cardOne;
    [SerializeField] public int cardTwo;
    [SerializeField] public int cardThree;
    [SerializeField] public int cardFour;
    [SerializeField] public int cardFive;
    [SerializeField] public int cardSix;
    [SerializeField] public int playerCardOne;
    [SerializeField] public int playerCardTwo;
    [SerializeField] public int playerCardThree;
    [SerializeField] public int playerCardFour;
    [SerializeField] public int playerCardFive;
    [SerializeField] public int playerCardSix;

    [Header("Checker")]
    public bool playerCardRemovalMode;
    public GameObject currentlyClickedCard;

    [Header("Audio")]
    [SerializeField] public AudioSource playerSFXAudioSource;
    [SerializeField] private AudioClip addCardSFX;
    [SerializeField] private AudioClip removeCardSFX;

    [Header("Arrays and Lists")]
    [SerializeField] public GameObject[] cardSetup;
    [SerializeField] public List<Sprite> cardBGFGTeamColorList = new List<Sprite>();
    [SerializeField] public List<GameObject> cardOnPlayList = new List<GameObject>();
    public List<GameObject> playerCardList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        cardsRemaining = playerCardPool.GetComponent<CardPool>().maxCard;
        cardsRemainingText.SetText(cardsRemaining.ToString());

        playerCardList = new List<GameObject>(playerCardPool.playerAvailableCardList);
        playerCardRefShort = cardPrefabRef.GetComponent<PlayerCard>();

        cardOne = 5;
        cardTwo = 5;
        cardThree = 4;
        cardFour = 3;
        cardFive = 2;
        cardSix = 1;

        /*
        cardSetupper(0);
        cardSetupper(1);
        cardSetupper(2); //These also called on EnemyController*/

        Enemy.GetComponent<EnemyController>().enemyInitialSetup(); //cardSettuppers also called here
    }

    // Update is called once per frame
    void Update()
    {
        //cardsRemaining = cardOne + cardTwo + cardThree + cardFour + cardFive + cardSix;
    }

    public void cardSetupper(int setupNo)
    {
        cardSetup[setupNo].transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = cardBGFGTeamColorList[0];
        cardSetup[setupNo].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = cardBGFGTeamColorList[1];

        if (cardSetup[setupNo].GetComponent<PlayerCard>().playerCardNo == 0)
            cardSetup[setupNo].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = false;
    }

    public void loadedDataCardInfo()
    {
        //print("Data Loaded");

        cardOne = 0;
        cardTwo = 0;
        cardThree = 0;
        cardFour = 0;
        cardFive = 0;
        cardSix = 0;

        for (int l = 0; l < playerCardList.Count; l++)
        {
            playerCardList[l].GetComponent<PlayerCard>().cardIsRemoved(true);
        }

        for (int i = 0; i < playerCardList.Count; i++)
        {
            //print("this ran");
            if (playerCardList[i].GetComponent<PlayerCard>().playerCardNo == 1 && playerCardOne > 0 && cardOne < playerCardOne)
            {
                playerCardList[i].GetComponent<PlayerCard>().cardIsRemoved(false);
                cardOne++;
            }

            else if (playerCardList[i].GetComponent<PlayerCard>().playerCardNo == 2 && playerCardTwo > 0 && cardTwo < playerCardTwo)
            {
                playerCardList[i].GetComponent<PlayerCard>().cardIsRemoved(false);
                cardTwo++;
            }

            else if (playerCardList[i].GetComponent<PlayerCard>().playerCardNo == 3 && playerCardThree > 0 && cardThree < playerCardThree)
            {
                playerCardList[i].GetComponent<PlayerCard>().cardIsRemoved(false);
                cardThree++;
            }

            else if (playerCardList[i].GetComponent<PlayerCard>().playerCardNo == 4 && playerCardFour > 0 && cardFour < playerCardFour)
            {
                playerCardList[i].GetComponent<PlayerCard>().cardIsRemoved(false);
                cardFour++;
            }

            else if (playerCardList[i].GetComponent<PlayerCard>().playerCardNo == 5 && playerCardFive > 0 && cardFive < playerCardFive)
            {
                playerCardList[i].GetComponent<PlayerCard>().cardIsRemoved(false);
                cardFive++;
            }

            else if (playerCardList[i].GetComponent<PlayerCard>().playerCardNo == 6 && playerCardSix > 0 && cardSix < playerCardSix)
            {
                playerCardList[i].GetComponent<PlayerCard>().cardIsRemoved(false);
                cardSix++;
            }

            else
                print("condition unfulfilled");
        }

        cardsRemaining = cardOne + cardTwo + cardThree + cardFour + cardFive + cardSix;
        cardsRemainingText.SetText(cardsRemaining.ToString());

        
    }

    public void saveDataCardInfo()
    {
        playerCardOne = cardOne; 
        playerCardTwo = cardTwo;   
        playerCardThree = cardThree;
        playerCardFour = cardFour; 
        playerCardFive = cardFive; 
        playerCardSix = cardSix;

        print(cardOne + cardTwo + cardThree + cardFour + cardFive + cardSix);
    }

    public void playCard()
    {
        if (currentlyClickedCard.GetComponent<PlayerCard>().playerCardOnPlay == false && cardOnPlayList.Count < 3)
        {
            playerSFXAudioSource.PlayOneShot(addCardSFX, 2f);

            for (int i = 0; i < playerCardPool.playerAvailableCardList.Count; i++)
            {
                if (currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo == playerCardPool.playerAvailableCardList[i].GetComponent<PlayerCard>().playerCardNo)
                {
                    cardPointTotaler(0);
                    cardOnPlayList.Add(playerCardPool.playerAvailableCardList[i]);

                    for (int l = 0; l < cardSetup.Length; l++) //l1
                    {
                        if (cardSetup[l].GetComponent<PlayerCard>().playerCardNo == 0
                            && cardOnPlayGridRef.transform.GetChild(l).GetComponent<PlayerCard>().playerCardNo == 0)
                        {
                            cardSetup[l].GetComponent<PlayerCard>().playerCardNo = currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo;
                            cardSetup[l].GetComponent<PlayerCard>().playerCardColor = currentlyClickedCard.GetComponent<PlayerCard>().playerCardColor;

                            cardSetup[l].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = playerCardPool.defaultCardList[currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo - 1].GetComponent<PlayerCard>().playerCardNumberGraphic;
                            cardSetup[l].transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = true;

                            break;
                        }
                    }

                    if (cardOnPlayList.Count >= 3)
                        cardColorComparer(0);

                    currentlyClickedCard.GetComponent<PlayerCard>().cardIsOnPlay(true);
                    //print("CardPlayed");
                    break;
                }

                else
                    continue;
            }
        }

        else if (cardOnPlayList.Count >= 3 && currentlyClickedCard.GetComponent<PlayerCard>().playerCardOnPlay == false)
        {
            Debug.Log("Maximum number of cards is already on play.");
        }


        if (currentlyClickedCard.GetComponent<PlayerCard>().playerCardOnPlay == true)
        {
            cardPointTotaler(1);
            cardColorComparer(1);

            playerSFXAudioSource.PlayOneShot(removeCardSFX);

            for (int k = 0; k < cardOnPlayList.Count; k++)
            {
                if (currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo == cardOnPlayList[k].GetComponent<PlayerCard>().playerCardNo)
                {
                    cardOnPlayList.Remove(cardOnPlayList[k]);
                    break; //IMPORTANT//
                           //Otherwise it will remove all cards with the same value
                }

                else
                {
                    //print("NotFound");
                }
            }


            for (int h = 0; h < playerCardList.Count; h++)
            {
                if (currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo == playerCardList[h].GetComponent<PlayerCard>().playerCardNo
                    && playerCardList[h].GetComponent<UnityEngine.UI.Button>().interactable == false)
                {
                    playerCardList[h].GetComponent<PlayerCard>().cardIsOnPlay(false);
                    //print("Put Back " + currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo);
                    break;
                }
            }

            currentlyClickedCard.GetComponent<PlayerCard>().playerCardColor = "";
            currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo = 0;
            currentlyClickedCard.GetComponent<UnityEngine.UI.Image>().sprite = null;
            currentlyClickedCard.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().enabled = false;
        }

        playerGameController.GetComponent<GameController>().changeStartRoundButtonGraphic(0);
    }

    public void playerRemoveEnemyCard()
    {
        currentlyClickedCard = null;
        //Enemy.GetComponent<EnemyController>().enemyCardRemovalMode(0);
        //playerCardRemovedCheck();

    }

    public void cardColorComparer(int mode)//0 start, 1 reset
    {
        if (mode == 0)
        {
            for (int i = 0; i < cardOnPlayList.Count; i++)
            {
                if (cardOnPlayList[i].GetComponent<PlayerCard>().playerCardColor == "G")
                {
                    cardColorGreen++;
                }

                if (cardOnPlayList[i].GetComponent<PlayerCard>().playerCardColor == "B")
                {
                    cardColorBlue++;
                }

                if (cardOnPlayList[i].GetComponent<PlayerCard>().playerCardColor == "R")
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

        else if(mode == 1)
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
            cardPointTotal += currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo;

        else if (mode == 1)
            cardPointTotal -= currentlyClickedCard.GetComponent<PlayerCard>().playerCardNo;

        cardPointTotalText.SetText(cardPointTotal.ToString());
    }

    public void playerCardRemovedCheck(int mode)//0 default, 1 enemy doesnt reset
    {
        //print("Checking for removed cards...");

        for (int i = 0;i < cardSetup.Length;i++)
        {
            if (cardSetup[i].GetComponent<PlayerCard>().removedCard == true)
            {
                for (int l = 0; l < playerCardList.Count; l++)
                {
                    if (cardSetup[i].GetComponent<PlayerCard>().playerCardNo == playerCardList[l].GetComponent<PlayerCard>().playerCardNo
                        && playerCardList[l].GetComponent<UnityEngine.UI.Button>().interactable == false)
                    {
                        //print("Found removed card >> " + l);

                        playerCardList[l].GetComponent<PlayerCard>().removedCard = true;
                        playerCardList[l].GetComponent<PlayerCard>().cardIsRemoved(true);

                        playerCardList.Remove(playerCardList[l]);

                        //cardsRemaining = playerCardList.Count;
                        //cardsRemaining = cardOne + cardTwo + cardThree + cardFour + cardFive + cardSix;
                        //cardsRemainingText.SetText(cardsRemaining.ToString());
                        cardsRemaining--;

                        break;
                    }
                } 
            }
            cardSetup[i].GetComponent<PlayerCard>().removedCard = false;
            //break;
        }

        

        currentlyClickedCard = cardSetup[0].GetComponent<PlayerCard>().gameObject;
        playCard();
        currentlyClickedCard = cardSetup[1].GetComponent<PlayerCard>().gameObject;
        playCard();
        currentlyClickedCard = cardSetup[2].GetComponent<PlayerCard>().gameObject;
        playCard();
        currentlyClickedCard = null;

        if (mode == 0)
            playerGameController.GetComponent<GameController>().newRound(0);

        for (int k = 0; k < playerCardList.Count; k++)
        {
            if (playerCardList[k].GetComponent<PlayerCard>().removedCard == true && playerCardList[k].GetComponent<UnityEngine.UI.Button>().interactable == true)
            {
                print("FOUND");
                playerCardList[k].GetComponent<PlayerCard>().cardIsRemoved(true);
                
            }
        }  
        cardsRemainingText.SetText(cardsRemaining.ToString());
    }

    public void cardRemover()
    {
        //playerCardList[UnityEngine.Random.Range(0, 2)].GetComponent<PlayerCard>().removedCard = true;

        int k = UnityEngine.Random.Range(0, playerCardList.Count);
        playerCardList[k].GetComponent<PlayerCard>().removedCard = true;

        switch (k)
        {
            case 1: //ALL DIFF
                cardOne--;
                break;
            case 2:
                cardTwo--;
                break;
            case 3: //ALL SAME
                cardThree--;
                break;
            case 4: //ALL SAME
                cardFour--;
                break;
            case 5: //ALL SAME
                cardFive--;
                break;
            case 63: //ALL SAME
                cardSix--;
                break;
        }

        /*
        if (k == 1)
        {
            cardOne--;
        }

        else if (k == 2)
        {
            cardTwo--;
        }

        else if (k == 3)
        {
            cardThree--;
        }

        else if (k == 4)
        {
            cardFour--;
        }

        else if (k == 5)
        {
            cardFive--;
        }

        else if (k == 6)
        {
            cardSix--;
        }*/

        //print("Random >> " + k);

        playerCardRemovedCheck(0);
    }

    
}
