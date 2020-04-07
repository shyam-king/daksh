using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorTemperatureUpdator : MonoBehaviour
{
    int temperatureLevel = 27;
    // Start is called before the first frame update
    void Start()
    {
        StatusUpdateControl control = GetComponent<StatusUpdateControl>();
        control.StatusUpdated += OnUpdate;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = "Temperature: " + temperatureLevel + " Celcius";
    }

    void OnUpdate(object sender, StatusUpdateEventArgs e)
    {
        int level = int.Parse(e.newStatus);
        temperatureLevel = level;
    }
}
