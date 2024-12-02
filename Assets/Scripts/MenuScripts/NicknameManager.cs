using UnityEngine;
using UnityEngine.UI;

public class NicknameManager : MonoBehaviour
{
    public InputField nicknameInputField;

    private const string NicknameKey = "PlayerNickname";

    public void SetNickname()
    {
        string nickname = nicknameInputField.text;

        if (!string.IsNullOrWhiteSpace(nickname))
        {
            // Save nickname in PlayerPrefs
            PlayerPrefs.SetString(NicknameKey, nickname);
            PlayerPrefs.Save();
            Debug.Log("Nickname saved ! " + nickname);
        }
        else
        {
            Debug.LogWarning("Nickname cannot be empty!");
        }
    }
}