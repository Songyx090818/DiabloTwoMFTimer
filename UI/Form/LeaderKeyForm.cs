using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks; // 引用 Task
using System.Windows.Forms;
using DiabloTwoMFTimer.Interfaces;
using DiabloTwoMFTimer.Models;
using DiabloTwoMFTimer.Repositories;
using DiabloTwoMFTimer.UI.Components;
using DiabloTwoMFTimer.UI.Theme;
using DiabloTwoMFTimer.Utils;

namespace DiabloTwoMFTimer.UI.Form;

public partial class LeaderKeyForm : System.Windows.Forms.Form
{
    private readonly ICommandDispatcher? _commandDispatcher;

    // UI 控件
    private FlowLayoutPanel _breadcrumbPanel = null!; // 面包屑导航栏
    private FlowLayoutPanel _itemsPanel = null!;      //按键选项区域
                                                      // private ThemedLabel _lblPrompt = null!;           // 提示文字

    // 状态数据
    private readonly Stack<KeyMapNode> _pathStack = new(); // 记录当前路径
    private List<KeyMapNode> _currentNodes = null!;       // 当前显示的节点列表

    private readonly IKeyMapRepository _keyMapRepository;


    // 构造函数：支持依赖注入，但也允许为空方便测试
    public LeaderKeyForm(ICommandDispatcher? commandDispatcher = null, IKeyMapRepository? keyMapRepository = null)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));
        _keyMapRepository = keyMapRepository ?? throw new ArgumentNullException(nameof(keyMapRepository));

        // 1. 窗体基础设置
        this.FormBorderStyle = FormBorderStyle.None;
        this.ShowInTaskbar = false;
        this.TopMost = true;
        this.StartPosition = FormStartPosition.Manual;
        this.BackColor = AppTheme.Colors.Background; // 深灰背景
        this.Opacity = 0.95; // 稍微一点透明度，增加现代感
        this.DoubleBuffered = true;

        // 2. 初始化 UI
        InitializeControls();

        // 3. 加载初始数据 (如果没传入数据，使用测试数据)
        // 实际使用时，应该从 KeyMapRepository 获取
        RefreshData();
        RefreshUI();
    }

    private void InitializeControls()
    {
        // 计算尺寸：宽度全屏，高度 20%
        var screen = Screen.PrimaryScreen?.Bounds ?? new Rectangle(0, 0, 1920, 1080);
        this.Width = screen.Width;
        this.Height = (int)(screen.Height * 0.2);

        // 定位到底部 (模仿 VS Code 或 Emacs 的 mini-buffer)
        this.Location = new Point(screen.X, screen.Bottom - this.Height);

        // --- 顶部：面包屑导航 ---
        _breadcrumbPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            Height = 40,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(20, 10, 0, 0),
            BackColor = AppTheme.Colors.ControlBackground // 稍微浅一点的背景区分头部
        };

        // --- 底部：按键列表 ---
        _itemsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(20, 20, 20, 20),
            AutoScroll = true
        };

        this.Controls.Add(_itemsPanel);
        this.Controls.Add(_breadcrumbPanel);
    }

    /// <summary>
    /// 核心方法：刷新界面显示
    /// </summary>
    private void RefreshUI()
    {
        _breadcrumbPanel.SuspendLayout();
        _itemsPanel.SuspendLayout();

        // 1. 更新面包屑
        _breadcrumbPanel.Controls.Clear();
        // 添加根节点标志
        AddBreadcrumbItem("Leader");
        foreach (var node in _pathStack.Reverse())
        {
            AddBreadcrumbItem(" > ");
            AddBreadcrumbItem(node.Text);
        }

        // 2. 更新按键列表
        _itemsPanel.Controls.Clear();

        // 排序：有 Key 的排前面，按字母顺序
        foreach (var node in _currentNodes.OrderBy(n => n.Key))
        {
            var itemControl = CreateItemControl(node);
            _itemsPanel.Controls.Add(itemControl);
        }

        // 如果没有子节点了，说明配置有问题或到了尽头
        if (_currentNodes.Count == 0)
        {
            var lbl = new ThemedLabel { Text = "没有可用操作", AutoSize = true, ForeColor = Color.Gray };
            _itemsPanel.Controls.Add(lbl);
        }

        _breadcrumbPanel.ResumeLayout();
        _itemsPanel.ResumeLayout();
    }

    private void AddBreadcrumbItem(string text)
    {
        var lbl = new Label // 面包屑用普通 Label 即可，或者用 ThemedLabel
        {
            Text = text,
            AutoSize = true,
            Font = new Font(AppTheme.Fonts.FontFamily, 12, FontStyle.Bold),
            ForeColor = AppTheme.Colors.Primary, // 砂金色
            Margin = new Padding(0)
        };
        _breadcrumbPanel.Controls.Add(lbl);
    }

    private Control CreateItemControl(KeyMapNode node)
    {
        // 1. 计算缩放后的尺寸
        int keyBoxSize = ScaleHelper.Scale(50);
        int itemWidth = ScaleHelper.Scale(180);
        int itemHeight = ScaleHelper.Scale(50);

        // 容器
        var panel = new Panel
        {
            Size = new Size(itemWidth, itemHeight),
            Margin = new Padding(0, 0, 15, 15),
            BackColor = Color.Transparent
        };

        // 按键显示 [T]
        var lblKey = new Label
        {
            Text = $"[{node.Key.ToUpper()}]",
            // 稍微把字号改小一点点 (14 -> 12)，配合 Consolas 宽字体更安全
            Font = new Font("Consolas", 12, FontStyle.Bold),
            ForeColor = AppTheme.Colors.Primary,
            Location = new Point(0, 0),
            Size = new Size(keyBoxSize, keyBoxSize),

            // 【关键】必须显式禁用 AutoSize，否则 Size 设置可能无效
            AutoSize = false,
            TextAlign = ContentAlignment.MiddleCenter,
            BorderStyle = BorderStyle.FixedSingle
        };

        // 功能描述
        var lblText = new Label
        {
            Text = node.Text,
            Font = AppTheme.Fonts.Regular,
            ForeColor = AppTheme.Colors.Text,
            // 让文字紧贴方块右侧
            Location = new Point(keyBoxSize + ScaleHelper.Scale(5), 0),
            Size = new Size(itemWidth - keyBoxSize - ScaleHelper.Scale(5), itemHeight),
            TextAlign = ContentAlignment.MiddleLeft
        };

        panel.Controls.Add(lblKey);
        panel.Controls.Add(lblText);
        return panel;
    }

    /// <summary>
    /// 全局键盘监听 (当窗体激活时)
    /// </summary>
    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        // 1. 处理 ESC：返回上一级 或 关闭
        if (keyData == Keys.Escape)
        {
            if (_pathStack.Count > 0)
            {
                _pathStack.Pop();
                // 重新获取上一层的节点列表
                if (_pathStack.Count > 0)
                    _currentNodes = _pathStack.Peek().Children ?? [];
                else
                    _currentNodes = _keyMapRepository.LoadKeyMap();

                RefreshUI();
            }
            else
            {
                this.Hide(); // 关闭窗口
            }
            return true;
        }

        // 2. 将按键转换为字符 (简单处理 A-Z, 0-9)
        string keyChar = GetCharFromKeys(keyData);
        if (!string.IsNullOrEmpty(keyChar))
        {
            HandleInput(keyChar);
            return true;
        }

        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void HandleInput(string key)
    {
        // 查找匹配的节点
        var targetNode = _currentNodes.FirstOrDefault(n => n.Key.Equals(key, StringComparison.OrdinalIgnoreCase));

        if (targetNode == null) return; // 无效按键

        if (targetNode.IsLeaf)
        {
            // --- 执行操作 ---
            ExecuteAction(targetNode);
        }
        else
        {
            // --- 进入下一级 ---
            if (targetNode.Children != null && targetNode.Children.Count > 0)
            {
                _pathStack.Push(targetNode);
                _currentNodes = targetNode.Children;
                RefreshUI();
            }
        }
    }

    private void ExecuteAction(KeyMapNode node)
    {
        this.Hide(); // 先关闭界面

        if (_commandDispatcher != null && !string.IsNullOrEmpty(node.Action))
        {
            // 真正的执行逻辑
            _ = _commandDispatcher.ExecuteAsync(node.Action);
        }
        else
        {
            // 测试用的反馈
            ThemedMessageBox.Show($"[演示模式] 执行命令:\n{node.Action}\n\n描述: {node.Text}", "Leader Key Action");
        }

        LogManager.WriteDebugLog("LeaderKeyForm", $"执行操作: {node.Action}");
        // 执行完重置状态，下次打开回到根目录
        ResetState();
    }

    private void ResetState()
    {
        LogManager.WriteDebugLog("LeaderKeyForm", $"重置状态: 清空路径栈");
        _pathStack.Clear();
        // 确保 _currentNodes 不为空
        if (_currentNodes == null || _currentNodes.Count == 0)
        {
            // 再次尝试重新加载，或者给个空列表
            _currentNodes = [];
        }
        RefreshUI();
    }

    // 失去焦点时自动隐藏
    protected override void OnDeactivate(EventArgs e)
    {
        base.OnDeactivate(e);
        // 调试时可以注释掉这行，否则一点其他地方窗口就没了，不方便看
        this.Hide();
        ResetState();
    }

    // --- 辅助方法 ---

    private string GetCharFromKeys(Keys key)
    {
        var k = key & ~Keys.Modifiers; // 忽略 Ctrl/Alt/Shift
        if (k >= Keys.A && k <= Keys.Z) return k.ToString().ToLower();
        if (k >= Keys.D0 && k <= Keys.D9) return k.ToString().Replace("D", "");
        if (k >= Keys.NumPad0 && k <= Keys.NumPad9) return k.ToString().Replace("NumPad", "");
        return string.Empty;
    }

    /// <summary>
    /// 公开一个刷新数据的方法，方便在修改配置后热重载
    /// </summary>
    public void RefreshData()
    {
        try
        {
            _currentNodes = _keyMapRepository.LoadKeyMap();
        }
        catch (Exception ex)
        {
            // 如果读取失败，至少保证程序不崩，给一个错误提示节点
            _currentNodes =
            [
                new KeyMapNode { Text = $"配置加载失败: {ex.Message}", Key = "!" }
            ];
        }
        ResetState();
    }

    protected override void OnVisibleChanged(EventArgs e)
    {
        base.OnVisibleChanged(e);

        // 当窗口变为可见时，强制重置到根目录
        if (this.Visible)
        {
            ResetState();
        }
    }
}