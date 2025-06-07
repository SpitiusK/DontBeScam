using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class AdventureGameManager : MonoBehaviour
{
    [SerializeField] private ImageSet startingImageSet;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image textBackgroundImage;
    [SerializeField] private TMP_Text storyText;
    [SerializeField] private Button choiceButtonFirst;
    [SerializeField] private Button choiceButtonSecond;
    [SerializeField] private Sprite storyTextBackground;
    [SerializeField] private Sprite choiceButtonBackground;
    [SerializeField] private Sprite defaultChoiceIcon;

    private const string DefaultChoiceText = "None";
    public ImageSet CurrentImageSet { get; set; }
    public NodeData CurrentNode { get; set; }
    public ImageSet StartingImageSet => startingImageSet; // Публичное свойство для доступа к startingImageSet

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (startingImageSet == null)
        {
            Debug.LogError("StartingImageSet не назначен в инспекторе!");
            return;
        }
        choiceButtonFirst.onClick.AddListener(OnFirstChoiceClicked);
        choiceButtonSecond.onClick.AddListener(OnSecondChoiceClicked);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (CurrentNode == null)
        {
            Debug.LogWarning("CurrentNode не установлен!");
            return;
        }

        backgroundImage.sprite = CurrentNode.BackgroundImage;
        storyText.text = CurrentNode.StoryText;

        if (CurrentNode.FirstChoice != null)
        {
            choiceButtonFirst.gameObject.SetActive(true);
            choiceButtonFirst.GetComponent<Image>().sprite = choiceButtonBackground;
            choiceButtonFirst.GetComponentInChildren<TMP_Text>().text = CurrentNode.FirstChoice.ChoiceText != "" ? CurrentNode.FirstChoice.ChoiceText : DefaultChoiceText;
            choiceButtonFirst.GetComponent<Image>().sprite = CurrentNode.FirstChoice.ChoiceIcon != null ? CurrentNode.FirstChoice.ChoiceIcon : defaultChoiceIcon;
        }
        else
        {
            choiceButtonFirst.gameObject.SetActive(false);
        }

        if (CurrentNode.SecondChoice != null)
        {
            choiceButtonSecond.gameObject.SetActive(true);
            choiceButtonSecond.GetComponent<Image>().sprite = choiceButtonBackground;
            choiceButtonSecond.GetComponentInChildren<TMP_Text>().text = CurrentNode.SecondChoice.ChoiceText != "" ? CurrentNode.SecondChoice.ChoiceText : DefaultChoiceText;
            choiceButtonSecond.GetComponent<Image>().sprite = CurrentNode.SecondChoice.ChoiceIcon != null ? CurrentNode.SecondChoice.ChoiceIcon : defaultChoiceIcon;
        }
        else
        {
            choiceButtonSecond.gameObject.SetActive(false);
        }
    }

    private void OnFirstChoiceClicked()
    {
        HandleChoiceSelection(CurrentNode.FirstChoice);
    }

    private void OnSecondChoiceClicked()
    {
        HandleChoiceSelection(CurrentNode.SecondChoice);
    }

    private void HandleChoiceSelection(ChoiceData choice)
    {
        if (choice == null)
        {
            Debug.LogWarning("Выбранный выбор не установлен!");
            return;
        }

        if (choice.NextImageSet != null)
        {
            CurrentImageSet = choice.NextImageSet;
            SceneManager.LoadScene("MainActionScene");
        }
        else
        {
            Debug.LogWarning("NextImageSet не установлен для выбранного выбора!");
        }
    }
}