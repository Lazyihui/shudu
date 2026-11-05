- A1 项目定位与参考来源
	- B1 当前项目：`D:\UnityProject\Hamdoku2` 是正在开发的 Unity 复刻项目，代码、资源与场景修改应落在此目录。

	- B2 原项目：`D:\0000001apkUnbag\01_逆向工程项目\Hamdoku2` 是待复刻原项目的拆解资料目录，用于核对玩法、页面、资源、数值和表现；默认只读，不修改原始取证资料。

	- B3 架构参考：`D:\UnityProject\Mahjong_j`。遵循其分层、职责与生命周期约定，结合 Hamdoku 实际业务实现，不套用麻将、提现等无关业务。

	- B4 本文件参考 `Mahjong_j/AGENTS.md` 与 `Mahjong_j/Docs/0.1.0.0/Architecture.md` 整理，于 2026-10-27 经用户确认建立。不复制参考项目的 Docs 文档。

- A2 协作约定
	- B1 修改前检查当前代码、场景、Prefab、SO 和引用关系，沿用已有实现，不另建平行启动、资源加载或 UI 管理体系。

	- B2 用户使用“比如”“例如”等举例用语时，理解其通用意图，检查当前任务范围内的同类问题，不只处理例子本身。

	- B3 用户明确要求先确认时，先列出计划创建或修改的脚本、职责、资源及交付边界，确认后实施。已确认范围内的必要修复和常规实现选择继续执行，不反复确认。

	- B4 参考架构未涉及且会改变职责或依赖关系的设计，先说明并向用户确认。不得擅自新增 interface、abstract class、单例或并行架构；已确认的 IPanelAsset 契约继续使用。

	- B5 区分原作事实、工程实现选择和待核实内容。拆解所得空方法体只代表结构；反汇编、静态配置和工具中间表示不等于运行验证结果。

	- B6 不覆盖用户已有修改，不修改任务范围外的文件；重命名、删除或迁移资源前先检查代码和序列化引用。

	- B7 后续每次迁移功能或资源前，先列明拟迁移的原作内容及证据、当前工程目标脚本与资源、职责与行为变化、交付和验证范围，向用户确认后实施。代码阅读引导的确认仅授权文档补充，不代表已授权功能迁移；已确认范围内的必要修复按 B3 执行。

- A3 文档规范
	- B1 只有用户明确要求时才新增文档或扩展文档主题；已要求维护的工作记录随项目修改更新，无需逐次确认。新增文档应在本文件添加入口链接。

	- B2 新增或重写的规范类 Markdown 使用 A1、A2…作为一级编号，B1、B2…作为二级编号，C1、C2…作为三级编号，更深层依次类推；每个父条目的子编号从 1 开始。

	- B3 每深入一级增加一个 Tab，正文与所属条目对齐。同级条目之间空一行，父条目与首个子条目之间不加空行。不使用 # 标题或混用扁平数字编号。

	- B4 普通说明使用普通列表，只有追踪完成状态的条目使用复选框。不得仅因标题包含“验收”或“待办”就把全部说明改成复选框。

	- B5 内容清楚紧凑，路径、类型名和验证边界准确。现有工作记录保留按日期与事项组织的格式，不因建立本规范而重写历史条目。

- A4 分层与上下文
	- B1 ClientMain 是启动装配入口，负责创建与注入依赖、连接系统事件和组织 Unity 生命周期。只有 ClientMain 可以向 Context 字段注入依赖。

	- B2 GameContext 及其他 Context 只声明数据与引用字段，不包含函数、显式构造函数或承载行为的属性，不自行创建依赖。

	- B3 静态 System 组织流程、阶段和更新，Context 作为参数传入；状态变更通过 Controller 完成，不直接修改业务运行数据。

	- B4 静态 Controller 负责业务判断、运行状态变更、Module 调用和 UI 更新，Context 作为参数传入。除 ClientMain 注入外，只有 Controller 可以修改 Context 中的运行值及所持状态、Entity 和集合。

	- B5 Panel／Widget 负责显示、局部视觉表现和交互上报，通过 Action／Func 委托交由 Controller 处理，不自行修改权威业务结果或调用 SDK。

	- B6 Module 封装具体能力，不持有或接收 Context，不通过委托间接获取 Context。Module 可以读取具体 Entity，但不得修改其业务字段和集合。

	- B7 Repository／Factory 管理实例查询、登记、创建和移除；Entity／运行状态保存业务数据，Model 保存具体数据结构。低层不得反向依赖或调用高层，结果通过委托或高层读取数据传递。

	- B8 使用 Context 中的对象时，先取语义清楚的局部变量，再操作对象，避免重复深层访问链。不得通过辅助函数、委托或可写引用绕过注入和状态修改职责。

