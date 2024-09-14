using UnityEngine;

public class GenerateCar : MonoBehaviour
{
    [SerializeField] private Transform carPos;
    [SerializeField] private GameObject _carPrefab;

    private GameObject _carRefs;

    private void Start()
    {
        if(_carRefs != null)
        {
            Destroy(_carRefs);
        }
        _carRefs = Instantiate(_carPrefab,carPos);
    }
}
