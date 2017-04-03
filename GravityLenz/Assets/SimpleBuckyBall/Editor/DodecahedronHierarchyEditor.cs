using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Platipus
{
	[CustomEditor(typeof(DodecahedronHierarchy))]
	public class DodecahedronHierarchyEditor : UnityEditor.Editor {

		private DodecahedronHierarchy dodeca;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			dodeca = (DodecahedronHierarchy) target;

			if( GUI.changed )
			{
				dodeca.initializeDodeca();
			}
		}

	}
}