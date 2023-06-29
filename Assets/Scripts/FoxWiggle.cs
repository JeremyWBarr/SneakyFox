using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxWiggle : MonoBehaviour
{
    float offset = 0;
    float offset_offset = .001f;
    RectTransform trans;

    void Start() {
        trans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {   
        var rand = UnityEngine.Random.insideUnitSphere;
        trans.anchoredPosition = new Vector2(trans.anchoredPosition.x+offset, trans.anchoredPosition.y) + (new Vector2(rand.x, rand.y) / 2);

        // if(offset > 0.2f || offset < -0.2f) offset_offset *= -1f;
        // offset += offset_offset;
// 
        // Debug.Log(offset);
    }
}
