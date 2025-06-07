using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[CreateAssetMenu(fileName = "NewImageSet", menuName = "Game/ImageSet")]
public class ImageSet : ScriptableObject
{
    [SerializeField] private string nameId;
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private NodeData nextNode;

    public string NameId => nameId;
    public IReadOnlyList<Sprite> Sprites => sprites;
    public NodeData NextNode => nextNode;

    // Метод для заполнения списка спрайтов из папки
    public void PopulateSpritesFromFolder(string folderPath)
    {
        if (string.IsNullOrEmpty(folderPath))
        {
            Debug.LogError("Путь к папке не указан!");
            return;
        }

        // Очистить текущий список
        sprites.Clear();

        // Получить все пути к файлам в папке
        string[] assetPaths = AssetDatabase.FindAssets("t:Sprite", new[] { folderPath });

        foreach (string guid in assetPaths)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            if (sprite != null)
            {
                sprites.Add(sprite);
            }
        }

        // Обновить инспектор
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
}