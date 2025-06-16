using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSceneScript : MonoBehaviour
{
    [SerializeField] private TMP_Text deceptionText;
    [SerializeField] private Button backButton;

    private void Start()
    {
        if (AdventureGameManager.Instance != null)
        {
            int count = AdventureGameManager.Instance.DeceptionCount;
            deceptionText.text = $"Вас обманули {count} раз.";
        }
        else
        {
            Debug.LogError("AdventureGameManager не найден!");
        }

        backButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync(SceneNames.MainMenu);
        });
    }
}