- A5 代码规范
	- B1 每份 C# 文件只定义一个类型，文件名与类型名对应，不定义嵌套业务类型。运行时与编辑器代码分别位于 Assets/Src_Runtime、Assets/Src_Editor，使用分离程序集。

	- B2 沿用 Hamdoku、Hamdoku.Main、Hamdoku.Controllers、Hamdoku.Editor 等已有命名空间和目录。名称表达实际职责，不保留无关参考项目名称。

	- B3 类型与函数使用 PascalCase 或已有 Panel_Home_Controller 等下划线分隔命名；字段、参数和局部变量使用 camelCase，允许 state_Loading 等既有形式，禁止名称以下划线开头。

	- B4 禁止单例和静态业务状态。静态类不持有运行字段，专用临时字段除外；const 和 static readonly 放在对应 Const 静态类，可配置业务值仍放入 SO。

	- B5 枚举的 0 值为 None，名称以 Type／Status／Enum 结尾，位域以 Flag 结尾。内部 ID 使用整数、整数枚举或由整数构成的 ID struct，不以字符串作为业务主索引；文件路径、资源标签及外部 SDK 标识按其契约使用字符串。

	- B6 单项查询使用 O(1) 或 O(log n) 索引。需要遍历散列集合时另外维护 List 或数组，并在增删时同步，不依赖字典遍历顺序。

	- B7 if、for、foreach 等语句使用大括号并展开代码块；函数、构造函数和属性访问器不写成单行。除匿名函数外不使用表达式体成员。

	- B8 不继承普通实例业务类；Unity MonoBehaviour／ScriptableObject、第三方要求或用户明确确认的继承除外。不为尚未需要的能力扩展抽象层。

	- B9 不擅自添加掩盖配置或资源错误的防御性 fallback。缺失引用、无效配置及不支持的数据暴露明确错误；原作正常分支和外部服务正常失败状态按业务处理。

- A6 UI、场景与资源
	- B1 正式运行入口为 Assets/Res_Runtime/Scene_Main.unity。维护现有相机与 Canvas；当前只有 Panel_Logo 作为启动面板随场景加载并由 ClientMain 绑定。

	- B2 其他 Panel 使用正式 Prefab，通过 AssetModule 的 Addressables default 标签加载，由 PanelFactory 创建并登记 PanelRepository。不得在场景、ClientMain 或 Context 中另持专用实例绕过管理。

	- B3 面板对外入口统一为 Open／Close。首次打开执行 Ctor 与委托注册，每次打开执行 Init，关闭时从 Repository 移除并执行 TearDown，清理监听、委托及实例。

	- B4 UI 使用 uGUI 与 TMP 文本，序列化引用随交付实际绑定。装饰图片和文字关闭 Raycast Target；交互入口具有有效 GraphicRaycaster、唯一 EventSystem 和按钮回调。

	- B5 布局覆盖目标竖屏、安全区域和合理宽高比变化；背景按比例铺满，文字检查缺字、截断、重叠和可读性。修改现有页面优先定点调整，不整体销毁重建。

	- B6 资源沿用 Assets/Res_Runtime，生产配置使用正式资产，不以运行时占位内容代替交付。编辑器工具可辅助制作，但用户打开正式场景后应能直接运行，不以先执行菜单作为前提。

	- B7 重命名或迁移脚本、Prefab、图片、SO 时保留必要的 .meta／GUID，并同步代码、场景、Prefab、SO 和 Addressables 引用。

	- B8 从隔离验证副本同步结果时，检查资源、.meta、程序集、包依赖及必要 ProjectSettings。尤其核对 EditorBuildSettings 的 Addressables 注册项与 DefaultObject GUID，不把 Library 缓存当作正式配置。

	- B9 UI 设计参考分辨率统一为 1080 × 1920，比例 9:16，目标方向为正向竖屏。依据原项目首页与游戏场景的 CanvasScaler 配置，使用 Scale With Screen Size、Match Width Or Height、匹配值 0.5，Reference Pixels Per Unit 为 100。后续页面和 Prefab 以此为布局基准，不自行更换参考尺寸。

	- B10 参考分辨率是 UI 设计坐标，不固定手机实际渲染为 1080 × 1920。移动端适配设备实际尺寸与安全区域；长屏、短屏通过锚点和布局调整，背景按比例铺满。横屏检查用于验证窗口变化时的布局容错，不代表产品支持横屏玩法。

	- B11 Player Settings 默认方向为 Portrait，默认桌面与 Android 窗口尺寸为 1080 × 1920，Web 默认预览尺寸为 540 × 960。编辑器 Game 视图验收优先使用 1080 × 1920 或 9:16，再检查其他竖屏比例；Player Settings 默认窗口尺寸不代替 Game 视图的独立预览设置。

	- B12 迁移 UI 前，必须先到原项目中查找对应 Prefab 及其依赖；页面保存在场景中时，提取原场景的 UI 层级作为迁移基础。不得跳过查找，自行搭建基础结构。

	- B13 找到原始 UI 后，保留其层级、布局和资源关系，再适配当前项目的命名、目录、脚本职责、生命周期、本地化和资源管理。

	- B14 确实找不到原始 Prefab／场景层级，或需要自行补建结构时，先说明查找结果、缺失部分和补建范围，经用户确认后实施。

