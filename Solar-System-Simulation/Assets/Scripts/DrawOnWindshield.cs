﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawOnWindshield : MonoBehaviour
{
    private GameObject baseMarker;
    private Dictionary<string, GameObject> markers = new Dictionary<string, GameObject>();

    private Dictionary<string, Color> nameColorMap = new Dictionary<string, Color>();

    // Start is called before the first frame update
    void Start()
    {
        baseMarker = transform.Find("BaseMarker").gameObject;
        nameColorMap.Add("Sun", Color.yellow);
        nameColorMap.Add("Mercury", Color.magenta);
        nameColorMap.Add("Venus", new Color(109f / 255f, 70f / 255f, 29f / 255f, 1f)); // brown
        nameColorMap.Add("Earth", Color.green);
        nameColorMap.Add("Mars", Color.red);
        nameColorMap.Add("Jupiter", Color.white);
        nameColorMap.Add("Saturn", new Color(1f, 118f / 255f, 0f, 1f)); // orange
        nameColorMap.Add("Uranus", Color.cyan);
        nameColorMap.Add("Neptune", Color.blue);
        nameColorMap.Add("Pluto", new Color(1f, 0f, 1f, 1f)); // purple
        nameColorMap.Add("Other", Color.grey);


        foreach (var pair in nameColorMap) {
            GameObject newMarker = Instantiate(baseMarker, transform);
            newMarker.name = pair.Key;
            newMarker.transform.Find("Marker").GetComponent<RawImage>().color = pair.Value;
            Text text = newMarker.transform.Find("Text").GetComponent<Text>();
            text.color = pair.Value;
            text.text = pair.Key;
            markers.Add(pair.Key, newMarker);
        }        
    }


    public void drawPointsWithName(List<RaycastHit> hits, List<string> names) {
        List<string> enabled = new List<string>();
        // hits is sorted from closes to farthest
        // show markers and update for each hit
        for (int i = 0; i < hits.Count; ++i) {
            RaycastHit hit = hits[i];
            enabled.Add(names[i]);
            Vector3 pos = transform.InverseTransformPoint(hit.point);
            pos.y += 200; // TODO: scale this depending on how close the object is? Ex: 200 / distance
            Debug.Log(pos);
            GameObject marker = markers.ContainsKey(names[i]) ? markers[names[i]] : markers["Other"];
            marker.GetComponent<RectTransform>().anchoredPosition = pos;
            setEnabled(marker, true);
        }

        // don't show markers not supposed to be shown
        foreach (var marker in markers) {
            if (!enabled.Contains(marker.Key)) {
                setEnabled(marker.Value, false);
            }
        }
    }

    void setEnabled(GameObject marker, bool enabled) {
        marker.transform.Find("Marker").GetComponent<RawImage>().enabled = enabled;
        marker.transform.Find("Text").GetComponent<Text>().enabled = enabled;
    }
}
