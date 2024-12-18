using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [Header("Attribute")]
    public string levelName;
    public string requiredLevelName;
    private SpriteRenderer spriteRenderer;
    private bool isUnlocked;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (string.IsNullOrEmpty(requiredLevelName) || PlayerPrefs.GetInt(requiredLevelName, 0) == 1)
        {
            UnlockButton();
        }
        else
        {
            LockButton();
        }
    }

    private void UnlockButton()
    {
        isUnlocked = true;
        spriteRenderer.color = Color.white; 
    }

    private void LockButton()
    {
        isUnlocked = false;
        spriteRenderer.color = Color.gray; 
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }
}
