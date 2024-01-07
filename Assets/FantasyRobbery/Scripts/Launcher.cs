using FantasyRobbery.Scripts.Ui;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public void LoadMainMenu()
    {
        UiService.Show<MainMenuScreen>();
    }
}
