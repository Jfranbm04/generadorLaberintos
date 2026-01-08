using UnityEngine;

public class Room : MonoBehaviour {

    [SerializeField] private GameObject[] wallDoors;

    bool connected;

    public void UpdateRoom(bool[] status) {

        connected = false;
        for (int i = 0; i < status.Length; i++) {

            wallDoors[i].SetActive(!status[i]);

            if (status[i])
                connected = true;

            if (!connected)
                Destroy(gameObject);
        }
    }
}
