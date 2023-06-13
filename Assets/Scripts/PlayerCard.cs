using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCard : MonoBehaviour
{
    [Header("Card Values")]
    public int playerCardNo;
    public string playerCardColor;

    [Header("Card Identifiers")]
    public bool playerCardOnPlay;
    public bool playerCardToRemove;
    public bool removedCard;

    //public int[] playerCardOnPlayValue;

    [Header("Card References")]
    public GameObject Player;
    public PlayerController playerCardPlayerController;
    public RemovedCard playerRemovedCard;

    [Header("Card Graphics")]
    //public Sprite playerCardGraphic;
    public Sprite playerCardBackground;
    public Sprite playerCardForeground;
    public Sprite playerCardNumberGraphic;

    [Header("Card Audio")]
    public AudioClip playerCardClickedSFX;
    public AudioClip playerCardRemovedSFX;

    //public GameObject cardPool;
    //public GameObject PCPlayerCardRef;
    //public GameObject PCCardOnPlayGridRef;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<UnityEngine.UI.Image>().sprite = playerCardGraphic;
        //gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = playerCardGraphic;

        playerRemovedCard = GameObject.FindGameObjectWithTag("RemovedCard").GetComponent<RemovedCard>();

        gameObject.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = playerCardBackground;
        gameObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = playerCardForeground;
        gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = playerCardNumberGraphic;

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

    public void cardIsRemoved(bool itIs)
    {
        cardIsOnPlay(false);

        if (itIs == true)
        {
            gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 50, 50, 50);
            gameObject.GetComponent<UnityEngine.UI.Button>().interactable = false;
            removedCard = true;
        }

        else if (itIs == false)
        {
            gameObject.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            gameObject.GetComponent<UnityEngine.UI.Button>().interactable = true;
            removedCard = false;
        }

        //print("Card Removed >> " + this.playerCardNo);
    }

    public void thisCard(bool setupCard)
    {
        //print("Clicked " + PCNo);
        playerCardPlayerController.currentlyClickedCard = gameObject;
        //this.gameObject.GetComponent<AudioSource>().PlayOneShot(playerCardClickedSFX);

        playerCardPlayerController.playCard();
    }


    // Update is called once per frame
    void Update()
    {

    }
}
