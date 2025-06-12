using UnityEngine;

[CreateAssetMenu(fileName = "NewChoice", menuName = "Game/Choice")]
public class ChoiceData : ScriptableObject
{
    [SerializeField] private string choiceText;
    [SerializeField] private Sprite choiceIcon;
    [SerializeField] private ImageSet nextImageSet;

    public string ChoiceText => choiceText;
    public ImageSet NextImageSet => nextImageSet;
}