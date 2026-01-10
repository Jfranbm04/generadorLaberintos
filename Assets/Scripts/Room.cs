using UnityEngine;

public class Room : MonoBehaviour
{

    // 0 - top, 1 - down, 2 - left, 3 - right
    [SerializeField] private GameObject[] wallDoors;

    public void UpdateRoom(bool[] status)
    {
        bool connected = false;

        for (int i = 0; i < status.Length; i++)
        {
            // Si status[i] es true (hay conexión), SetActive(false) hace que el muro desaparezca
            wallDoors[i].SetActive(!status[i]);

            if (status[i])
            {
                connected = true;
            }
        }

        // Si la habitación no está conectada a nada, la borramos
        if (!connected)
        {
            Destroy(gameObject);
        }
    }
}