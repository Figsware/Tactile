using UnityEngine;

namespace Tactile.Gadgets.InspectorUtility
{
    public class MeshRendererUtility : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;   
        
        public void SetColor(Color color)
        {
            meshRenderer.material.color = color;
        }

        public void SetColor(string hex)
        {
            
        }
    }
}