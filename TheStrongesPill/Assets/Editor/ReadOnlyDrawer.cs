using UnityEditor;
using UnityEngine;

// Этот класс определяет, как поля с атрибутом ReadOnly должны отрисовываться в редакторе.
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false; // Отключаем возможность изменения поля
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true; // Включаем обратно возможность изменения полей после этого поля
    }
}