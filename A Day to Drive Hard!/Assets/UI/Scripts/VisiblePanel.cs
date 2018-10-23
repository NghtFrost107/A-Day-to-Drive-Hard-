using UnityEngine;

public class VisiblePanel : MonoBehaviour {

    public GameObject objectToTrack;

    public void ChangePanelState()
    {
        objectToTrack.SetActive(objectToTrack.activeSelf ? false : true);

        if (objectToTrack.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

}
