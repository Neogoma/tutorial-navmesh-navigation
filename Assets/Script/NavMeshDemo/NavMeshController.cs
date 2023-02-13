using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
namespace Neogoma.Stardust.Demo.NavMeshDemo
{
    /// <summary>
    /// Hold necessary data for navigation. bakes navmesh on start
    /// </summary>
    public class NavMeshController : MonoBehaviour
    {
        /// <summary>
        /// Map unique identifier.
        /// </summary>
        public string mapUUID;
        /// <summary>
        /// Nav Mesh Surface
        /// </summary>
        private NavMeshSurface surface;
        // Start is called before the first frame update
        void Start()
        {
            surface = GetComponent<NavMeshSurface>();
            surface.BuildNavMesh();
        }
    }
}
