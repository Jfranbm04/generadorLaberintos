using TMPro;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    public int sizeXField = 10;
    public int sizeYField = 10;
    public TMP_InputField initPositionField;
    public TMP_InputField nRoomsField;

    public MapGenerator mapGenerator;
    public GameObject panelPrincipal;

    public void GenerateDungeon()
    {
        // Validamos que los campos tengan números
        if (int.TryParse(initPositionField.text, out int initPos) &&
            int.TryParse(nRoomsField.text, out int rooms))
        {
            if (panelPrincipal != null) panelPrincipal.SetActive(false);

            // Configuramos los parámetros antes de generar
            mapGenerator.size = new Vector2Int(sizeXField, sizeYField);
            mapGenerator.SetInitPosition(initPos);
            mapGenerator.SetNRooms(rooms);

            // Llamamos al generador
            mapGenerator.MazeGenerator();
        }
        else
        {
            Debug.LogError("Valores inválidos en los campos de texto.");
        }
    }
}