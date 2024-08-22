using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddNewPlayer : MonoBehaviour
{
    [SerializeField] TMP_InputField nameText;
    private void Awake()
    {
        Debug.Log(PlayerName.playerName);
        if (PlayerName.playerName != null)
        {
            nameText.textComponent.text = PlayerName.playerName;
        }
        else
        {
            Debug.Log("null");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
