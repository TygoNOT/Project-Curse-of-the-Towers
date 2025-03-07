using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money : MonoBehaviour
{
    public int CurrentGold;
    public Text GoldAmount;
    public InputField InputGold;

    void Start()
    {
        if (!PlayerPrefs.HasKey("PlayerGold"))
        {
            CurrentGold = 20; 
            SaveGold(); 
        }
        else
        {
            CurrentGold = PlayerPrefs.GetInt("PlayerGold"); 
        }
        UpdateGoldAmount();
    }

    public void UpdateGoldAmount()
    {
        GoldAmount.text = "Gold: " + CurrentGold.ToString();
    }


    public void SpawnGold(int amount)
    {
        amount = int.Parse(InputGold.text);
        CurrentGold = CurrentGold + amount;
        UpdateGoldAmount();
    }

    public void GainGold(int amount)
    {
        CurrentGold += amount;
        UpdateGoldAmount();
        SaveGold();
    }

    public bool PayGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            UpdateGoldAmount();
            SaveGold();
            return true;
        }
        else
        {
            return false;
        }
    }

        public void ResetGold()
    {
        CurrentGold = 20;
        UpdateGoldAmount();
        SaveGold();
    }
    private void SaveGold()
    {
        PlayerPrefs.SetInt("PlayerGold", CurrentGold);
        PlayerPrefs.Save();
    }

}
