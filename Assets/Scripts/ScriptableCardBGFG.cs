using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SOCardTeam", menuName = "ScriptableObjects/SOCardTeamColor")]
public class ScriptableCardBGFG : ScriptableObject
{
    public List<Sprite> cardBackgroundList = new List<Sprite>();
    public List<Sprite> cardForegroundList = new List<Sprite>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
