using UnityEngine;

public class AdventureGameManager : MonoBehaviour
{
    public static AdventureGameManager Instance { get; private set; }

    [SerializeField] private ImageSet startingImageSet;
    public ImageSet CurrentImageSet { get; set; }
    public NodeData CurrentNode { get; set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public ImageSet StartingImageSet => startingImageSet;
}