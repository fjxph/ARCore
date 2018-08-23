using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogTarget : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        if (GlobalEvent.ImComing != null)
            GlobalEvent.ImComing(this.transform);
    }
    
}
