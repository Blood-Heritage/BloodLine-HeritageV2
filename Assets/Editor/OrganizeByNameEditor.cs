using UnityEngine;
using UnityEditor;

public class OrganizeObjectsEditor : EditorWindow
{
    private string parentName = "NewParent"; // Default parent name
    private string searchPrefix = ""; // Default search prefix

    [MenuItem("Tools/Organize Objects by Prefix")]
    public static void ShowWindow()
    {
        GetWindow<OrganizeObjectsEditor>("Organize Objects");
    }

    void OnGUI()
    {
        GUILayout.Label("Organize Objects in Hierarchy", EditorStyles.boldLabel);

        // Input fields
        searchPrefix = EditorGUILayout.TextField("Search Prefix:", searchPrefix);
        parentName = EditorGUILayout.TextField("Parent Name:", parentName);

        GUILayout.Space(10);

        if (GUILayout.Button("Organize Objects"))
        {
            OrganizeObjects();
        }
    }

    void OrganizeObjects()
    {
        if (string.IsNullOrEmpty(searchPrefix) || string.IsNullOrEmpty(parentName))
        {
            Debug.LogWarning("Please enter a valid search prefix and parent name.");
            return;
        }

        GameObject parentObj = GameObject.Find(parentName) ?? new GameObject(parentName); // Create parent if missing
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent == null && obj.name.StartsWith(searchPrefix)) // Only unparented objects
            {
                Undo.RecordObject(obj.transform, "Reparent Object");
                obj.transform.SetParent(parentObj.transform);
                EditorUtility.SetDirty(obj);
            }
        }
    }
}