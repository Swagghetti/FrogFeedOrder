using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject levelsPanel;


    public void OnPlayButtonPressed()
    {
        levelsPanel.SetActive(true);
        menuPanel.SetActive(false);
    }

    public void OnBackButtonPressed()
    {
        levelsPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
}
