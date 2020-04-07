using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorPressureUpdator : MonoBehaviour
{
    int pressureLevel = 1;
    // Start is called before the first frame update
    void Start()
    {
        StatusUpdateControl control = GetComponent<StatusUpdateControl>();
        control.StatusUpdated += OnUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Pressure: " + pressureLevel + "atm";
    }

    void OnUpdate(object sender, StatusUpdateEventArgs e)
    {
        pressureLevel = int.Parse(e.newStatus);
    }
}
