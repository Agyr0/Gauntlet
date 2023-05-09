using UnityEditor;
using UnityEngine.UI;
using UnityEngine;


[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    #region Variables

    SerializedProperty state;

    SerializedProperty levelCanvas;
    SerializedProperty title;
    SerializedProperty levelText;

    SerializedProperty warrior;
    SerializedProperty warriorHealth;
    SerializedProperty warriorScore;
    SerializedProperty warriorInventory;

    SerializedProperty valkyrie;
    SerializedProperty valkyrieHealth;
    SerializedProperty valkyrieScore;
    SerializedProperty valkyrieInventory;

    SerializedProperty wizzard;
    SerializedProperty wizzardHealth;
    SerializedProperty wizzardScore;
    SerializedProperty wizzardInventory;

    SerializedProperty elf;
    SerializedProperty elfHealth;
    SerializedProperty elfScore;
    SerializedProperty elfInventory;

    #endregion

    bool levelGroup, warriorGroup, valkyrieGroup, wizzardGroup, elfGroup = false;

    private void OnEnable()
    {
        state = serializedObject.FindProperty("state");

        levelCanvas = serializedObject.FindProperty("levelCanvas");
        title = serializedObject.FindProperty("title");
        levelText = serializedObject.FindProperty("levelText");

        warrior = serializedObject.FindProperty("warrior");
        warriorHealth = serializedObject.FindProperty("warriorHealth");
        warriorScore = serializedObject.FindProperty("warriorScore");
        warriorInventory = serializedObject.FindProperty("warriorInventory");

        valkyrie = serializedObject.FindProperty("valkyrie");
        valkyrieHealth = serializedObject.FindProperty("valkyrieHealth");
        valkyrieScore = serializedObject.FindProperty("valkyrieScore");
        valkyrieInventory = serializedObject.FindProperty("valkyrieInventory");

        wizzard = serializedObject.FindProperty("wizzard");
        wizzardHealth = serializedObject.FindProperty("wizzardHealth");
        wizzardScore = serializedObject.FindProperty("wizzardScore");
        wizzardInventory = serializedObject.FindProperty("wizzardInventory");

        elf = serializedObject.FindProperty("elf");
        elfHealth = serializedObject.FindProperty("elfHealth");
        elfScore = serializedObject.FindProperty("elfScore");
        elfInventory = serializedObject.FindProperty("elfInventory");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.indentLevel++;
        EditorGUILayout.Space(10);
        EditorGUILayout.PropertyField(state);
        EditorGUILayout.Space(10);

        #region Level
        levelGroup = EditorGUILayout.BeginFoldoutHeaderGroup(levelGroup, "Level Canvas");
        if (levelGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(levelCanvas);
            EditorGUILayout.PropertyField(title);
            EditorGUILayout.PropertyField(levelText);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorGUILayout.Space(10);

        #region Warrior
        warriorGroup = EditorGUILayout.BeginFoldoutHeaderGroup(warriorGroup, "Warrior");
        if (warriorGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(warrior);
            EditorGUILayout.PropertyField(warriorHealth);
            EditorGUILayout.PropertyField(warriorScore);
            EditorGUILayout.PropertyField(warriorInventory);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorGUILayout.Space(10);

        #region Valkyrie
        valkyrieGroup = EditorGUILayout.BeginFoldoutHeaderGroup(valkyrieGroup, "Valkyrie");
        if (valkyrieGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(valkyrie);
            EditorGUILayout.PropertyField(valkyrieHealth);
            EditorGUILayout.PropertyField(valkyrieScore);
            EditorGUILayout.PropertyField(valkyrieInventory);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorGUILayout.Space(10);

        #region Wizzard
        wizzardGroup = EditorGUILayout.BeginFoldoutHeaderGroup(wizzardGroup, "Wizzard");
        if (wizzardGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(wizzard);
            EditorGUILayout.PropertyField(wizzardHealth);
            EditorGUILayout.PropertyField(wizzardScore);
            EditorGUILayout.PropertyField(wizzardInventory);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorGUILayout.Space(10);

        #region Elf
        elfGroup = EditorGUILayout.BeginFoldoutHeaderGroup(elfGroup, "Elf");
        if (elfGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(elf);
            EditorGUILayout.PropertyField(elfHealth);
            EditorGUILayout.PropertyField(elfScore);
            EditorGUILayout.PropertyField(elfInventory);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

    }
}