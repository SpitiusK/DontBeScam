using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public Button PlayButton;
    public Button ExitButton;

    void Start()
    {
        PlayButton.onClick.AddListener(OnPlayButtonClicked);
        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void OnPlayButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    void OnExitButtonClicked()
    {
        Application.Quit();
    }
}