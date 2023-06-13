using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[CreateAssetMenu(fileName = "SOPlayerCard", menuName = "ScriptableObjects/SOPlayerCard")]
public class ScriptablePlayerCard : ScriptableObject
{
    public int scriptableCardNo;
    public string scriptableCardColor;

    /*Strings are apparently a tiny bit slower, in case we're gonna switch to int
     * Colors Code
     * Green 1
     * Blue 2
     * Red 3
    */

    public Sprite scriptableCardBackground;
    public Sprite scriptableCardForeground;

    public Sprite scriptableCardGraphic;
    public Sprite scriptableCardButtonGraphic; //Modify the button graphic

    public string SPCString; //Modify the text in the button freely

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public int getScriptablePlayerCardNo()
    {
        //Debug.Log("getSPCNumber()" + SPCNo);
        return scriptableCardNo;
    }

    public Sprite getSPCSprite()
    {
        //Debug.Log("getSPCSprite()");
        return scriptableCardGraphic;
    }

}
