using System.Collections.Generic;
using UnityEngine;

namespace Limb.Data
{
    [CreateAssetMenu(fileName = "LimbData", menuName = "Zombie Hospital/Limb Data", order = 0)]
    public class LimbData : ScriptableObject
    {
        [Header("Zombie")]
        public Vector3 zombiePosition;
        public Vector3 zombieRotation;
        public GameObject teeth;
        public GameObject teethTop;
        public GameObject teethBottom;

        [Header("Camera")]
        public Vector3 cameraStartPosition;
        public Vector3 cameraStartRotation;
        
        public Vector3 cameraDetachPosition;
        public Vector3 cameraDetachRotation;

        public Vector3 cameraSelectPosition;
        public Vector3 cameraSelectRotation;
        
        public Vector3 cameraScrewPosition;
        public Vector3 cameraScrewRotation;

        public float cameraSpeed;

        [Header("Tray")] public GameObject tray;
        public float traySpeed;
        public Vector3 trayStartPosition;
        public Vector3 trayPosition;
        //public Vector3 trayRotation;
        //public List<GameObject> handPrefabs;

        [Header("Particles")] 
        public GameObject bloodPrefab;

        [Header("Screw")] public GameObject screwPrefab;
        public Vector3 screwPosition;

        [Header("Screwdriver")] public GameObject screwdriverPrefab;
        public Vector3 screwdriverPosition;

        [Header("Spin")] public int rotationSpeed;
        public float spinDistance;
    }
}