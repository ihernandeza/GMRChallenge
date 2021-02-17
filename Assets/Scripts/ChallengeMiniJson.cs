using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// Same Challenge, but using MiniJson, this allow read an dynamic json, with N numbers of cols
/// NOTE: ColumnHeaders MUST match Data Array Keys.
/// </summary>
public class ChallengeMiniJson : MonoBehaviour
{
    public Text title;
    public GameObject item;
    public GridLayoutGroup m_gridLayoutGroup;
    public Button m_buttonRefresh;

    JsonMain json;

    #region States
    private void Awake()
    {
        m_buttonRefresh.onClick.AddListener(OnRefreshButton);
    }

    void Start()
    {
        StartCoroutine(LoadParseJson());
    }

    #endregion



    #region Json Grid Creation
    IEnumerator LoadParseJson()
    {
        //Disable Refresh Button while data is loading...
        m_buttonRefresh.interactable = false;

        //Load Text with Json
        string jsonText = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "JsonChallenge.json"));

        yield return jsonText;

        //Clean Json, for some reason, it has extra "," ...
        //NOTE: all these 4 lines can be removed if Json is well formatted.
        jsonText = jsonText.Trim();
        jsonText = jsonText.Replace(Environment.NewLine, "");
        jsonText = jsonText.Replace(",}", "}");
        jsonText = jsonText.Replace(",]", "]");

        //Parse Json
        var dict = Json.Deserialize(jsonText) as Dictionary<string, object>;
       
        //Set UI Columns Amount Based on Headers
        List<object> headers = new List<object>();
        headers = (List<object>)dict["ColumnHeaders"];
        m_gridLayoutGroup.constraintCount = headers.Count;

        //Title
        title.text = (string)dict["Title"];

        //Create Headers
        item.SetActive(true);
        foreach (string header in headers)
        {
            Text currentItem = Instantiate(item, m_gridLayoutGroup.transform).GetComponent<Text>();
            currentItem.text = "<b>" + header + "</b>";
        }

        //Create Data and populate grid
        List<object> members = new List<object>();
        members = dict["Data"] as List<object>;

        int i = 0;
        foreach(object member in members)
        {
            Dictionary<string, object> memberData = members[i] as Dictionary<string, object>;

            foreach (string header in headers)
            {
                Text currentItem = Instantiate(item, m_gridLayoutGroup.transform).GetComponent<Text>();
                currentItem.text = memberData[header].ToString();
            }

            i++;

        }

        //Disable TD Prefab 
        item.SetActive(false);

        
        //Enable Refresh Button
        m_buttonRefresh.interactable = true;
    }

    void ClearGrid()
    {
        foreach (Transform child in m_gridLayoutGroup.transform)
            Destroy(child.gameObject);
    }

    #endregion


    #region Listeners

    public void OnRefreshButton()
    {
        m_buttonRefresh.interactable = false;
        ClearGrid();
        StartCoroutine(LoadParseJson());
    }

    #endregion
}