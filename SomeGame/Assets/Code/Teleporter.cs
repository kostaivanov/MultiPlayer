using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform destination;

    internal Transform GetDestination()
    {
        return destination;
    }


}
