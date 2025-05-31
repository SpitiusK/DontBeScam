using UnityEngine;

public enum ChoiceType
{
    First,
    Second
}

[CreateAssetMenu(fileName = "NewNode", menuName = "Game/Node")]
public class NodeData : ScriptableObject
{
    [SerializeField] private string storyText;
    [SerializeField] private Sprite backgroundImage;
    [SerializeField] private ChoiceData firstChoice;
    [SerializeField] private ChoiceData secondChoice;

    public string StoryText { get => storyText; set => storyText = value; }
    public Sprite BackgroundImage { get => backgroundImage; set => backgroundImage = value; }
    public ChoiceData FirstChoice { get => firstChoice; set => firstChoice = value; }
    public ChoiceData SecondChoice { get => secondChoice; set => secondChoice = value; }

    public void SetChoice(ChoiceType type, ChoiceData choice)
    {
        switch (type)
        {
            case ChoiceType.First:
                FirstChoice = choice;
                break;
            case ChoiceType.Second:
                SecondChoice = choice;
                break;
        }
    }
}