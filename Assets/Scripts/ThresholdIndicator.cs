using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdIndicator : MonoBehaviour
{
    void Start()
    {
    }
    public void updateThreshold(float threshold) {

        if (threshold!=transform.localScale.y) {
            transform.localScale -= new Vector3(0, transform.localScale.y-threshold, 0);
        }
    }
}
