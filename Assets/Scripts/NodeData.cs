using UnityEngine;

[CreateAssetMenu(fileName = "NewNode", menuName = "Game/Node")]
public class NodeData : ScriptableObject
{
    [SerializeField] private string storyText;
    [SerializeField] private ChoiceData firstChoice;
    [SerializeField] private ChoiceData secondChoice;

    public string StoryText => storyText;
    public ChoiceData FirstChoice => firstChoice;
    public ChoiceData SecondChoice => secondChoice;
}