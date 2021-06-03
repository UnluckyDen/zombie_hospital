using System.Collections.Generic;
using Extension;
using UnityEngine;

namespace UnityComponents
{
    public class LocationManager : SingletonDestroy<LocationManager>
    {
        [System.Serializable]
        public struct ReplaceStruct
        {
            public GameObject hideGameObject;
            public GameObject showGameObject;

            public Vector3 cameraPosition;
            public Vector3 cameraRotation;

            public int cost;
        }
        
        [System.Serializable]
        public struct RoomMaterialsStruct
        {
            public Material floorMaterial;
            public Material roofMaterial;
            public Material leftWallMaterial;
            public Material rightWallMaterial;
            public Material cornerMaterial;

            public int cost;

            public Material[] GetMaterialsArray()
            {
                return new[]
                {
                    floorMaterial,
                    roofMaterial,
                    rightWallMaterial,
                    leftWallMaterial,
                    cornerMaterial
                };
            }
        }

        [SerializeField] private MeshRenderer roomMeshRenderer;
        [SerializeField] private List<ReplaceStruct> furnitureSettings = new List<ReplaceStruct>();
        [SerializeField] private List<ReplaceStruct> decorSettings = new List<ReplaceStruct>();
        [SerializeField] private List<RoomMaterialsStruct> roomMaterials = new List<RoomMaterialsStruct>();

        private void Start()
        {
            SetupRoom();
        }
        
        public void SetupRoom()
        {
            for (var i = 0; i < GameManager.FurnitureIndex; i++)
            {
                furnitureSettings[i].hideGameObject.transform.localScale = Vector3.zero;
                furnitureSettings[i].showGameObject.transform.localScale = Vector3.one;
            }
            for (var i = 0; i < GameManager.DecorIndex; i++)
            {
                decorSettings[i].hideGameObject.transform.localScale = Vector3.zero;
                decorSettings[i].showGameObject.transform.localScale = Vector3.one;
            }

            if(GameManager.RoomIndex <= roomMaterials.Count)
                roomMeshRenderer.materials = roomMaterials[GameManager.RoomIndex - 1].GetMaterialsArray();
        }
        
        public void RefreshRoom()
        {
            for (var i = 0; i < GameManager.FurnitureIndex; i++)
            {
                //furnitureSettings[i].hideGameObject.transform.localScale = Vector3.zero;
                if (furnitureSettings[i].showGameObject.transform.localScale != Vector3.one)
                    furnitureSettings[i].showGameObject.AddComponent<ShowObjectUc>();
            }
            for (var i = 0; i < GameManager.DecorIndex; i++)
            {
                //decorSettings[i].hideGameObject.transform.localScale = Vector3.zero;
                if (decorSettings[i].showGameObject.transform.localScale != Vector3.one)
                    decorSettings[i].showGameObject.AddComponent<ShowObjectUc>();
            }

            if(GameManager.RoomIndex <= roomMaterials.Count)
                roomMeshRenderer.materials = roomMaterials[GameManager.RoomIndex - 1].GetMaterialsArray();
        }

        public void GetFurnitureInfo(out int cost, out int level, out bool maxLevel, out Vector3 cameraPosition, out Vector3 cameraRotation)
        {
            level = GameManager.FurnitureIndex;
            maxLevel = level > furnitureSettings.Count - 1;
            if (maxLevel)
            {
                cost = 0;
                cameraPosition = Vector3.zero;
                cameraRotation = Vector3.zero;
            }
            else
            {
                cost = furnitureSettings[level].cost;
                cameraPosition = furnitureSettings[level].cameraPosition;
                cameraRotation = furnitureSettings[level].cameraRotation;
            }
        }
        
        public void GetDecorInfo(out int cost, out int level, out bool maxLevel, out Vector3 cameraPosition, out Vector3 cameraRotation)
        {
            level = GameManager.DecorIndex;
            maxLevel = level > decorSettings.Count - 1;
            if (maxLevel)
            {
                cost = 0;
                cameraPosition = Vector3.zero;
                cameraRotation = Vector3.zero;
            }
            else
            {
                cost = decorSettings[level].cost;
                cameraPosition = decorSettings[level].cameraPosition;
                cameraRotation = decorSettings[level].cameraRotation;
            }
        }

        public void GetRoomInfo(out int cost, out int level, out bool maxLevel)
        {
            level = GameManager.RoomIndex;
            maxLevel = level > roomMaterials.Count - 1;
            cost = maxLevel ? 0 : roomMaterials[level].cost;
        }
    }
}