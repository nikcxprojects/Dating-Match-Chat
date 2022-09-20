using UnityEngine;
using UnityEngine.UI;

public class GirlCell : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image _girlImage;
    [SerializeField] private Text _girlText;
    [SerializeField] private Image _actionImage;
    [SerializeField] private Sprite[] _actionSprites;

    private GirlData _data;
    public void SetGirlData(GirlData data) => _data = data;
    public GirlData girlData => _data;
    
    public void InitializeCell()
    {
        if (_data == null) 
            return;

        _girlImage.sprite = _data.photoGirl;
        _girlText.text = _data.infoGirl;
    }

    public void Like()
    {
        _actionImage.gameObject.SetActive(true);
        _actionImage.sprite = _actionSprites[0];
    }

    public void Dislike()
    {
        _actionImage.gameObject.SetActive(true);
        _actionImage.sprite = _actionSprites[1];
    }

    public void ResetLike()
    {
        _actionImage.gameObject.SetActive(false);
        _actionImage.sprite = null;
    }
}
