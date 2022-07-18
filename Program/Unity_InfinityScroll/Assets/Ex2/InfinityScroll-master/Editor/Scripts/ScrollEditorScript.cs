using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Ex2
{

	[CustomEditor(typeof(InfiniteScroll), true)]
	public class LevelScriptEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			InfiniteScroll scroll = (InfiniteScroll)target;
			scroll.initOnAwake = EditorGUILayout.Toggle("Init on awake", scroll.initOnAwake);
			base.OnInspectorGUI();
		}
	}
}