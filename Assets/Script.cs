using UnityEngine;
using Ink.Runtime;

public class Script: MonoBehaviour
{
    // ink JSON
    [SerializeField] private TextAsset inkFile;
    // 画布
    [SerializeField] private Canvas canvas;

    // ink 故事
    private Story _inkStory;
    // 是否需要新的故事片段
    private bool _storyNeeded;
    // 距离（用来确定内容和按钮位置）
    private int _padding = 10;

    /* UI Prefabs */
    // 文本
    [SerializeField] private UnityEngine.UI.Text text;
    // 按钮
    [SerializeField] private UnityEngine.UI.Button btnChoice;

    // 初始化
    private void Awake()
    {
        _storyNeeded = true;
        _inkStory = new Story(inkFile.text);
        
        // 设置玩家名称
        _inkStory.variablesState["player_name"] = "Someone";
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_storyNeeded) return;

        // 清空画布内容
        RemoveChildren();

        // 各个元素的偏移，需重新计算
        float vOffset = 0;

        // 如果故事可以继续
        if (_inkStory.canContinue)
        {
            var storyText = Instantiate(text);
            
            // 取得要显示的文字
            storyText.text = _inkStory.Continue();
            
            // 定位相关
            storyText.transform.SetParent(canvas.transform, false);
            storyText.transform.Translate(new Vector2(0, vOffset));
            vOffset -= (storyText.fontSize + _padding);
        }

        // 如果有多个选择，那么逐个处理
        if (_inkStory.currentChoices.Count > 0)
        {
            for (var i = 0; i < _inkStory.currentChoices.Count; i++)
            {
                // 定位相关
                var choiceButton = Instantiate(btnChoice);
                choiceButton.transform.SetParent(canvas.transform, false);
                choiceButton.transform.Translate(new Vector2(0, vOffset));

                // 取得选择
                var choice = _inkStory.currentChoices[i];

                // 设置按钮的选择文本
                var choiceText = choiceButton.GetComponentInChildren<UnityEngine.UI.Text>();
                choiceText.text = choice.text;

                // 定位相关
                var layoutGroup = choiceButton.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>();
                vOffset -= (choiceText.fontSize + layoutGroup.padding.top + layoutGroup.padding.bottom + _padding);

                // 获取按钮点击后对应的路径
                var path = choice.pathStringOnChoice;
                choiceButton.onClick.AddListener(delegate { ChoicePathSelected(path); });
            }
        }

        // 本次操作完成，等待响应
        _storyNeeded = false;
    }

    /// <summary>
    /// 清空画布内容
    /// </summary>
    private void RemoveChildren()
    {
        var childCount = canvas.transform.childCount;
        for (var i = childCount - 1; i >= 0; --i)
        {
            Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// 选择路径
    /// </summary>
    /// <param name="path">路径名称</param>
    private void ChoicePathSelected(string path)
    {
        _inkStory.ChoosePathString(path);
        _inkStory.Continue();
        _storyNeeded = true;
    }
}