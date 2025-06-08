using TMPro;
using UnityEngine;
   using UnityEngine.EventSystems;
   using UnityEngine.SceneManagement;
   using UnityEngine.UI;

   public class ChoiceScript : MonoBehaviour
   {
       [SerializeField] private Image backgroundImage;
       [SerializeField] private TMP_Text storyText;
       [SerializeField] private Button choiceButtonFirst;
       [SerializeField] private Button choiceButtonSecond;

       private void Start()
       {
           // Сбрасываем выбранный UI-элемент для предотвращения автоматического клика
           EventSystem.current.SetSelectedGameObject(null);

           AdventureGameManager manager = AdventureGameManager.Instance;
           if (manager == null)
           {
               Debug.LogError("AdventureGameManager не найден!");
               return;
           }

           NodeData currentNode = manager.CurrentNode;
           if (currentNode == null)
           {
               Debug.LogError("CurrentNode не установлен!");
               return;
           }

           // Устанавливаем фоновое изображение как последний спрайт из текущего ImageSet
           backgroundImage.sprite = manager.CurrentImageSet.Sprites[manager.CurrentImageSet.Sprites.Count - 1];

           storyText.text = currentNode.StoryText;

           SetupChoiceButton(choiceButtonFirst, currentNode.FirstChoice);
           SetupChoiceButton(choiceButtonSecond, currentNode.SecondChoice);
       }

       private void SetupChoiceButton(Button button, ChoiceData choice)
       {
           if (choice == null)
           {
               button.gameObject.SetActive(false);
               return;
           }
           if (choice.NextImageSet == null)
           {
               Debug.LogWarning("NextImageSet не установлен для выбора: " + choice.ChoiceText);
           }
           button.gameObject.SetActive(true);
           button.GetComponentInChildren<TMP_Text>().text = choice.ChoiceText;
           button.onClick.AddListener(() =>
           {
               if (AdventureGameManager.Instance != null)
               {
                   AdventureGameManager.Instance.CurrentImageSet = choice.NextImageSet;
                   SceneManager.LoadSceneAsync(SceneNames.MainActionScene);
               }
               else
               {
                   Debug.LogError("AdventureGameManager не найден или NextImageSet не установлен для выбора: " + choice.ChoiceText);
               }
           });
       }
   }