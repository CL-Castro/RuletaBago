using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SFB;
using FlexFramework.Excel;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using sharpPDF;
using sharpPDF.Enumerators;

public class FraseController : MonoBehaviour
{
    [Header("List Settings")] 
    public Text NumOfPhrasesInputField;
    public int CurrentNumOfPhrases = 0;
    // Start is called before the first frame update
    public Text NumberOfPhrases;
    public List<GameObject> Phrases;
    public GameObject PhraseItemTemplate;
    public GameObject Content;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreasePlayers()
    {
        NumberOfPhrases.text = (int.Parse(NumberOfPhrases.text) + 1).ToString();
    }

    public void DecreasePlayers()
    {
        NumberOfPhrases.text = (int.Parse(NumberOfPhrases.text) - 1).ToString();
    }

    public void AddPhraseItems()
    {
        for (int i = 0; i < int.Parse(NumberOfPhrases.text); i++)
        {
            var copy = Instantiate(PhraseItemTemplate);
            copy.transform.SetParent(Content.transform);
            copy.GetComponent<InputField>().text = "Frase " + (i + 1);
            Phrases.Add(copy);
        }
    }
    
    public void RestartData()
    {
        NumberOfPhrases.text = 0.ToString();
        foreach (var player in Phrases)
        {
            Destroy(player);
        }
        Phrases.Clear();
    }
    
    public void AddPhrasesFromExcelFile()
    {
        string[] path = StandaloneFileBrowser.OpenFilePanel("Seleccione un archivo excel", "", "xlsx", false);
        if (path.Length > 0)
        {
            RestartData();
            var book = new WorkBook(path[0]);
            var sheet = book[0];
            for (int i = 0; i < sheet.Count; i++)
            {
                var row = sheet[i];
                if (!row.IsEmpty())
                {
                    for (int j = 0; j < row.Count; j++)
                    {
                        if (row[j].Text.Length > 3)
                        {
                            Debug.Log(i + " player " + row[j].Text);
                            AddPhraseItemFromExcel(i, row[j].Text);
                        }
                    }
                }
            }
            CurrentNumOfPhrases = sheet.Count;
            NumOfPhrasesInputField.text = sheet.Count.ToString();
        }
    }

    public void AddPhraseItemFromExcel(int index, string name)
    {
        var copy = Instantiate(PhraseItemTemplate);
        copy.transform.SetParent(Content.transform);
        copy.GetComponent<Player>().index = index;
        copy.GetComponent<InputField>().text = name;
        Phrases.Add(copy);
    }
}
