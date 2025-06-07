using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void OnPlayButtonClicked()
    {
        AdventureGameManager manager = FindObjectOfType<AdventureGameManager>();
        if (manager == null)
        {
            Debug.LogError("AdventureGameManager не найден!");
            return;
        }

        if (manager.StartingImageSet == null)
        {
            Debug.LogError("StartingImageSet не назначен в AdventureGameManager!");
            return;
        }

        manager.CurrentImageSet = manager.StartingImageSet;
        SceneManager.LoadScene("MainActionScene");
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}