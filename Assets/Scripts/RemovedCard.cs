using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class RemovedCard : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;
    [SerializeField] private EnemyController enemyController;

    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerController playerController;

    [SerializeField] private int[] remainingCardNo;
    [SerializeField] private int[] removedCardNo;
    [SerializeField] private TextMeshProUGUI[] removedCardText;
    [SerializeField] public GameObject[] removedCardGameObjects;
    [SerializeField] public List<Sprite> cardBGFGList = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        enemyController = Enemy.GetComponent<EnemyController>();
        playerController = Player.GetComponent<PlayerController>();

        cardBGFGList = enemyController.cardBGFGTeamColorList;

        //Invoke("setBackForeGround", 0.1f);
        setBackForeGround();
    }

    public void removedCardSetRemainingCard()
    {
        for (int i = 1; i < remainingCardNo.Length; i++)
        {
            if (remainingCardNo[i] != enemyController.enemyControllerCardRemaining[i])
            {
                remainingCardNo[i] = enemyController.enemyControllerCardRemaining[i];
            }
        }
    }

    private void setBackForeGround()
    {
        for (int i = 0; i < removedCardGameObjects.Length; i++)
        {
            removedCardGameObjects[i].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = cardBGFGList[0];
            removedCardGameObjects[i].transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = cardBGFGList[1];
        }
    }
    
    public void markCardToRemove()
    {
        for (int i = 0;i < enemyController.cardSetup.Length;i++)
        {
            //print(enemyController.cardSetup[i].GetComponent<EnemyCard>().enemyCardNo);
            
            if (enemyController.cardSetup[i].GetComponent<EnemyCard>().removedCard == false 
                && playerController.currentlyClickedCard.GetComponent<EnemyCard>().enemyCardNo != enemyController.cardSetup[i].GetComponent<EnemyCard>().enemyCardNo
                && removedCardGameObjects[enemyController.cardSetup[i].GetComponent<EnemyCard>().enemyCardNo - 1].transform.GetChild(3).gameObject.activeInHierarchy == true)
            {
                //print("thisRan");
                removedCardGameObjects[enemyController.cardSetup[i].GetComponent<EnemyCard>().enemyCardNo - 1].transform.GetChild(3).gameObject.SetActive(false);
            }

            if (enemyController.cardSetup[i].GetComponent<EnemyCard>().removedCard == true)
            { 
                removedCardGameObjects[enemyController.cardSetup[i].GetComponent<EnemyCard>().enemyCardNo - 1].transform.GetChild(3).gameObject.SetActive(true);
            }
            
        }      
    }

    public void cardRemovedCheckUpdate()
    {
        //print("cardRemovedCheckUpdate()");

        for (int i = 1;i < remainingCardNo.Length;i++)
        {
            if (remainingCardNo[i] != enemyController.enemyControllerCardRemaining[i])
            {
                //remainingCardNo[i] = enemyController.enemyControllerCardRemaining[i];

                removedCardNo[i]++;
                removedCardText[i - 1].SetText(removedCardNo[i].ToString());
            }
        }
    }
    public void cardRemovedCheckLoaded()
    {
        if (enemyController.enemyControllerCardRemaining[1] < 5)
        {
            removedCardNo[1] = 5 - enemyController.enemyControllerCardRemaining[1];
            removedCardText[0].SetText(removedCardNo[1].ToString());
        }

        if (enemyController.enemyControllerCardRemaining[2] < 5)
        {
            removedCardNo[2] = 5 - enemyController.enemyControllerCardRemaining[2];
            removedCardText[1].SetText(removedCardNo[2].ToString());
        }

        if (enemyController.enemyControllerCardRemaining[3] < 4)
        {
            removedCardNo[3] = 4 - enemyController.enemyControllerCardRemaining[3];
            removedCardText[2].SetText(removedCardNo[3].ToString());
        }

        if (enemyController.enemyControllerCardRemaining[4] < 3)
        {
            removedCardNo[4] = 3 - enemyController.enemyControllerCardRemaining[4];
            removedCardText[3].SetText(removedCardNo[4].ToString());
        }

        if (enemyController.enemyControllerCardRemaining[5] < 2)
        {
            removedCardNo[5] = 2 - enemyController.enemyControllerCardRemaining[5];
            removedCardText[4].SetText(removedCardNo[5].ToString());
        }

        if (enemyController.enemyControllerCardRemaining[6] < 1)
        {
            removedCardNo[6] = 1 - enemyController.enemyControllerCardRemaining[6];
            removedCardText[5].SetText(removedCardNo[6].ToString());
        }


    }
}
