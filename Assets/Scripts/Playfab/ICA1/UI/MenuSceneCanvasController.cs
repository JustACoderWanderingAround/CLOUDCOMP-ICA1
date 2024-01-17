using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MenuSceneCanvasController : MonoBehaviour
{

    enum CurrentActiveCanvas
    {
        CANVAS_MAINSELECTION = 0,
        CANVAS_EMAIL = 1,
        CANVAS_USERNAME = 2,
        CANVAS_REGISTER = 3,
        CANVAS_RESET = 4,
        NUM_CANVAS
    }
    CurrentActiveCanvas currentActiveCanvas;

    [SerializeField]
    private List<GameObject> canvasList;
    // Start is called before the first frame update
    void Start()
    {
        currentActiveCanvas = 0;
        foreach (GameObject go in canvasList)
        {
            go.SetActive(false);
        }
        canvasList[(int)currentActiveCanvas].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwapCanvas(int canvasIndex)
    {
        DisableCurrentCanvas();
        switch(canvasIndex)
        {
            case 0:
                currentActiveCanvas = CurrentActiveCanvas.CANVAS_MAINSELECTION;
                break;
            case 1:
                currentActiveCanvas = CurrentActiveCanvas.CANVAS_EMAIL;
                break;
            case 2:
                currentActiveCanvas = CurrentActiveCanvas.CANVAS_USERNAME;
                break;
            case 3:
                currentActiveCanvas = CurrentActiveCanvas.CANVAS_REGISTER;
                break;
            case 4:
                currentActiveCanvas = CurrentActiveCanvas.CANVAS_RESET;
                break;
            default:
                Debug.Log("You're fucked. UI dun goofed. canvasIndex > 3 || < 0.");
                break;
        }
        EnableCurrentCanvas();
    }
    public void DisableCurrentCanvas()
    {
        canvasList[(int)currentActiveCanvas].SetActive(false);
    }
    public void EnableCurrentCanvas()
    {
        canvasList[(int)currentActiveCanvas].SetActive(true);
    }
}
