using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainActionScript : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    private ImageSet currentImageSet;
    private int currentSpriteIndex;

    private void Start()
    {
        AdventureGameManager manager = FindObjectOfType<AdventureGameManager>();
        if (manager == null)
        {
            Debug.LogError("AdventureGameManager не найден!");
            return;
        }

        currentImageSet = manager.CurrentImageSet;
        if (currentImageSet == null || currentImageSet.Sprites.Count == 0)
        {
            Debug.LogError("ImageSet не назначен или пуст!");
            return;
        }

        currentSpriteIndex = 0;
        backgroundImage.sprite = currentImageSet.Sprites[currentSpriteIndex];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            AdvanceToNextSprite();
        }
    }

    private void AdvanceToNextSprite()
    {
        currentSpriteIndex++;
        if (currentSpriteIndex < currentImageSet.Sprites.Count)
        {
            backgroundImage.sprite = currentImageSet.Sprites[currentSpriteIndex];
        }
        else
        {
            AdventureGameManager manager = FindObjectOfType<AdventureGameManager>();
            if (manager != null)
            {
                manager.CurrentNode = currentImageSet.NextNode;
                SceneManager.LoadScene("ChoiceScene");
            }
            else
            {
                Debug.LogError("AdventureGameManager не найден!");
            }
        }
    }
}