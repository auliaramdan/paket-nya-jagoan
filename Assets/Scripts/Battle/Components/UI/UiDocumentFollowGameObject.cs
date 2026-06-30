using UnityEngine;
using UnityEngine.UIElements;

public class UiDocumentFollowGameObject : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private Vector3 offset, pos;
    
    private Camera _camera;
    private VisualElement _container;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _container = uiDocument.rootVisualElement.Q("container");
        _container.style.position = Position.Absolute;
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        pos = _camera.WorldToViewportPoint(target.transform.position + offset);
        _container.style.left = Length.Percent(pos.x * 100);
        _container.style.bottom = Length.Percent(pos.y * 100);
    }
}
