using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Пользовательское окно редактора для управления объектами NodeData.
/// Позволяет создавать, редактировать и удалять узлы, а также управлять их выборами (FirstChoice и SecondChoice).
/// </summary>
public class NodeEditorWindow : EditorWindow
{
    // Список всех загруженных объектов NodeData
    private List<NodeData> nodes;
    // Позиция прокрутки для списка узлов
    private Vector2 scrollPosition;
    // Текущий выбранный узел для редактирования
    private NodeData selectedNode;
    // Временное имя для создания или переименования узлов
    private string newNodeName;

    /// <summary>
    /// Пункт меню для открытия окна редактора узлов.
    /// </summary>
    [MenuItem("Window/Node Editor")]
    public static void ShowWindow()
    {
        // Открывает окно редактора с заголовком "Node Editor"
        GetWindow<NodeEditorWindow>("Node Editor");
    }

    /// <summary>
    /// Вызывается при активации окна.
    /// </summary>
    private void OnEnable()
    {
        // Загружает все узлы при открытии окна
        LoadNodes();
    }

    /// <summary>
    /// Загружает все объекты NodeData из указанной директории и ее поддиректорий.
    /// </summary>
    private void LoadNodes()
    {
        // Находит все активы типа NodeData в папке "Assets/GameData/Nodes" и ее поддиректориях
        string[] guids = AssetDatabase.FindAssets("t:NodeData", new[] { "Assets/GameData/Nodes" });
        nodes = new List<NodeData>();
        foreach (string guid in guids)
        {
            // Преобразует GUID в путь к активу
            string path = AssetDatabase.GUIDToAssetPath(guid);
            // Загружает актив по указанному пути
            NodeData node = AssetDatabase.LoadAssetAtPath<NodeData>(path);
            if (node != null)
            {
                // Добавляет узел в список
                nodes.Add(node);
            }
        }
    }

    /// <summary>
    /// Обрабатывает отрисовку пользовательского интерфейса окна.
    /// </summary>
    private void OnGUI()
    {
        // Проверяет, инициализирован ли список узлов
        if (nodes == null)
        {
            // Загружает узлы, если список пуст
            LoadNodes();
        }

        // Отображает заголовок редактора
        EditorGUILayout.LabelField("Node Editor", EditorStyles.boldLabel);

        // Область для создания нового узла
        EditorGUILayout.LabelField("Create New Node", EditorStyles.boldLabel);
        // Поле для ввода имени нового узла
        newNodeName = EditorGUILayout.TextField("Node Name", newNodeName);
        if (GUILayout.Button("Create New Node"))
        {
            // Если имя не указано, использует имя по умолчанию
            if (string.IsNullOrWhiteSpace(newNodeName))
            {
                newNodeName = "NewNode";
            }
            // Создает новый узел с указанным именем
            CreateNewNode(newNodeName);
        }

        // Начинает область прокрутки для списка узлов
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < nodes.Count; i++)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            // Кнопка для выбора узла
            if (GUILayout.Button(nodes[i].name))
            {
                // Устанавливает выбранный узел и его имя
                selectedNode = nodes[i];
                newNodeName = selectedNode.name;
            }
            // Кнопка для удаления узла
            if (GUILayout.Button("Delete"))
            {
                // Удаляет узел и его папку
                DeleteNode(nodes[i], i);
            }
            EditorGUILayout.EndHorizontal();

