using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XomracCore.DialogueSystem.SerializedData;

namespace XomracCore.DialogueSystem
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue System/New Dialogue")]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] private string _dialogueName;
        public string DialogueName => _dialogueName;

        [SerializeField] private Speaker _defaultSpeaker;
        public Speaker DefaultSpeaker => _defaultSpeaker;
        
        [SerializeReference] private List<NodeData> _nodes = new();
        public List<NodeData> Nodes => _nodes;
        
        [SerializeReference] private List<EdgeData> _edges = new();
        public List<EdgeData> Edges => _edges;
        
        public void Clear()
        {
            _nodes.Clear();
            _edges.Clear();
        }
	
        public void SetNodes(List<NodeData> newNodes)
        {
            _nodes.Clear();
            _nodes.AddRange(newNodes);
        }
	
        public void SetEdges(List<EdgeData> newEdges)
        {
            _edges.Clear();
            _edges.AddRange(newEdges);
        }
	
#if UNITY_EDITOR

        public void Save()
        {
            Debug.Log("Saving Dialogue");
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
        
        

        
    }

}
