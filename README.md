# UnityEasyLibs

一套轻量、零依赖、开箱即用的 Unity 通用功能库。所有模块统一使用 `EasyLibs` 命名空间，按职责划分为 UI、动画、配置、注入、本地化与工具集六大类，代码结构清晰、耦合度低，可作为独立组件按需引入到任意 Unity 项目。

## 目录结构

```
UnityEasyLibs/
├── EasyAutoUI     # UI 自动化与数据绑定
├── EasyAnim       # 2D 帧动画
├── EasyConfig     # Excel 配置表加载
├── EasyInject     # 轻量依赖注入与信号系统
├── EasyLocalize   # 多语言本地化
└── EasyUtils      # 通用工具集、对象池、编辑器扩展
```

---

## 模块详解

### 1. EasyAutoUI —— UI 自动化与数据绑定

通过 `AutoContainer` 与 `AutoWidget` 两个核心组件，将 UI 层级结构自动关联，免去手动拖拽引用与硬编码查找。

**核心能力：**

- **自动关联**：`AutoContainer` 递归扫描子节点，自动将带 `AutoWidget` 的节点按名称注册，运行时通过名称即可获取控件。
- **名称获取控件**：`GetWidget(name)`、`GetWidget<T>(name)` 支持按名称取控件或指定组件。
- **统一操作接口**：`IAutoWidget` 抽象了 Text / Toggle / Slider / Button / Image / Dropdown / InputField / Animator 等控件，提供 `SetValue`、`SetButtonEvent`、`PlayAnimation` 等一组统一方法。
- **数据绑定**：`Bind<T>(widgetName, data, fieldName)` 将任意 `BaseBindingData` 的字段与 UI 控件绑定，数据变化自动驱动 UI 刷新（内置 `TextBinding`、`TMPBinding`、`ImageBinding`、`InputFieldBinding`、`ToggleBinding`）。
- **虚拟控件**：`VirtualWidget` 提供空操作兜底，控件缺失时不会报错，便于 UI 迭代期平滑过渡。
- **编辑器工具**：`AutoUiTool.ConnectWidgets`（快捷键 `Ctrl+Shift+I`）一键完成关联。

**内置实用控件（UtilityWidgets）：**

| 控件 | 说明 |
| --- | --- |
| `EasyScrollView` | 高性能虚拟滚动列表，支持横向/纵向、每行多列、首/尾固定项、复用回收 |
| `MoreEasyScrollView` | 非复用型滚动列表，提供 `SnapTo` 定位 |
| `EasyTabView` | Tab 页签切换，支持关联对象与切换回调 |
| `EasyToggleGo` | 用 Toggle 控制一组 GameObject 显隐 |
| `EasyNumAdjust` | 数字加减控件（带上下限） |
| `EasyLoader` | 简单列表加载器 |
| `EasyEditableLabel` | 可编辑文本标签（TextMeshPro，含粘贴/提交回调） |

**快速上手：**

```csharp
[SerializeField] AutoContainer m_ui;

void Start()
{
    // 按名称设置文本
    m_ui.SetValue("txtTitle", "Hello");

    // 绑定按钮事件
    m_ui.SetButtonEvent("btnConfirm", OnConfirm);

    // 数据绑定：data.m_strValue 变化时，txt01 自动刷新
    m_ui.Bind<TextBinding>("txt01", data, "m_strValue");
}
```

### 2. EasyAnim —— 2D 帧动画

以 `ScriptableObject` 配置帧序列的轻量帧动画方案，不依赖 Mecanim。

- **`AnimClip`**：单段动画，定义帧序列 `List<Sprite>`、播放间隔 `interval`、播放模式 `once` / `loop`。
- **`AnimSet`**：动画集合，按名称管理多段 `AnimClip`。
- **`AnimationPlayer`**：`SpriteRenderer` 版播放器，循环播放单段动画。
- **`AnimatorPlayer`**：`SpriteRenderer` 版多段播放器，`Play(clipName, speed)` / `Stop()`，支持播放速度、结束置空、默认帧恢复。
- **`UIAnimatorPlayer`**：`Image`（UGUI）版播放器，支持自动播放、循环与默认 Sprite 回退。

> 通过 `Assets > Create > EasyLibs` 菜单即可创建 `AnimClip` 与 `AnimSet` 资源。

### 3. EasyConfig —— Excel 配置表加载

将 Excel 表格直接转为强类型配置对象，供运行时查询。

- **`SettingMgr`**：单例配置管理器，`LoadData<T>(name)` 批量加载、`GetInfo<T>(id)` 按 id 查询，内置缓存。
- **`BaseCfg`**：配置基类，含 `id` 字段与数组解析辅助（`|` 分隔）。
- **Excel 自动解析**：基于 ExcelDataReader，编辑器下直接读取 `.xlsx`，运行时读取导出的 JSON（`Resources/Settings/`），自动识别 int / double / string / bool 类型。
- **编辑器工具**：`EasyLibs/Export Settings` 菜单批量导出全部配置表。

