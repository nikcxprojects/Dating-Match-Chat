using UnityEngine;
using UnityEngine.UI;


public class MatchCell : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _girlImage;
    [SerializeField] private Text _girlText;

    private GirlData _data;
    public void SetGirlData(GirlData data) => _data = data;

    public void InitializeCell()
    {
        if (_data == null)
            return;

        _girlImage.sprite = _data.photoGirl;
        _girlText.text = _data.infoGirl;
    }

    public void Profile()
    {
        GameObject.FindObjectOfType<UIMenu>().OpenProfileGirl(_data);
    }
}
