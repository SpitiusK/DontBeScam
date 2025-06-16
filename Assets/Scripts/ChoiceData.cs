using UnityEngine;

[CreateAssetMenu(fileName = "NewChoice", menuName = "Game/Choice")]
public class ChoiceData : ScriptableObject
{
    [SerializeField] private string choiceText;
    [SerializeField] private ImageSet nextImageSet;
    [SerializeField] private bool isScum;
    public string ChoiceText => choiceText;
    public ImageSet NextImageSet => nextImageSet;
}