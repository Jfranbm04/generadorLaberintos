using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonController : MonoBehaviour {
    public int sizeXField = 10;
    public int sizeYField = 10;
    public TMP_InputField initPositionField;
    public TMP_InputField nRoomsField;

    public MapGenerator mapGenerator;
    public GameObject panelPrincipal; 

    public void GenerateDungeon()
    {
        if (
            int.TryParse(initPositionField.text, out int initPosition) && int.TryParse(nRoomsField.text, out int nRooms)) {

            if (panelPrincipal != null) {
                panelPrincipal.SetActive(false); 
            }
            mapGenerator.size = new Vector2Int(sizeXField, sizeYField);
            mapGenerator.roomSize = new Vector2(10, 10);
            mapGenerator.initPosition = initPosition;
            mapGenerator.nRooms = nRooms;
            mapGenerator.MazeGenerator();
        }
        else {
            Debug.LogError("Por favor, introduce valores válidos.");
        }
    }
}