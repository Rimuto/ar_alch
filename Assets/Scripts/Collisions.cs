using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Collisions : MonoBehaviour
{
    private string ResourceTag = "Resource";
    public GameObject visualPart;

    public GameObject ConnectedMixedObject;
    private static ResourcesData ResourcesData1;

    public GameObject OtherResource;

    private void Start()
    {
        ResourcesData1 = Resources.Load<ResourcesData>("Data/ResourcesData");
        visualPart = gameObject.transform.GetChild(0).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<ObjectType>())
        {
            Debug.Log($"not find{other.gameObject.name}");
        }

        ObjectType obj = gameObject.transform.GetChild(0).GetComponent<ObjectType>();
        ObjectType otherType = other.gameObject.transform.GetChild(0).GetComponent<ObjectType>();
        
        
        var type = other.gameObject.GetComponent<ObjectType>().type;
        if (obj.type == type)
        {
            Debug.Log($"Equal objects");
            return;
        }

        Debug.Log($"Collide {obj.name} with {other.gameObject.name}");
        TryGenerateMix(obj, otherType);
        //if (obj.type == ObjectType.TObjectType.Tflask_1 && type == ObjectType.TObjectType.Tflask_2)
        //{
        //    //Vector3 newpos = (other.transform.position + visualPart.transform.position) / 2;

        //    other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        //    visualPart.SetActive(false);

        //    // ConnectedMixedObject = Instantiate(MixedObject, newpos, Quaternion.identity);
        //}

    }

    private void TryGenerateMix(ObjectType current, ObjectType other)
    {
        Debug.Log($"Try combine {current.name} with {other.name}");

        if (current.type == other.type)
        {
            Debug.Log($"The same ObjectType {current.type.ToString()} on 2 GameObjects");
            return;
        }

        var type = other.type;

        foreach(var r in ResourcesData1.Descriptions)
        {
            if ((r.Type == current.type && type == r.OtherType) ||
                r.Type == type && r.OtherType == current.type)
            {
                Vector3 newpos = (other.transform.position + visualPart.transform.position) / 2;
                other.gameObject.SetActive(false);
                visualPart.SetActive(false);

                ConnectedMixedObject = Instantiate(r.MixedPrefab, newpos, Quaternion.identity);

                var parent = other.GetComponentInParent<Collisions>();
                parent.ConnectedMixedObject = ConnectedMixedObject;
                OtherResource = parent.transform.gameObject;
                parent.OtherResource = gameObject;

                Debug.Log($"Successfull combine resources {gameObject.name} with {other.gameObject.name}");
                return;
            }
        }

        Debug.Log($"Couldn't combine resources {gameObject.name} with {other.gameObject.name}");
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"TriggerExit {other.gameObject.name}");

        if (!other.gameObject.CompareTag(ResourceTag))
        {
            return;
        }

        Debug.Log($"TriggerExit {other.gameObject.name} ConnectedMixedObject {ConnectedMixedObject}");
        var parent = other.gameObject.transform;
        Debug.Log($"{parent.name} <> {ConnectedMixedObject.name}");
        if (parent.name.CompareTo(ConnectedMixedObject.name) != 0)
        {
            Destroy(ConnectedMixedObject);
            ConnectedMixedObject = null;

            OtherResource.GetComponent<Collisions>().visualPart.SetActive(true);
            visualPart.SetActive(true);
        }
    }
}