            // Формирует строку с информацией о связях узла
            string connections = "";
            if (nodes[i].FirstChoice != null && nodes[i].FirstChoice.NextNode != null)
            {
                connections += "First: " + nodes[i].FirstChoice.NextNode.name + " ";
            }
            if (nodes[i].SecondChoice != null && nodes[i].SecondChoice.NextNode != null)
            {
                connections += "Second: " + nodes[i].SecondChoice.NextNode.name;
            }
            if (!string.IsNullOrEmpty(connections))
            {
                // Отображает связи узла
                EditorGUILayout.LabelField("Connections: " + connections);
            }
            EditorGUILayout.EndVertical();
        }

        // Завершает область прокрутки
        EditorGUILayout.EndScrollView();

        if (selectedNode != null)
        {
            // Добавляет отступ перед областью редактирования
            EditorGUILayout.Space();
            // Отображает заголовок редактируемого узла
            EditorGUILayout.LabelField($"Editing Node: {selectedNode.name}", EditorStyles.boldLabel);
            // Редактирует свойства выбранного узла
            EditNode(selectedNode);
            // Уведомление о необходимости нажатия кнопки для сохранения
            EditorGUILayout.LabelField("Changes are not saved until you press 'Apply Changes'.", EditorStyles.helpBox);
            if (GUILayout.Button("Apply Changes"))
            {
                // Применяет изменения, включая переименование
                ApplyChanges();
            }
        }
    }

    /// <summary>
    /// Применяет изменения к выбранному узлу, включая переименование, если необходимо.
    /// </summary>
    private void ApplyChanges()
    {
        if (selectedNode != null)
        {
            if (newNodeName != selectedNode.name)
            {
                // Получает текущий путь актива узла
                string currentPath = AssetDatabase.GetAssetPath(selectedNode);
                // Получает путь к папке узла
                string currentFolder = Path.GetDirectoryName(currentPath);
                // Получает родительскую папку
                string parentFolder = Path.GetDirectoryName(currentFolder);
                // Формирует новое имя папки
                string newFolderName = newNodeName;
                string newFolderPath = Path.Combine(parentFolder, newFolderName);

                // Проверяет, существует ли папка с новым именем
                if (AssetDatabase.IsValidFolder(newFolderPath))
                {
                    Debug.LogError("Папка с именем '" + newFolderName + "' уже существует.");
                    return;
                }

                // Перемещает папку узла
                string error = AssetDatabase.MoveAsset(currentFolder, newFolderPath);
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError("Не удалось переместить папку: " + error);
                    return;
                }

                // Переименовывает актив NodeData внутри папки
                string oldAssetName = Path.GetFileName(currentPath);
                string newAssetPath = Path.Combine(newFolderPath, newNodeName + ".asset");
                string newCurrentPath = Path.Combine(newFolderPath, oldAssetName);
                error = AssetDatabase.RenameAsset(newCurrentPath, newNodeName);
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError("Не удалось переименовать актив: " + error);
                    return;
                }

                // Обновляет ссылку на выбранный узел
                selectedNode = AssetDatabase.LoadAssetAtPath<NodeData>(newAssetPath);
                selectedNode.name = newNodeName;
            }
            // Сохраняет все изменения
            AssetDatabase.SaveAssets();
        }
    }

    /// <summary>
    /// Редактирует свойства указанного узла.
    /// </summary>
    /// <param name="node">Узел для редактирования.</param>
    private void EditNode(NodeData node)
    {
        // Поле для редактирования имени узла
        newNodeName = EditorGUILayout.TextField("Node Name", newNodeName);

        // Отслеживает изменения текста истории
        EditorGUI.BeginChangeCheck();
        node.StoryText = EditorGUILayout.TextField("Story Text", node.StoryText);
        if (EditorGUI.EndChangeCheck())
        {
            // Помечает узел как измененный
            EditorUtility.SetDirty(node);
        }

        // Отслеживает изменения фонового изображения
        EditorGUI.BeginChangeCheck();
        node.BackgroundImage = (Sprite)EditorGUILayout.ObjectField("Background Image", node.BackgroundImage, typeof(Sprite), false);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(node);
        }

        // Область для первого выбора
        EditorGUILayout.LabelField("First Choice", EditorStyles.boldLabel);
        if (node.FirstChoice == null)
        {
            if (GUILayout.Button("Create First Choice"))
            {
                // Создает первый выбор
                CreateChoice(node, "FirstChoice", ChoiceType.First);
            }
        }
        else
        {
            // Редактирует существующий первый выбор
            EditChoice(node.FirstChoice);
            if (GUILayout.Button("Remove First Choice"))
            {
                // Удаляет первый выбор
                RemoveChoice(node.FirstChoice);
                node.FirstChoice = null;
                EditorUtility.SetDirty(node);
                AssetDatabase.SaveAssets();
            }
        }

        // Область для второго выбора
        EditorGUILayout.LabelField("Second Choice", EditorStyles.boldLabel);
        if (node.SecondChoice == null)
        {
            if (GUILayout.Button("Create Second Choice"))
            {
                // Создает второй выбор
                CreateChoice(node, "SecondChoice", ChoiceType.Second);
            }
        }
        else
        {
            // Редактирует существующий второй выбор
            EditChoice(node.SecondChoice);
            if (GUILayout.Button("Remove Second Choice"))
            {
                // Удаляет второй выбор
                RemoveChoice(node.SecondChoice);
                node.SecondChoice = null;
                EditorUtility.SetDirty(node);
                AssetDatabase.SaveAssets();
            }
        }
    }

    /// <summary>
    /// Создает новый объект ChoiceData для указанного узла.
    /// </summary>
    /// <param name="node">Узел, к которому относится выбор.</param>
    /// <param name="choiceName">Базовое имя для актива выбора.</param>
    /// <param name="choiceType">Тип выбора (First или Second).</param>
    private void CreateChoice(NodeData node, string choiceName, ChoiceType choiceType)
    {
        // Получает путь к папке узла
        string nodePath = AssetDatabase.GetAssetPath(node);
        string folderPath = Path.GetDirectoryName(nodePath);
        // Формирует путь для нового актива выбора
        string choicePath = Path.Combine(folderPath, choiceName + ".asset");
        choicePath = AssetDatabase.GenerateUniqueAssetPath(choicePath);
        // Создает новый объект ChoiceData
        ChoiceData newChoice = ScriptableObject.CreateInstance<ChoiceData>();
        // Сохраняет актив в папке узла
        AssetDatabase.CreateAsset(newChoice, choicePath);
        // Устанавливает выбор в узле
        node.SetChoice(choiceType, newChoice);
        // Помечает узел как измененный
        EditorUtility.SetDirty(node);
        // Сохраняет изменения
        AssetDatabase.SaveAssets();
    }

    /// <summary>
    /// Редактирует свойства указанного объекта ChoiceData.
    /// </summary>
    /// <param name="choice">Выбор для редактирования.</param>
    private void EditChoice(ChoiceData choice)
    {
        // Отслеживает изменения текста выбора
        EditorGUI.BeginChangeCheck();
        choice.ChoiceText = EditorGUILayout.TextField("Choice Text", choice.ChoiceText);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(choice);
        }
        // Отслеживает изменения иконки выбора
        EditorGUI.BeginChangeCheck();
        choice.ChoiceIcon = (Sprite)EditorGUILayout.ObjectField("Choice Icon", choice.ChoiceIcon, typeof(Sprite), false);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(choice);
        }
        // Отслеживает изменения следующего узла
        EditorGUI.BeginChangeCheck();
        choice.NextNode = (NodeData)EditorGUILayout.ObjectField("Next Node", choice.NextNode, typeof(NodeData), false);
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(choice);
        }
    }

    /// <summary>
    /// Удаляет указанный объект ChoiceData.
    /// </summary>
    /// <param name="choice">Выбор для удаления.</param>
    private void RemoveChoice(ChoiceData choice)
    {
        // Получает путь к активу выбора
        string path = AssetDatabase.GetAssetPath(choice);
        // Удаляет актив
        AssetDatabase.DeleteAsset(path);
    }

    /// <summary>
    /// Создает новый объект NodeData с указанным именем.
    /// </summary>
    /// <param name="nodeName">Имя нового узла.</param>
    private void CreateNewNode(string nodeName)
    {
        // Базовый путь для папки узлов
        string basePath = "Assets/GameData/Nodes";
        string folderPath = Path.Combine(basePath, nodeName);
        // Обеспечивает уникальность имени папки
        int counter = 1;
        string uniqueFolderPath = folderPath;
        while (AssetDatabase.IsValidFolder(uniqueFolderPath))
        {
            uniqueFolderPath = folderPath + "_" + counter;
            counter++;
        }
        // Создает папку для узла
        string folderGuid = AssetDatabase.CreateFolder(basePath, Path.GetFileName(uniqueFolderPath));
        if (string.IsNullOrEmpty(folderGuid))
        {
            Debug.LogError("Не удалось создать папку: " + uniqueFolderPath);
            return;
        }
        // Создает актив NodeData внутри папки
        string assetPath = Path.Combine(uniqueFolderPath, nodeName + ".asset");
        assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);
        NodeData newNode = ScriptableObject.CreateInstance<NodeData>();
        newNode.name = nodeName;
        AssetDatabase.CreateAsset(newNode, assetPath);
        // Сохраняет изменения
        AssetDatabase.SaveAssets();
        // Перезагружает список узлов
        LoadNodes();
    }

    /// <summary>
    /// Удаляет указанный узел и его папку.
    /// </summary>
    /// <param name="node">Узел для удаления.</param>
    /// <param name="index">Индекс узла в списке.</param>
    private void DeleteNode(NodeData node, int index)
    {
        if (node != null)
        {
            // Получает путь к активу узла
            string assetPath = AssetDatabase.GetAssetPath(node);
            // Получает путь к папке узла
            string folderPath = Path.GetDirectoryName(assetPath);
            // Удаляет всю папку с активами
            AssetDatabase.DeleteAsset(folderPath);
            // Удаляет узел из списка
            nodes.RemoveAt(index);
            // Сохраняет изменения
            AssetDatabase.SaveAssets();
        }
    }
}