using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeethIformation : MonoBehaviour
{
    [System.Serializable] public struct TeethStruct
    {
        public GameObject teethTop;
        public GameObject teethBottom;

        public bool IsEmpty()
        {
            return teethTop != null;
        }
    }

    public TeethStruct teethStruct;
    
}
