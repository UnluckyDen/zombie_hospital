using System.Collections.Generic;
using UnityEngine;

namespace Trepanation.Data
{
    [CreateAssetMenu(fileName = "TrepanationData", menuName = "Zombie Hospital/Trepanation Data", order = 0)]
    public class TrepanationData : ScriptableObject
    {
        [Header("Zombie")]
        public GameObject teethPrefab;
        public GameObject teethPrefabTop;
        public GameObject teethPrefabBottom;
        
        public Vector3 zombiePosition;
        public Vector3 zombieRotation;
        
        public float zombieRotateSpeed;

        [Header("Camera")]
        public Vector3 cameraStartPosition;
        public Vector3 cameraStartRotation;
        
        public Vector3 cameraTrepanationPosition;
        public Vector3 cameraTrepanationRotation;
        
        public Vector3 cameraExtractionPosition;
        public Vector3 cameraExtractionRotation;
        
        public Vector3 cameraInsertPosition;
        public Vector3 cameraInsertRotation;
        
        public float cameraSpeed;

        [Header("Grinder Tool")]
        public GameObject grinderGameObject;
        public Vector3 grinderStartPosition;
        public Vector3 grinderEndPosition;
        
        public Vector3 grinderRotation;
        public Vector3 grinderUseRotation;
        
        public Vector3 grinderShiftPosition;
        public Vector3 grinderUseShiftPosition;

        [Header("Tray")] 
        public GameObject trayGameObject;
        
        public Vector3 trayStartPosition;
        public Vector3 trayInsertPosition;
        
        public Vector3 trayRotation;

        public List<GameObject> trayItems;

        [Header("Stapler")]
        public GameObject staplerGameObject;
        public Vector3 staplerStartPosition;
        public Vector3 staplerEndPosition;
        
        public Vector3 staplerRotation;
        public Vector3 staplerUseRotation;

        public Vector3 staplerShiftPosition;
        public Vector3 staplerUseShiftPosition;

        public int staplesCount;

        [Header("Staple")]
        public GameObject stapleGameObject;
        public Vector3 stapleShiftPosition;
        public Vector3 stapleShiftRotation;
    }
}