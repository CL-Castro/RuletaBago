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

public class PlayersController : MonoBehaviour
{
    [Header("List Settings")] 
    public Text NumOfPlayersInputField;
    public int CurrentNumOfPlayers = 0;
    // Start is called before the first frame update
    public Text NumberOfPlayers;
    public List<GameObject> Players;
    public GameObject PlayerItemTemplate;
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
        NumberOfPlayers.text = (int.Parse(NumberOfPlayers.text) + 1).ToString();
    }

    public void DecreasePlayers()
    {
        NumberOfPlayers.text = (int.Parse(NumberOfPlayers.text) - 1).ToString();
    }

    public void AddPlayerItems()
    {
        for (int i = 0; i < int.Parse(NumberOfPlayers.text); i++)
        {
            var copy = Instantiate(PlayerItemTemplate);
            copy.transform.SetParent(Content.transform);
            copy.GetComponent<InputField>().text = "Jugador " + (i + 1);
            Players.Add(copy);
        }
    }
    
    public void RestartData()
    {
        NumberOfPlayers.text = 0.ToString();
        foreach (var player in Players)
        {
            Destroy(player);
        }
        Players.Clear();
    }
    
    public void AddPlayersFromExcelFile()
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
                            AddPlayerItemFromExcel(i, row[j].Text);
                        }
                    }
                }
            }
            CurrentNumOfPlayers = sheet.Count;
            NumOfPlayersInputField.text = sheet.Count.ToString();
        }
    }
    
    public void AddPlayerItemFromExcel(int index, string name)
    {
        var copy = Instantiate(PlayerItemTemplate);
        copy.transform.SetParent(Content.transform);
        copy.GetComponent<Player>().index = index;
        copy.GetComponent<InputField>().text = name;
        Players.Add(copy);
    }
}
