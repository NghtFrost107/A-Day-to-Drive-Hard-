using UnityEngine;

public class VisiblePanel : MonoBehaviour {

    public GameObject objectToTrack;

    public void ChangePanelState()
    {
        objectToTrack.SetActive(objectToTrack.activeSelf ? false : true);
    }

}
