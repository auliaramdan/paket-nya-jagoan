using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleStatusUI : MonoBehaviour
{
    [SerializeField]
    private UIDocument uiDocument;

    [SerializeField]
    private BattleEntity entity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = uiDocument.rootVisualElement;
        root.dataSource = entity;

        BindToVisualElement(root.Q("atk-status-label"), "text", PropertyPath.FromName(nameof(BattleEntity.Atk)));
        BindToVisualElement(root.Q("def-status-label"), "text", PropertyPath.FromName(nameof(BattleEntity.Def)));
        BindToVisualElement(root.Q("spd-status-label"), "text", PropertyPath.FromName(nameof(BattleEntity.Spd)));

        BindToVisualElement(root.Q("health-bar"), "value", PropertyPath.FromName(nameof(BattleEntity.CurrentHealth)));

        var progressBar = root.Q<ProgressBar>("health-bar").Q(className: "unity-progress-bar__progress");
        var colorBinding = new DataBinding
        {
            dataSource = entity,
            dataSourcePath = PropertyPath.FromName(nameof(BattleEntity.CurrentHealth))
        };
        colorBinding.sourceToUiConverters.AddConverter((ref float v) =>
            new StyleColor(Color.Lerp(Color.red, Color.green, v/100)));
        progressBar.SetBinding("style.backgroundColor", colorBinding);
    }

    private void BindToVisualElement(VisualElement visualElement, string bindingId, PropertyPath propertyPath)
    {
        visualElement.SetBinding(bindingId,
            new DataBinding { dataSourcePath = propertyPath, bindingMode = BindingMode.ToTarget });
    }
}
