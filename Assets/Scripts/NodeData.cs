using UnityEngine;

[CreateAssetMenu(fileName = "NewNode", menuName = "Game/Node")]
public class NodeData : ScriptableObject
{
    [SerializeField] private Sprite backgroundImage;
    [SerializeField] private string storyText;
    [SerializeField] private ChoiceData firstChoice;
    [SerializeField] private ChoiceData secondChoice;
    
    public Sprite BackgroundImage => backgroundImage;
    public string StoryText => storyText;
    public ChoiceData FirstChoice => firstChoice;
    public ChoiceData SecondChoice => secondChoice;
}