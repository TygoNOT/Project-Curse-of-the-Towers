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
        UpdateGoldAmount();
    }

    public void UpdateGoldAmount()
    {
        GoldAmount.text = "Gold: " + CurrentGold.ToString();
    }

    public void GainGold(int amount)
    {
        amount = int.Parse(InputGold.text);
        CurrentGold = CurrentGold + amount;
        UpdateGoldAmount();
    }

    public bool PayGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold = CurrentGold - amount;
            UpdateGoldAmount();
            return true;
        }
        else
        {
            return false;
        }
    }

        public void ResetGold()
    {
        CurrentGold = 0;
        UpdateGoldAmount();
    }

}
