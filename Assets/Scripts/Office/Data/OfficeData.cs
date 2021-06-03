using UnityEngine;

namespace Office.Data
{
    [CreateAssetMenu(fileName = "OfficeData", menuName = "Zombie Hospital/Office Data", order = 0)]
    public class OfficeData : ScriptableObject
    {
        public Vector3 cameraStartPosition;
        // public Vector3 cameraStartRotation;
        public float cameraStartAngle;
        public float minAngle;
        public float maxAngle;
        public float cameraSpeed;
        public float lookDistance;

        public float scaleSpeed;
    }
}