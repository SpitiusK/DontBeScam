using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdventureGameManager : MonoBehaviour
{
    [SerializeField] private Image backgroundImage; // Фон сцены
    [SerializeField] private Image textBackgroundImage; // Фон текста
    [SerializeField] private TMP_Text storyText; // Текст истории
    [SerializeField] private Button choiceButtonFirst; // Первая кнопка
    [SerializeField] private Button choiceButtonSecond; // Вторая кнопка
    [SerializeField] private Sprite storyTextBackground; // Общий фон текста
    [SerializeField] private Sprite choiceButtonBackground; // Общий фон кнопок
    [SerializeField] private Sprite defaultChoiceIcon; // Стандартное изображение для иконки на кнопке
    [SerializeField] private NodeData startingNode; // Начальный узел
    private const string DefaultChoiceText = "None";
    private NodeData currentNode; // Текущий узел

    /// <summary>
    /// Инициализирует компонент при старте сцены.
    /// Устанавливает фоновые изображения, начальный узел и слушатели событий для кнопок.
    /// </summary>
    private void Start()
    {
        // Проверка, назначены ли критические поля в Inspector
        if (startingNode == null)
        {
            Debug.LogError("startingNode is not assigned in Inspector!");
            return;
        }
        if (defaultChoiceIcon == null)
        {
            Debug.LogError("defaultChoiceIcon is not assigned in Inspector!");
            return;
        }

        // Инициализация фоновых изображений для текста и кнопок
        InitializeBackgrounds();

        // Установка начального узла игры
        currentNode = startingNode;

        // Обновление UI на основе начального узла
        UpdateUI();

        // Настройка слушателей для обработки нажатий на кнопки
        SetupButtonListeners();
    }

    /// <summary>
    /// Устанавливает фоновые изображения для текста и кнопок.
    /// Применяет <see cref="storyTextBackground"/> к текстовому фону и <see cref="choiceButtonBackground"/> к кнопкам.
    /// </summary>
    private void InitializeBackgrounds()
    {
        // Установка фона для текста
        textBackgroundImage.sprite = storyTextBackground;

        // Установка фона для первой кнопки
        choiceButtonFirst.GetComponent<Image>().sprite = choiceButtonBackground;

        // Установка фона для второй кнопки
        choiceButtonSecond.GetComponent<Image>().sprite = choiceButtonBackground;
    }

    /// <summary>
    /// Настраивает слушатели событий для кнопок выбора.
    /// Удаляет старые слушатели и добавляет новые для обработки нажатий на <see cref="choiceButtonFirst"/> и <see cref="choiceButtonSecond"/>.
    /// </summary>
    private void SetupButtonListeners()
    {
        // Очистка всех существующих слушателей для первой кнопки
        choiceButtonFirst.onClick.RemoveAllListeners();

        // Добавление нового слушателя для первой кнопки
        choiceButtonFirst.onClick.AddListener(OnFirstChoiceClicked);

        // Очистка всех существующих слушателей для второй кнопки
        choiceButtonSecond.onClick.RemoveAllListeners();

        // Добавление нового слушателя для второй кнопки
        choiceButtonSecond.onClick.AddListener(OnSecondChoiceClicked);
    }

    /// <summary>
    /// Обновляет пользовательский интерфейс на основе текущего узла (<see cref="currentNode"/>).
    /// Обновляет фон сцены, текст истории и состояние кнопок выбора.
    /// Если текущий узел равен null, выводит сообщение в консоль и завершает выполнение.
    /// </summary>
    private void UpdateUI()
    {
        // Логирование вызова метода для отладки
        Debug.Log("UpdateUI called");

        // Проверка, не является ли текущий узел null
        if (currentNode == null)
        {
            Debug.Log("currentNode is null");
            return;
        }

        // Вывод отладочной информации о текущем узле
        LogNodeDebugInfo();

        // Обновление фона и текста сцены
        UpdateSceneElements();

        // Обновление первой кнопки выбора
        UpdateChoiceButton(ChoiceType.First);

        // Обновление второй кнопки выбора
        UpdateChoiceButton(ChoiceType.Second);
    }

    /// <summary>
    /// Выводит отладочную информацию о текущем узле в консоль.
    /// Логирует данные о <see cref="currentNode"/>, его выборах, их текстах и иконках, а также стандартной иконке.
    /// </summary>
    private void LogNodeDebugInfo()
    {
        // Логирование текущего узла
        Debug.Log("_currentNode: " + currentNode);

        // Логирование данных о первом выборе
        Debug.Log("_currentNode.FirstChoice: " + currentNode?.FirstChoice);

        // Логирование текста первого выбора, с "None" при отсутствии текста
        Debug.Log("_currentNode.FirstChoice.ChoiceText: " + (currentNode?.FirstChoice?.ChoiceText ?? "None"));

        // Логирование иконки первого выбора, с "None" при отсутствии иконки
        Debug.Log("_currentNode.FirstChoice.ChoiceIcon: " + (currentNode?.FirstChoice?.ChoiceIcon != null ? currentNode.FirstChoice.ChoiceIcon.name : "None"));

        // Логирование данных о втором выборе
        Debug.Log("_currentNode.SecondChoice: " + currentNode?.SecondChoice);

        // Логирование текста второго выбора, с "None" при отсутствии текста
        Debug.Log("_currentNode.SecondChoice.ChoiceText: " + (currentNode?.SecondChoice?.ChoiceText ?? "None"));

        // Логирование иконки второго выбора, с "None" при отсутствии иконки
        Debug.Log("_currentNode.SecondChoice.ChoiceIcon: " + (currentNode?.SecondChoice?.ChoiceIcon != null ? currentNode.SecondChoice.ChoiceIcon.name : "None"));

        // Логирование стандартной иконки, с "None" при отсутствии
        Debug.Log("defaultChoiceIcon: " + (defaultChoiceIcon != null ? defaultChoiceIcon.name : "None"));
    }

    /// <summary>
    /// Обновляет основные элементы сцены: фон и текст истории.
    /// Устанавливает <see cref="backgroundImage"/> и <see cref="storyText"/> на основе данных текущего узла.
    /// </summary>
    private void UpdateSceneElements()
    {
        // Установка фонового изображения сцены
        backgroundImage.sprite = currentNode.BackgroundImage;

        // Установка текста истории
        storyText.text = currentNode.StoryText;
    }

    private enum ChoiceType
    {
        First,
        Second
    }

    /// <summary>
    /// Обновляет состояние кнопки выбора (первой или второй).
    /// Устанавливает интерактивность кнопки, текст и иконку на основе данных выбора.
    /// </summary>
    /// <param name="choiceType">Тип кнопки: первая (<see cref="ChoiceType.First"/>) или вторая (<see cref="ChoiceType.Second"/>).</param>
    private void UpdateChoiceButton(ChoiceType choiceType)
    {
        // Определение, какая кнопка обновляется (первая или вторая)
        Button button = choiceType == ChoiceType.First ? choiceButtonFirst : choiceButtonSecond;

        // Получение данных выбора для текущей кнопки
        ChoiceData choiceData = choiceType == ChoiceType.First ? currentNode.FirstChoice : currentNode.SecondChoice;

        // Получение трансформации кнопки для поиска дочерних компонентов
        Transform buttonTransform = button.transform;

        // Проверка, доступен ли следующий узел для выбора (кнопка активна, если узел есть)
        bool isInteractable = choiceData?.NextNode != null;
        button.interactable = isInteractable;

        // Поиск компонента текста на кнопке
        var textComponent = buttonTransform.Find("ChoiceText")?.GetComponent<TMP_Text>();

        // Поиск компонента иконки на кнопке
        var iconComponent = buttonTransform.Find("ChoiceIcon")?.GetComponent<Image>();

        // Проверка наличия компонентов текста и иконки
        if (textComponent != null && iconComponent != null)
        {
            // Обновление содержимого кнопки (текста и иконки)
            UpdateButtonContent(choiceType, textComponent, iconComponent, choiceData);
        }
        else
        {
            // Обработка случая, если компоненты отсутствуют
            HandleMissingComponents(choiceType, textComponent, iconComponent);
        }
    }

    /// <summary>
    /// Обновляет содержимое кнопки: текст и иконку.
    /// Устанавливает значения по умолчанию, если данные выбора отсутствуют.
    /// </summary>
    /// <param name="choiceType">Тип кнопки: первая или вторая.</param>
    /// <param name="textComponent">Компонент текста кнопки (<see cref="TMP_Text"/>).</param>
    /// <param name="iconComponent">Компонент иконки кнопки (<see cref="Image"/>).</param>
    /// <param name="choiceData">Данные выбора (<see cref="ChoiceData"/>), связанные с кнопкой.</param>
    private void UpdateButtonContent(ChoiceType choiceType, TMP_Text textComponent, Image iconComponent, ChoiceData choiceData)
    {
        // Установка текста кнопки, если ChoiceText отсутствует или пустой, используется "None"
        string choiceText = string.IsNullOrEmpty(choiceData?.ChoiceText) ? DefaultChoiceText : choiceData.ChoiceText;
        textComponent.text = choiceText;

        // Логирование установленного текста для отладки
        Debug.Log($"{choiceType}Text.text: " + choiceText);

        // Установка иконки кнопки, если ChoiceIcon отсутствует, используется defaultChoiceIcon
        
        var choiceIcon = choiceData?.ChoiceIcon == null? defaultChoiceIcon : choiceData.ChoiceIcon;
        iconComponent.sprite = choiceIcon;
        
        // Установка непрозрачного цвета для иконки
        iconComponent.color = Color.white;

        // Логирование обновления кнопки
        string logMessage = $"{choiceType} button updated: " + choiceText;
        Debug.Log(logMessage);

        // Логирование установленной иконки
        Debug.Log($"{choiceType} icon sprite set to: " + (iconComponent.sprite != null ? iconComponent.sprite.name : "null"));
    }

    /// <summary>
    /// Обрабатывает случай, когда компоненты кнопки (текст или иконка) отсутствуют.
    /// Отключает кнопку и сбрасывает значения компонентов, если они доступны.
    /// </summary>
    /// <param name="choiceType">Тип кнопки: первая или вторая.</param>
    /// <param name="textComponent">Компонент текста кнопки (<see cref="TMP_Text"/>).</param>
    /// <param name="iconComponent">Компонент иконки кнопки (<see cref="Image"/>).</param>
    private void HandleMissingComponents(ChoiceType choiceType, TMP_Text textComponent, Image iconComponent)
    {
        // Сброс текста, если компонент текста доступен
        if (textComponent != null) textComponent.text = "";

        // Установка стандартной иконки и цвета, если компонент иконки доступен
        if (iconComponent != null)
        {
            iconComponent.sprite = defaultChoiceIcon;
            iconComponent.color = Color.white;
        }

        // Определение кнопки для отключения
        Button button = choiceType == ChoiceType.First ? choiceButtonFirst : choiceButtonSecond;

        // Отключение кнопки
        button.interactable = false;

        // Логирование отсутствия компонентов с указанием, какие именно отсутствуют
        Debug.LogWarning($"{choiceType} button components missing: Text - {textComponent != null}, Icon - {iconComponent != null}");
    }

    /// <summary>
    /// Обрабатывает нажатие на первую кнопку выбора.
    /// Переходит к следующему узлу, если он доступен.
    /// </summary>
    private void OnFirstChoiceClicked()
    {
        // Обработка выбора для первой кнопки
        HandleChoiceSelection(currentNode.FirstChoice);
    }

    /// <summary>
    /// Обрабатывает нажатие на вторую кнопку выбора.
    /// Переходит к следующему узлу, если он доступен.
    /// </summary>
    private void OnSecondChoiceClicked()
    {
        // Обработка выбора для второй кнопки
        HandleChoiceSelection(currentNode.SecondChoice);
    }

    /// <summary>
    /// Обрабатывает выбор игрока и переходит к следующему узлу.
    /// Если следующий узел отсутствует, выводит сообщение в консоль.
    /// </summary>
    /// <param name="choice">Данные выбранного выбора (<see cref="ChoiceData"/>).</param>
    private void HandleChoiceSelection(ChoiceData choice)
    {
        // Логирование нажатия кнопки
        Debug.Log("Button clicked");

        // Проверка наличия следующего узла для перехода
        if (choice?.NextNode != null)
        {
            // Переход к следующему узлу
            currentNode = choice.NextNode;

            // Обновление UI для нового узла
            UpdateUI();
        }
        else
        {
            // Логирование отсутствия следующего узла с указанием текущего выбора
            Debug.LogWarning("No next node for selected choice. Current choice: " + (choice != null ? choice.ChoiceText : "null"));
        }
    }
}