**快速上手：**

```csharp
// 定义配置类，字段名与 Excel 表头一致
[Serializable]
public class ItemCfg : BaseCfg
{
    public string name;
    public int price;
}

// 加载与查询
List<ItemCfg> all = SettingMgr.Instance.LoadData<ItemCfg>("Item");
ItemCfg item = SettingMgr.Instance.GetInfo<ItemCfg>("1001");
```

### 4. EasyInject —— 轻量依赖注入与信号系统

**依赖注入（DI）：**

- **`InjectContext`**：全局注入容器，`Bind<T>(obj)` 注册、`Inject(obj)` 通过反射注入字段。
- **`[Inject]`** 特性：标记需要注入的字段，支持 `[Inject("id")]` 按标识符区分同一类型的多个绑定。
- **生命周期**：`BindingData` 支持 `single`（单例）/ `transient`（每次新建）两种绑定方式。
- **`MonoBinding`**：组件化绑定，在 Inspector 中配置要绑定的 MonoBehaviour / GameObject / ScriptableObject，Awake 自动注册、OnDestroy 自动解绑。
- **`MonoInject`**：场景注入入口，自动为自身及子物体上的所有 MonoBehaviour 执行注入。

**信号系统（Signal）：**

- **`SignalBus`**：类型安全的发布/订阅事件总线，`Subscribe<TSignal>` / `Fire(signal)` / `SubscribeOnce<TSignal>`。
- **命令模式**：`RegisitCommand<TSignal>(commandType)` 将信号绑定到 `BaseCmd` 命令，信号触发时自动实例化并执行命令（命令内字段同样支持注入）。

**快速上手：**

```csharp
public class Player : MonoBehaviour
{
    [Inject] private IGameService m_service;   // 自动注入

    void Start()
    {
        SignalBus bus = new SignalBus();
        bus.Subscribe<PlayerDead>(OnPlayerDead);
    }
}
```

### 5. EasyLocalize —— 多语言本地化

运行时多语言切换，支持文本与图片的本地化。

- **`LocalizeMgr`**：本地化管理器，`ChangeLanguage(index/name)` 一键切换，自动通知所有已注册控件刷新。
- **`LocalizeConfig`**：语言配置资产，定义语言列表、字体属性（按语言适配字号/字体）、本地化图片。
- **本地化控件**：`LocalizeTextWidget`、`LocalizeImageWidget`、`LocalizeDropdownWidget`、`LocalizeInputWidget`、`LocalizeMeshTextWidget`、`LocalizeGoWidget`，挂载即可自动注册与刷新。
- **参数化文本**：`GetText("key[param]")` 支持占位符替换，如 `"第 [n] 关"`。
- **字体适配**：不同语言可配置不同字体与字号缩放比例，解决多语言排版差异。

### 6. EasyUtils —— 通用工具集

**`Util` 静态工具类**，涵盖常用场景操作：

- 批量递归设置 Layer / Color / Alpha
- 唯一 ID（GUID）、时间戳、时间格式化
- 随机数、按权重随机、随机不重复索引
- 数学运算：贝塞尔曲线、绕点旋转、等比缩放、Clamp
- 字符串处理：高亮、替换、MD5、日期解析
- 纹理处理：裁剪透明边缘
- 安全遍历 `SafeListInvoker`（遍历中删除安全）
- 彩色日志输出

**`Bezier`**：二次 / 三次贝塞尔曲线插值。

**`Singleton<T>`**：线程安全的泛型单例基类。

**`GameObjectPool`**：基于 `Stack` 的对象池，支持预热缓存、获取与回收。

**编辑器扩展（Editor）：**

| 工具 | 说明 |
| --- | --- |
| `GameToolsMenu` | 打开游戏场景、清空 PlayerPrefs、导出配置、源码转 UTF-8、导出源码等菜单项 |
| `SceneTools` | 场景辅助工具 |
| `HierarchyTextColor` | Hierarchy 层级文本着色 |
| `ImageUsageFinder` | 图片资源引用查找 |
| `AnalyticTools` | 分析工具 |

---

## 快速开始

1. 将 `UnityHybridLib` 目录复制到你的 Unity 项目中（或仅复制所需的单个模块目录）。
2. 代码统一位于 `EasyLibs` 命名空间，直接 `using EasyLibs;` 使用。
3. 使用 `EasyConfig` 前需引入 `ExcelDataReader` 与 `Newtonsoft.Json` 依赖；其余模块零依赖。

## 依赖

| 依赖 | 用途 | 是否必需 |
| --- | --- | --- |
| ExcelDataReader | Excel 配置解析 | 仅 `EasyConfig` |
| Newtonsoft.Json | JSON 序列化 | 仅 `EasyConfig` |
| TextMeshPro | UI 文本 | `EasyAutoUI` / `EasyLocalize` 部分控件 |

## License

[MIT](./LICENSE)
