using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public Animator MenuAnimator;

    public GameObject zones;

    public GameObject[] zonesArray;

    // Use this for initialization
    void Start()
    {
        MenuAnimator.SetBool("IsMenu", true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        MenuAnimator.SetBool("IsMenu", false);
        Invoke("StartGameAfterDelayTime", 0.6f);
    }

    void StartGameAfterDelayTime()
    {
        if (!PlayerPrefs.HasKey("CurrentLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 1);
        }
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }

    public void SelectZone(string zoneName)
    {
        for (int i = 0; i < zonesArray.Length - 1; i++)
        {
            if (zonesArray[i].name == zoneName)
            {
                zonesArray[i].SetActive(true);
            }
        }

        zones.SetActive(false);
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
        MenuAnimator.SetBool("IsMenu", false);
        Invoke("QuitGameAfterDelay", 0.6f);
    }

    void QuitGameAfterDelay()
    {
        Application.Quit();
    }
}
