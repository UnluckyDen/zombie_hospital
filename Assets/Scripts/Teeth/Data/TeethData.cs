using System.Collections.Generic;
using UnityEngine;

namespace Teeth.Data
{
    [CreateAssetMenu(fileName = "TeethData", menuName = "Zombie Hospital/Teeth Data", order = 0)]
    public class TeethData : ScriptableObject
    {
        [Header("Zombie")]
        public Vector3 zombiePosition;

        [Header("Hammer")] public GameObject hammerPrefab;
        public Vector3 hammerPosition;
        public float hammerSpeed;
        public float xRange;

        [Header("Tray")] public GameObject trayPrefab;
        public Vector3 trayStartPosition;
        public Vector3 trayPosition;

        [Header("Camera")]
        public Vector3 cameraStartPosition;
        public Vector3 cameraStartRotation;
        
        public Vector3 cameraTeethPosition;
        public Vector3 cameraTeethRotation;
        
        public Vector3 cameraInsertPosition;
        public Vector3 cameraInsertRotation;
        
        public float cameraSpeed;

        [Header("Teeth")] public List<Transform> teeth;
    }
}