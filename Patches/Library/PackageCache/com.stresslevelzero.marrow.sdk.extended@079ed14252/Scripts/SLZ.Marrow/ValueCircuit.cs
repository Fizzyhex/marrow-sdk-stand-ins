using UnityEngine;

namespace SLZ.Marrow.Circuits
{
    [AddComponentMenu("MarrowSDK/Circuits/Nodes/Value Node")]
    public class ValueCircuit : Circuit
    {
        [Tooltip("The Value Node outputs the specified constant value")]
        [SerializeField]
        private float _value = 1f;
        public float Value
        {
            get => _value;
            set
            {
                _value = value;
            }
        }
    }
}