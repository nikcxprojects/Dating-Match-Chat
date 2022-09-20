using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainWindow;
    [SerializeField] private GameObject _chooseWindow;
    [SerializeField] private GameObject _homeWindow;

    [SerializeField] private GameObject _girlPrefab;
    [SerializeField] private Transform _girlsTransform;
    [SerializeField] private List<GirlData> _allGirls;

    private RectTransform _currentPeople;
    private List<GameObject> _peopleList = new List<GameObject>();
    [SerializeField] private List<GirlData> _likeGirls = new List<GirlData>();
    [SerializeField] private List<GirlData> _dislikeGirls = new List<GirlData>();

    [SerializeField] private GameObject _matchPrefab;

    [SerializeField] private Transform _historyTransform;
    [SerializeField] private Transform _likesTransform;
    [SerializeField] private Transform _dislikesTransform;

    private List<GameObject> _historyPeoples = new List<GameObject>();
    private List<GameObject> _likesPeoples = new List<GameObject>();
    private List<GameObject> _dislikesPeoples = new List<GameObject>();

    [SerializeField] private GameObject _profileWindow;
    [SerializeField] private GirlCell _profileGirl;

    private void OnEnable() => SwipeManager.SwipeEvent += SwipeEvent;
    private void OnDisable() => SwipeManager.SwipeEvent -= SwipeEvent;

    private void SwipeEvent(Vector2 direction)
    {
        SwipeGirl(direction == Vector2.right);
    }
    
    public void InitializeMatch()
    {
        InitializeMatchList(_likeGirls, _likesTransform, _likesPeoples);
        InitializeMatchList(_dislikeGirls, _dislikesTransform, _dislikesPeoples);
        InitializeMatchList(_likeGirls, _historyTransform, _historyPeoples);
        InitializeMatchList(_dislikeGirls, _historyTransform, _historyPeoples);
    }

    public void ClearMatch()
    {
        foreach (GameObject people in _historyPeoples)
            Destroy(people);

        foreach (GameObject people in _likesPeoples)
            Destroy(people);

        foreach (GameObject people in _dislikesPeoples)
            Destroy(people);

        _historyPeoples.Clear();
        _dislikesPeoples.Clear();
        _likesPeoples.Clear();
    }

    private void InitializeMatchList(List<GirlData> list, Transform transformWindow, List<GameObject> saveList)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {

            var obj = Instantiate(_matchPrefab, transformWindow);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<MatchCell>().SetGirlData(list[i]);
            obj.GetComponent<MatchCell>().InitializeCell();
            saveList.Add(obj);

        }
        _likesTransform.GetComponentInParent<ScrollRect>().normalizedPosition = Vector3.zero;

    }

    public void ClearGirl()
    {
        foreach (GameObject people in _peopleList)
            Destroy(people);

        _peopleList.Clear();
        _currentPeople = null;
    }

    public void OpenProfileGirl(GirlData girlData)
    {
        _profileWindow.SetActive(true);
        _profileGirl.SetGirlData(girlData);
        _profileGirl.InitializeCell();
        _currentPeople = _profileGirl.GetComponent<RectTransform>();
    }

    public void CloseProfileGirl()
    {
        _profileWindow.SetActive(false);
        _currentPeople = null;
    }

    public void SwipeGirl(bool isLike)
    {
        if (_currentPeople == null)
            return;

        Debug.Log("QQ");

        if (isLike)
        {
            if (!_likeGirls.Contains(_currentPeople.GetComponent<GirlCell>().girlData))
            {
                _likeGirls.Add(_currentPeople.GetComponent<GirlCell>().girlData);
                if (_dislikeGirls.Contains(_currentPeople.GetComponent<GirlCell>().girlData))
                    _dislikeGirls.Remove(_currentPeople.GetComponent<GirlCell>().girlData);
            }
        }
        else
        {
            if (!_dislikeGirls.Contains(_currentPeople.GetComponent<GirlCell>().girlData))
            {
                _dislikeGirls.Add(_currentPeople.GetComponent<GirlCell>().girlData);
                if (_likeGirls.Contains(_currentPeople.GetComponent<GirlCell>().girlData))
                    _likeGirls.Remove(_currentPeople.GetComponent<GirlCell>().girlData);
            }
        }

        _currentPeople.eulerAngles = Vector2.zero;

        if (_profileWindow.activeSelf)
        {
            ClearMatch();
            InitializeMatch();
            return;
        }

        Destroy(_currentPeople.gameObject);    
        _peopleList.RemoveAt(_peopleList.Count - 1);
        if (_peopleList.Count > 0)
            _currentPeople = _peopleList[_peopleList.Count - 1].GetComponent<RectTransform>();
        else
            _currentPeople = null;
    }

    public void Loading() => StartCoroutine(ILoading());
    private IEnumerator ILoading()
    {
        yield return new WaitForSeconds(1.1f);
        _chooseWindow.SetActive(true);
        _mainWindow.SetActive(false);

        InitializeGirls();
    }

    public void InitializeGirls()
    {
        foreach(GirlData girl in _allGirls)
        {
            var obj = Instantiate(_girlPrefab, _girlsTransform);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<GirlCell>().SetGirlData(girl);
            obj.GetComponent<GirlCell>().InitializeCell();
            _peopleList.Add(obj);
        }

        _currentPeople = _peopleList[_peopleList.Count - 1].GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (_currentPeople == null)
            return;

        Vector2 swipeDelta = SwipeManager.instance.swipeDelta;
        if (swipeDelta.magnitude != 0)
        {
            if (swipeDelta.x > 0)
            {
                float percent = -swipeDelta.magnitude * 0.05f;
                _currentPeople.eulerAngles = new Vector3(0, 0, Mathf.Clamp(percent, -6, 0));
                if (_currentPeople.eulerAngles.z > 4.0f)
                    _currentPeople.GetComponent<GirlCell>().Like();
            }
            else if(swipeDelta.x < 0)
            {
                float percent = swipeDelta.magnitude * 0.05f;
                _currentPeople.eulerAngles = new Vector3(0, 0, Mathf.Clamp(percent, 0, 6));
                if (_currentPeople.eulerAngles.z > 4.0f)
                    _currentPeople.GetComponent<GirlCell>().Dislike();
            }
        }
        else
        {
            _currentPeople.eulerAngles = Vector2.zero;
            _currentPeople.GetComponent<GirlCell>().ResetLike();
        }
    }
}