- A7 配置、存档与本地化
	- B1 规则、关卡、解锁条件、时长和表现参数等生产配置统一使用 ScriptableObject。原作 JSON／CSV 可作为导入来源，导入后使用 SO 维护，避免两套生产配置并存。

	- B2 玩家进度、教程状态、请求状态和运行数据独立于共享 SO。Controller 修改业务数据，SaveModule 封装序列化及文件读写，不因缺失存档以外的读取错误静默重置进度。

	- B3 存档维护版本和合法性，使用临时文件及替换方式写入。验证存档与用户进度隔离，不把测试通关数据复制回正式环境。

	- B4 文案使用 L10NModule 现有 TryGet(UINameType, out string) 入口，每个文案条目对应一份 L10NSO，包含当前支持语言。Panel／Controller 不另建页面专用翻译表。

	- B5 每种语言由 L10NFontSO 指向一份覆盖所需字符的 TMP 字体，L10NModule 统一注册、切换和注销文本。禁止字体 Fallback 拼接同一种语言，禁止页面自行判断语言选择字体。

	- B6 当前语言为简体中文和英文。增加语言时同步扩展文案、语言选择、字体映射、离线字符烘焙与布局验证，不能仅添加翻译字段就宣称支持完成。

	- B7 每个实际接入的 SDK 独立封装 Module，由 Controller 调用并处理回调。未接入服务明确上报请求或显示准备状态，不使用假成功、日志或模拟数据冒充真实结果。

- A8 验证与交付
	- B1 按页面及其完整功能推进，交付同时包含资源、Prefab、脚本、运行状态、SO、本地化、交互和相邻已实现页面的接入。公共能力在首次需要的功能中实现，后续复用。

	- B2 检查正常流程及本次修改相关的失败、重复打开、重复初始化、返回、释放和进度恢复。按改动运行必要验证，不为低影响修改增加无意义测试或重复执行未受影响的检查。

	- B3 分别说明代码实现、Unity 编译、Play Mode、资源内容打包、应用构建、安装和真机验证状态。Addressables 内容打包通过不代表 Android APK 构建或真机验收通过。

	- B4 报告写明验证环境、范围、结果和限制。原作静态资料、后台渲染、编辑器点击及手机触摸验收不得互相替代；失败与编辑器自身异常分别记录，不隐瞒未解决的项目错误。

	- B5 隔离副本验证后核对同步完整性，检查当前工程能直接打开正式场景运行。不得因副本中执行过配置工具，就默认当前项目注册和配置正确。

	- B6 Unity 版本以 ProjectSettings/ProjectVersion.txt 为准，包版本以 Packages/manifest.json 与 packages-lock.json 为准。当前为 Unity 6000.5.2f1、Addressables 2.10.3，不直接复制参考项目旧包版本；升级前核对兼容性并验证。

	- B7 每次功能或资源迁移交付时，必须同步补充 [迁移验收文档](Docs/MigrationAcceptance.md)，面向用户写明本批范围、打开入口、操作步骤、预期结果、旧进度兼容、已知限制及开发验证状态。按批次追加，保留历史验收结论；开发自测通过不得代替用户验收，不自行勾选用户验收项目。用户反馈的问题与复验结果更新到对应批次。

