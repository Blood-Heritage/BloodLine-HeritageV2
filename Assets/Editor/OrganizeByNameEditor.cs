using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class OrganizeObjectsEditor : EditorWindow
{
    private string parentName = "NewParent";
    private string searchPrefix = "";

    private Transform currentRoot = null;
    private Stack<Transform> navigationStack = new();

    [MenuItem("Tools/Organize Objects by Prefix")]
    public static void ShowWindow()
    {
        GetWindow<OrganizeObjectsEditor>("Organize Objects");
    }

    void OnGUI()
    {
        GUILayout.Label("Organize Objects in Hierarchy", EditorStyles.boldLabel);
        
        GUILayout.Space(20);
        GUILayout.Label("Navigate GameObjects", EditorStyles.boldLabel);

        Transform[] children = GetCurrentChildren();
        int count = Mathf.Min(20, children.Length);
        for (int i = 0; i < count; i++)
        {
            Transform child = children[i];
            if (GUILayout.Button(child.name))
            {
                navigationStack.Push(currentRoot);
                currentRoot = child;
            }
        }

        GUILayout.Space(10);
        GUIStyle backButtonStyle = new GUIStyle(GUI.skin.button);
        backButtonStyle.fixedHeight = 30; // Doble de alto
        if (GUILayout.Button("Ir Atrás", backButtonStyle))
        {
            if (navigationStack.Count > 0)
            {
                currentRoot = navigationStack.Pop();
            }
        }
        
        GUILayout.Space(30);
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

        GameObject parentObj = GameObject.Find(parentName) ?? new GameObject(parentName);

        IEnumerable<GameObject> searchScope;

        if (currentRoot == null)
        {
            searchScope = GetRootObjects();
        }
        else
        {
            searchScope = GetChildren(currentRoot);
        }

        foreach (GameObject obj in searchScope)
        {
            if (obj.name.StartsWith(searchPrefix))
            {
                Undo.RecordObject(obj.transform, "Reparent Object");
                obj.transform.SetParent(parentObj.transform);
                EditorUtility.SetDirty(obj);
            }
        }
    }

    Transform[] GetCurrentChildren()
    {
        if (currentRoot == null)
        {
            return GetRootTransforms();
        }
        else
        {
            List<Transform> result = new();
            foreach (Transform child in currentRoot)
                result.Add(child);
            return result.ToArray();
        }
    }

    GameObject[] GetChildren(Transform parent)
    {
        List<GameObject> result = new();
        foreach (Transform child in parent)
        {
            result.Add(child.gameObject);
        }
        return result.ToArray();
    }

    Transform[] GetRootTransforms()
    {
        List<Transform> rootTransforms = new();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
        {
            if (go.transform.parent == null && go.scene.IsValid())
            {
                rootTransforms.Add(go.transform);
            }
        }
        return rootTransforms.ToArray();
    }

    GameObject[] GetRootObjects()
    {
        List<GameObject> roots = new();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.parent == null && obj.scene.IsValid())
            {
                roots.Add(obj);
            }
        }
        return roots.ToArray();
    }
}
