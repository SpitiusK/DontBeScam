using UnityEngine;

public class AdventureGameManager : MonoBehaviour
{
    [SerializeField] private ImageSet startingImageSet;
    public ImageSet CurrentImageSet { get; set; }
    public NodeData CurrentNode { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public ImageSet StartingImageSet => startingImageSet;
}