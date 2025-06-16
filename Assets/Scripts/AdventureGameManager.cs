using UnityEngine;

public class AdventureGameManager : MonoBehaviour
{
    public static AdventureGameManager Instance { get; private set; }

    [SerializeField] private ImageSet startingImageSet;
    public ImageSet CurrentImageSet { get; set; }
    public NodeData CurrentNode { get; set; }
    public int DeceptionCount { get; private set; } = 0;

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

    public void IncrementDeceptionCount()
    {
        DeceptionCount++;
    }

    public ImageSet StartingImageSet => startingImageSet;
}