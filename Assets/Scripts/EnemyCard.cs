using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyCard : MonoBehaviour
{
    [Header("Card Values")]
    public int enemyCardNo;
    public string enemyCardColor;
    public int thisCardRemaining;
    public bool thisCardRemovalMode;

    [Header("Card Identifiers")]
    public bool enemyCardOnPlay;
    public bool enemyCardToRemove;
    public bool removedCard;

    //public int[] enemyCardOnPlayValue;

    [Header("Card References")]
    public GameObject Player;
    public PlayerController enemyCardPlayerController;
    public GameObject Enemy;
    public EnemyController enemyCardEnemyController;
    public GameSettings enemyCardGameSettings;
    public RemovedCard enemyRemovedCard;

    [Header("Card Graphics")]
    //public Sprite enemyCardGraphic;
    public Sprite enemyCardBackground;
    public Sprite enemyCardForeground;
    public Sprite enemyCardNumberGraphic;

    //public GameObject cardPool;
    //public GameObject PCEnemyCardRef;
    //public GameObject PCCardOnPlayGridRef;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<UnityEngine.UI.Image>().sprite = enemyCardGraphic;
        //gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardGraphic;
        enemyRemovedCard = GameObject.FindGameObjectWithTag("RemovedCard").GetComponent<RemovedCard>();
        enemyCardPlayerController = Player.GetComponent<PlayerController>();

        gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardBackground;
        gameObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardForeground;
        gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = enemyCardNumberGraphic;
    }

    public void cardIsOnPlay(bool itIs)
    {
        if (itIs == true)
        {
            gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 50);
            gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;
        }

        else if (itIs == false)
        {
            gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
        }
    }
    
    public void thisCard()
    {
        //enemyCardPlayerController.currentlyClickedCard = gameObject;
        //enemyCardEnemyController.currentlyClickedCard = enemyCardPlayerController.currentlyClickedCard;
        if (enemyCardToRemove == true)
        {
            //print("REMOVED >> " + enemyCardNo);
            enemyCardPlayerController.currentlyClickedCard = gameObject;
            

            enemyCardEnemyController.cardOnPlayGridRefTransform.GetChild(3).gameObject.SetActive(false);

            for (int i = 0;i <= 2;i++)
            {
                enemyCardEnemyController.cardSetup[i].transform.GetChild(3).gameObject.SetActive(false);
                enemyCardEnemyController.cardSetup[i].GetComponent<EnemyCard>().removedCard = false;
            }

            this.gameObject.GetComponent<EnemyCard>().removedCard = true;
            this.gameObject.transform.GetChild(3).gameObject.SetActive(true);

            enemyRemovedCard.markCardToRemove();

            //Enemy.GetComponent<EnemyController>().enemyCardRemovedCheck(0);
        }
        

    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
