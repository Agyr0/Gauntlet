using UnityEditor;
using UnityEngine.UI;
using UnityEngine;


[CustomEditor(typeof(UIManager))]
public class UIManagerEditor : Editor
{
    #region Variables

    SerializedProperty state;
    SerializedProperty _eventSystem;

    SerializedProperty startCanvas;     
    SerializedProperty startFirstSelectedButton;

    SerializedProperty pauseCanvas;     
    SerializedProperty pauseFirstSelectedButton;

    SerializedProperty gameOverCanvas;
    SerializedProperty gameOverFirstSelectedButton;
    SerializedProperty creditsCanvas;
    SerializedProperty creditsFirstSelectedButton;

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

    bool  levelGroup, startGroup, pauseGroup, warriorGroup, valkyrieGroup, wizzardGroup, elfGroup, gameOverGroup, creditsGroup = false;      

    private void OnEnable()
    {
        state = serializedObject.FindProperty("state");

        startCanvas = serializedObject.FindProperty("startCanvas");     
        startFirstSelectedButton = serializedObject.FindProperty("startFirstSelectedButton");     

        pauseCanvas = serializedObject.FindProperty("pauseCanvas");     
        pauseFirstSelectedButton = serializedObject.FindProperty("pauseFirstSelectedButton");     

        gameOverCanvas = serializedObject.FindProperty("gameOverCanvas");
        gameOverFirstSelectedButton = serializedObject.FindProperty("gameOverFirstSelectedButton");

        creditsCanvas = serializedObject.FindProperty("creditsCanvas");
        creditsFirstSelectedButton = serializedObject.FindProperty("creditsFirstSelectedButton");

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
        EditorGUILayout.PropertyField(_eventSystem);
        EditorGUILayout.Space(10);
        EditorGUI.indentLevel--;

        #region Start
        startGroup = EditorGUILayout.BeginFoldoutHeaderGroup(startGroup, "Start Canvas");       
        if (startGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(startCanvas);
            EditorGUILayout.PropertyField(startFirstSelectedButton);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorGUILayout.Space(10);

        #region Pause
        pauseGroup = EditorGUILayout.BeginFoldoutHeaderGroup(pauseGroup, "Pause Canvas");       
        if (pauseGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(pauseCanvas);
            EditorGUILayout.PropertyField(pauseFirstSelectedButton);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorGUILayout.Space(10);

        #region Game Over
        gameOverGroup = EditorGUILayout.BeginFoldoutHeaderGroup(gameOverGroup, "Game Over Canvas");       
        if (gameOverGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(gameOverCanvas);
            EditorGUILayout.PropertyField(gameOverFirstSelectedButton);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

        EditorGUILayout.Space(10);

        #region Credits
        creditsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(creditsGroup, "Credits Canvas");
        if (creditsGroup)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.PropertyField(creditsCanvas);
            EditorGUILayout.PropertyField(creditsFirstSelectedButton);
            EditorGUILayout.Space(10);

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        #endregion

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

        serializedObject.ApplyModifiedProperties();

    }
}