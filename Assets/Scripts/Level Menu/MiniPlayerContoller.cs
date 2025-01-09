using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniPlayerContoller : MonoBehaviour
{
    [Header("References")]
    public Transform movepoint;
    public LayerMask whatStopsMovement;


    [Header("Attribute")]
    public float movespeed = 5f;
    public string nextLevelName;

    public Save save;
    public Animator anim;
    private void Start()
    {
        movepoint.parent = null;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movepoint.position, movespeed * Time.deltaTime);

        if(Vector3.Distance(transform.position, movepoint.position) <= .05f)
        {
            anim.SetBool("moving", false);
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movepoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.5f, whatStopsMovement))
                {
                    movepoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                }
            }

            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if (!Physics2D.OverlapCircle(movepoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.3f, whatStopsMovement))
                {
                    movepoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
            }
        }
        else
        {
            anim.SetBool("moving", true);
        }

        if (!string.IsNullOrEmpty(nextLevelName) && Input.GetKeyDown(KeyCode.Return))
        {
            LoadNextLevel();
        }

    }

    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            save = GameObject.Find("InventoryCanvas").GetComponent<Save>();

                save.SaveInventory();
                SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.LogWarning("Next level name is not set!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelButton"))
        {
            LevelButton button = collision.GetComponent<LevelButton>();

            if (button != null && button.IsUnlocked())
            {
                nextLevelName = button.levelName;
            }
            else
            {
                Debug.Log("This level is locked. You need to complete the previous level.");
                nextLevelName = "";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LevelButton"))
        {
            nextLevelName = "";
        }
    }

    public void MainMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
