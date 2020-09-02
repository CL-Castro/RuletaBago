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
    
    [Header("Tablero de puntuaciones")]
    public GameObject textPrefab;
    public GameObject ContentNombre;
    public GameObject Tablero;
    public GameObject ContentPuntaje;
    public bool visible;
    private List<GameObject> Scores = new List<GameObject>();

    public GameObject ErrorPanel;
    public Text ErrorMessage;

    void Start()
    {
        visible = true;
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

    public void SetPlayers()
    {
        ErrorPanel.SetActive(false);
        if (!ValidatePlayersData())
        {
            ShowErrorPanel("Todos los jugadores deben tener un 'Nombre'");
            return;
        }

        if (!ValidateNumberOfPlayers())
        {
            ShowErrorPanel("El número mínimo de jugadores requeridos es 2");
            return;
        }
        foreach (var player in Players)
        {
            player.GetComponent<Player>().Name = player.GetComponent<InputField>().text;
            player.GetComponent<Player>().Score = 0;
        }
    }
    
    public void Leaderboard()
    {
        visible = !visible;
        if (!visible)
        {
            Tablero.SetActive(true);
            foreach (var player in Players)
            {
                var tempNombre = Instantiate(textPrefab);
                //Parent to the panel
                tempNombre.transform.SetParent(ContentNombre.transform);
                tempNombre.GetComponent<Text>().text = player.GetComponent<Player>().Name;
            
                var tempPuntaje = Instantiate(textPrefab);
                //Parent to the panel
                tempPuntaje.transform.SetParent(ContentPuntaje.transform);
                tempPuntaje.GetComponent<Text>().text = player.GetComponent<Player>().Score.ToString();
                
                Scores.Add(tempNombre);
                Scores.Add(tempPuntaje);
            }

            visible = false;
        }
        else
        {
            Tablero.SetActive(false);
            if (Scores != null)
            {
                foreach (var score in Scores)
                {
                    Destroy(score);
                }
            }
            visible = true;
        }
    }
    public bool ValidatePlayersData()
    {
        foreach (var player in Players)
        {
            if (player.GetComponent<InputField>().text.Length < 3)
            {
                return false;
            }
        }

        return true;
    }

    public bool ValidateNumberOfPlayers()
    {
        if (Players.Count < 2)
        {
            return false;
        }
        return true;
    }
    public void ShowErrorPanel(string Error)
    {
        ErrorPanel.SetActive(true);
        ErrorMessage.text = Error;
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
