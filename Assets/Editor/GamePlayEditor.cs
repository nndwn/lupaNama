#if UNITY_EDITOR
using System.Collections.Generic;
using CultureShock.Scripts.GamePlay;
using CultureShock.Scripts.Main;
using UnityEditor;
using UnityEngine;

namespace CultureShock.Editor
{


    [CustomEditor(typeof(MoveBgAndCamera))]
    public class MoveBgAndCameraEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var moveBgCamera = target as MoveBgAndCamera;
            if (moveBgCamera != null) moveBgCamera.GetObject();
        }
    }

    [CustomEditor(typeof(AllTriggerAction))]
    public class AllTriggerEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var allTrigger = target as AllTriggerAction;
            if (allTrigger != null)
            {
                allTrigger.GetObject();

            }
        }
    }

    [CustomEditor(typeof(InputGame))]
    public class InputGameEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var inputGame = target as InputGame;
            if (inputGame != null) inputGame.GetObject();

        }
    }

    [CustomEditor(typeof(TextActionPanel))]
    public class TextActionPanelEditor : UnityEditor.Editor
    {

        private bool[] _foldoutGroup;
        private void OnEnable()
        {
            if (target is TextActionPanel textActionPanel)
            {
                textActionPanel.FindGameObject();
            }
        }

    }

    [CustomEditor(typeof(GameController))]
    public class ControllerEditor : UnityEditor.Editor
    {


        //private string[] _configStrings = new[] { "common", "rated text" };
        //private int _configInt;

        private void OnEnable()
        {
            if (target is GameController controller)
            {
                controller.GetObject();
            }
        }

        public override void OnInspectorGUI()
        {
            // _configInt = EditorGUILayout.Popup("Configuration", _configInt, _configStrings);
            if (target is GameController controller)
            {
                if (!Application.isPlaying)
                {
                    controller.album = EditorGUILayout.Popup("Album", controller.album, controller.AlbumStrings);
                    controller.clip = EditorGUILayout.Popup("Clip", controller.clip, controller.ClipString);
                    controller.mode = EditorGUILayout.Popup("Mode", controller.mode, controller.ModeString);

                    if (!controller.autoPlay)
                    {
                        controller.modeCreate = EditorGUILayout.Toggle("Create Mode", controller.modeCreate);
                        if (controller.modeCreate)
                        {
                            controller.autoPlay = false;
                        }
                    }
                }
                if (!controller.modeCreate)
                {
                    controller.autoPlay = EditorGUILayout.Toggle("Auto Play", controller.autoPlay);
                    if (controller.autoPlay)
                    {
                        controller.holdDelay = EditorGUILayout.FloatField("Delay", controller.holdDelay);
                        controller.modeCreate = false;
                    }
                }

                

                if (controller.modeCreate)
                {
                    if (GUILayout.Button("Remove All Tile")) controller.tilemap.ClearAllTiles();
                    
                    if (GUILayout.Button("Save Tile")) controller.Save = true;
                }
                // EditorGUI.BeginDisabledGroup(true);
                // _configInt = EditorGUILayout.IntField("test", _configInt);
                // EditorGUI.EndDisabledGroup();
            }

            DrawDefaultInspector();
        }
    }
    [CustomEditor(typeof(CreateMode))]
    public class CreateModeEditor : UnityEditor.Editor
    {
        private SerializedProperty _listAlbum;
        
        public void OnEnable()
        {
            _listAlbum = serializedObject.FindProperty("listAlbum");
            if (target is CreateMode createMode)
            {
                createMode.GetObject();
             
            }
        }


    }
}

#endif