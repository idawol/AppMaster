using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnImage : MonoBehaviour {

    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.Rotate(new Vector3(0, 0, -90));
    }
}
