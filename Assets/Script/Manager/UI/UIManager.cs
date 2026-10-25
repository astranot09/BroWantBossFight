using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    [SerializeField] private List<GameObject> openPanels = new List<GameObject>();
    private bool canOpenPanel = true;

    public void OpenPanel(GameObject panel)
    {
        if (panel == null || !canOpenPanel) return;

        if (!openPanels.Contains(panel))
        {
            openPanels.Add(panel);
        }

        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        if (panel == null) return;

        panel.SetActive(false);
        openPanels.Remove(panel);
    }

    public void ChangeAllPanel(GameObject newPanel)
    {
        CloseAllPanels();

        if (newPanel != null)
        {
            OpenPanel(newPanel);
        }
    }

    public void CloseAllPanels()
    {
        for (int i = 0; i < openPanels.Count; i++)
        {
            if (openPanels[i] != null)
            {
                openPanels[i].SetActive(false);
            }
        }

        openPanels.Clear();
    }
}