using System;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter _baseCounter;
    [SerializeField] private GameObject[] _visualGameObjects;

    void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if (e.SelectedCounter == _baseCounter)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        foreach (GameObject _visualGameObject in _visualGameObjects)
        {
            _visualGameObject.SetActive(true);
        }
    }

    private void Hide()
    {
        foreach (GameObject _visualGameObject in _visualGameObjects)
        {
            _visualGameObject.SetActive(false);
        }
    }
}
