using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectType : MonoBehaviour
{
    public enum TObjectType
    {
        Tflask_1, Tflask_2, Tflask_3, Tflask_4, Tflask_5
    };

    [SerializeField]
    public TObjectType type;
}
