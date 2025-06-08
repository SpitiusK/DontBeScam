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

    public void OnPlayButtonClicked()
    {
        if (AdventureGameManager.Instance == null)
        {
            Debug.LogError("AdventureGameManager не найден!");
            return;
        }

        AdventureGameManager.Instance.CurrentImageSet = AdventureGameManager.Instance.StartingImageSet;
        SceneManager.LoadSceneAsync(SceneNames.MainActionScene);
    }

    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}