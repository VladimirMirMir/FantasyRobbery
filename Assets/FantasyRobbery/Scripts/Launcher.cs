using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
    }
}
