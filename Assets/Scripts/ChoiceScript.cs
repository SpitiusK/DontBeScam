using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ChoiceScript : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private Button choiceButtonFirst;
    [SerializeField] private Button choiceButtonSecond;

    private void Start()
    {
        AdventureGameManager manager = FindObjectOfType<AdventureGameManager>();
        if (manager == null)
        {
            Debug.LogError("AdventureGameManager не найден!");
            return;
        }
        NodeData currentNode = manager.CurrentNode;
        if (currentNode == null)
        {
            Debug.LogError("CurrentNode не установлен!");
            return;
        }
        backgroundImage.sprite = currentNode.BackgroundImage;
        storyText.text = currentNode.StoryText;

        SetupChoiceButton(choiceButtonFirst, currentNode.FirstChoice);
        SetupChoiceButton(choiceButtonSecond, currentNode.SecondChoice);
    }

    private void SetupChoiceButton(Button button, ChoiceData choice)
    {
        if (choice == null)
        {
            button.gameObject.SetActive(false);
            return;
        }
        button.gameObject.SetActive(true);
        button.GetComponentInChildren<TMP_Text>().text = choice.ChoiceText;
        button.onClick.AddListener(() =>
        {
            AdventureGameManager manager = FindObjectOfType<AdventureGameManager>();
            if (manager != null)
            {
                manager.CurrentImageSet = choice.NextImageSet;
                SceneManager.LoadScene("MainActionScene");
            }
        });
    }
}