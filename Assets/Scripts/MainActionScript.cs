using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainActionScript : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    private ImageSet _currentImageSet;
    private int _currentSpriteIndex;
    private AsyncOperation _asyncLoad;

    private void Start()
    {
        AdventureGameManager manager = AdventureGameManager.Instance;
        if (manager == null)
        {
            Debug.LogError("AdventureGameManager не найден!");
            return;
        }

        _currentImageSet = manager.CurrentImageSet;
        if (_currentImageSet == null || _currentImageSet.Sprites.Count == 0)
        {
            Debug.LogError("ImageSet не назначен или пуст!");
            return;
        }

        _currentSpriteIndex = 0;
        backgroundImage.sprite = _currentImageSet.Sprites[_currentSpriteIndex];
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame || Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AdvanceToNextSprite();
        }
    }

    private void AdvanceToNextSprite()
    {
        _currentSpriteIndex++;
        if (_currentSpriteIndex < _currentImageSet.Sprites.Count)
        {
            backgroundImage.sprite = _currentImageSet.Sprites[_currentSpriteIndex];
        }
        else
        {
            if (_currentImageSet.NextNode != null)
            {
                AdventureGameManager.Instance.CurrentNode = _currentImageSet.NextNode;
                _asyncLoad = SceneManager.LoadSceneAsync(SceneNames.ChoiceScene);
                _asyncLoad.allowSceneActivation = false; // Отложите активацию сцены
                StartCoroutine(ActivateSceneAfterDelay(0.1f)); // Запустите корутину для активации через задержку
            }
            else
            {
                SceneManager.LoadSceneAsync(SceneNames.MainMenu);
            }
        }
    }
    
    private IEnumerator ActivateSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Ждём 0.5 секунды (можно настроить)
        _asyncLoad.allowSceneActivation = true; // Активируем сцену
    }
}