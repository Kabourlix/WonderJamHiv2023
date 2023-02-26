using System;
using UnityEngine;

namespace Game.Scripts.Enemies
{
    public class Patroller : MonoBehaviour
    {
        [SerializeField] private Waypoints[] sequences;
        [SerializeField] private bool loop = true;

        private int _indexWaypoints = 0;
        private int _indexSequences = 0;

        
        public Transform GetNextWaypoint()
        {
            if(_indexSequences >= sequences.Length) return null;
            
            var seq = sequences[_indexSequences];
            _indexWaypoints = seq.loop ? (_indexWaypoints + 1) % seq.waypoints.Length : _indexWaypoints + 1;
            return _indexWaypoints >= seq.waypoints.Length ? null : seq.waypoints[_indexWaypoints];
        }

        public void NextSequence()
        {
            _indexSequences = loop ? (_indexSequences + 1) % sequences.Length : _indexSequences + 1;
            _indexWaypoints = 0;
        }

        
    }
    
    [Serializable]
    public struct Waypoints
    {
        public Transform[] waypoints;
        public bool loop;
    }
}
