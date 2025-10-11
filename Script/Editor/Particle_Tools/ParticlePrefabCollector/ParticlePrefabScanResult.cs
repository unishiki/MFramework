#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace Game.Editor.ParticlePrefabCollector
{
    
    [System.Serializable]
    public class ParticlePrefabScanResult : ScriptableObject
    {
        public List<string> prefabPaths = new List<string>();
        public List<string> scanFolders = new List<string>();
        public System.DateTime ScanTime;
    }
}
#endif