- A9 工作记录
	- B1 [项目工作记录](WORK_LOG.md)：记录开发、资源接入、问题修复、验证结果和待办事项。

	- B2 每次完成项目修改后同步更新记录，写明日期、修改内容、主要文件、验证范围与结果、未完成或未验证事项。日期按用户所在时区记录。

	- B3 记录反映实际结果，不把方案、事件预留、空接口或准备提示写成目标功能已完成。本文件维护稳定规范，进度和历史留在工作记录中，避免重复维护。

	- B4 [Hamdoku2 UI 参考库](Docs/UILibrary/README.md)：离线入口为 [UI Library.html](<Docs/UILibrary/UI Library.html>)；[资料缺口](Docs/UILibrary/Missing.md) 与 [验证记录](Docs/UILibrary/Verification.md) 分别维护来源不足和实际验证结果。

	- B5 [UI 资料 TODO](Docs/UILibrary/TODO.md)：追踪用户确定留待后续补充的游戏画面与状态，区分已收到资料与已完成实现。

	- B6 [玩法迁移 TODO](Docs/GameplayMigrationTODO.md)：追踪首批教程、主线棋盘、结算、失败重开与进度恢复的实施及验证。

	- B7 [玩法分析](Docs/UILibrary/玩法分析.html)：离线查看核心规则、操作、失败补救、模式成长及原作证据边界。

	- B8 [小狗面包房换皮风格](Docs/UILibrary/themes/PuppyBakery/STYLE.md)：记录已确认头像的绘本造型、配色、尺寸与后续素材一致性要求。

	- B9 [Prefab 与原作层级资料](Docs/UILibrary/PrefabCollection/index.html)：集中查看当前工程 Prefab 快照、原作序列化对象层级、配置及动态 UI 代码证据；完整来源与哈希见同目录 manifest.json。

	- B10 [原作可读代码](D:/0000001apkUnbag/01_逆向工程项目/Hamdoku2/03_code/decompiled_csharp/README.md)：集中查看本轮 Cpp2IL 方法 IL 恢复、dnSpy C# 伪代码、游戏方法索引及恢复诊断；不等同于原始源码或运行验证。

	- B11 [迁移验收文档](Docs/MigrationAcceptance.md)：用户按批次执行的操作、预期结果、验收状态与问题记录；后续每次迁移交付必须追加或更新对应批次。

	- B12 [主线50关迁移 TODO](Docs/Campaign50MigrationTODO.md)：追踪第21—50关、主线阶段奖励、旧存档兼容及开发验证；黄金挑战仅分析依赖。

	- B13 [下一批迁移 TODO](Docs/NextMigrationTODO.md)：主线101—300关与无限模式完整本地玩法的执行计划、证据核查、存档兼容及验证交付；文档确认不等同于功能迁移授权。

- A10 原作代码查阅与迁移依据
	- B1 原作可读代码根目录为 `D:/0000001apkUnbag/01_逆向工程项目/Hamdoku2/03_code/decompiled_csharp`，保持只读。先读该目录 README.md 的恢复边界，再通过 GAME_INDEX.md 定位类型；GameAssemblies 包含四个游戏程序集的 283 份显式 C# 文件，AllAssemblies 用于补查其他程序集调用关系。

	- B2 迁移前先读对应业务类型、调用方及状态机，再以 game-method-index.tsv 的 assembly、type、recovered_token 定位具体方法，检查 status、diagnostic_count、source_line 以及 method-issues.jsonl、source-diagnostics.json。方法可定位或未检测到诊断均不表示逻辑恢复正确。

	- B3 带 NoteDecompilerIssue、无效 IL、异常占位、可疑零值或控制流的代码，应沿索引的 structure、native、isil 路径回查，并结合原作配置、对象层级及实际运行证据。恢复 DLL 的 Token／RVA 不等同于原 metadata Token／ARM64 地址；空结构方法只提供签名，工具替代值不能作为业务数值依据。

	- B4 按业务入口查阅启动、首页、棋盘、教程、结算、进度和设置，具体类型链接见 [代码阅读与业务定位](Docs/UILibrary/PrefabCollection/index.html#code-guide)。这些入口是分析起点，不构成已经确认的运行调用链或迁移清单。

	- B5 明确区分原作静态证据、运行核实、当前工程实现选择和待核实项。迁移行为按当前 ClientMain／System／Controller／Module／Panel 分层实现，配置使用正式 SO；不直接复制恢复 C# 到 Assets，不照搬原作单例或静态业务状态。UI 迁移仍须先核对原层级与资源依赖，并按 A2-B7 确认迁移范围。

- A11 项目展示
	- B1 [项目 README](README.md)：演示视频查看与下载入口。
