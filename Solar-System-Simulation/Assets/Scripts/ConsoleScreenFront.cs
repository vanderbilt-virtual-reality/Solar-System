using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleScreenFront : MonoBehaviour
{
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject detailsText;
    private List<string> enabledList = new List<string>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showWarningText(bool enabled, string name)
    {
        if (name == "Sun") return;
        Debug.Log($"Here: {enabled}, {name}");
        if(!enabled && enabledList.Contains(name))
        {
            warningText.GetComponent<Text>().enabled = enabled;
            enabledList.Remove(name);
        }
        else if (enabled && !enabledList.Contains(name))
        {
            Debug.Log("HERE2");
            warningText.GetComponent<Text>().enabled = enabled;
            enabledList.Add(name);
        } else if (enabled && enabledList.Contains(name))
        {
            warningText.GetComponent<Text>().enabled = enabled;
        }
    }

    public void showDetails(bool enabled, string details)
    {
        Text t = detailsText.GetComponent<Text>();
        t.enabled = enabled;
        t.text = details;
    }
}
