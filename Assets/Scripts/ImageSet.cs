using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewImageSet", menuName = "Game/ImageSet")]
public class ImageSet : ScriptableObject
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private NodeData nextNode;
    [SerializeField] private bool isEnd;

    public bool IsEnd => isEnd;
    public IReadOnlyList<Sprite> Sprites => sprites;
    public NodeData NextNode => nextNode;

    /// <summary>
    /// Возвращает true, если список спрайтов пуст или не инициализирован.
    /// </summary>
    public bool IsEmpty => sprites == null || sprites.Count == 0;

#if UNITY_EDITOR
    // Метод для заполнения списка спрайтов из папки
    public void PopulateSpritesFromFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("Путь к папке не указан!");
            return;
        }

        // Очистить текущий список
        sprites?.Clear();

        // Получить все пути к файлам в папке
        string[] assetPaths = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });

        foreach (string guid in assetPaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null)
            {
                sprites ??= new List<Sprite>(); // Инициализация, если null
                sprites.Add(sprite);
            }
        }

        // Обновить инспектор
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}