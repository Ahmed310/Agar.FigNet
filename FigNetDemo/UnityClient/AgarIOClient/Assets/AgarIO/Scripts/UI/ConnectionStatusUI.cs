using UnityEngine;
using UnityEngine.UI;

public class ConnectionStatusUI : MonoBehaviour
{
    [SerializeField] private Text status;

    public void SetStatus(string _status) 
    {
        status.text = _status;
    }

}
