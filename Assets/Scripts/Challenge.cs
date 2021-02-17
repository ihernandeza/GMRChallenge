using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// For this challenge I decide to use only Built-in Unity Classes 
/// </summary>
public class Challenge : MonoBehaviour
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
        json = new JsonMain();
            json = JsonUtility.FromJson<JsonMain>(jsonText);
        

        //Set UI Columns Amount
        m_gridLayoutGroup.constraintCount = json.ColumnHeaders.Length;

        //Title
        title.text = json.Title;

        //Headers
        item.SetActive(true);
        foreach (string header in json.ColumnHeaders)
        {
            Text currentItem = Instantiate(item, m_gridLayoutGroup.transform).GetComponent<Text>();
            currentItem.text = "<b>" + header + "</b>";
        }

        //Data
        foreach (Member member in json.Data)
        {
            Text currentItem = Instantiate(item, m_gridLayoutGroup.transform).GetComponent<Text>();
            currentItem.text = member.ID;

            currentItem = Instantiate(item, m_gridLayoutGroup.transform).GetComponent<Text>();
            currentItem.text = member.Name;

            currentItem = Instantiate(item, m_gridLayoutGroup.transform).GetComponent<Text>();
            currentItem.text = member.Role;

            currentItem = Instantiate(item, m_gridLayoutGroup.transform).GetComponent<Text>();
            currentItem.text = member.Nickname;
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


#region Json Objects
[Serializable]
public class JsonMain
{
    public string Title;
    public string[] ColumnHeaders;
    public Member[] Data;

    public static JsonMain CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<JsonMain>(jsonString);
    }
}

[Serializable]
public class Member
{
    // Unfortunately, JsonUtility does not allow parse a json without specify their keys, instead of that we can use NewtonSoft or MiniJson
    public string ID, Name, Role, Nickname; //Add New Fields if Required
}

#endregion