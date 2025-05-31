using UnityEngine;

[CreateAssetMenu(fileName = "NewChoice", menuName = "Game/Choice")]
public class ChoiceData : ScriptableObject
{
    [SerializeField] private string choiceText; // Текст кнопки
    [SerializeField] private Sprite choiceIcon; // Иконка кнопки
    [SerializeField] private NodeData nextNode; // Следующий узел

    public string ChoiceText {get => choiceText; set => choiceText = value; }
    public Sprite ChoiceIcon {get => choiceIcon; set => choiceIcon = value; }
    public NodeData NextNode {get => nextNode; set => nextNode = value; }
}