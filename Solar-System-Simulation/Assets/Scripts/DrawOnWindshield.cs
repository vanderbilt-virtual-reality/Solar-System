using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DrawOnWindshield : MonoBehaviour
{
    [SerializeField] private GameObject baseMarker;

    private GameObject otherBaseMarker;

    private static string MarkerName = "Marker";
    private static string MainTextName = "MainText";
    private static string SubTextName = "SubText";

    private Dictionary<string, GameObject> markers = new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> otherMarkers = new Dictionary<string, GameObject>();

    private Dictionary<string, Color> nameColorMap = new Dictionary<string, Color>();

    // Start is called before the first frame update
    void Start()
    {
        
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
        //nameColorMap.Add("Other", Color.grey);


        foreach (var pair in nameColorMap) {
            GameObject newMarker = Instantiate(baseMarker, transform);
            newMarker.name = pair.Key;
            newMarker.transform.Find(MarkerName).GetComponent<RawImage>().color = pair.Value;
            Text mainText = newMarker.transform.Find(MainTextName).GetComponent<Text>();
            mainText.color = pair.Value;
            mainText.text = pair.Key;
            Text subText = newMarker.transform.Find(SubTextName).GetComponent<Text>();
            subText.color = pair.Value;
            subText.text = "0";
            markers.Add(pair.Key, newMarker);
        }

        otherBaseMarker = Instantiate(baseMarker, transform);
        otherBaseMarker.name = "OtherBase";
        otherBaseMarker.transform.Find(MarkerName).GetComponent<RawImage>().color = Color.grey;
        Text otherMainText = otherBaseMarker.transform.Find(MainTextName).GetComponent<Text>();
        otherMainText.color = Color.grey;
        otherMainText.text = "Other";
        Text otherSubText = otherBaseMarker.transform.Find(SubTextName).GetComponent<Text>();
        otherSubText.color = Color.grey;
        otherSubText.text = "0";
    }


    public void drawPointsWithName(List<PlanetTracker.HitObj> hitObjs) {
        List<string> enabled = new List<string>();
        // hits is sorted from closes to farthest
        // show markers and update for each hit
        foreach(PlanetTracker.HitObj hitObj in hitObjs)
        {
            Vector3 pos = transform.InverseTransformPoint(hitObj.hit.point);
            pos.y += 200; // TODO: scale this depending on how close the object is? Ex: 200 / distance
            GameObject marker;
            if (markers.ContainsKey(hitObj.name)) 
            {
                marker = markers[hitObj.name];
            }

            else 
            {
                // create a other marker based on the otherBaseMarker
                if (!otherMarkers.ContainsKey(hitObj.name))
                {
                    var newMarker = Instantiate(otherBaseMarker, transform);
                    newMarker.transform.Find(MainTextName).GetComponent<Text>().text = hitObj.name;
                    newMarker.name = hitObj.name;
                    otherMarkers.Add(hitObj.name, newMarker);
                }

                marker = otherMarkers[hitObj.name];
            }

            enabled.Add(hitObj.name);
            marker.GetComponent<RectTransform>().anchoredPosition = pos;
            marker.transform.Find(SubTextName).GetComponent<Text>().text = $"{Math.Round(hitObj.distance, 3):E3}m";
            setEnabled(marker, true);
        }

        // don't show markers not supposed to be shown
        foreach (var marker in markers)
        {
            if (!enabled.Contains(marker.Key)) {
                setEnabled(marker.Value, false);
            }
        }

        foreach (var marker in otherMarkers)
        {
            if (!enabled.Contains(marker.Key))
            {
                setEnabled(marker.Value, false);
            }
        }
    }

    void setEnabled(GameObject marker, bool enabled) {
        marker.transform.Find(MarkerName).GetComponent<RawImage>().enabled = enabled;
        marker.transform.Find(MainTextName).GetComponent<Text>().enabled = enabled;
        marker.transform.Find(SubTextName).GetComponent<Text>().enabled = enabled;
    }
}
