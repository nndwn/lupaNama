using System;
using CultureShock.Scripts.Main;
using UnityEditor;
using UnityEngine;

namespace CultureShock.Editor
{
    [CustomEditor(typeof(Validasi))]
    public class ValidasiEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            if (target is Validasi validasi)
            {
                validasi.GetObject();
            }
        }
    }

    [CustomEditor(typeof(SingletonData))]
    public class SingletonDataEditor : UnityEditor.Editor
    {
        // private void OnEnable()
        // {
        //     var data = target as Data;
        //     if (data != null)
        //     {
        //         data.GetAlbum();
        //     }
        // }
    }
    [CustomEditor(typeof(CreateAlbum))]
    public class CreateAlbumEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (target is CreateAlbum createAlbum)
                createAlbum.cover =
                    EditorGUILayout.ObjectField("Target Change Sprite", createAlbum.cover, typeof(Sprite), true) as
                        Sprite;
            DrawDefaultInspector();
        }
    }
    [CustomEditor(typeof(CheckAnalisa))]
    public class AnalisaEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            var analisa = target as CheckAnalisa;
            if (analisa != null) analisa.GetObject();
        }
    }
    [CustomEditor(typeof(CreateClip))]
    public class CreateClipEditor : UnityEditor.Editor
    {
        private void OnEnable()
        {
            if (target is CreateClip createClip)
            {
                if (createClip.settings is null) return;
                createClip.FillField();
                
            }
        }

        public override void OnInspectorGUI()
        {
            if (target is CreateClip createClip)
            {
                createClip.settings = EditorGUILayout.ObjectField("Setting", createClip.settings, typeof(Settings),true) as Settings;
                if (createClip.settings is null) return;
                createClip.cover =
                    EditorGUILayout.ObjectField("Target Change Sprite", createClip.cover, typeof(Sprite), true) as
                        Sprite;
                createClip.level =  EditorGUILayout.Popup("Difficult", createClip.level, createClip.settings.nameLevel);

            }
            DrawDefaultInspector();
        }
    }

    [CustomEditor(typeof(Settings))]
    public class SettingsScritableEditor : UnityEditor.Editor
    {
        private SerializedProperty _posXbutton;
        private SerializedProperty _albumName;
        private SerializedProperty _nameMode;
        private SerializedProperty _tilebase;
        private SerializedProperty _levelName;
        private SerializedProperty _rateName;
        private SerializedProperty _rateLimit;
        private SerializedProperty _ratePoint;

        public void OnEnable()
        {
            _posXbutton = serializedObject.FindProperty("posXButton");
            _albumName = serializedObject.FindProperty("nameAlbum");
            _nameMode = serializedObject.FindProperty("nameMode");
            _tilebase = serializedObject.FindProperty("tileBase");
            _levelName = serializedObject.FindProperty("nameLevel");
            _rateName = serializedObject.FindProperty("nameRate");
            _rateLimit = serializedObject.FindProperty("limitRate");
            _ratePoint = serializedObject.FindProperty("ratePoint");
        }


        public override void OnInspectorGUI()
        {
            if (target is Settings settings)
            {

                void FormatField(int reference, int count, SerializedProperty property, string nameString)
                {
                    EditorGUI.indentLevel++;
                    if (reference != 0)
                    {
                        if (count == 0 || count != reference)
                            property.arraySize = reference;
                        EditorGUILayout.PropertyField(property, new GUIContent(nameString));

                    }
                    EditorGUI.indentLevel--;
                }

                serializedObject.Update();
                settings.countButton = EditorGUILayout.IntField("Count Button", settings.countButton);
                FormatField(settings.countButton, settings.posXButton.Length, _posXbutton, "button position");

                settings.countAlbum = EditorGUILayout.IntField("Count Album", settings.countAlbum);
                FormatField(settings.countAlbum, settings.nameAlbum.Length, _albumName, "Album");

                settings.countMode = EditorGUILayout.IntField("Count Mode", settings.countMode);
                FormatField(settings.countMode, settings.nameMode.Length, _nameMode, "Mode");
                
                settings.tileBaseCount = EditorGUILayout.IntField("Count Tilebase", settings.tileBaseCount);
                FormatField(settings.tileBaseCount, settings.tileBase.Length, _tilebase, "tilebase");

                settings.level = EditorGUILayout.IntField("Count Level", settings.level);
                FormatField(settings.level, settings.nameLevel.Length, _levelName, "Name Level");

                settings.rate = EditorGUILayout.IntField("Count Rate", settings.rate);
                FormatField(settings.rate, settings.nameRate.Length, _rateName, "Name Rate");
                FormatField(settings.rate, settings.limitRate.Length, _rateLimit, "Limit Rate");
                FormatField(settings.rate, settings.ratePoint.Length, _ratePoint, "Point Rate");
                serializedObject.ApplyModifiedProperties();
            }
            DrawDefaultInspector();
        }
    }

}