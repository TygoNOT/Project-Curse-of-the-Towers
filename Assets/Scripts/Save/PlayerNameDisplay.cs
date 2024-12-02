using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameDisplay : MonoBehaviour
{
    public TMP_Text nicknameDisplayText;

    private const string NicknameKey = "PlayerNickname";
    void Start()
    {
        if (PlayerPrefs.HasKey(NicknameKey))
        {
            string savedNickname = PlayerPrefs.GetString(NicknameKey);
            nicknameDisplayText.text = savedNickname;
        }
        else
        {
            nicknameDisplayText.text = "Guest";
        }
    }

}
