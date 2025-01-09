using UnityEngine;
using UnityEngine.UI;

public class ResetButtonController : MonoBehaviour
{
    public Button resetButton;

    void Start()
    {
        if (HasSavedData())
        {
            resetButton.gameObject.SetActive(true); 
        }
        else
        {
            resetButton.gameObject.SetActive(false);
        }
    }

    private bool HasSavedData()
    {
        return !string.IsNullOrEmpty(PlayerPrefs.GetString("PlayerStatsSaves")) ||
               !string.IsNullOrEmpty(PlayerPrefs.GetString("EquippableSlotsData")) ||
               !string.IsNullOrEmpty(PlayerPrefs.GetString("EquipmentSlotsData")) ||
               !string.IsNullOrEmpty(PlayerPrefs.GetString("ItemSlotsData")) ||
               !string.IsNullOrEmpty(PlayerPrefs.GetString("PetSlotsData"))||
               PlayerPrefs.HasKey("PlayerGold");
    }

    public void ResetPlayerData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        resetButton.gameObject.SetActive(false);
    }
}
