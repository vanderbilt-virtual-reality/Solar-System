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
    private static string Sub2TextName = "Sub2Text";

    private Dictionary<string, GameObject> markers = new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> otherMarkers = new Dictionary<string, GameObject>();

    private Dictionary<string, Color> nameColorMap = new Dictionary<string, Color>();
    private CharacterMovement characterMovement;

    // Start is called before the first frame update
    void Start()
    {
        characterMovement = GameObject.FindObjectOfType<CharacterMovement>();
        
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
            Text sub2Text = newMarker.transform.Find(Sub2TextName).GetComponent<Text>();
            sub2Text.color = pair.Value;
            sub2Text.text = "0";
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
        Text otherSub2Text = otherBaseMarker.transform.Find(Sub2TextName).GetComponent<Text>();
        otherSub2Text.color = Color.grey;
        otherSub2Text.text = "0";
    }


    public void drawPointsWithName(List<PlanetTracker.HitObj> hitObjs) {
        List<string> enabled = new List<string>();
        // hits is sorted from closes to farthest
        // show markers and update for each hit
        foreach(PlanetTracker.HitObj hitObj in hitObjs)
        {
            Vector3 pos = transform.InverseTransformPoint(hitObj.hit.point);

            float verticalShift = 50; // shift up to see planet

            if (hitObj.distance < 10000000000)
            {
                //Debug.Log($"Lerp: { Mathd.Lerp(1, 100, 1 - ((hitObj.distance - 100000) / 10000000000))}");
                verticalShift += (float) Mathd.Lerp(1, 100, 1 - ((hitObj.distance - 100000) / 10000000000));
            }
            pos.y += verticalShift; // TODO: scale this depending on how close the object is? Ex: 200 / distance
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
            if (characterMovement.ActualSpeed == 0)
            {
                marker.transform.Find(Sub2TextName).GetComponent<Text>().text = $"ETA at current speed:\nNot moving";
            }
            else
            {
       
                double time = hitObj.distance / characterMovement.m_MoveSpeed;
                string timeStr = "sec";

                if (time / 60 >= 1 && timeStr == "sec")
                {
                    time = time / 60;
                    timeStr = "min";
                }
                if (time / 60 >= 1 && timeStr == "min")
                {
                    time = time / 60;
                    timeStr = "hr";
                }
                if (time / 24 >= 1 && timeStr == "hr")
                {
                    time = time / 24;
                    timeStr = "day(s)";
                }
                if (time / 7 >= 1 && timeStr == "day(s)")
                {
                    time = time / 7;
                    timeStr = "wk";
                }
                if (time / 52 >= 1 && timeStr == "wk")
                {
                    time = time / 52;
                    timeStr = "yr";
                }


                marker.transform.Find(Sub2TextName).GetComponent<Text>().text = $"ETA at current speed:\n{Math.Round(time, 2)} {timeStr}";
            }
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
        marker.transform.Find(Sub2TextName).GetComponent<Text>().enabled = enabled;
    }
}
