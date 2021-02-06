using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Callbacks;

public class SceneCollectionsWindow : EditorWindow
{
    string _newCollectionName = "New Scene Collection";

    [MenuItem ("Tools/Scene Collections")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(SceneCollectionsWindow), true, "Save Scene Collection", true);
    }

    void OnGUI() {
        GUILayout.Box("To save the current Scene set up in the Hierarchy as a Scene Collection, enter a name below and click the Save Scene Collection button.");

        EditorGUILayout.Space();

        _newCollectionName = EditorGUILayout.TextField("Name", _newCollectionName);

        if(GUILayout.Button("Save Scene Collection")) {
            if(!AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/SceneCollections/" + _newCollectionName + ".asset") || 
                EditorUtility.DisplayDialog("Warning", "A Scene collection by that name already exists. Do you want to overwrite it?", "Yes", "Cancel")) {

                SceneSetup[] setups = EditorSceneManager.GetSceneManagerSetup();
                if (_newCollectionName.Equals("")) _newCollectionName = "New Scene Collection";
                SceneCollection collection = Create(_newCollectionName, setups);
            }
        }

        EditorGUILayout.Space();

        GUILayout.Box("Scene Collection assets are saved in Assets/SceneCollections. To load a Scene Collection, open the asset by double clicking it in the Project view, like you would a normal Scene.");

    }

    public static SceneCollection Create(string name, SceneSetup[] setups) {
        SceneCollection collection = ScriptableObject.CreateInstance<SceneCollection>();
        collection.name = name;
        collection.setups = setups;
        if (!AssetDatabase.IsValidFolder("Assets/SceneCollections"))
            AssetDatabase.CreateFolder("Assets", "SceneCollections");
        AssetDatabase.CreateAsset(collection, "Assets/SceneCollections/" + name + ".asset");
        return collection;
    }

    [OnOpenAssetAttribute(0)]
    public static bool OpenSceneCollection(int instanceID, int line) {
        Object obj = EditorUtility.InstanceIDToObject(instanceID);
        string path = AssetDatabase.GetAssetPath(obj);
        if (AssetDatabase.GetMainAssetTypeAtPath(path).ToString().Equals("SceneCollection")) {
            SceneSetup[] setups = ((SceneCollection)obj).setups;
            if (setups.Length > 0)
                EditorSceneManager.RestoreSceneManagerSetup(((SceneCollection)obj).setups);
            else
                Debug.LogError("Could not load " + obj.name + ": it contains no Scene set ups.");
            return true;
        }
        return false;
    }
}
