using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleScreenFront : MonoBehaviour
{
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject detailsText;
    private List<string> enabledList = new List<string>();
    private List<string> detailsList = new List<string>();
    private float timeLeft = .5f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeft <= 0)
        {
            timeLeft = .5f;
            if (warningText.GetComponent<Text>().color == Color.black)
            {
                warningText.GetComponent<Text>().color = Color.red;
            }
            else
            {
                warningText.GetComponent<Text>().color = Color.black;
            }
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }

    public void showWarningText(bool enabled, string name)
    {
        if (name == "Sun") return;
        if(!enabled && enabledList.Contains(name))
        {
            warningText.GetComponent<Text>().enabled = enabled;
            enabledList.Remove(name);
        }
        else if (enabled && !enabledList.Contains(name))
        {
            warningText.GetComponent<Text>().enabled = enabled;
            enabledList.Add(name);
        } else if (enabled && enabledList.Contains(name))
        {
            warningText.GetComponent<Text>().enabled = enabled;
        }
    }

    public void showDetails(bool enabled, Dictionary<string, string> details, string name)
    {
        Text t = detailsText.GetComponent<Text>();
        string d = "";
        if (details != null)
        {
            d = $"Distance: {details["distance"]}\nRadius: {details["radius"]}\nType: {details["type"]}\nOther info: {details["accessories"]}";
        }

        if (!enabled && detailsList.Contains(name))
        {
            t.enabled = enabled; // false
            detailsList.Remove(name);
        }
        else if (enabled && !detailsList.Contains(name))
        {
            t.enabled = enabled; // true
            t.text = d;
            detailsList.Add(name);
        }
        else if (enabled && detailsList.Contains(name))
        {
            t.enabled = enabled; // true
            t.text = d;
        }
    }
}
