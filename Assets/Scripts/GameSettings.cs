using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using System.IO;

public class GameSettings : MonoBehaviour
{
    private static GameObject instance;
    [SerializeField] private GameSaveData gameSaveData;
    public int chosenCardColor;
    public int chosenLevel;
    public bool dataLoadAvailable;
    public bool dataLoaded;
    public int soundOnOff;

    [Header("Dev Test")]
    public TextMeshProUGUI debugLog;
    public bool debugPrint;

    // Start is called before the first frame update
    void Awake()
    {  
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(this);

        if (instance == null)
            instance = this.gameObject;
        else
            Destroy(gameObject);

        if (dataLoaded == true)
        {
            chosenCardColor = gameSaveData.savedCardColorData;
            chosenLevel = gameSaveData.savedDifficultyData;
        }

        dataExistenceCheck();

        //debugLog = GameObject.FindGameObjectWithTag("DebugLog").GetComponent<TextMeshProUGUI>();
        //Debug.Log(Application.persistentDataPath);
    }

    void Start()
    {
        //check permission

        //check permission
        AndroidRuntimePermissions.Permission result = AndroidRuntimePermissions.RequestPermission("android.permission.EXTERNAL_STORAGE");

        /*
        if (result == AndroidRuntimePermissions.Permission.Granted)
        {
            Debug.Log("We have permission to access external storage!");
            debugLog.SetText("We have permission to access external storage!");

        }
        else if (result != AndroidRuntimePermissions.Permission.Granted)
        {
            Debug.Log("Permission state: " + result);
            debugLog.SetText("Permission state: " + result);
        
        }*/
    }

    void Update()
    {
        //debugLog = GameObject.FindGameObjectWithTag("DebugLog").GetComponent<TextMeshProUGUI>();
        dataExistenceCheck();  

    }

    private void dataExistenceCheck()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/SaveData.txt"))
        {
            dataLoadAvailable = true;
            //print("DATA FOUND");
        }

        else if (!System.IO.File.Exists(Application.persistentDataPath + "/SaveData.txt"))
        {
            dataLoadAvailable = false;
        }
    }
     

}
