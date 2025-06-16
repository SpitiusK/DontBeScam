using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ImageSet))]
public class ImageSetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ImageSet imageSet = (ImageSet)target;

        if (GUILayout.Button("Заполнить из папки"))
        {
            string folderPath = EditorUtility.OpenFolderPanel("Выберите папку со спрайтами", "Assets/GameData/ImageSetValue", "");
            if (!string.IsNullOrEmpty(folderPath))
            {
                // Преобразовать путь в формат, понятный Unity
                string relativePath = "Assets" + folderPath.Replace(Application.dataPath, "").Replace("\\", "/");
                imageSet.PopulateSpritesFromFolder(relativePath);
            }
        }
    }
}