# 项目工作记录

记录实际完成的工作、验证结果及尚未完成的功能。按日期追加，每项说明修改内容、主要文件和验证情况。

## 2026-10-27

### 项目约定

- 创建 `AGENTS.md`，明确当前 Unity 项目为 `D:\UnityProject\Hamdoku2`，待复刻原项目为 `D:\0000001apkUnbag\01_逆向工程项目\Hamdoku2`。
- 明确以 `D:\UnityProject\Mahjong_j` 为架构参考。
- 创建本工作记录，并在 `AGENTS.md` 中添加后续更新要求。

### 启动架构

- 搭建 `ClientMain → System → Controller → UI / Module` 的运行结构，并分离运行时与编辑器程序集。
- `ClientMain` 负责创建和注入依赖；`GameContext` 仅保存字段；Controller 负责修改运行状态；System 负责组织流程。
- 实现启动状态、完成和失败通知、重复初始化处理及退出清理。
- 主要文件：`Assets/Src_Runtime/ClientMain.cs`、`Assets/Src_Runtime/GameContext.cs`、`Assets/Src_Runtime/System/Loading/`、`Assets/Src_Runtime/Controllers/LoadingController.cs`。

### Logo 面板

- 导入原作背景、完整 Logo、动画分片及所需音效。
- 实现 `Panel_Logo`、面板 Controller、`Widget_LogoIntro` 与 `LogoConfigSO`；动画配置初值依据原作提取资料。
- 实现分片入场、回弹、音效时序、完整 Logo 静止姿态、呼吸和上下浮动。
- Logo 使用不受 `Time.timeScale` 影响的时间更新，启动流程包含最低展示时长和一次性完成通知。
- 在现有 `Scene_Main` 中配置 `ClientMain`、Canvas、Logo 面板和音频引用，加入安全区域适配及背景按比例铺满。
- 主要资源：`Assets/Res_Runtime/Scene_Main.unity`、`Assets/Res_Runtime/Config/LogoConfig.asset`、`Assets/Res_Runtime/UI/Logo/`。
- 验证：Unity 编译、Play Mode 启动、暂停游戏时间时的 Logo 更新、音效次数、重复初始化、释放及横竖屏截图检查通过。记录位于 `Logs/LogoValidation/`。
- 边界：动画按提取参数与可用代码资料还原，尚未完成与原应用运行录像逐帧对比。

### 首页与本地进度

- 实现 `AssetModule`、`PanelFactory`、`PanelRepository` 和面板枚举，首页以 Addressables 的 `default` 标签加载 Prefab。
- 接入兼容当前 Unity 6000.5.2f1 的 Addressables 2.10.3；参考项目所用 1.22.3 因调用已移除 API 无法在当前版本编译。
- 实现 `HomeSystem`、`HomeController`、`HomeState`、`HomeSystemEvents`、`Panel_Home` 及面板 Controller。
- 创建首页 Prefab，接入原作卡片、图标、Logo、按钮音效以及安全区域和横竖屏适配。
- 实现主线关卡显示、困难关表现，以及排行榜通关 10 关、每日挑战通关 20 关、无限模式通关 100 关的解锁条件；业务配置由 `HomeConfigSO` 维护。
- 实现本地 JSON 进度的写入与读取，包括教程完成状态、主线通关数、语言和每日连胜数；写入先使用临时文件，再替换正式文件。
- 接入中英文文案 SO、字体映射和离线烘焙字体；语言及字体统一由 `L10NModule` 管理。
- 打通 Logo 到首页的切换，场景加入 Input System 的 EventSystem，锁定按钮可点击查看解锁条件。
- 新玩家进入首页时上报教程请求；主线、每日挑战、无限模式、排行榜及设置按钮通过委托上报对应请求。
- 修正英文每日挑战标题与图标重叠的间距。
- 主要资源：`Assets/Res_Runtime/UI/Home/Panel_Home.prefab`、`Assets/Res_Runtime/Config/HomeConfig.asset`、`Assets/Res_Runtime/Localization/`、`Assets/Res_Runtime/Font/`、`Assets/AddressableAssetsData/`。
- 验证：编译、资源标签加载、Logo 到首页切换、真实 EventSystem 射线和点击、10／20／100 关解锁边界、困难关表现、中英文字体、重复打开、存档往返读取、重新加载场景恢复进度和退出清理通过；运行错误为 0。Addressables 内容打包通过。
- 验证在隔离副本中执行，测试用通关进度未复制到当前项目。记录和截图位于 `Logs/HomeValidation/`。

### Addressables 启动报错修复

- 用户反馈 `Invalid path in TextDataProvider`，路径指向 `Library/com.unity.addressables/aa/Windows/settings.json`，并伴随运行数据为空及资源键加载失败。
- 原因：同步首页资源时遗漏 `ProjectSettings/EditorBuildSettings.asset` 中的 `com.unity.addressableassets` 注册项，编辑器未正确关联已有 Addressables 配置。
- 补齐注册项，并核对其 GUID 与 `Assets/AddressableAssetsData/DefaultObject.asset.meta` 一致。
- 修改 `Assets/Src_Editor/EM/HomeSetupEM.cs`，使配置工具重新注册已有 DefaultObject，避免因注册缺失而重复创建配置。
- 验证：不调用首页配置工具，直接打开 `Scene_Main` 进入 Play Mode，首页启动与相关检查通过，运行错误为 0。记录位于 `Logs/HomeValidation/AddressablesFix/`。

### 项目规范文档整理

- 经用户确认，参考 Mahjong_j 的协作与架构约定，重写根目录 `AGENTS.md`；未创建或复制 Docs 文档。
- 建立项目定位、协作、文档格式、分层与 Context、代码、UI 与资源、配置与本地化、验证交付及工作记录共九部分规范。
- 保留现有 `WORK_LOG.md` 的日期记录格式与历史内容，明确后续同步更新要求。
- 结合当前实际实现注明 Scene_Main、Panel_Logo 启动例外、首页 Prefab 管理、中英文本地化入口、版本来源，以及隔离验证同步时的配置检查。
- 验证：核对规范与当前项目入口、Context、包版本和本地化 API；本次仅修改 Markdown，未修改业务代码、场景或资源，未运行 Unity 测试。

### 分辨率与屏幕方向标准

- 核对原项目 `CanvasScaler__level1__145.json` 和 `CanvasScaler__level2__147.json`，首页与游戏场景均为 1080 × 1920、Scale With Screen Size、宽高匹配值 0.5、Reference Pixels Per Unit 100。
- 将此配置确定为本项目 UI 设计基准，并补充至 `AGENTS.md`；实际手机显示继续适配设备尺寸与安全区域，不固定设备渲染分辨率。
- 当前 `Scene_Main` 的 CanvasScaler 已符合基准，无需改动；`ProjectSettings/ProjectSettings.asset` 默认方向改为正向竖屏，桌面和 Android 默认窗口改为 1080 × 1920，Web 默认预览改为 540 × 960，关闭其他方向的自动旋转选项。
- 编辑器 Game 视图有独立尺寸设置，后续以 1080 × 1920 或 9:16 预览验收；本次未操作 Game 视图，也未执行真机方向验证。
- 验证：对照原作配置、当前场景和项目设置进行静态核对；未修改业务脚本，未运行 Unity 编译或 Play Mode。

### 1080 × 1920 预览统一与布局验证

- 检查当前场景与首页 Prefab，Canvas 和首页 Content 均已采用 1080 × 1920；现有 Game 视图布局文件显示预览为 750 × 1650。
- 新增 `Assets/Src_Runtime/Generic/Const/UIResolutionConst.cs`，集中维护 UI 参考尺寸、宽高匹配和像素单位常量；Logo 与首页制作工具改为引用同一尺寸常量，避免后续生成资产时基准不一致。
- 新增 `Assets/Src_Editor/EM/ResolutionSetupEM.cs`，提供 `Hamdoku/Preview/1080 x 1920 Portrait` 菜单，选择或创建固定分辨率预览。当前工程已写入一次性请求，下次 Unity 脚本重载后应用；若正在 Play Mode，则退出后应用。该工具仅改变编辑器预览，不强制设备渲染分辨率。
- 未重建场景或 Prefab，现有标准比例下的布局经过截图核对，无需为分辨率改变重新排列控件。
- 验证：隔离副本使用 Unity 6000.5.2f1 编译并运行，成功选择 1080 × 1920 固定 Game 预览，确认竖屏默认方向、窗口尺寸、Canvas 模式及 0.5 匹配值；Logo 到首页正常。
- 验证 1080 × 1920、1080 × 2400、720 × 1280 及模拟顶部 110／底部 70 像素安全区，首页五个按钮均位于可用区域，截图检查通过，运行错误为 0。记录与图片位于 `Logs/ResolutionValidation/`；未进行真机验证。

### 当前未完成事项

以下事项截至本次记录仍未完成，分辨率标准的确定不改变这些功能的实现状态。

- 新手教学、第一关棋盘与实际游戏规则流程尚未接入。
- 每日挑战、无限模式、排行榜和设置页尚未实现；目前首页负责锁定提示与请求上报，目标功能显示“功能准备中”。
- 每日连胜目前只读取和显示存档值，尚未实现连胜结算。
- 尚未接入真实广告、登录、云存档或排行榜服务。
- 当前验证覆盖编辑器运行与 Addressables 内容打包，尚未完成 Android 安装包及真机触摸、音频和性能验证。


### Hamdoku2 离线 UI 参考库（2026-10-27）

- 按已确认范围创建 `Docs/UILibrary/`，参考麻将项目最新组件库的组织方式，以 Hamdoku 原作素材建立奶油色、棕色与柔和功能色的独立参考库。
- 提供离线 HTML/CSS/JS、统一浏览器配置、色板与字体、间距规范、按钮与开关、首页解锁状态、中英首页标题、棋盘视觉状态、弹窗、动效和资源搜索/详情/分页。
- 复制 708 张 Sprite、10 份字体、11 份配置/文案证据及 4 张当前工程首页验证截图；记录来源资源包、pathID、尺寸、透明边界和 SHA-256。当前工程截图明确标注为复刻工程截图，非原应用截图。
- 核对原作首页/游戏 CanvasScaler 与最新项目约定，参考库记录 1080×1920、9:16 正向竖屏、匹配值 0.5、PPU 100；未修改生产 Canvas 设置。
- 主要文件：`Docs/UILibrary/UI Library.html`、`UI Library.css`、`UI Library.js`、`UI Library.config.js`、`extract.py`、`manifest.json`、`catalog.js`、`README.md`、`Missing.md`、`Verification.md`、`verify.cjs`；在 `AGENTS.md` 增加文档入口。
- 验证：Chrome 后台 file:// 检查通过；718 项资源、11 份证据及 4 张截图哈希一致，708 张 PNG 全部解码；1440/768/375 px 无页面横向溢出；解锁边界、输入/空态/进度、棋盘状态、弹窗与焦点恢复、加载/重试、检索分页和减少动态效果检查通过，无 JavaScript/控制台/资源请求错误。已人工查看桌面概览、首页、棋盘和手机设置预览。
- 验证明细与生成截图：`Docs/UILibrary/verification-results.json`、`Docs/UILibrary/previews/`。初次弹窗截图采在动画开始，已修订脚本等待动画结束并更新最终预览。
- 尚未完成：原应用截图/操作录像对照、Sprite 九宫格逐项核验、其他页面与语言全覆盖、音频触发映射。当前首页模式卡 Item 复用记录为后续整改项，本轮未重构运行资源。
- 本次未修改 Unity 业务、Prefab、场景或生产 SO，未运行 Unity 编译、Play Mode、Addressables 内容打包、Android 构建或真机检查。浏览器棋盘与弹层为明确标注的示例，不代表玩法或目标页面已完成。用户需要补充的资料按优先级列于 `Docs/UILibrary/Missing.md`。


### UI参考库第二版：组件补齐与用户截图对照（2026-10-27）

- 按用户确认的五项顺序补齐按钮规格/字形视觉居中、九宫格与可调间距、标签页/列表、Hamdoku专属组件和游戏截图对照。
- 新增 `Docs/UILibrary/UI Library.components.js`、`UI Library.components.css`，扩展 `UI Library.config.js` 与入口。提供五种按钮皮肤及标准/紧凑/禁用/忙碌状态、动态文案测量、160–360px切片拉伸、4–32px内距、文字层级、键盘标签、0/1/2/6项列表、徽标、骨架及补充输入状态。
- 接收并保存8张用户截图，包含首页、棋盘、设置、商店、每日连胜、个人资料6类画面；两张重复首页以哈希标记duplicateOf。全部原件保留于 `Docs/UILibrary/references/original/`，与4张当前工程旧截图区分来源；未将剪贴板路径作为后续浏览依赖。
- 扩展 `extract.py`，只读解析原资源的708项Sprite元数据，记录原始rect/border/PPU/textureRect/裁切偏移及导出border换算；加入ShopCatalog快照。总计718项资源、12份证据、12张参考图，原PNG/字体字节保持不变。
- 专属组合支持8个标签：首页、截图转录9×9棋盘、七行设置、7个商品、七天连胜、个人资料以及补充的日历/排行榜。个人资料展示18项头像/姿态、19个边框和6件服装；提供12种区域图案目录。日历、排行榜与语言弹层明确标记未有对应截图。
- 修复本地file://下图案mask的CORS问题，改用原PNG字节内嵌；商店礼包图核对为offer_art_starter_pack。截图与浏览器组合可并排查看，重写README、Missing和Verification，移除已不准确的“没有原应用画面”“切片全部未知”等缺口描述。
- 验证：`verify.cjs`与新增 `verify-components.cjs` 后台Chrome检查通过，708张图像解码、资源/证据/截图哈希检查通过；8个组合在1440/768/375px无横向溢出，按钮测量、切片端点、间距、列表键盘/领取、设置、棋盘、商品说明、头像/边框分类选择、日历、排行榜状态及资源用途过滤通过，无JavaScript/控制台/资源请求错误。已查看桌面/手机预览，结果在 `verification-results.json`、`verification-components.json` 与 `previews/`。
- 未完成：逐像素视觉匹配、逐字体截图像素居中验收、全部资源切片拉伸验收、完整动画与输入语义、商品服务及真实奖励。教程、通关、失败/复活、确认弹窗、日历/排行榜/无限、语言列表、商店下半部分与个人资料编辑/服装佩戴等缺图状态详见Missing。
- 本轮未修改Unity业务/Prefab/场景/SO或存档，未运行Unity编译、Play Mode、内容打包、APK构建和真机检查。网页可交互不等于对应Unity页面或服务已实现。

### UI 资料待办记录（2026-10-27）

- 按用户要求新增 `Docs/UILibrary/TODO.md`，将失败/复活、退出/重新开始确认、每日挑战/排行榜/无限模式、语言/资料编辑/服装佩戴、购买各结果五类资料列为未完成待办，留待后续补充。
- 记录已查看 `handoku.mp4` 的范围，明确教程、通关与商店下半部分已有录像依据，但尚未接入参考库；同步修正 `Missing.md` 中对应的过时缺口，在 `AGENTS.md` 添加 TODO 入口。
- 验证：静态核对五类待办均保留未完成状态，文档相对链接有效。仅修改文档，未修改参考库交互或 Unity 文件，未运行浏览器或 Unity 测试。

### Logo 启动页与首页按原作整理（2026-10-27）

- 对照原项目 `LobbyLayoutConfig__resources.assets__1481.json`、level1 的 RectTransform/Image、LogoAnimator 配置和用户提供的首页截图，定点调整现有场景及 Prefab，未重建启动、资源加载或面板管理体系。
- 修正首页把裁切后的 Logo 当作完整 1080 方形构图拉伸的问题，按原作 800 方形节点换算可见尺寸；启动页与首页统一到原作 Logo 锚点和偏移。启动页新增固定设计坐标 Content，沿用首页的安全区域等比适配方式。
- 调整每日挑战图标、标题、日期、开始按钮、排行榜图标、主线按钮与顶部入口的位置和尺寸；恢复三张卡片的 80 像素 Sprite border，并按原 Image 的 1.8／2.2 倍率切片，避免圆角被放大。主线文案改为白色；修正无限模式背景缩窄导致图标落在按钮外的问题。
- 使用原作成品图补齐商店、默认个人资料头像与边框；每日连胜卡接入点击。新增 Shop／Profile／Streak 请求，仍经 Panel 委托与 Controller 处理，目标页尚未实现，明确显示“功能准备中”。设置入口去除原作截图中没有的额外文字标签。
- 首页补齐原 LogoAnimator 的上下浮动参数（高度 2.5、速度 1.2），由 HomeConfigSO 维护；重复打开时恢复初始显示姿态。保留现有关卡进度、锁定提示和本地化。无限模式的窄按钮排布属于对现有复刻布局的修复，尚无对应原应用截图作逐像素确认。
- 主要文件：`Assets/Res_Runtime/Scene_Main.unity`、`Assets/Res_Runtime/UI/Home/Panel_Home.prefab`、三张卡片 `.meta`、新增三个原作图片及 `.meta`、`HomeConfig.asset`、`Panel_Home.cs`、`Panel_Logo.cs`、`Panel_Home_Controller.cs`、`HomeRequestType.cs`、`HomeConfigSO.cs`、`HomeSetupEM.cs`、`LogoSetupEM.cs`。
- 验证：隔离副本使用 Unity 6000.5.2f1，编译和实际 Play Mode 通过；正式 Scene_Main 启动、default 标签资源加载、八个按钮中心射线命中、三个新增请求、重复 Open、Close/Open、中英切换均通过，项目运行错误 0。生成并查看 1080×1920、1080×2400 和模拟安全区域的相机后台渲染截图；后台渲染不等同编辑器 Game 视图或手机验收。
- 首轮验证被 UnityEditor.Search.SearchDatabase 自身索引越界异常中断，已单独保留初次失败记录与编辑器诊断；随后将该编辑器异常与项目运行错误分开记录，后续验证通过。记录在 `Logs/UIRefineValidation/`，编译及运行日志在 `Logs/UIRefine-Compile.log`、`Logs/UIRefine-Play.log`，初次记录在 `Logs/UIRefine-FirstAttempt.txt`。
- 同步后核对场景、Prefab、图片 `.meta` 字节一致、内部 fileID 引用完整、Addressables 注册与 DefaultObject GUID 正确。未同步 Library、测试存档或测试用 HomeConfig；正式存档文件名保持不变。当前工程直接使用已绑定资产，无需先执行制作菜单。本轮未在当前已打开的 Unity 窗口再执行 Play Mode。
- 尚未实现倒计时、每日完成勾选、连胜奖励、商店／个人资料／连胜详情页及真实服务；没有以模拟倒计时或奖励冒充完成。尚未做逐像素复刻、动画录像逐帧对照、Addressables 重新内容打包、APK 构建、安装或真机验证。


### 追加排行榜与葵花籽耗尽截图（2026-10-27）

- 保存用户新提供的两张游戏画面至 `Docs/UILibrary/references/original/ranking-overlay.png` 与 `seed-empty-revive.png`，在 `index.json` 登记来源、尺寸和SHA-256。
- 更新 `TODO.md` 与 `Missing.md`：排行榜前三名/部分条目/个人行及葵花籽耗尽选择界面已有截图；广告结果、棋盘恢复、重新开始确认、其他排名状态仍未覆盖，五类TODO均保留未完成。
- 验证：两张归档文件与输入哈希一致，可解码，TODO包含五个未完成条目，相对链接有效。仅归档与更新文档，未修改参考库组件、catalog/manifest发布快照或Unity文件，未进行运行时验收。

### Panel_Logo 空引用修复与瓜子加载页补齐（2026-10-27）

- 用户反馈 `Panel_Logo.Ctor` 抛出 InvalidOperationException。初次核查正式场景发现新增必填的 `contentRoot` 为 0，而 Content 节点已存在；随后场景被保存为有效的新 Content 绑定，本轮沿用该绑定和当前 Logo 父节点，未覆盖为旧验证副本的节点 ID。错误提示补全 ContentRoot，避免误导为 SafeRoot 或 Widget_LogoIntro 缺失。
- 修正 `HomeSetupEM`：引用丢失时优先复用 LogoMotion 的真实 Content 父节点，并显式标脏组件，避免再次生成平行 Content。`LogoSetupEM` 加入仅在编辑器执行的已知绑定迁移；它修复序列化引用，运行时继续严格校验。为当前已打开场景准备一次性编辑器迁移请求，退出 Play／脚本重载后或下次进入 Play 前应用，无需手工执行制作菜单；如有未保存场景修改则保留脏状态，不自动保存用户其他修改。
- 根据用户新截图，纠正上一轮将启动页对齐首页的处理：启动页单独采用较小的 Logo 与上移构图，补齐格言、署名、底部版权和五颗瓜子。复用原作 `icon_life`、`seed_off`、`seed_pop` 图像，填充后隐藏对应虚线空槽，显示当前瓜子的弹动标记。
- 参考原作 `LoadingScreenConfig__resources.assets__1480.json`、`LoadingScreen__level0__12.json` 和中英文原文，使用五颗、75 像素槽位、14 像素间距、0.34 秒弹动、1.18 弹动倍率及 0.3 秒完成停留。版面按用户截图在 1080×1920 设计坐标中落实，未宣称逐像素匹配或完整格言随机库已实现。
- 加载进度由 Controller 根据 Addressables 实际操作进度、最低展示时长及 Logo 动画完成状态共同推进；全部完成且停留结束后才进入首页。这是当前复刻工程的启动阶段进度，不是下载字节百分比，也不是原应用内部进度算法的运行验证。
- 新增三个独立 L10NSO：Loading_Quote、Loading_Author、Loading_Copyright；启动时仍使用同一 L10NModule，按既有存档语言注册字体，完整资源加载后替换本地化数据集并重新应用语言。更新原有中英静态字体图集以覆盖新增字符，未引入字体 fallback。制作工具修复静态字体再次烘焙时未取得源字体的问题，连续执行两次配置通过。
- 主要文件：`Scene_Main.unity`、`LogoConfig.asset`、`UI/Logo/` 三张图片与 meta、`Font/Chinese SDF.asset`、`Font/English SDF.asset`、三个加载文案 SO、Addressables Home 组；以及 `Panel_Logo.cs`、`Panel_Logo_Controller.cs`、`LoadingController.cs`、`LoadingSystem.cs`、`LoadingState.cs`、`AssetModule.cs`、`L10NModule.cs`、`HomeController.cs`、`LogoConfigSO.cs`、`UINameType.cs`、两份现有制作工具。
- 验证：隔离副本 Unity 6000.5.2f1 编译通过；不调用配置菜单直接加载保存的 Scene_Main，Play Mode 启动、ContentRoot 绑定、本地化由启动到完整资源的切换、进度单调及完成条件、五颗瓜子 0／60／100% 显示、标记隐藏、完成停留和首页关闭重开均通过，项目运行错误 0。检查中英字体与 1080×1920／1080×2400 后台相机渲染截图；静态显示状态检查不代表原应用加载过程的实时对比。
- Unity Search 的编辑器索引异常单独记录，未作为项目错误忽略。制作工具首轮批处理未打开 Scene_Main、重复静态字体烘焙源引用问题均已修复并复跑。记录、脚本、用户原图和最终截图位于 `Logs/LoadingScreenValidation/`；另有 `Logs/LogoBindingFix-Play.log`、`Logs/LoadingScreen-Setup.log`、`Logs/LoadingScreen-Play.log`。
- 同步后核对场景、LogoConfig、字体图集、三份文案与 meta、Addressables 组字节一致，全部 Panel_Logo 必填引用非 0，五组填充／空槽引用及场景内部 fileID 有效，Addressables DefaultObject 注册正确。正式 HomeConfig 与存档文件名保留，未同步验证存档或测试用 HomeConfig。尚未在用户当前 Unity 窗口执行 Play，未重新内容打包、构建 APK、安装或真机验证。

### UI 参考库新增玩法分析（2026-10-27）

- 按用户要求参考 Mahjong_j/Docs/RichMahjongMatch/01_玩法文档.html 的阅读结构，新增 Docs/UILibrary/玩法分析.html，采用 Hamdoku 奶油色与棕色风格；内容覆盖核心循环、三条规则、单击/双击/滑动教程、生命与道具、失败补救、主线/每日/连胜/排行榜/无限模式及表现和结算。
- 加入可操作4×4约束示意，使用原作 Campaign 样例0区域与答案；明确区分示意规则、原作文案/配置/截图、原作待验证行为和当前工程实现。未将静态配置组合推断为已验证运行规则，未宣称广告、购买或排行榜服务已经完成。
- 只读复制原资料 Campaign_样例.md 与 Tutorial_4x4__sharedassets1.assets__11.json 到本库 evidence，两份副本与原件 SHA-256 一致；本页独立引用，不改既有 catalog/manifest 发布快照。复用棋盘、耗尽提示、每日连胜和排行榜四张已归档截图。
- 在 UI Library.html 侧栏添加玩法分析入口，更新 README.md 与 AGENTS.md 文档入口；保留已有项目修改。预览位于 Docs/UILibrary/previews/gameplay-analysis-1440.png 和 gameplay-analysis-375.png。
- 验证环境与结果：Windows 后台 Chrome / Playwright，以 file:// 打开；31处本地文件链接有效，4张原图加载解码正常，1440/768/375px无页面横向溢出，16格规则示意的相邻冲突/暂不冲突/显示布局/重置及参考库入口跳转通过，页面JavaScript、控制台及请求错误为0。手机宽表采用内部横向滚动，避免配置字段挤压正文。
- 未完成或未验证：原作更深层落子判定、精确输入时序、道具效果/库存及广告结果、各模式完整运行流程、结算公式与购买服务仍需核对；既有资料TODO状态不变。本次只修改文档及网页，不修改Unity脚本/资源/场景/SO/存档，未执行Unity编译、Play Mode、内容打包、APK构建、安装或真机检查。
### 每日连胜页面与本地业务迁移（2026-10-27）

- 按用户确认范围新增 `StreakSystem`、`StreakController`、`StreakState`、`StreakSaveModel`、`Panel_Streak_Controller`、`Panel_Streak`、`Widget_StreakDay`、`Widget_StreakPlant`、`StreakConfigSO` 和 `StreakSetupEM`。沿用 ClientMain 注入、Controller 修改运行数据、Panel/Widget 委托上报，以及 PanelFactory/Repository 的 Ctor、Init、TearDown 生命周期；未新增单例、接口或资源加载体系。
- 只读核对原作 GameProgress 的 GetDailyStreak、NotifyWin、OnFirstWinOfDay、StreakSlot、StreakStage、StreakCut、AccrueStreakReward 及原 GameConfig、StreakPageConfig。按当天第一次有效胜利记录日期，同日重复胜利不累计；今天尚未获胜时保留昨日连续天数，漏日后当前连胜归零、最佳记录保留。当前有效来源接入主线非教程胜利，其他模式后续在其权威结算中复用 RecordWin，未把登录或打开页面当作胜利。
- 原作七天周期第 3/5/7 天的基础奖励为提示 1、排除辅助 1、提示与排除辅助各 1；配置由 StreakConfig.asset 维护，并按日期与周期计算待领数量。实际领取后保存额外库存；棋盘本关原有道具用尽时消耗该库存，重复点击领取、重复结算及礼盒预览不重复发奖。胜利记录与完成棋盘由同一次进度保存写入；待展示页面标记、未领奖励与已领库存保留在存档。
- 接通首页连胜入口；当天首胜在结算继续后进入连胜页，继续返回后续主线，返回按钮回到首页。新增正式 Panel_Streak.prefab、Widget_StreakDay.prefab、19 份中英文 L10NSO 及 Addressables/default 注册。采用 1080×1920、安全区域和背景比例铺满，包含当前/最佳连胜、星期与完成勾选、礼盒预览、幼苗/花苞/花瓣/盛开和后续花丛、蝴蝶表现。原作图片直接接入，光晕和胶囊底图为编辑器烘焙的正式图像；修复植物子节点锚点、礼盒尺寸及继续按钮皮肤。成长动画和后期花丛排布为依据静态资料落实的复刻实现，未宣称原作录像逐帧或逐像素一致。
- 按原配置实现断签恢复资格和放弃入口。广告 SDK 尚未接入，恢复按钮明确显示不可用，不恢复日期、不模拟广告成功或追加奖励；广告加倍、云端时间与云存档也未接入。日期采用设备本地日历；日期回退不补记旧日，不宣称具备服务端校时或防作弊能力。
- 扩展现有 PlayerSaveModel/SaveModule 与 HomeController 初始化。共享存档版本保持其他并行功能已升级的 4，制作工具不会降级版本；旧版缺少日期证据的 dailyStreak 仅保留为最佳记录，不伪造历史胜利或补发奖励。验证发现 JsonUtility 将可空嵌套对象写为全零数据，现有存档读写统一采用 DataContractJsonSerializer，相关模型显式声明数据成员，兼容旧字段缺省并保留真正的 null；维持版本/合法性检查、临时文件与替换写入，读取错误不静默重置。
- 主要接入文件：ClientMain.cs、GameContext.cs、HomeController.cs、GameController.cs、Panel_Game_Controller.cs、PlayerSaveModel.cs、SaveModule.cs、AssetModule.cs、UINameType.cs、UIPanelType.cs；新增连胜脚本分别位于既有 System、Controllers、Entities_Sub、UI、Module/Assets/TM 和 Src_Editor/EM 目录，资源位于 Assets/Res_Runtime/UI/Streak、Config/StreakConfig.asset 与 Localization/Streak_*.asset。保留其他对话的游戏与排行榜改动，未覆盖其场景、主线 Prefab、配置或正式玩家进度。
- 验证：隔离副本 Unity 6000.5.2f1 编译通过；以当前工程完整快照直接运行已保存 Scene_Main，不调用制作菜单，Play Mode 92 项检查通过、项目运行错误 0。覆盖首页实际按钮、主线真实解题胜利和结算继续跳转、当前/最佳记录、同日防重、3/5/7 日奖励、重复领取、库存持久化与真实棋盘消耗、关闭重开、跨年/闰日/漏日/回退日期、断签未接入状态与放弃、版本 1/2 迁移和损坏存档错误。中英字体及界面切换、1080×1920/1080×2400/1080×1620 和模拟顶部 110/底部 70 像素安全区域、按钮中心射线命中均通过；另查看第 4/30/60 天成长截图。测试日期、连续通关和道具数量只存在于隔离验证存档，不写回正式进度。
- 最初隔离快照包含另一功能尚未完成的文件，曾出现编译依赖不完整；序列化初次验证及中途共享资源 GUID 同步也曾失败，均保留日志，修正后以当前完整快照复跑通过。UnityEditor.Search 的 SearchDatabase 越界异常单独记录为编辑器诊断，未混入项目运行错误。最终记录与截图在 Logs/StreakValidation，验证辅助代码和同步/GUID 核对记录在 Logs/StreakMigration。
- 同步时按资源路径映射并保留当前工程已有 GUID，逐项同步新资源、Prefab、SO、本地化、字体及 Streak 组/Schema；未复制 Library、验证专用脚本到正式程序集、验证 HomeConfig 或验证存档。核对本轮连胜脚本及共享接入点与最终验证快照一致，已同步资源与快照一致；验证结束后另一任务向 RankingConfigSO 增加 assetVersion 字段，该项不属于本轮连胜验证范围，正式存档文件名仍为 hamdoku-progress.json，EditorBuildSettings 的 Addressables DefaultObject 注册有效。用户当前打开的 Unity 窗口未由本轮操作进入 Play Mode；相机后台渲染不替代 Game 视图操作或手机触摸验收。尚未执行本轮 Addressables 内容打包、应用/APK 构建、安装或真机验证。

### 首批玩法迁移：教程、五关主线与局内恢复（2026-10-27）

- 按用户要求先创建 `Docs/GameplayMigrationTODO.md`，在 `AGENTS.md` 添加入口，再按任务项实施。本批完成独立 4×4 教程与主线前 5 关；第 6 关明确提示尚待迁移，没有循环地图伪造后续进度。
- 使用原作 `Tutorial_4x4` 的区域和答案；首个引导落点依据 native `PlaceGuided(3,1)`。实现双击放置、单击排除／取消排除、滑动连续标记、教程高亮和自由完成。原作完整确认步骤、手指及动画时序仍待补齐。
- 根据 1–5 关 grade 1 上限、native 尺寸表候选及 Campaign 尺寸交叉核对，首批尺寸为 4、5、6、6、8；导入 146 张对应 DDA 地图为独立 PuzzleSO（4×4 为 9 张、5×5 为 6 张、6×6 为 86 张、8×8 为 45 张）。按尺寸池选择并避开上一张 ID，保存具体地图 ID，返回／重载不重新抽图。此实现没有宣称还原完整动态选图、地图变换、游标、技能升降或原应用运行结果。
- 实现 GameState、PuzzleModule、GameSystem 和 GameController；规则检查覆盖每行、每列、每区域一只及八方向不相邻。依据原作 GameState.IsCorrectCell 调用关系，带答案的落点同时校验答案，错误扣瓜子且不放入仓鼠。
- 新增正式 Panel_Game、Panel_GameDialog 与 Widget_BoardCell Prefab，沿用原有 Canvas、PanelFactory、PanelRepository 和 Addressables default 加载；没有新建平行启动或 UI 管理体系。导入 29 张实际使用的原作图片与 4 段音效，序列化绑定实际落在资源中。
- 接入基础分数、仓鼠计数、瓜子、计时、通关／完美标题、下一关、返回首页、取消／确认重开、生命耗尽及同棋盘重新开始。提示揭示实际答案；排除辅助标记已放仓鼠产生的冲突格。广告复活按钮明确显示服务未接入，不发放模拟奖励。
- 新增 36 份独立中英文 L10NSO，继续使用 L10NModule 的统一文字与字体入口；扩展现有静态字体图集，未加字体 fallback。全局语言切换补齐已打开游戏页与游戏弹层的刷新。
- 存档扩展具体棋盘、标记、生命、错误、道具、分数、计时、教程步骤及状态；恢复时校验地图 ID、关号、答案、枚举和完成状态。完成结果防止重复加关；返回首页后的已完成局清空，局内及失败局保留。暂停／退出沿用 Controller 保存和临时文件替换写入。
- 检查期间同工程同步加入排行榜与连胜代码，保留它们的 Context、入口、通关记录和奖励道具接入，没有用旧副本覆盖共享脚本。联合检查暴露 Unity JSON 对缺失引用字段的兼容问题；保留合并后的 DataContract JSON 与存档版本 4 约定，明确序列化错误，实际验证旧版 1 数据读取、当前存档往返及 activePuzzle 为 null 的恢复，不静默重置用户进度。
- 为满足同步后的启动依赖，使用现有 StreakSetupEM 在隔离副本制作正式连胜资产并补齐当前工程缺少的资源。最终核对采用连胜任务更新后的七天奖励配置、Single Sprite 导入及 PetalRing 绑定；补节点时保留原有层级，未销毁重建首页或游戏页。连胜全量表现及排行榜服务仍按其各自任务验证，本条不将其整体记为本批完成。
- 主要文件：`Assets/Src_Runtime/Controllers/GameController.cs`、`Controllers/Panel/Game/`、`System/Game/`、`Module/Puzzle/PuzzleModule.cs`、`Module/Assets/TM/GameConfigSO.cs`、`PuzzleSO.cs`、`UI/Panel/Game/`、`UI/Widget/Widget_Board.cs`、`Widget_BoardCell.cs`、`Entities_Sub/Save/PuzzleSaveModel.cs`；并接入已有 ClientMain、GameContext、HomeController、AssetModule、SaveModule、PlayerSaveModel 与枚举。制作入口为 `Assets/Src_Editor/EM/GameSetupEM.cs`；正式资源位于 `Assets/Res_Runtime/UI/Game/`、`Audio/Game/`、`Config/Puzzles/`、`Config/GameConfig.asset` 和 `Localization/Game_*.asset`。
- 验证：Unity 6000.5.2f1／Addressables 2.10.3 隔离副本，直接使用正式 Scene_Main 与资产快照，无需先执行制作菜单；编译和实际 Play Mode 通过，项目运行错误 0。检查唯一 EventSystem、真实射线命中与 PointerEventData 分发、教程三种手势、146 张地图答案、错误扣瓜子、耗尽输入阻断、广告无假奖励、同图重开、提示与排除、取消重开、弹层暂停计时、退出存档、场景实际重载恢复、重复 Open／结算、五关推进与批次边界、面板释放。另核对全局语言入口对游戏页和已打开结算弹层的即时刷新。
- 画面：已生成并查看中文／英文、1080×1920、1080×2400 及注入安全区的后台相机渲染截图；文本字体覆盖与实际绑定通过。它们不是用户当前 Game 视图的人工点击、手机触摸或原应用逐像素验收；音效已接入但没有实际扬声器试听。
- 内容打包：最终 Windows Addressables 内容打包通过（约 20.37 秒），不等同 Android 应用构建。未进行 APK 构建、安装或真机验证，也未验证 DataContract 序列化的 IL2CPP 应用构建表现。
- 同步：交付核对 453 个文件与验证副本字节一致；当前编辑器新生成的 GUID 得到保留，187 份玩法资产的外部 GUID 静态核对无缺失。正式场景 SHA-256 保持 `8be20026277e2c4b8c0b1853bd4c72263c2809e3bbcc756033cf7a800ce42056`；Gameplay／Streak 组与 DefaultObject 注册有效，正式文件名仍为 `hamdoku-progress.json`。没有同步 Library、验证存档、测试 HomeConfig 或测试编辑器脚本。
- 初次失败与修复：自动化驱动的同帧新 UI 射线检查及局部变量编译错误已修正；记录保留。共享工程新 GUID、连胜配置结构／花瓣引用／图集导入与旧存档兼容问题均经后续检查解决。Unity Search 索引越界作为编辑器自身异常单独保留，未混作项目错误。最终结果和截图位于 `Logs/GameplayMigrationValidation/`，历次运行日志位于其中 `Runs/`；来源及同步明细位于 `Logs/GameplayMigration/`。
- 后续：第 6 关后的动态难度、固定关及完整选图；原作完整教程、开局揭示、逻辑提示解释、计分细节／连击、成绩统计、完整结算与奖励表现；设置页、其他模式及真实服务；APK／真机验收。均保留在 TODO，未把基础可玩流程写成完整原作复刻完成。


## 2026-10-27 小狗面包房换皮素材归档

- 用户确认奶油白＋雾灰蓝、垂耳小狗轮廓与绘本风格，以本次图片作为后续同套素材的视觉参考。
- 新建 `Docs/UILibrary/themes/PuppyBakery/assets/Sprite/`，保存已确认的 `hamster_idle_04__sharedassets2.assets__6.png`；沿用原文件名对应原素材，新增 `manifest.json` 记录替换关系、来源和确认状态。当前仅有一张素材，不表示整套换皮完成。
- 验证：Python/Pillow 核对 PNG 为 509×439、RGBA，归档图片与用户确认文件 SHA-256 一致。静态检查 UI Library HTML/JS：目前仅支持素材浏览、筛选和页面预览，没有上传替换、主题切换或 Unity 资源写入功能。
- 边界：未覆盖原始资料，未修改参考库页面引用、Unity 代码、Prefab、SO 或运行素材；本次未执行 Unity 编译、Play Mode、内容打包、应用构建及真机验证。


## 2026-10-27 小狗面包房风格文字定稿

- 按用户要求新增 `Docs/UILibrary/themes/PuppyBakery/STYLE.md`，以已确认图片锁定垂耳小狗、奶油白与雾灰蓝配色、绘本笔触及头像交付规格，并区分已确认头像与待设计的身体、服装和 UI。
- 明确保留当前头像 509×439 像素与居中呈现，但不再要求沿用原仓鼠轮廓；其他素材按各自原图尺寸制作。已在 `AGENTS.md` 增加入口。
- 验证：核对风格说明中的基准图、manifest 链接及实际 PNG 尺寸、RGBA 和透明边界；文档采用 A/B 编号与 Tab 缩进。仅文档修改，未执行 Unity 编译、Play Mode 或打包验证。

## 2026-10-27 排行榜本地表现与服务接口

- 按用户确认的范围实施：新增正式 `Panel_Ranking`、`Panel_RankingDialog`、`Widget_RankingEntry` Prefab，复用原作领奖台、奖牌、礼盒、瓜子、头像、装扮、标题横幅和背景；包含前三名、可滚动榜单、本人排名、赛季倒计时、规则与奖励说明、返回、重试和“去收集吧！”主线入口。采用现有 Canvas、PanelFactory／Repository、Addressables default 标签与 Open／Close 生命周期，没有新增并行 UI 管理或 C# interface。
- 新增 `RankingSystem`／`RankingState`、`RankingController`、两份 Panel Controller、`RankingModule`、请求／结果／条目／存档 Model 及状态枚举。由 ClientMain 装配和组织更新，Controller 修改状态，Panel／Widget 只显示和上报交互；关闭、释放与请求代次／赛季检查处理重复打开和过期回调。
- `RankingModule.FetchBoard`、`SubmitClear`、`ClaimReward` 为后续服务接入边界，目前明确返回 ServiceUnavailable。正式页面显示服务准备中、未知赛季与排名；没有生成在线竞争数据、模拟上报成功或发放赛季奖励。服务快照携带 UTC Unix 秒与赛季 ID；收到后以单调时钟计算倒计时，期满及切回前台重新请求，不在本地结算排名。
- 主线实际通关接入持久待上报队列：记录客户端标识、递增事件 ID、关卡、棋盘、剩余瓜子槽、游戏分数和完成时间；确认赛季未知时记录为 0。没有把剩余槽位或棋盘分数擅自转换成排行榜瓜子。重复结算不重复入队，失败保留记录，只有对应事件得到真实成功确认后才移除。领取请求保存赛季与幂等 token，未确认成功不改变领取结果。
- 存档主版本升至 4，支持旧版 1／2／3 进入迁移流程，补充排行榜子版本与合法性检查；旧通关次数不补造历史上报事件。为现有存档类型与新队列数据声明可选 DataContract 成员，兼容缺失的新字段并保留 null 棋盘；新版本损坏的队列明确报错。StreakSetupEM 使用 Math.Max 保留更高的存档版本。正式文件名 `hamdoku-progress.json` 与排行榜通关 10 关解锁条件保留。
- 新增 `RankingConfigSO` 正式配置、23 份中英文 L10NSO 和静态字体所需字符；预览数据保存于 `Assets/Res_Editor/Ranking/RankingPreview.asset`，没有 default 标签或正式场景引用。Play 后回到首页，可通过 `Hamdoku/Ranking Preview/` 切换 Normal、Loading、Empty、Failed、Not Participating、Ended、Reward Available、Claimed；预览标识始终显示，预览领取不发奖，也不写入正式排名进度。
- 主要接入文件：`ClientMain.cs`、`GameContext.cs`、`HomeController.cs`、`GameController.cs`、`SaveModule.cs`、存档 Model、`AssetModule.cs`、PanelType／UINameType、`RankingSetupEM.cs`；资源为 `UI/Ranking/`、`Config/RankingConfig.asset`、`Res_Editor/Ranking/`、Ranking 本地化、现有中英字体、Addressables Ranking 组及其 schema。保存了实际 Prefab 与序列化绑定，正常运行无需先执行制作菜单。
- 验证环境：Windows、Unity 6000.5.2f1、Addressables 2.10.3，独立工程 `D:/UnityProject/HamdokuRankingValidation-20261027`。编译通过；最终把主工程的正式文件重新覆盖验证副本，未调用制作菜单直接打开 Scene_Main，以独立旧版 v2 存档启动通过。正式离线入口、原解锁门槛、重试、重复打开／关闭、规则与奖励弹窗、八种预览、倒计时归零、前台恢复、真实关卡通关后的单次入队与落盘、重复提交防护、失败保留、无效队列拒绝、返回首页保存 null 棋盘、连胜自动弹出后返回以及幂等释放通过，最终项目运行错误 0。
- 查看并核对中英文与 1080×1920、1080×2400、1080×1620、1920×1080 后台相机渲染截图；修正列表行宽、名次数字、按钮形状及礼盒位置。横屏仅检查窗口变化容错，后台渲染不等于真机触摸。StandaloneWindows64 Addressables 内容打包通过；未构建 Android APK、安装或进行真机验收，也未验证真实网络和服务端结算。
- 同步仅合并排行榜新组与资源，核对 DefaultObject 注册、正式存档名／版本／解锁条件、预览不入正式标签，以及当前运行脚本、排行榜资源、本地化、字体和 Addressables 与最终验证快照一致。记录、制作与验证脚本、截图、打包结果和同步核对保存在 `Logs/RankingMigration/`，最终结果见 `Verified/result.txt`；Unity Search 编辑器索引异常单独记录于 `Verified/editor-diagnostics.txt`。原作具体计分、赛季周期、同分规则、奖励内容及后端协议仍待确认，当前不宣称已完成真实在线排行榜或领奖。


## 2026-10-27 Sudoku Pup 启动页整体视觉候选

- 按用户确认将游戏名 Sudoku Pup／《数独小汪》补充到 `Docs/UILibrary/themes/PuppyBakery/STYLE.md`，创建 `assets/UI/Logo/` 分页素材目录。
- 基于确认的小狗图片与用户提供的布局参考，通过内置 image_gen 生成启动页整体候选，保存 `previews/Panel_Logo_overall_v01_1080x1920.png` 和生成说明 JSON。候选包含蓝白遮阳篷、烘焙纹样、Sudoku Pup 招牌、候选文案及五枚饼干加载标记，均等待用户确认。
- 验证：Pillow 核对最终整体图为1080×1920像素；风格文档名称已更新。尚未切图或制作正式Logo分层素材，未替换运行资源；本次未执行Unity编译、Play Mode、打包或真机验证。

## 游戏页改为原始层级迁移（2026-10-27）

- 按用户要求停止以手工基础结构重建游戏页。只读核对原 APK：游戏页保存在 `level2`／导出 `Game.unity` 的 Canvas 中，并非独立游戏页 Prefab；棋格是 `sharedassets2.assets` 中的 `Cell.prefab`。提取 Canvas 下除原 ResultPopup 外的 35 个节点，以及 Cell 的 5 个节点，保留原 Header、StatusRow、HamsterCounter、LifeCounter、RuleBanner、BoardArea／BoardFrame／CellsGrid、BottomBar／ItemButton_A／ItemButton_B／Badge 与 StrokeA／StrokeB 层级及源 pathID。
- 正式资产仍为 `Assets/Res_Runtime/UI/Game/Panel_Game.prefab`、`Widget_BoardCell.prefab`，保留原有两份 .meta GUID 和 Addressables 登记。沿用当前项目的 Hamdoku 命名空间、Panel／Widget 与 Controller 委托、PanelFactory／Repository 生命周期和 L10N 字体入口；清除原项目业务脚本、Canvas／Raycaster、静态按钮回调等依赖，不引入第二套启动或 UI 管理。原 ResultPopup 不接入，继续由已实现的 Panel_GameDialog 管理弹窗。
- 原作通过代码生成的分数、规则卡片、辅助图案等在编辑器中实化；新增安全区设计缩放容器与教程／错误提示绑定。补入原 HudBar、RoundedFill、仓鼠计数图及 help 图片，保留 PNG 字节。调整顶部双行信息和商店／设置位置、规则整体框与内卡片、棋盘白框及间距、道具原底板与红色角标、白色叉号；取消重复按钮底板、道具下方额外文字及底部用时显示。4×4 教程与主线棋盘规模保留各自玩法数据；重开按钮使用原 TipButton 节点移至右下角。
- 主要脚本：`Assets/Src_Editor/EM/GameOriginalUIEM.cs` 负责原资产适配，`GameSetupEM.cs` 删除手工 CreateGame／CreateCell 基础生成路径；缺失原资产时明确报错。`Panel_Game.cs`、对应 Controller 与 `GameController.cs` 增加顶部入口委托／请求上报，商店和设置尚未迁移，点击明确显示准备状态；`Widget_BoardCell.cs` 绑定原两根白色笔画。更新 Game_Level／Game_Score 两份本地化 SO 的双行排版。
- 验证环境：隔离工程 `D:/UnityProject/HamdokuGameplayMigrationValidation-20261027`，Unity 6000.5.2f1、Addressables 2.10.3。最终 Unity 编译与 Play Mode 通过；覆盖正式场景启动、按钮射线／回调、教程双击／单击／滑动、前五关、提示道具、失败／重开、返回与实际场景重载恢复、重复开关及释放，最终项目错误 0。人工查看后台相机渲染的教程、8×8 主线、中英文及 1080×2400 安全区画面，并修复英文规则溢出。该渲染／事件模拟不等同编辑器手工点击或手机触摸验收。
- Windows Addressables 内容打包通过（16.74 秒）。未进行 Android 应用构建、安装或真机验证。正式场景、存档文件名及用户进度未替换，未同步 Library、测试 HomeConfig 或验证存档。
- 来源、提取脚本、pathID／依赖记录、实际验证与预览保存在 `Logs/GameUIRefinement/`。原包序列化版本 23 导致 AssetRipper 直接解析失败；只对工作副本转换版本标记以导出场景／Prefab，并以原始 UnityPy 数据和已有 Configs 恢复组件字段，未改动原取证目录。初次导入的 GameObject 版本、移除 Canvas 后的零缩放、按钮 Pivot、圆角和英文换行问题均已修复并重新验证；历次失败日志保留。Unity Search 索引越界单独记录为编辑器异常。
- 最终同步核对：18 份本次脚本／Prefab／文案／新增图片与 meta 与已验证副本字节一致；两份 Prefab 及两份文案 SO 的外部 GUID 无缺失，DefaultObject 注册指向正式 GUID，Scene_Main 哈希保持不变。详见 `Logs/GameUIRefinement/sync-audit.json`。

## UI 迁移来源规范（2026-10-27）

- 经用户确认，在 `AGENTS.md` 的 A6 追加 B12–B14：迁移 UI 必须先查找原项目 Prefab／场景层级及依赖，以原结构为基础适配当前项目；找不到来源或需要自行补建结构时，说明范围并经用户确认后实施。
- 验证：核对条目内容与已确认方案一致，编号连续、Tab 缩进及空行符合文档规范。本次仅修改 `AGENTS.md` 与本工作记录，未修改脚本或 UI 资源，无需 Unity 编译及运行验证。


## 2026-10-27 正式 UI 图片按命名规范整理

- 按 `Assets/0cyh/UI图片资源命名规范.md`，完成 `Assets/Res_Runtime/UI/` 下 96 张 PNG 的原目录重命名，使用 `btn`／`Img`／`bg` 前缀、实际功能归属与 PascalCase 用途；跨功能使用的图片标为 Common。核对实际图片与 Prefab／场景／SO 引用，棋盘辅助图案改用 Paw／Heart／Flower 等内容名称，礼盒按颜色、奖牌按金银铜、植物按生长状态、蝴蝶按颜色与序列帧命名，去除原 atlas 编号与无意义数字。
- 图片及 `.meta` 成对改名，96 份图片内容、96 份 `.meta` 字节及 GUID 均保持不变；保留已有 Sprite 子资源名称、internalID、裁切和导入配置。目录、Prefab 节点与脚本业务字段未改名，没有移动目录、合并图片或调整 Addressables 登记。
- 同步 `Assets/Src_Editor/EM/` 下 `LogoSetupEM.cs`、`HomeSetupEM.cs`、`GameSetupEM.cs`、`GameOriginalUIEM.cs`、`RankingSetupEM.cs`、`StreakSetupEM.cs` 中的直接路径、Sprite 名称、数组、跨页资源分派、Pill 调用、纹理生成路径和文件名匹配的边框导入条件。棋盘前八种辅助图案的配置顺序保持不变。
- 验证环境：当前 Windows 主工程，Python 3／Pillow 静态检查。96 张 PNG 数量不变，新名符合格式、目标无冲突、旧路径及旧 `.meta` 均已移除，图片与元数据 SHA-256 全部与改名前一致。293 份现有 Prefab／场景／SO 序列化文件字节不变；102 处字面量 Sprite／GameSprite 加载、图片直接路径及资源名数组等均核对存在，旧资源加载名已清除。新旧路径与 GUID 对照为 `Logs/UIRename/mapping.csv`／`mapping.json`，验证结果为 `Logs/UIRename/verification.json`。
- 验证边界：本次未执行 Unity 编译、重新导入后的显示检查、Play Mode、Addressables 内容打包、应用构建、安装或真机验证；静态引用及字节核对不替代这些运行验证。保留任务开始前已有的工程修改，未修改原项目取证目录。


## 2026-10-27 Sudoku Pup 启动页首批素材拆分

- 用户确认 1080×1920 整体方案并授权切图。核对当前正式目录后发现名称已更新为 `Img_Common_Logo.png`、`Img_Logo_*`、`bg_Common_Menu.png`，因此按当前名称而非此前旧名称制作，未改动或覆盖正式运行资源。
- 新增 `Docs/UILibrary/themes/PuppyBakery/assets/UI/Logo/` 下 13 张 PNG 和页面 `manifest.json`，逐项保持正式 PNG 像素尺寸；加载弹出图保留 128×76 画布和现有 seed_pop_0／1／2 子图矩形。未复制或生成替换用 Unity .meta，保留正式 GUID 与子图 ID 的要求记入位置参考。
- 完整 Logo、标题、角色可见部分、睁眼、爪子、加载饼干和蓝色爪印从确认稿提取与透明分离；使用内置 image_gen 补全无文案背景、空招牌和闭眼叠加层。另生成过无眼头部探索底图，最终角色层仍采用确认稿像素并局部去除眼睛，探索底图仅留在工作预览目录。
- 主要工作与检查文件：`previews/Logo_Slicing/slice_logo.py`、`reassemble_review.py`、`validation.json`、`contact_sheet_v01.png`、`page_static_reassembled_1080x1920.png`、`page_layers_reassembled_1080x1920.png`、`layout_reference.json`。同步更新主题根 manifest 和 STYLE.md。
- 验证环境与结果：本机 Python/Pillow、NumPy、SciPy；13 张文件名与正式目录逐一对应、像素尺寸一致、RGBA 合法，背景全不透明，其余图层具有真实透明区。人工查看棋盘底色的素材总览和1080×1920离线分层拼合图，修复爪子切边残留、饼干底色和标题边缘碎片。素材生成与切图完成，未宣称 Unity 接入完成。
- 接入边界：确认稿为更高的两行招牌，旧 LogoConfig 的部件大小／位置不能直接复现；背景含顶部遮阳篷，需要检查 EnvelopeParent 的中心裁切。眼睛、前景爪子、盖章位置、动画到完整 Logo 的切换仍待 Unity 接入验证。背景不烘焙引言与署名，文案仍需通过现有 L10NSO/TMP 更新。本次没有修改运行脚本、Scene、Prefab、SO、包配置或正式图片；未执行 Unity 编译、Play Mode、Addressables 打包、应用构建、安装及真机验证。

## 2026-10-27 批量提交

- 按用户要求将现有改动分为三批本地提交：玩法／连胜／排行榜及正式 UI 资源、UI 参考库与 Sudoku Pup 主题素材、项目规范与工作记录。相关脚本、Prefab、SO、本地化、字体、Addressables 与图片重命名一并保存。
- 验证：核对 Git 暂存范围、提交结果及最终工作区状态。本次没有修改业务实现或正式资源，没有重新执行 Unity 编译、Play Mode、内容打包、应用构建、安装或真机验证；既有验证结果与限制见对应历史记录。未推送远程。

## 2026-10-27 首页向日葵动画接入

- 只读核对原项目 `level1`／导出 `Main.unity` 的 `StreakCard/Icon` 层级（位置 -115,0、尺寸 105×142），以及 `LobbyLayoutConfig__resources.assets__1481.json`、`LobbyLayout.Apply`、`UiSway`、`UiSparkles` 和 `UiFactory.SparkleSprite` 的 ARM64。原作通过代码添加摆动与 3 枚闪光，不是序列帧；保留当前正式 Prefab 的向日葵图片、位置与层级，仅增加对应闪光子节点。
- `Panel_Home` 增加花茎支点补偿、随机初相、主波与轻微谐波摆动，并通过实时协程完成环形区域随机位置／尺寸／角度的闪光淡入、停留、淡出与间歇。首次／重复打开重置动画，关闭清理协程；装饰图片关闭 Raycast Target，不改变连胜卡片入口。由现有 Panel Controller 传入 HomeConfig，没有增加运行管理或业务状态体系。
- 原配置摆动幅度 3°、支点高度比例 0.04，包含 5.3 秒字段；原 `LobbyLayout.Apply` 未将该周期字段赋给 UiSway，故采用组件构造默认 5.5 秒。谐波幅度 0.22、频率倍数 2.17、相位 1.3；闪光数量 3、尺寸范围 34–60、白色 alpha 0.95，其他范围取自原组件默认值／原代码。以上为静态资料与反汇编还原，不等同原 APK 运行对照验收。
- 主要文件：`Assets/Src_Runtime/UI/Panel/Home/Panel_Home.cs`、`Controllers/Panel/Home/Panel_Home_Controller.cs`、`Module/Assets/TM/HomeConfigSO.cs`、`Assets/Res_Runtime/Config/HomeConfig.asset`、`Assets/Res_Runtime/UI/Home/Panel_Home.prefab`、新增 `Img_Home_SunflowerSparkle.png` 及 meta、`Assets/Src_Editor/EM/HomeSetupEM.cs`。按原 UiFactory 的 128×128 程序化星芒算法离线实化 PNG，配置全部存入正式 SO；Prefab 已绑定，正常运行无需先执行制作菜单。沿用原 Addressables default 管理。
- 验证环境：Windows、Unity 6000.5.2f1、Addressables 2.10.3，隔离工程 `D:/UnityProject/HamdokuGameplayMigrationValidation-20261027`。当前正式文件覆盖副本后，Unity 编译、正式 Scene_Main 启动、教程返回首页、摆动与花茎固定、闪光 alpha、timeScale=0 时播放、重复初始化 5 次、关闭重开与旧实例释放通过，最终运行错误 0。人工查看中英文、1080×1920 与 1080×2400 后台相机截图；另生成 1080×1620 截图。Windows Addressables 内容打包通过；未进行 Android APK 构建、安装或真机验证，后台相机渲染及事件检查不替代手机触摸验收。
- 首次验证脚本误把新存档自动进入教程后的状态当作首页异常，第二次等待首页导致超时；修正为通过现有 GameController.ReturnHome 后最终通过，未因此修改正式业务流程。Unity Search 索引越界另记为编辑器异常。日志、源参数、验证脚本、截图、打包结果与一致性核对位于 `Logs/HomeSunflower/`；`sync-audit.json` 核对正式脚本、Prefab、图片及 meta、场景、Addressables 注册与验证快照一致，HomeConfig 仅验证存档文件名不同。未回写验证存档、Library 或验证专用配置，未修改原始取证资料。


## 2026-10-27 Sudoku 标题中心爪印独立切图

- 用户要求标题图片里的狗爪印可以单独运动。按原 Logo 盖章语义处理 Sudoku 字母 o 内的奶油色爪印，替换此前侧边蓝色装饰爪印的选取。
- 更新 `Docs/UILibrary/themes/PuppyBakery/assets/UI/Logo/Img_Logo_Cross.png`，保持正式文件88×89像素；从确认稿提取四个趾垫和一个掌垫，工作母图为 `previews/Logo_Slicing/center_paw_master.png`。更新 `Img_Logo_BoardFront.png` 为完整深棕字母 o 底层，移动爪印时不再露出透明爪形缺口。旧版保存在 `before_center_paw_split/`。
- 同步切图脚本、页面素材 manifest、布局参考、素材总览及分层拼合预览；STYLE.md 记录中心爪印槽位。新增 `center_paw_stamp_preview.gif` 演示出现、放大盖章与轻微回弹，`center_paw_rest_preview.png` 为静止帧。
- 验证：Pillow／SciPy 检查爪印包含5个独立形状，原透明镂空被补全；13张原有素材仍通过同名、同尺寸、RGBA和透明区检查。人工查看爪印透明图及标题静止拼合帧。GIF为离线动画演示，未修改Unity正式资源、运行脚本、Scene或SO，未执行Unity编译、Play Mode、构建或真机验证。


## 2026-10-27 Logo 正式图片替换与 old/new 归档

- 按用户要求在 `Assets/Res_Runtime/UI/Logo/` 下创建 `old/` 与 `new/`；13张原图按原文件名归档到 old，13张已切好新版按同名安装到 new。
- new 继承原始 .meta 字节，保持原GUID、Sprite导入设置和多子图ID；old 使用独立新GUID并保留原图片字节和导入结构。原有Scene、SO及其他页面对原GUID的引用现指向new，不需批量改写序列化引用。目录.meta已创建。
- 修改 `Assets/Src_Editor/EM/LogoSetupEM.cs`、`HomeSetupEM.cs` 的明确图片加载路径至new，LogoSetupEM批量导入扫描范围限制为new，避免影响old。维护离线切图脚本的原始尺寸来源为old，同步主题manifest和STYLE.md。
- 验证：本机Python/Pillow核对13张old图片SHA-256与替换前一致，13张new与已确认切图来源逐字节一致、尺寸一致、活动.meta逐字节保留；扫描Assets下.meta确认本次迁移的活动与归档GUID唯一。检查运行/编辑器C#中的Logo路径均已指向new，未发现这批原GUID出现在Addressables独立条目中。明细见 `Logs/LogoSkinReplacement/replacement.json`。
- 边界：本次仅完成图片安装、旧图归档与必要的编辑器路径修复，没有修改Scene、Prefab、LogoConfig布局、运行时动画或本地化文案。编辑器当前已打开，本次未主动关闭、重启或覆盖其内存场景。尚未验证Unity重导入、编译、Play Mode、Addressables内容打包、应用构建、安装或真机；两行招牌部件位置、眼睛及爪印动画对齐、顶部背景裁切和静态Logo切换仍待运行接入。

## 运行时脚本重载导致交互失效与教程反馈修复（2026-10-27）

- 用户反馈 Unity Game 运行窗口已显示棋盘，但点击没有反应。排查正式场景、EventSystem、Input System 动作、棋盘射线及教程门控；读取正式存档仅用于定位，未删除、重置或写入测试进度。
- 在隔离工程实际触发 `EditorUtility.RequestScriptReload()` 复现：原 ClientMain 仅在 Awake 装配 Context，脚本重载后 Context 为 null，已有 UI 仍留在场景，Update 进入 LoadingSystem 时产生空引用。失败结果保存在 `Logs/GameUIRefinement/Reload-before.txt`。
- 修改 `Assets/Src_Runtime/ClientMain.cs`：依赖创建仍集中在 ClientMain 的 CreateContext；保留正常 Awake／Start 启动时序，OnDisable 保存并按既有 System／Controller 生命周期释放，OnEnable 在重载后重新装配和初始化。未增加单例、静态业务状态或平行启动体系，也未通过空引用检查静默跳过更新。实际重载后的 Context、教程进度、面板唯一性和 Input System 点击全部通过，结果见 `Reload-after.txt`。
- 修正教程误点静默返回：GameController 对不符合当前步骤的操作显示本步骤提示；Panel_Game_Controller 从 GameConfigSO 读取目标并格式化行列坐标。调整 Game_Place／Game_Tap 两份本地化 SO，使用已有 TutorialFocus 图片显示醒目橙色边框；GameOriginalUIEM 同步该设置，未补建基础 UI 层级。同步离线烘焙的中英文字体，检查当前全部本地化文案所需字符均无缺失。
- 验证环境：Windows、Unity 6000.5.2f1、Addressables 2.10.3，专用隔离工程 `D:/UnityProject/HamdokuInputValidation-20261027`。通过 InputSystem.QueueStateEvent 驱动模拟 Mouse／Touchscreen 设备，再由正式 InputSystemUIInputModule 处理，验证鼠标单击／双击／拖动、触摸单击／双击、商店和返回按钮、错误教程格反馈及正确目标推进。此次设备动作验证不等同实际鼠标手工验收或手机触摸验收。初版后台驱动受 GameView 焦点路由影响，修正验证副本的测试焦点设置后通过，未修改正式项目的输入／焦点配置。
- Unity 编译及 Play Mode 回归通过：正式 Scene_Main 启动、全部 146 份地图合法性、教程、前五关、道具、失败恢复／重开、返回／实际场景重载恢复、重复通关防护及释放；最终项目错误 0。查看教程、中英文及长屏安全区后台渲染。Windows Addressables 内容打包通过（32.23 秒）；未构建、安装 Android 应用或进行真机验证。Unity Search 索引异常与项目运行错误分开记录。
- 同步核对：本次 9 份脚本／Prefab／文案／字体与已验证副本字节一致，正式存档文件名保留，Scene_Main 哈希未变化。未同步验证存档、HomeConfig、Library 或测试编辑器脚本。当前旧 Play 会话可能遗留修复前实例，已提示用户停止一次 Play 后重新运行正式场景。验证脚本、历次日志、同步核对及最终结果保存在 `Logs/GameUIRefinement/`，本次最终结果集中于 `InputValidation/`。

## 2026-10-27 个人资料页迁移

- 查找原 APK 导出的 Main／Game／Boot 场景和 Cell Prefab，未找到个人资料页序列化层级；结合 ProfileGridView／UiFactory 的动态创建调用、ProfileConfig、GameConfig 和用户截图，在用户明确确认补建范围后制作正式 `Assets/Res_Runtime/UI/Profile/Panel_Profile.prefab`。只读复制 71 张原始 Sprite，沿用原头像／边框 ID、色板与切片边界；29 个头像槽位中按原 IsAvatarHidden 隐藏 ID 6，实际列表 28 项；21 个边框槽位保留空 ID 8／9，显示 19 项。包括六件服装预览和不穿戴项。
- 新增 ProfileConfigSO、ProfileSaveModel、ProfileState／System、ProfileController、Panel_Profile_Controller、Panel_Profile、Widget_ProfileItem 及对应枚举；复用 ClientMain 装配、PanelFactory／Repository、Addressables default 和 Open／Close 生命周期。页面接通首页头像入口、返回、资料说明、六项统计、UID 复制、昵称编辑／取消、80 条原作座右铭、三个可滚动标签、选择勾选及锁定提示。首页头像和边框随存档选择刷新；提示按 SO 配置三秒消退，模态编辑错误保留至处理。
- 存档升级到版本 5，兼容版本 1–4，保留主线、教程、棋盘、连胜和排行榜队列。新安装记录开始日期；旧存档不补造日期和历史统计，以未知或带 + 的累计值标识。连续登录、连续通关、满生命的三颗种子通关、首次尝试通关按本工程的实际登录／通关／失败／确认重开记录，教程不计入，重复结算不重复累计；这是当前工程落实的统计口径，未宣称已与原 APK 运行行为逐项对照。迁移已完成棋盘时不把下一关误记为重试。
- 新增 117 份中英文 L10NSO；统一经 L10NModule 注册、取文案和切换字体，离线补烘焙现有两份字体，使用同一字体资产的多图集，不添加字体 fallback。昵称最长 12 字，只接受两种语言现有字体均可显示的文字／字母、数字和空格；当前英文字体不覆盖汉字，因此汉字昵称尚不能保存，不以缺字方框代替。昵称、座右铭和 UID 仅为本地资料，未接入云端账号或昵称审核服务。
- 会员头像、会员边框、未取得的赛季／活动服装保持锁定，会员按钮明确提示服务未接入；在线第 1 名次数显示未知。不模拟会员开通、在线排名、奖励领取或服装所有权。普通头像／边框解锁读取从原静态 GameConfig 导入的 SO 数值，未假定线上配置已验证。
- 验证环境：Windows、Unity 6000.5.2f1、Addressables 2.10.3，隔离工程 `D:/UnityProject/HamdokuProfileValidation-20261027`。Unity 编译、正式 Scene_Main 启动和 Play Mode 验证通过，项目运行错误及缺字警告均为 0；覆盖按钮回调、保存往返、无效昵称／损坏存档拒绝、重复打开／初始化、关闭销毁、返回首页头像同步、版本 1–4 升级、已完成棋盘升级、实际教程／主线通关、重开和失败统计。后台相机渲染检查中文、英文、头像／边框／服装、说明／座右铭及 1080×1920、1080×2400、1080×1620、安全区域和横屏容错；这些是编辑器回调与后台渲染验证，不代替人工鼠标或手机触摸验收。
- Windows Addressables 内容打包通过，最终日志为 `Logs/ProfileMigration/Content-final.log`。Unity Search 启动时的索引越界异常与项目运行错误分开保留；未构建 APK、安装应用或执行真机验收。
- 同步：150 份正式资源同步回当前工程，296 项脚本／资源／配置核对记录于 `Logs/ProfileMigration/sync-audit.json`；HomeConfig 仅将 saveVersion 从 4 改为 5，正式存档文件名保持不变。核对 Scene_Main、Addressables DefaultObject／EditorBuildSettings 注册、程序集、包版本及依赖，未同步验证存档、Library 或验证专用编辑器脚本。原始资料和用户已有修改保留；源参数、生成／验证脚本、截图和历次日志留在 `Logs/ProfileMigration/`。


## 2026-10-27 Sudoku Pup Logo 动画错位与结束跳变修复

- 用户报告动画异常并授权自行检查调整。通过Computer Use尝试读取当前Unity窗口，两次捕获超时；未把失败窗口读取当作实际观察。建立 `D:/UnityProject/HamdokuLogoPupValidation-20261027` 隔离副本，直接渲染正式Scene_Main和现有Widget_LogoIntro的Init/Tick序列，修复前帧确认头部被旧遮罩截断、标题与前爪比例错位、动画结束切换完整图片导致尺寸跳变。
- 修改 `Assets/Res_Runtime/Config/LogoConfig.asset` 与 `Scene_Main.unity`：根据新素材实际透明边距和确认页面位置校准角色、睁闭眼、招牌、前爪、中心爪印与静态Logo大小／中心；设置矩形揭示遮罩，调小入场超调与挤压回弹。调整既有引言、署名与加载行位置避免与更高招牌重叠，背景改为顶部对齐以保留遮阳篷。未改动现有文案内容。
- 修改 `Widget_LogoIntro.cs`：入场结束后保留已经落定的图层，继续整体呼吸浮动，不再切到几何结构不同的完整图片；静态分支仍保留。`LogoConfigSO.cs` 增加静态完整Logo显示尺寸／中心字段，`LogoSetupEM.cs` 使用这些字段并将前爪层放到招牌前景。沿用现有Widget和SO，无新增启动或UI架构。
- 修正 `new/Img_Logo_HamsterMask.png` 为白色矩形技术遮罩，修正 `new/Img_Logo_BoardFront.png` 中爪印隐藏时的浅色爪形残影；同步主题源图、切图脚本、manifest和风格记录。保持原PNG尺寸及活动GUID，old目录不变。
- 验证环境：Unity6000.5.2f1、同包配置的隔离副本。先后生成修复前、修复后与最终31帧1080×1920实际Unity渲染序列；运行视觉Play Mode测试，验证完成回调仅一次、结束保持分层、重复Init／TearDown和关闭入场分支，并渲染1080×2400长屏及1080×1620短屏。该测试关闭ClientMain以隔离存档与其他服务，不表示完整客户端启动／导航流程复测。测试脚本仅在隔离副本，未加入正式工程。
- 同步检查：复制前核对当前目标文件仍与任务开始备份一致，未覆盖并发修改；同步后核对场景、SO、3份C#及2张PNG与通过验证的副本字节一致，13张运行PNG与主题源图一致、原活动GUID保留。场景未序列化测试相机或禁用ClientMain。完整记录 `Logs/LogoAnimationFix/verification.json`，原始文件备份 `OriginalFiles/`。
- 查看效果：`Logs/LogoAnimationFix/logo_animation_fixed.gif` 来自最终Unity渲染帧；`before_after.png` 为相同时刻修复前后对照。独立Play Mode结果在 `PlayMode/play-result.txt`。Unity初始授权握手错误随后恢复，另出现UnityEditor.Search.SearchDatabase索引启动的ArgumentOutOfRangeException，单独记为编辑器异常；未发现本次项目C#编译错误或Logo验证断言失败。
- 本次未执行Addressables内容打包、应用构建、安装或真机验证；未宣称当前已打开的主工程窗口完成UI点击验收，也未将隔离视觉Play Mode等同于完整启动服务验收。


## 2026-10-27 Logo 右侧蓝色爪印补点

- 按用户截图蓝框，补齐招牌右侧蓝色装饰爪印缺少的一枚趾点。原图为两枚小趾点加一枚掌垫，现为三枚小趾点加一枚掌垫，与左侧简化爪印的结构对应；中心字母 o 内可动爪印不变。
- 使用内置image_gen对局部爪印补点，提取新增趾点并匹配现有雾灰蓝颜色后做局部合成。同步 `Assets/Res_Runtime/UI/Logo/new/Img_Logo_BoardFront.png`、`Img_Common_Logo.png` 及主题源图、front_master、logo_extracted_master、切图脚本与manifest。保留既有像素尺寸、GUID、透明留白及动画布局，仅新增小范围像素。
- 验证：Pillow／SciPy核对局部独立形状由3变4，掌垫和原有趾点保留；两张运行图片与主题源图逐字节一致，变化像素均限定在新增趾点区域，详见 `Logs/LogoPawDotFix/verification.json`；已查看局部放大预览 `right_paw_after_preview.png`。本次为局部静态贴图修正，未重复Unity编译／Play Mode、打包或真机验证，未改动动画代码、Scene和SO。

## 2026-10-27 全部 Prefab 与原作序列化层级集中整理

- 按用户要求建立 `Docs/UILibrary/PrefabCollection/`，离线可搜索入口为 `index.html`，来源、分类及SHA-256保存在 `manifest.json`；采集与核验脚本位于 `Logs/PrefabCollection/`。AGENTS.md与UI参考库README已补入口。
- 原拆解资料目录递归检索未发现独立 `.prefab` 或 `.unity` 文件。按原asset_map读取全部86个GameObject及86个Transform/RectTransform，输出86份含子树的层级JSON，覆盖10个根对象，保留原对象ID、名称、父子顺序、布局与组件引用；集中373份组件/对象序列化或原始字节、191份配置、118类相关代码结构及可用ARM64/ISIL证据，并保留完整原资源索引用于依赖查询。JSON不是可直接导入Unity的Prefab，未补造原作层级。
- 当前工程Assets下全部10个正式Prefab及对应.meta逐字节复制到 `CurrentProjectPrefabs/`，保留原路径和哈希。副本放在Docs下避免重复GUID导入，依赖仍在当前工程，不是可独立移植的资源包；正式Prefab及其引用未移动或修改。
- 设置弹窗：原静态GameObject未发现SettingsPopup，已集中SettingsPopupLayout配置及SettingsPopup的Build、BuildGroup、BuildLangList等方法结构、反汇编和ISIL资料。动态构建属于静态证据推断；空方法体不是恢复源码，未执行原APK运行核实，也未实现设置弹窗。
- 验证环境：Windows、Python；采集脚本完成，核对全部复制文件与原文件SHA-256一致，全部原GameObject有Transform映射，层级遍历无缺失子节点或循环；全部离线入口链接存在，根层级合计覆盖86个对象，结果见 `Logs/PrefabCollection/verification.json`。资料整理未改运行脚本、场景、SO和存档；未执行Unity编译、Play Mode、Addressables打包、应用构建、安装或真机验证。


## 2026-10-27 标题中心爪印持续呼吸动画

- 按用户要求使标题字母 o 内的中心爪印在盖章结束后持续轻微放大缩小。修改 `Widget_LogoIntro.cs` 的完成后显示分支，独立设置xMark.localScale，保持局部位置不变；沿用整体Logo原有轻微呼吸与浮动。
- `LogoConfigSO.cs` 新增xIdlePulseAmount／xIdlePulseSpeed表现参数，`LogoConfig.asset` 设置0.08和2.8，对应约0.92–1.08缩放与2.24秒周期；更新既有STYLE.md。无新增素材、节点或架构。
- 验证：Unity6000.5.2f1隔离副本编译并运行视觉Play Mode，断言两相位局部比例不同且位于配置范围、锚定位置不变、完成事件不重复；重复初始化／释放、静态分支及三种竖屏渲染通过。主工程3份修改文件与副本逐字节一致。结果 `Logs/LogoPawBreathing/Final/play-result.txt`。
- 首次测试采样的两个时刻恰好接近正弦波相同比例，差值断言失败；调整为约半周期分隔后通过，未因此更改实际动画实现。保留首次结果，区分测试取样问题和项目实现问题。
- 验证禁用ClientMain以隔离存档／服务，不替代完整启动流程和真机验收；本次未执行内容打包、应用构建或真机验证。


## 2026-10-27 Logo 页面素材目录补充整体样式图

- 按用户要求将已确认的1080×1920整体方案复制到 `Docs/UILibrary/themes/PuppyBakery/assets/UI/Logo/Panel_Logo_Style_1080x1920.png`，便于与该页分层素材一起查看。
- 在 `STYLE.md` 的风格基准中补充文件链接和图片预览，明确整体样式参考与正式运行贴图、后续局部修正截图的区别。
- 验证：图片尺寸1080×1920，与确认稿SHA-256一致，文档相对链接目标存在。未修改Unity运行素材或代码，无需重复编译、Play Mode或打包验证。

## 2026-10-27 教程流程与棋盘点击继续对齐原作

- 对照原作 `handoku.mp4` 约 11–27 秒、TutorialController／CellView 的 native 与 ISIL、原 Tutorial_4x4 配置、原中英文文案及 `ui_hand_tap` 成品资源。原目录保持只读，继续使用已经迁移的 level2 UI 层级、正式 Panel／Widget Prefab 和现有资源加载／面板生命周期。
- 教程改为第一行第三格双击、颜色规则确认、同行同列六格逐格排除、第四行第二格双击、相邻紫格滑动排除、第二行第一格双击、自由解题与提示预览、独立完成卡及开始第一关。加入阶段输入门控、原手指资产、聚焦遮罩、上下卡片和教程独立配色；隐藏教程中的主线分数、导航和道具栏。最后核对提示入口使用原灯泡素材。配色与当前线性色彩空间下遮罩透明度属于依据录像校准的实现选择，不作为原始静态配置值。
- 棋盘单击在按下时上报，第二次按下识别双击；双击按输入设备匹配，兼容连续两次触摸具有不同 touchId。滑动激活阈值采用原 CellView 的 0.45 格宽比例，按实际屏幕路径采样，支持画叉与从叉号起笔连续擦除；补入按压缩放、失焦／停用取消及关闭释放委托。
- `PuzzleSaveModel.tutorialFlowVersion` 区分新旧教程。旧版尚在 Playing 的简化教程从新第一步重走，不清主线进度；新流程保存阶段与棋盘，已完成教程仍保留完成状态。教程放置不累计主线分数或关卡进度。
- 主要文件：GameController.cs、Panel_Game_Controller.cs、Panel_Game.cs、Widget_Board.cs、Widget_BoardCell.cs、PuzzleModule.cs、PuzzleSaveModel.cs、GameState.cs、GameConfigSO.cs／GameConfig.asset、UINameType.cs、两份游戏 Prefab、新教程文案 SO 和手指 PNG。GameOriginalUIEM／GameSetupEM 将原动态教程结构实化并烘焙字体；运行程序集补入已经安装的 InputSystem 引用。没有新增启动、资源或 UI 管理体系。
- 验证环境：Windows，Unity 6000.5.2f1、Addressables 2.10.3，独立工程 `D:/UnityProject/HamdokuTutorialValidation-20261027`；验证存档独立于用户存档。Unity 编译、实际 Play Mode、逐帧 InputSystem 鼠标完整教程、不同触点 ID 的触摸双击、按下时响应、滑动插值与擦除、提示无消耗预览、完成进入第一关、保存恢复、磁盘存档往返、失败重开、旧教程迁移及重复 Open 通过，项目运行错误 0。人工查看中英文、1080×1920 与 1080×2400 注入安全区的后台相机截图；它们不等同编辑器人工点击或手机触摸验收。
- Windows Addressables 内容打包通过，最后图标修正后再次内容打包通过。核对 13 份实现／配置与验证副本内容一致，两份 Prefab、两份字体及其 meta 一致；7 份新教程文案仅登记一次，默认加载标签有效，提示灯泡绑定正确。未同步验证存档、HomeConfig 的验证文件名或 Library；正式场景无需先执行制作菜单。
- 初次检查发现旧验证脚本步骤、同帧重新创建 UI 后立即射线的测试条件和共享隔离目录变化，修订测试并改用独立副本。UnityEditor.Search 的启动索引异常单列保留，未计为项目运行异常。原始失败、最终输入结果、画面、内容打包结果及同步哈希在 `Logs/TutorialMatch/`，更新 GameplayMigrationTODO.md。
- 仍待对齐：逐笔划叉、仓鼠放置／庆祝、光圈形态和完整动画时序，提示解释与延时自动强调。未进行应用构建、APK、安装、真机触摸或实际扬声器试听；未声称完整原作复刻。

## 2026-10-27 设置页绘本风格切图

- 用户确认设置页样图后要求开始切图；查看UI参考库原设置截图、SettingsPopupLayout配置、动态SettingsPopup代码结构及现有主题素材。原作未发现独立SettingsPopup层级的既有取证结论保持不变，没有据空方法体宣称恢复页面。
- 使用内置image_gen制作无字结构底稿及图标底稿，再以Python／Pillow／NumPy／SciPy分割、清理透明边缘和保存独立PNG。生成描述以确认图为参考：保留奶油白、雾灰蓝、暖棕描边与克制绘本笔触，分别输出无字面板／标题牌／按钮／开关轨道和独立图标，不将可本地化文案烘焙进素材。新底稿属于重建素材，不是确认图无损裁切。
- 28张素材位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Settings/，包含27张RGBA透明组件与1张1080×1920不透明背景；另存确认整页样式图、manifest、源底稿、切图脚本、九宫格建议及布局数据。背景沿用Logo页干净底图，滑块与分隔线从确认图提取。同步主题manifest和STYLE.md入口。
- 离线验证：28张图片均非空，RGBA、实际尺寸、哈希与透明边界已记录；27张组件四边均保持透明。人工查看清理后的素材总览及1080×1920分层拼合图，修正初次分隔线底色残留和开关九宫格缩放变形。首次拼合脚本因选择底图边距超过目标高度失败，调整后完成，未涉及Unity项目错误。
- 本次交付为待接入主题素材，未修改正式运行图片、场景、Prefab、SO、运行脚本或用户进度；设置页尚未实现。未执行Unity编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证。检查结果在previews/Settings_Slicing/verification.json；离线拼合文本使用Windows字体作位置演示，不代表运行字体效果。

## 2026-10-27 商店页面迁移（服务端逻辑预留）

- 只读核对原包 ShopService.Bootstrap、ShopPopup.Awake／Build 及各商品卡片构建的 native／ISIL、ShopPopupLayout、ShopCatalog 和原始 Sprite；完整导出场景只有 Main／Game／Boot 与 Cell Prefab，商店完整层级由运行时代码创建。依据原作动态结构、布局参数和用户截图制作正式 `Assets/Res_Runtime/UI/Shop/Panel_Shop.prefab`，没有增加场景常驻商店、Canvas、EventSystem 或平行资源管理体系。
- 接入新手礼包、特别优惠、去广告三档、放大镜三档、提示三档，共 11 个商品；包括滚动、商品说明、AUTO 规则本地演示、关闭及恢复购买入口。使用 20 张原作 PNG、正式 ShopConfigSO、31 份中英 L10NSO 和现有每语言单份 TMP 字体；日历／无限符号使用原作图标，补烘焙日元符号，不添加字体 fallback。商品说明数量由 SO 格式化，不另外维护一份礼包数量。
- 截图优先的展示覆盖：新手礼包 5／5、JP¥1,100，特别优惠 40／40、JP¥2,280，去广告 JP¥210／650／1,580；拆解目录原始礼包 10／10、30／30 未被改写。下半部分商品数量和 USD 参考价来自原 ShopCatalog；这些均为静态参考展示，不是平台报价。特别优惠显示“时间待同步”，不伪造服务端期限或把截图的 07:41:05 当成实际倒计时。原始值、覆盖值和素材元数据保存在 `Logs/ShopMigration/source.json`。
- 新增 ShopState／ShopSystem／ShopController／ShopModule、具体请求与商品模型、必要枚举、Panel_Shop／Widget_ShopProduct 和对应 Panel Controller；ClientMain 统一注入并组织生命周期，AssetModule 登记正式 SO／Prefab。沿用 IPanelAsset 与 Action 回调，没有新增 C# interface、abstract class 或单例。ShopModule 的 FetchCatalog／Purchase／Restore 预留调用入口，当前明确返回 ServiceUnavailable；购买、恢复购买不提交订单、不扣款、不发奖、不写入购买权益，重复请求及关闭后的回调按请求编号隔离。
- 首页和主线游戏页的现有商店按钮均接通。首页进入后关闭返回首页；游戏页进入前通过 GameController.PauseForShop 保存棋盘并关闭游戏 Panel，暂停计时，关闭商店后通过现有 GameSystem.Open／存档恢复原关卡。商店期间购买服务未接入，不改变道具、生命或棋盘；应用退出释放商店时不重新打开其他页面。
- 主要文件：`Assets/Src_Runtime/Controllers/ShopController.cs`、`Controllers/Panel/Shop/Panel_Shop_Controller.cs`、`System/Shop/`、`Module/Shop/ShopModule.cs`、`Module/Assets/TM/ShopConfigSO.cs`、`Entities_Sub/Shop/`、`UI/Panel/Shop/Panel_Shop.cs`、`UI/Widget/Widget_ShopProduct.cs`、相关 Shop 枚举；以及 ClientMain.cs、GameContext.cs、HomeController.cs、GameController.cs、AssetModule.cs、UINameType.cs、UIPanelType.cs、`Assets/Src_Editor/EM/ShopSetupEM.cs`、正式 Prefab／SO／图片／文案／字体及 Addressables 登记。制作菜单仅用于生成资产，正式场景运行不依赖先执行菜单。
- Unity 6000.5.2f1／Addressables 2.10.3，在独立 `D:/UnityProject/HamdokuShopValidation-20261027` 验证。Unity 编译通过，Play Mode 最终 46 项自动检查通过：正式场景启动与首页入口、11 个商品请求、恢复购买、存档内存／磁盘不被服务占位修改、说明与 AUTO 演示、重复打开／初始化、关闭销毁、返回首页、游戏页入口、商店期间停止计时、英文状态返回原棋盘且生命／道具／计时不变。项目运行错误和缺字警告为 0；新存档启动与已完成教程的隔离存档分别验证，不复制测试进度回正式环境。
- 人工检查后台相机渲染的中英文、上下滚动、说明／恢复／AUTO 弹窗、1080×1920、1080×2400、1080×1600、注入安全区和 1920×1080 容错画面。检查基于绑定按钮回调和后台渲染，不等同编辑器人工鼠标／手机触摸验收；横向窗口仅作布局容错。最终 Windows Addressables 内容打包通过，不代表 APK／应用构建、安装或真机验证通过。
- 初次制作发现字体无无限符号，改用原图；英文重开首帧文案顺序与窗口变化时滚动位置已修复。初次隔离副本因同步到的教程脚本／Prefab 处于更新中而缺少 tutorialRoot，保留失败日志；同步当前工程最新教程资源后，新存档启动复验通过。同步核查修复了注册合并位置和两个图标的旧 GUID 引用；正式 Prefab 引用均能在当前 Assets／uGUI 包解析，11 个商品、12 个演示叉号和 23 处静态文本绑定完整，33 项商店登记均具有 default 标签，原有登记与字体字符得到保留。EditorBuildSettings、DefaultObject GUID、正式场景和包配置与验证环境一致；不从 Library 复制正式配置。
- 运行结果、各轮失败与修复、后台截图、最终内容打包日志、GUID 修复及同步／正式工程核查分别保存在 `Logs/ShopMigration/` 的 result.txt、Play-*.log、Content-integrated.log、guid-repairs.json、sync-audit.json 和 formal-audit.json。UnityEditor.Search 启动索引异常单独保存在 editor-diagnostics.txt，不计为项目运行错误。未接入支付 SDK、服务端商品／订单／验单／权益／恢复逻辑；未进行应用构建、APK、安装、真机触摸或实际扬声器试听。


- 最终同步保护检测到同期设置页更新后，保留全部设置实现，重新复制当前工程 Assets 到隔离环境，并通过 `Play-integrated.log` 的 46 项商店自动检查及 `Content-integrated.log` 的 Windows Addressables 内容打包。保留 Settings 引入的存档版本 6 和全部偏好字段；正式 default 组共 180 项，其中商店 33 项，未覆盖其他页面登记。最终源码、场景／配置、资源依赖及登记再次核对，未同步测试存档或测试文件名。

## 2026-10-27 设置页迁移

- 在用户确认迁移及补建范围后实施。再次只读解析原 APK 的 data.unity3d：10 个序列化资源文件、86 个 GameObject 中仅有 Main／Game 两个 SettingsButton，没有 SettingsPopup 的序列化层级或 Prefab；原生代码 Awake 调用 ResolveCfg／Build／Hide，Build 通过 UiFactory.NewRect 创建子节点。依照原构建职责、SettingsPopupLayout、Sprite 和用户截图制作正式 `Assets/Res_Runtime/UI/Settings/Panel_Settings.prefab`，不是从空方法体推断运行结果。原资料保持只读。
- 新增 SettingsController、SettingsSystem／State、Panel_Settings_Controller、Panel_Settings、SettingsSaveModel、SettingsConfigSO、VibrationModule 和设置相关枚举。复用现有 ClientMain 注入、PanelFactory／Repository、Addressables default、Open／Close、Ctor／Init／TearDown。本次设置面板 ID 为 50，避开同期接入的商店 ID 40；未新增单例、interface、独立资源加载器或 UI 管理体系。主要变更还包括 GameContext、HomeController、GameController、Panel_Game_Controller、Widget_Board／BoardCell、AudioModule、SaveModule、PlayerSaveModel、Panel_Logo_Controller、AssetModule，以及编辑器 SettingsSetupEM。
- 正式接通首页和游戏设置入口、五项开关、四种连击语音选项、简中／英文选择、保存进度说明、教程回放、客服／隐私／条款入口和当前应用版本。音效关闭会停止音效与语音；音乐独立循环，语音独立通道；区域边框、图案即时更新已有棋盘，设置覆盖层暂停计时并阻断棋盘输入。教程复用现有可交互流程，回放期间不保存教程棋盘、不改变正式教学完成标记，完成或退出后恢复原入口、原棋盘和设置弹窗。关闭、重复打开和重载遵循现有释放约定。
- 复制 19 张原始 Sprite 与 31 段原始音频，导入原始切片边界。三套同名连击音频按原 ResourceManager 的 cute／female／male 路径与 pathID 分别绑定；默认音效／音乐／震动开启，边框／图案关闭，语音 Cute，依据原 GameSettings 及 ComboVoice 的静态资料。背景音乐音量 0.3 来自原 AudioManager 常量；边框宽度／颜色、语音音量和线性色彩空间下的遮罩校准为本工程实现选择，详见 `Logs/SettingsMigration/source.json`。手机震动使用 Unity 移动平台接口，Windows 编译排除此调用；未宣称还原原作的精细震动波形。
- 存档升级为 v6，兼容 v1–v5；新设置独立保存于玩家数据，不修改共享 SO。新增 26 份中英文 L10NSO，沿用统一 L10NModule 和每语言单份 TMP 字体；保留同期新增字符后补烘焙，未添加字体 fallback。语言选中态使用原配置高亮色，避免现有字体不支持的勾号。修正英文存档启动时 Logo 的刷新顺序：先设置对应语言文字，再执行会强制布局的安全区刷新；同时在 Logo 播音前应用已保存的音效偏好。
- “保存进度”明确当前仅本机保存；账号登录、云同步／跨设备恢复未接入。客服与本应用隐私政策／服务条款的正式 HTTPS 地址尚未配置，点击给出明确说明；没有套用原应用政策或模拟成功。版本读取 Application.version，不照抄截图 v1.2.2。
- 验证环境：Windows、Unity 6000.5.2f1、Addressables 2.10.3，隔离工程 `D:/UnityProject/HamdokuSettingsValidation-20261027`，专用存档 settings-validation-20261027.json。最终 `Play-integrated-2.log` 和 `result.txt` 通过：正式 Scene_Main 启动、按钮委托、所有偏好切换及序列化、关闭重开／重复初始化、实例和音乐源唯一性、设置计时暂停和棋盘输入阻断、边框／图案实际显示、教程回放完成／退出及原棋盘恢复、正常教程和真实主线通关、实际场景重启恢复、v1–v5 逐版升级、无效设置 ID 拒绝写入；最终项目运行错误与缺字警告均为 0。检查中英文、语言选择和服务说明，以及 1080×1920、1080×2400、1080×1620、安全区、1920×1080 容错的后台相机渲染。按钮回调和后台截图不替代人工鼠标或手机触摸验收，音频播放调用不等同听感验收。
- Windows Addressables 内容打包最终通过，`Content-integrated-2.log` 记录耗时 23.373 秒。前期隔离副本的教程脚本／Prefab／程序集依赖不同步、设置和新商店面板 ID 冲突、字体勾号缺字、英文启动文本刷新次序及 Windows player 编译下 Handheld 不可用均已修正并保留历次失败日志；Unity Search 索引越界作为编辑器自身异常另存 editor-diagnostics.txt。未构建 Android APK、安装应用或进行真机声音／震动／触摸验证。
- 同步 222 份设置相关正式文件，232 项脚本／资源／配置核对见 `Logs/SettingsMigration/sync-audit.json`，随后再次检查所有已核对文件与通过验证副本一致，Assets GUID 无重复。合并同期商店和输入更新，保留正式场景、当前游戏 Prefab 图片引用、程序集、包依赖、Addressables DefaultObject／EditorBuildSettings 注册和已有条目；HomeConfig 仅 saveVersion 5→6，正式存档文件名不变。未同步验证存档、Library 或验证专用编辑器脚本，正式场景不需要先执行制作菜单。预览见 `settings-chinese.png`、`settings-english.png` 和 `contact-sheet.png`。

## 2026-10-27 再次批量提交

- 按用户要求分批保存当前改动：个人资料／商店／设置接入与教程、Logo 修复及正式资源；Prefab 快照与原作层级证据；Sudoku Pup Logo／设置页主题素材；项目规范、玩法 TODO 与工作记录。
- 本次仅执行 Git 归档与记录更新，核对提交范围、提交结果及最终工作区状态。没有重新执行 Unity 编译、Play Mode、Addressables 内容打包、应用构建、安装或真机验证；各功能既有验证结果及限制保留在对应历史记录中。未推送远程。

## 2026-10-27 设置切图改为现有19张资源一一对应，待审图

- 用户明确要求只切图，确认切图后才开始替换。读取Assets/Res_Runtime/UI/Settings现有19张PNG、meta和Panel_Settings.prefab引用，以该目录为契约；未沿用第一版自行拆出的28张结构。当前目录已有正式Prefab，上一批记录中的页面未实现描述仅对应当时状态。
- 待确认资源另存Docs/UILibrary/themes/PuppyBakery/assets/UI/Settings/v02/，19张文件名、像素尺寸与原资源一致；清单记录原GUID、PPU、九宫格边距和前后透明边界。外层PopupPanel与PopupPanelInner分开，保留原透明蒙版及28像素九宫格边距；RoundedFill、CircleFill逐字节保留白色可着色遮罩。设置项图标包含底块，icon_save保留软盘含义；不加入独立ON/OFF贴图、标题牌、文字或整页背景。第一版素材保留。
- 使用内置image_gen补充空面板与保存图标底稿，其他图标从前批高分辨率底稿切取，清理透明边缘后等比归一化；面板只调整可拉伸中部与边缘以保持角区。生成提示词、脚本、底稿、19张总览、原图与候选图对照及检查结果在previews/Settings_Slicing_v02/。
- 按用户要求使用skill-creator更新C:/Users/Administrator/.codex/skills/.system/imagegen/SKILL.md的Existing Unity UI reskin slicing规则：先查实际资源及消费者，保持文件／尺寸／留白／边距和资源职责，通用Fill保持可着色，候选版本先审后替换；切图任务不包含脚本、Prefab层级或功能修改。Python UTF-8模式下quick_validate通过。
- 离线验证：19张名称集合、尺寸、RGBA均匹配；两种Fill哈希与原图一致；正式Settings目录所有文件在前后SHA-256校验中完全一致，未复制或修改meta。人工查看总览和原图／候选图对照，未替换运行图片、Prefab、SO或代码。未执行Unity导入／编译、Play Mode、内容打包、应用构建或真机验收。下一步为用户审图；没有将本轮制作授权视为正式资源替换授权。

## 2026-10-27 原作方法 IL 恢复与 AI 可读 C# 导出

- 用户确认参照 FrenzySpinHappyJackpot 的工具流程，并授权在默认只读的原拆解目录内新增恢复产物。本轮没有修改原 APK、native、metadata、已有拆解结果或 Unity 运行代码；当前工程仅新增 AGENTS.md 阅读入口并追加本记录。
- 使用原目录现有 Cpp2IL 2022.1.0／.NET 10，以 Unity 6000.5.2f1、匹配的 ARM64 libil2cpp.so 和 global-metadata.dat 为输入，通过 dll_il_recovery 输出 `D:/0000001apkUnbag/01_逆向工程项目/Hamdoku2/03_code/recovered_il/`。工具实际识别 metadata 版本 106，生成 107 份 DLL；与既有结构 DLL 的程序集名称覆盖一致。
- 使用 D:/bin/dnSpy.Console.dll 导出 `03_code/decompiled_csharp/AllAssemblies/` 的 8,652 份 C# 文件；另导出 `GameAssemblies/` 的 283 份游戏 C#，显式保留状态机、访问器和隐藏成员。两轮导出退出码均为 0。该输出是重建伪代码，不能当作原始源码或已编译通过的 Unity 代码。
- 新增恢复／导出脚本 `08_tools/11_recover_readable_csharp.py`、基于 dnlib 的 `08_tools/readable_csharp_audit/` 和索引脚本 `08_tools/12_index_readable_csharp.py`。阅读入口为 `03_code/decompiled_csharp/README.md`、GAME_INDEX.md；程序集／类型／全部方法／游戏方法 TSV、逐方法诊断 JSONL、C# 导出诊断、manifest 和 verification 均位于同目录。完整命令、时间、退出码、输入哈希、IL 审计和交付检查在 `logs/readable_csharp/`。
- 结果边界：工具报告 19,324／19,326 个尝试处理方法未抛出恢复异常，此计数包含辅助方法，四舍五入的“100%”不表示语义完整。四个游戏程序集除 4 个辅助方法外，共 7,047 个方法；7,046 个生成方法体，1 个 IScorePercentileSource.TryGetPercentile 接口方法没有实体方法体。4,465 个游戏方法体带有工具诊断，2,581 个未检测到诊断，均不保证正确执行。两个 Firebase.Firestore 方法出现 ELF 地址解析异常，保留失败日志和异常占位；Unity／System 等 72,447 个基础库方法被工具主动占位，没有宣称恢复其逻辑。
- 验证环境：Windows、Python 3.14.6、.NET SDK 10.0.101、本地 dnSpy。审计全部 107 个 DLL 的类型／方法／指令与诊断，游戏全部 7,047 个原方法均能定位显式 C# Token；核对 8,935 份 C# 和 107 份 DLL 的 SHA-256，1,059 个新说明／游戏目录链接均存在；原 APK、native、metadata 生成前后哈希一致。发现的 645 行 C# 导出诊断保留，未人工填造缺失业务。C# 内 Token／RVA 属于重建 DLL，原 ARM64 地址须查旧 method_index.json 和结构 Address 注解。
- 本轮未执行导出 C# 编译、Unity 编译、Play Mode、Addressables 打包、应用构建、安装、真机或原 APK 动态回归；服务端代码、PairIP VM 完整语义和全部方法语义还原仍未完成。后续复刻可按索引读取 C# 调用线索，并回到原始 ARM64／ISIL、配置和运行证据核验。

## 2026-10-27 游戏页原尺寸切图（仅资料交付）

- 用户确认焦糖饼干提示图样式，随后明确只切图并保存至Docs/UILibrary/themes/PuppyBakery/assets/UI，不替换正式Game目录。本轮交付在该目录的Game子目录，34张PNG文件名和分辨率逐项匹配正式原图，不包含meta。
- 查看Panel_Game.prefab、Widget_BoardCell.prefab、GameOriginalUIEM和Widget_BoardCell的实际拼接／着色关系：规则栏为总底板＋3张独立卡片，每卡左82×82规则图＋右TMP；Img_Game_Hint是放大镜，Img_Game_Guide是灯泡，均含按钮底，数量角标另行显示。规则图保持478×478和原3×3排除语义；教程手指保留原Sprite切片x3、y0、206×236。
- 18张为主题制作或复用：使用已确认509×439小狗头像、已制作主题重开箭头；其余从确认稿提取并按原尺寸整理，背景／空HUD／空按钮／放大的饼干规则底稿通过内置image_gen补绘，使用Pillow／NumPy／SciPy完成透明区清理、分割、等比归一化与图块拼合。12张辅助纹样、3张共享白色遮罩和1张教程手指共16张保留原文件，避免破坏运行时着色、辅助识别、聚焦／遮挡与教程定位。
- 来源、每张尺寸、原九宫格边距、Sprite模式、保留原因和哈希见assets/UI/Game/manifest.json；底稿、完整生成描述、切图脚本、素材总览、920×128实际规则栏、1080×1920离线拼合和检查结果位于previews/Game_Slicing。更新主题清单和STYLE.md对应入口，未新建规范文档。
- Windows本机Python 3.14／Pillow／NumPy／SciPy完成离线检查：34张文件名集合与实际分辨率均对应、RGBA且非空；16张保留项字节一致；正式34张PNG及meta哈希均未变化。人工查看透明底总览、实际规则栏及整页拼合，修正按钮九宫格边缘收窄／凸起及灯泡高光被透明分割误删。初次使用的捆绑Python缺少SciPy，改用本机已有完整依赖后处理通过，无需安装依赖或修改Unity项目。
- 本轮只交付候选切图，不修改正式运行资源、脚本、Prefab、场景、SO、存档或Addressables。离线拼合的布局、调色和Windows文字仅用于审图，不冒充Unity截图；未执行Unity编译、Play Mode、资源内容打包、应用构建、安装或真机验证。与确认整页稿完全一致的布局和运行表现仍需后续实际接入验证。

## 2026-10-28 原作代码阅读引导补充

- 用户确认本轮四文件的文档补充范围，并要求后续迁移内容先确认。AGENTS.md 新增迁移前确认规则和原作代码查阅顺序；PrefabCollection/index.html 增加启动、首页、棋盘、教程、结算、进度、设置的可读 C# 入口，README.md 同步导航。
- 明确 GameAssemblies／AllAssemblies 用途、方法 Token 与诊断定位、ARM64／ISIL 交叉核对和证据边界；既有结构快照及其 manifest 保持原范围。原拆解目录只读，未复制恢复 C# 到 Assets。
- Windows 本机文件检查：新增的原作代码与诊断链接目标全部存在，页面锚点及 README 入口匹配。未执行浏览器点击与显示验证；本轮不涉及 Unity 运行文件，未执行 Unity 编译、Play Mode、内容打包、应用构建、安装或真机验证。
- 本轮不迁移功能、UI 或资源；具体迁移清单、目标文件、行为变化和验证范围将在后续实施前提交用户确认。保留本轮开始前已有的主题素材及工作记录修改。

## 2026-10-28 游戏工具切图局部重绘（仅主题资料）

- 用户明确确认后，使用内置image_gen分别重绘主题Game目录的Img_Game_Guide.png与Img_Game_Hint.png。灯泡改为完整连续的圆润灯体、自然收口的灯颈、整齐双段棕色底座和局部高光；放大镜保留棕色镜框与右下手柄，小狗改为侧向寻找、略收眉眼及小闭口，保持奶油脸、雾灰蓝垂耳与画面右侧眼斑。
- 两图均等比归一化至114×118 RGBA，清理生成底稿外围孤立透明碎点，边界四周透明。旧图、生成底稿、完整提示词、归一化脚本与前后对照保存在Docs/UILibrary/themes/PuppyBakery/previews/Game_Tools_Revision_20261028。更新assets/UI/Game/manifest.json来源与哈希、现有切图脚本修订应用入口、总览及离线拼合；没有更新已确认整体原稿。
- Windows／本机Python／Pillow／NumPy／SciPy离线检查通过：两图114×118、RGBA、非空、四边透明，人工查看原尺寸及放大对照；本轮前后哈希确认其余32张候选PNG不变，正式Assets/Res_Runtime/UI/Game所有既有文件未改。全量34张名称、尺寸和原正式PNG／meta未改动检查继续通过。未修改Unity脚本、Prefab、场景、SO或存档；未执行Unity编译、Play Mode、Addressables内容打包、应用构建、安装与真机验证。

## 2026-10-28 设置页v02图片安装与old/new归档

- 用户授权按Panel_Logo方式替换设置页素材并添加old/new。安装前检查19张PNG、meta、Panel_Settings.prefab引用和SettingsSetupEM图片路径，确认候选哈希与原图未偏离上次审图版本。
- 将主题assets/UI/Settings/v02的19张PNG安装至Assets/Res_Runtime/UI/Settings/new；原PNG逐字节归档至old。new继承原meta及GUID，old仅替换meta主GUID为独立值；新增两个目录meta。Panel_Settings.prefab及其meta原位不变，未修改页面层级、运行逻辑、SO、存档或Addressables配置。
- SettingsSetupEM导入扫描限定new，Sprite查找目录改为Settings/new。同步主题清单与STYLE.md安装状态。未执行该配置菜单，避免带入其他配置行为。
- Windows/Python/Pillow文件层验证：new及old各19张，尺寸匹配，新图与主题源图哈希一致，old与安装前PNG哈希一致，new的meta逐字节保留，Settings目录GUID无重复，Prefab哈希不变。结果Logs/SettingsImageInstall/verification.json。
- 未执行Unity编译、实际导入验收、Play Mode、Addressables内容打包、应用构建、安装或真机验证。本次仅完成素材安装，未将文件检查视为运行页面视觉验收。

## 2026-10-28 新手教程迁移与现有实现修订

- 用户明确本轮只先迁移游戏内新手引导，并确认原作流程、交互限制、视觉表现和恢复／重看衔接范围。原拆解目录保持只读；沿用当前 ClientMain、GameSystem、GameController、PanelFactory／Repository、SO和本地化体系，未引入平行架构。
- 核对原作可读C#的TutorialController.Run／CrossAll／PlaceGuided状态机、TutorialFinger及对应结构／ARM64、原教程4×4配置和已有录像抽帧。明确差异：两段CrossAll都允许点击与拖动并锁定已划叉格；后两次放置突出目标所在整片色区；划叉完成后停留0.5秒；自由解题闲置8秒后才指向提示卡，预览用过后停止再次催促。已据此修订，完成进入主线与设置重看维持现有接口。
- GameController／GameState增加临时停留、闲置与提示使用状态，恢复时由已保存棋盘重建待完成的停留，不保存表现计时器；保留tutorialFlowVersion=1与旧版0重启教程兼容。PuzzleModule检查阶段与必要落子／划叉一致性，允许自由阶段擦除早期教学叉；不将无效存档静默重置。输入仍由Controller裁决，Panel／Widget只显示和上报。
- Panel_Game、Panel_Game_Controller、Widget_Board／Widget_BoardCell补双击节奏、原作抬起／按下两张手指切换、拖动指示、波纹、文本／遮罩淡入、逐笔划叉、放置弹出与六星点、完成摆动；调整提示卡指示位置，使手掌在1080×1920中完整显示。原ui_hand_open Sprite逐字节复制为Img_Game_TutorialFingerOpen.png；原按下手指PNG和多Sprite切片保持不变，修正Prefab由旧21300000指向实际-2995789504102893163切片引用，解决白块。制作工具保留已有Sprite模式，避免维护时覆盖切片。
- 新增两份中英文色区提示L10NSO（橙色／紫色）及default标签注册，补烘焙中文“橙／紫”相关字符，继续由L10NModule统一字体。GameConfigSO／GameConfig.asset保存停留、闲置、淡入、手势和表现时长。0.5、8、0.38、0.45、0.13、0.12、1、1.2、0.42等对应原教程／手指静态常量；逐笔划叉0.24秒、放置0.42秒、庆祝1.2秒与具体弹跳／星点曲线为本工程实现选择。新增波纹和星点为依据原过程式表现制作的透明生产PNG，未伪称原始导出图片。完整主线逻辑提示解释面板及逐帧视觉还原不在本轮完成声明内。
- 主要文件：GameController.cs、GameState.cs、Panel_Game_Controller.cs、Panel_Game.cs、Widget_Board.cs、Widget_BoardCell.cs、PuzzleModule.cs、GameConfigSO.cs／GameConfig.asset、UINameType教程条目、GameOriginalUIEM／GameSetupEM、两份正式游戏Prefab、中文SDF字体、两份新增文案SO、三份新增PNG及其meta、Gameplay Addressables组；更新Docs/GameplayMigrationTODO.md。原Prefab／字体GUID保留，测试存档、测试HomeConfig、Library缓存和打包产物均未复制回正式工程。
- Windows隔离工程D:/UnityProject/HamdokuTutorialValidation-20261027（复用既有缓存）使用Unity 6000.5.2f1／Addressables 2.10.3。最终以已交付序列化资源直接启动Scene_Main，无需先执行制作菜单；Unity编译和Play Mode通过，97项断言涵盖完整教程、错误目标拦截、点击／拖动混用与锁叉、停留恢复、色区聚焦、闲置提示与预览不扣道具、模拟不同触点ID双击、完成进入第一关、重看完成／退出保留主线、存档往返、无效阶段拒绝、旧教程兼容、重复打开、主线失败重开及文案／引用检查。检查中英文1080×1920、1080×2400模拟安全区后台渲染；项目运行错误0。交付引用与标签静态检查、Windows Addressables内容打包通过，内容打包不代表Android应用构建通过。
- 验证初次暴露测试辅助脚本调用了不存在的SettingsController.Close（辅助脚本编译错误）、新增文案注册列表位置错误和旧手指fileID失效，均已修正并重新验证。UnityEditor.Search索引异常单独保存，未计作项目运行错误；未执行APK构建、安装、真机触摸／实际扬声器试听，也未宣称原作逐帧动态回归通过。
- 交付核对：25份本轮实现／资源文件与隔离验证副本哈希一致；共用UINameType仅核对教程条目，保留同期个人资料新增项。正式hamdoku-progress.json文件名、Addressables DefaultObject GUID与EditorBuildSettings注册一致。保留同期设置／个人资料／主题素材改动；隔离结果只覆盖本轮教程快照，不替代这些并行改动的验收。明细、脚本、初次失败日志、最终result.txt、截图、asset-result.txt、content-result.txt和delivery-manifest.json保存在Logs/TutorialMigration20261028/。

## 2026-10-28 个人资料页继续补充

- 按本轮确认范围，对照原作 ProfileGridView、PlayerProfile、ProfileConfig、CostumeOverlay、FrameFx／FrameFxDef、服装 CSV、ProfileConfig 序列化数据与原始截图。原目录保持只读；带诊断的恢复代码回查方法索引、method-issues、source-diagnostics、ARM64 与 ELF 只读常量，不将恢复占位值作为业务参数。
- 补齐头像／边框／服装共54项的组合预览、会员标记、原标签选中／未选中贴图、皇冠徽章与布局、红点和浏览已读保存。6件服装导入174组头像适配参数、来源赛季及原始ID映射（当前项目服装索引保持不变，对应原ID 1／2／3／7／5／6）；新增26张原作手部／皇冠光晕／星点Sprite。皇冠位置、开口与徽章颜色／坐标依据原静态配置和native常量；星点时序、格子预览尺寸及简化变色光晕为工程实现选择，不宣称完整FrameFx分层、原shader扫光或逐帧还原。
- 修正旧实现未检查frameRankWins导致排行榜奖励边框可误选的问题；无真实排行榜结果的奖励保持锁定，徽章显示配置门槛，不伪造冠次数。v1–v6升级至v7，旧存档若选中未经确认的排行榜奖励框迁回默认框，其他资料／进度保留；新增已读位记录。会员／赛季服务未接入，服装仍不可领取或佩戴，显示实际赛季来源及明确提示。首页同步新头像组合与红点，不写入假权益。
- 定点维护Panel_Profile／Panel_Home正式Prefab；修复既有Notice子背景盖住文字、服装预览与名称重叠，保留旧序列化对象和GUID。主要脚本：ProfileController、Panel_Profile_Controller、Panel_Home_Controller、Panel_Profile、Panel_Home、Widget_ProfileItem、ProfileConfigSO、ProfileSaveModel、ProfileState、SaveModule、ProfileSetupEM；新增Widget_ProfilePortrait、Widget_ProfileNewDot、ProfileCostumeFitModel、ProfileCompletionEM和3份本地化SO。ProfileSetupEM衔接补充工具，工具重复执行不重复创建节点；交付资源已保存，运行无需先执行菜单。
- 验证环境：Windows、Unity 6000.5.2f1、Addressables 2.10.3，隔离工程D:/UnityProject/HamdokuProfileCompletionValidation-20261028。最终Unity编译、Scene_Main首次启动、Play Mode及Windows Addressables内容打包通过；验证54项绑定、168种可见头像／服装组合、锁定选择、来源提示、中英文、红点已读序列化、v1–v6资料迁移与旧奖励框修复、重复初始化、释放和返回首页。后台渲染检查1080×1920、1080×2400、1080×1620及模拟安全区；项目运行错误与缺字警告0。测试以编辑器按钮回调和后台相机截图进行，不替代人工鼠标或真机触摸验收。
- 初次验证出现缺少costumeHeadPivot字段、图片导入shape错误及皇冠条件遗漏，均已修正并重新验证。首次Scene_Main启动受同期更新中的旧棋盘Prefab绑定快照阻断，随后只读同步当前工程最新棋盘Prefab到副本，最终首次启动通过；未修改本轮范围外棋盘实现。UnityEditor.Search索引越界与退出临时分配器警告单独保留在日志，未冒称项目错误。
- 同步31份制作结果资源，核对15份实现脚本与验证快照一致、正式Prefab／SO引用可在Assets及PackageCache中解析、字体包含当前工程已有字符；仅向default组追加3份本地化注册，保留同期Gameplay组修改。HomeConfig只升级saveVersion至7，正式hamdoku-progress.json文件名不变；DefaultObject／EditorBuildSettings及包版本核对一致，未同步Library、测试存档或验证专用脚本。资料、历次失败／最终日志、截图、result.txt、content-result.txt、sync-audit.json与delivery-audit.json位于Logs/ProfileCompletion/。
- 未执行Android APK构建、安装或真机验收；完整会员权益、赛季奖励领取与排行榜真实结果、完整原作边框shader／分层特效仍未完成。


## 2026-10-28 个人资料边框列表底部露出修正

- 按用户截图定点修正现有边框页：第四行皇冠贴图向上超出格子，使原730高可见区底部露出少量皇冠。Panel_Profile.prefab中仅将Grid1与其Viewport高度改为722，Grid1中心从-565上移至-561，顶部保持-200；不调整头像、服装列表及皇冠尺寸，不改变滚动和选择逻辑，保留层级、对象ID和GUID。
- ProfileSetupEM同步边框列表高度及顶部固定的定位公式，后续制作时保留修正；无需先执行菜单。
- Windows／Python静态检查通过：第三行格子下沿深度719，第四行皇冠完整图形矩形上沿约725.414，修正后的可见区深度722位于两者之间；另外两页可见区仍为730。检查两个RectTransform绑定及顶部位置不变。未执行本轮Unity编译、Play Mode截图、Addressables内容打包、应用构建、安装或真机验证；实际滚动与运行视觉仍未验证。

## 2026-10-28 Logo页背景顶部蓝色雨棚移除

- 按用户截图去掉Panel_Logo背景顶部蓝白雨棚，使用imagegen编辑奶油色烘焙底纹；新增Logo专用bg_Logo.png及Sprite导入meta，避免改动首页共用的bg_Common_Menu.png。正式Scene_Main仅替换Background的Sprite引用及对应宽高比，LogoSetupEM同步专用资源名，运行无需先执行菜单。Logo人物、文字、加载图标和逻辑不变。
- 主要文件：Assets/Res_Runtime/UI/Logo/new/bg_Logo.png（852×1846）、对应.meta、Assets/Res_Runtime/Scene_Main.unity、Assets/Src_Editor/EM/LogoSetupEM.cs。
- Windows静态检查通过：新图片目视检查顶部雨棚已移除；蓝色像素筛查为0，正式场景Sprite GUID与meta对应、AspectRatioFitter比例匹配图片，首页保留原背景引用。验证脚本首次因PowerShell只读变量命名冲突中断，改名后首页引用检查通过，未修改项目业务。本轮未运行Unity编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证；编辑器当前运行画面尚未验收。
## 2026-10-28 新手引导动画过渡打磨

- 按用户要求继续完善已确认的新手引导表现。检查现有Panel_Game、Widget_Board／Widget_BoardCell、GameConfigSO与正式Prefab引用；已有手指双击／拖动、波纹、逐笔划叉、落子星点及完成庆祝继续沿用，本轮未迁移新增原作资源或扩展业务流程。
- Panel_Game为规则卡、操作卡及确认按钮补整体缩放入场，文本／全屏遮罩使用平滑曲线，手指按压与抬起缩放改为连续过渡。Widget_BoardCell的落子弹出与格子遮罩使用平滑曲线。现有SO淡入、按压和落子时长继续生效；新增曲线与卡片缩放属于工程表现选择，未声称原作逐帧还原。
- 修复全屏遮罩首次直接以目标透明度开始而没有淡入、格子遮罩初始化后使用旧起点插值导致闪变，以及重复配置从动画当前透明度读取目标值的问题。初始缩放和遮罩透明度由已有正式资源读取，未改Prefab层级、GUID、存档或Controller权威状态。
- 主要文件：Assets/Src_Runtime/UI/Panel/Game/Panel_Game.cs、Assets/Src_Runtime/UI/Widget/Widget_BoardCell.cs。Windows本机dotnet build Hamdoku.csproj --no-restore通过，0警告／0错误；目标文件git diff --check通过（仅Git行尾转换提示）。本轮未执行Unity编辑器编译验收、Play Mode连续帧检查、Addressables内容打包、应用构建、安装或真机验证；此前迁移验证不替代本轮动态验收。

## 2026-10-28 设置页首页与游戏内内容区分

- 按用户两张截图及确认范围，补齐同一设置面板在首页与游戏内的差异：首页保留“保存进度”、隐藏“重新开始”；主线游戏隐藏“保存进度”、显示黄色“重新开始”。面板高度、标题、设置组、分隔线、教程、底部链接与版本位置根据正式SettingsConfig重新计算，保持现有1080×1920布局和设置换皮素材；不重建原层级。
- 原作证据来自只读SettingsPopup的ShowAccountBlock、InGamePlaying、Relayout、OnRestart及GameController.CanRestart，按game-method-index检查恢复状态并结合ARM64／ISIL核对。原作设置通过动态代码制作，沿用已有SettingsPopupLayout静态配置与当前正式Prefab补充；教程因CanRestart条件隐藏重开。当前首页保存入口继续使用已有明确状态回应，本轮未新增或宣称完成云存档SDK、每日挑战广告重开或其他模式。
- 设置页重开先关闭并移除设置面板，再复用GameController.Restart重置当前棋盘、计时、分数、错误和道具数量，记录已有个人资料重试状态并保存；关卡、题目和主线通关进度保持。既有游戏重开确认与失败重开共用同一流程；首页／教程拒绝隐藏重开操作，游戏内拒绝隐藏保存入口。保留教程重看后返回原棋盘及重新打开设置的流程。
- 主要文件：Assets/Src_Runtime/UI/Panel/Settings/Panel_Settings.cs、Controllers/Panel/Settings/Panel_Settings_Controller.cs、Controllers/SettingsController.cs、Controllers/GameController.cs、Generic/Enum/SettingsActionType.cs、Assets/Src_Editor/EM/SettingsSetupEM.cs，以及Assets/Res_Runtime/UI/Settings/Panel_Settings.prefab。新增重开节点和正式序列化引用，复用Game目录已有黄色按钮／重开图标及Game_Restart中英文L10NSO；未新增资源加载、Context注入或UI管理体系。定点制作入口重复执行不重复创建节点，原制作入口同步调用；已交付制作后的Prefab，运行无需先执行菜单。
- 验证环境：Windows、Unity 6000.5.2f1、Addressables 2.10.3，隔离工程D:/UnityProject/HamdokuSettingsContextValidation-20261028。本轮快照Unity编译、正式Scene_Main启动和Play Mode通过，共68项检查覆盖两入口、布局高度、重复打开／释放、隐藏操作拦截、中英文、共有开关、重开保持题目与进度、磁盘存档恢复、原重开确认／实际失败重开、教程重看返回及教程重开限制。后台渲染检查中文与英文1080×1920、1080×2400模拟安全区、1080×1620；项目运行错误和缺字警告0。Windows Addressables内容打包通过，不代表Android应用构建通过。
- 首轮验证发现布局更新提前ForceUpdateCanvases，导致切换英文字体时旧中文内容出现缺字警告；调整为先刷新文案后布局，并在打开时先Refresh再Init，复测通过。验证辅助脚本及首轮失败、最终日志、68项结果、截图、内容打包结果和交付哈希核对位于Logs/SettingsContext/。当前工程六份修改脚本、正式Prefab及meta、SettingsConfig、包清单／锁文件和EditorBuildSettings与验证快照一致；仅同步本轮Prefab，未同步Library、测试存档、测试HomeConfig或验证专用编辑器脚本。保留同期游戏动画等其他修改，本轮隔离快照不代替后续并行修改的验收。
- 最终资源核对通过：原有504个序列化对象全部保留，新增13个对象；无重复fileID、本地引用缺失或外部GUID缺失，正式场景唯一EventSystem与GraphicRaycaster有效，Addressables DefaultObject注册和设置GUID可解析，正式存档文件名保持不变。仅清理新增Prefab对象的Unity默认空字段行尾空格，语义不变；相关git diff --check通过。详见Logs/SettingsContext/resource-audit.json及delivery-hashes.json。
- 未执行本轮Android APK构建、安装、真机触摸／扬声器试听或原APK动态回归；运行检查使用编辑器按钮回调与后台渲染，不替代人工触摸验收。

## 2026-10-28 设置开关改为蓝色

- 按用户截图主体色取样，将SettingsConfig.asset的trackOnColor由青绿色改为蓝色#609BD6，trackOffColor改为蓝灰色#9BB1BB；Panel_Settings.ShowSwitch继续读取正式SO。同步SettingsSetupEM中新建配置初始值，以及Panel_Settings.prefab五个开关的默认Image颜色，保留已有对象、GUID、按钮回调和其他素材。
- Windows／Python静态检查通过：五个开关引用及默认颜色与配置一致，全部Prefab对象ID保持，相关git diff --check通过。仅调整颜色，本轮未运行Unity编译、Play Mode截图、Addressables内容打包、APK构建或真机验收；截图中的纹理和描边不属于本轮纯颜色调整。

## 2026-10-28 设置页两张面板边框重新生成，待确认

- 用户反馈正式页面外框及内部卡片的边框变宽、圆角出现棕色实底，要求图二细而流畅的边框。确认只重做PopupPanel.png与PopupPanelInner.png，先交候选与拉伸对照，确认后才替换；其余图标、布局和功能不变。
- 检查发现上一版panel_fit强制复用原图alpha并将透明处RGB延展，导致新轮廓圆角外的棕色区域被旧alpha显露。本次使用内置image_gen分别生成两张干净底图，再仅按新底图自身轮廓清理边缘，归一化到351×512、512×428；不修改已安装版本或旧脚本。候选位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Settings/v03/，原生成图、提示词与处理脚本在previews/Settings_BorderFix_v03/。
- 面板角区归一化到28像素九宫格切线内，四周保留2像素透明边距；不将棕色填入圆角外部。离线制作当前版／候选版对照、1080×1920长面板与不同高度卡片拉伸图、不同宽度拉伸图。采用现有panelPpum0.6与groupPpum0.7近似固定边框显示尺寸，未以此替代Unity实际渲染。
- Windows／Python／Pillow／NumPy／SciPy验证：两图RGBA与原画布尺寸对应，四角及邻近外侧采样alpha均为0，28像素切线与直边中段的左边界一致。人工查看对照及两种拉伸预览。Settings整个正式目录所有文件前后哈希一致，未写入PNG／meta、Prefab、SO或运行脚本。结果previews/Settings_BorderFix_v03/verification.json。
- 未执行Unity导入、编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证。当前等待用户审图，未开始正式替换。

## 2026-10-28 确认后的设置页v03细边框安装

- 用户确认两张v03候选边框并要求替换游戏内素材。安装前核对确认稿哈希、原尺寸、当前正式图与审图时哈希、28像素九宫格边距和Prefab中的原GUID引用。
- 仅替换Assets/Res_Runtime/UI/Settings/new/PopupPanel.png与PopupPanelInner.png。两张PNG与主题v03源图逐字节一致；保留现有meta、GUID及导入参数，Prefab引用继续生效。其余17张PNG、old归档、目录meta、Prefab、SO和脚本未改。上一版v02主题源图保留。
- 同步v03清单、主题manifest及STYLE.md安装状态。Windows／Python／Pillow哈希检查确认Settings正式目录仅两张指定PNG发生变化，其余文件完全一致；结果Logs/SettingsBorderInstall/verification.json。
- 本次完成图片安装，未执行Unity实际导入验收、编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证；离线拉伸预览不等同于实际运行画面。

## 2026-10-28 商店页小狗面包房整体风格稿

- 用户确认保留截图商品结构及信息，按奶油白、雾灰蓝和烘焙金色绘本风格生成整页审图稿；本次授权仅包含整体效果图，不包含切图及正式资源接入。
- 使用内置image_gen，以用户商店截图、已确认小狗头像及Logo页风格图为参考，生成三个商品区与九张商品卡。期限、数量、日元价格和恢复购买入口保留；放大镜说明中的仓鼠随主题改为小狗。文案属于效果图内的图像文字，后续接入应使用正式TMP及本地化。
- 文件位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Shop/：Panel_Shop_Style_v01_source.png保留生成原图，Panel_Shop_Style_1080x1920.png归一化为1080×1920，generation_prompt.txt保存生成描述。已人工检查角色、商品分区、数量与价格，文件尺寸检查通过。当前为待用户审图方案，未宣称定稿。
- 未修改Assets正式资源、meta、Prefab、场景、SO或代码；未进行Unity编译、Play Mode、内容打包、应用构建、安装或真机验证。

## 2026-10-28 商店页去除背景顶部垂边

- 按用户要求使用内置image_gen局部修订商店页，移除背景最上沿整排蓝白遮阳篷垂边并补全奶油色背景，保留小狗、关闭按钮、招牌背后的小遮阳篷及三个商品区。
- 修订稿保存在Docs/UILibrary/themes/PuppyBakery/assets/UI/Shop/Panel_Shop_Style_v02_1080x1920.png，原生成图为同目录Panel_Shop_Style_v02_source.png；保留v01。人工查看顶部与商品信息，归一化输出1080×1920。本次仍为效果图，未切图或接入正式资源，未执行Unity运行及构建验证。

## 2026-10-28 个人资料服装六款候选，待用户选择

- 用户确认六款服装候选范围后开始生成：01小面包师（厨师帽与浅蓝领巾）、02可颂兜帽、03草莓奶油帽、04蓝白睡帽、05南瓜帽、06奶油黄格纹头巾。实际查看已确认头像，以其奶油白脸、雾灰蓝垂耳、画面右侧眼斑与绘本笔触为参考，使用内置image_gen逐款生成。
- 六张试戴图与六张独立服装透明PNG保存在Docs/UILibrary/themes/PuppyBakery/assets/UI/Profile/Costumes/v01/。试戴与独立图分别生成，不能当作逐像素对应分层；尺寸约1350×1165，01独立图1349×1166。当前为选款候选，未做正式原尺寸归一化或全部头像组合的叠加适配。
- 编号总览、可同时查看试戴与独立图的overview.html、完整提示词与来源generation.json、哈希及抽样透明通道检查verification.json保存在previews/Profile_Costumes_v01/。Windows／System.Drawing检查12张PNG均为32位ARGB，抽样均存在透明与不透明像素；人工查看六款试戴、独立图及总览。保留生成图原alpha，未将旧服装蒙版套入新图。
- 未修改Assets正式资源、meta、Prefab、SO、场景或业务脚本；未执行Unity编译、Play Mode、Addressables内容打包、应用构建、安装或真机验收。等待用户选款，正式接入前仍需校准尺寸、耳朵遮挡及服装位置，并另行确认接入范围。


## 2026-10-28 前20关主线选关与进度迁移、用户验收文档

- 用户确认按前述建议开始迁移，并要求交付供其实际操作的验收文档、在AGENTS.md添加入口及后续迁移持续维护规则。本批完成范围为前20关主线选关与进度，保留第21关边界；未扩展每日挑战玩法、完整结算／提示、广告、支付或真实排行榜服务。原拆解目录保持只读。
- 对照原作DynamicDifficulty、CampaignMapBank、CampaignDifficulty的可读代码、game-method-index、method-issues、source-diagnostics及ARM64／ISIL。将提取地图逐条与CampaignDdaMaps原始二进制字段、区域和答案交叉核对；原前十关尺寸资料与后续BoardSize的ELF常量表相互补充。原作静态证据不等同原APK运行对比，恢复C#未复制到Assets。
- GameConfig扩展到20关，尺寸依次4、5、6、6、8、6、7、8、9、7、8、10、10、9、10、10、9、10、10、8。接入所需3436张原地图，其中第10／20关固定题为原entry9876／9877、7×7／8×8；保留原146份独立PuzzleSO与GUID，新增3290份PuzzleSO作为现有GameConfig子资产，避免生成数千独立文件。补齐原BoardView的10种区域颜色，绑定已有第9／10种图案，无新增棋盘、Prefab或图片。
- 新增CampaignController、CampaignSaveModel、CampaignSelectionModel，扩展现有PuzzleModule、GameController／State、PuzzleSO／GameConfigSO、PlayerSaveModel／PuzzleSaveModel及HomeController。前5关只用grade1，第6—20关最高grade2；连续两次无重开且不用道具通关提升难度，重试通关降级；按照原池顺序推进游标并排除最近100张地图身份，固定关优先覆盖。前20关原transform为0，未将第51关起的旋转／镜像提前实现。每日首次游戏降级与已操作未完成棋盘的新日期恢复按静态分支接入。
- 存档升至v8，保留正式hamdoku-progress.json文件名，使用现有原子写入。当前题目、道具使用及失败／重开次数与选图状态独立于共享SO，均由Controller修改；Module只读业务状态、计算选择结果。v1—v7保留旧棋盘与通关进度，缺失的难度／游标历史从一级建立并记录已知上题／当前题，不伪造过去记录。失败弹窗重开不重复计算同一次失败。设置和商店返回、教程重看、重启保持同一道题，旧第5关结算可继续第6关。
- 维护ProfileSetupEM／ProfileCompletionEM／SettingsSetupEM中存档版本赋值为Math.Max，防止后续制作工具将v8降回旧版本；只改版本保护，保留同期个人资料和设置修改。更新本批完成的中英文提示及GameplayMigrationTODO，移除其中设置页尚未接入的过时描述，保留首批历史范围。
- 新增Docs/MigrationAcceptance.md，给用户提供12项手动验收操作、预期结果、关卡尺寸、范围与限制、开发验证状态和问题记录。所有用户勾选保持未勾选，状态为待验收。AGENTS.md A8-B7添加每批迁移必须维护验收文档的稳定规则，A9-B11添加入口；后续按批次追加并保留历史反馈。
- 验证环境Windows、Unity6000.5.2f1／Addressables2.10.3，隔离工程D:/UnityProject/HamdokuCampaign20Validation-20261028。以正式交付资产直接打开Scene_Main，不执行制作菜单，最终Unity编译和实际Play Mode通过163项断言，涵盖完整教程进入主线、20关推进、固定关、难度／新日期分支、重开／失败重开、返回首页、实际场景重载、商店／设置重看、重复Open和通关去重、v1—v7恢复后继续到6、v8磁盘存档、无效游标拒绝。163为断言数，不代表163种独立场景；最终项目运行错误0。
- 已查看中英文10×10棋盘1080×1920、1080×2400模拟安全区和1080×1620后台渲染；Windows Addressables内容打包最终通过。核对3436张地图及答案、全部GameConfig引用、default标签、336份本批代码／配置／关卡／meta／程序集／包清单／EditorBuildSettings／DefaultObject与验证快照一致。最终只规范化本批资产换行及空字段行尾空白，序列化数据不变；目标git diff --check通过。未复制测试存档、测试文件名、Library缓存或打包输出回正式工程。
- 初次dotnet检查使用未纳入新增文件的旧生成csproj而报类型缺失，随后以Unity实际编译为准。首次隔离缓存复制误漏文件名含lock的ColorBlock／UnlockReloadAssembliesTask等包源码，补全缓存后编译通过；未改包版本或以项目fallback掩盖。首次导入脚本在写本地化时遇到Python正则替换转义错误，修正并完成余下写入。一次整合验证发生Unity原生崩溃，保留play-integrated.log，D3D11与2个工作线程重跑play-integrated-2.log通过，具体原因未确定；UnityEditor.Search启动索引异常单列记录。未将旧result.txt误当崩溃运行的新结果。
- 证据、导入／审计脚本、各轮失败／通过日志、163项结果、截图、最终内容打包结果和delivery-audit.json位于Logs/Campaign20Migration/。隔离快照包含当时并行更新，但本轮结果不替代之后的其他页面修改验收。未执行Android APK／应用构建、安装、真机触摸／扬声器／性能或原APK动态选图回归；用户验收仍待执行。


## 2026-10-28 个人资料卡俱乐部说明补全

- 用户确认后，将个人资料右上角“i”打开的纯文字说明补为原作俱乐部图文弹窗。只读查阅原作ClubPage.ShowHelp／BuildHelpRows、HamsterPopup及方法索引，核对ARM64／ISIL和恢复诊断；原作使用动态UI，结合用户截图、Sprite原始九宫格与中英文文案在现有Help节点定点补齐，未复制恢复C#或新增UI管理体系。
- 补入原作popup_frame、popup_btn_pink、popup_btn_close、club_ico_stamp四张图及meta，复用已有资料页日历、皇冠、火焰图标。说明为30天会员、最多20次奖励、两种道具各2个和7天申请条件，数字存入ProfileConfigSO；新增9份中英文L10NSO并更新已有标题，接入default标签和离线中文字体。说明展示不代表会员购买、发奖或资格服务已实现。
- Panel_Profile新增说明标题／四组文案／关闭与确认按钮绑定，继续通过Action交给现有Controller关闭，TearDown解除监听。Controller区分说明与原有昵称／座右铭编辑框，按原有本地化统一刷新。说明使用深色遮罩，编辑框保留原遮罩；正式Prefab已经生成，不要求用户先执行制作菜单。新增ProfileInfoSetupEM作为可重复执行的定点制作入口，ProfileSetupEM衔接此步骤，保留同期其他工作的存档版本Math.Max修复。
- 主要文件：Assets/Res_Runtime/UI/Profile/Panel_Profile.prefab与Info/、Assets/Res_Runtime/Config/ProfileConfig.asset、Assets/Res_Runtime/Localization/Profile_Help*.asset、Assets/Res_Runtime/Font/Chinese SDF.asset、Assets/AddressableAssetsData/AssetGroups/Default Local Group.asset；Panel_Profile.cs、Panel_Profile_Controller.cs、ProfileConfigSO.cs、UINameType.cs、ProfileInfoSetupEM.cs、ProfileSetupEM.cs。本批验收步骤追加至Docs/MigrationAcceptance.md，用户验收保持待执行。
- Windows／Unity 6000.5.2f1／Addressables 2.10.3，隔离工程D:/UnityProject/HamdokuProfileInfoValidation-20261028：Unity编译、73项Play Mode检查通过；覆盖正式Scene_Main启动、四行数值、中英文刷新、三次开关、确认／关闭／返回、释放重开、昵称与座右铭回归。1080×1920中英文、1080×2400模拟安全区、1080×1620后台渲染及文字溢出检查通过。dotnet build Hamdoku.csproj --no-restore通过，0警告／0错误。最终独立Unity编译退出码0；Windows Addressables内容打包通过。
- 配图和遮罩经后台渲染检查后，改用已有原作线性日历／火焰图标，避免重复资源；保留原有6567个Prefab对象，新增96个对象，无重复fileID、本地引用或外部GUID缺失。同步正式资源、meta、SO、字体及9条default注册，核对包清单、EditorBuildSettings、DefaultObject与必要依赖；保留原Addressables分组和条目顺序。未复制验证脚本、Library、测试HomeConfig或存档，未覆盖同期主线开发修改。同步脚本首次检查因同期ProfileSetupEM修改而中止，保留该修改后继续；资源审计补纳入包内uGUI／TMP脚本GUID，最终通过，详见Logs/ProfileInfo/。
- Unity Search索引自身异常单独记录于editor-diagnostics.txt；项目运行错误及缺字警告0。所有截图属于编辑器后台渲染，按钮检查使用回调，不能代替人工点击和真机触摸。本轮未执行应用构建、安装、真机或原APK动态验收；会员／真实奖励服务仍未接入。

## 2026-10-28 项目长按钮绘本风格候选，未安装

- 用户确认按设置页样图中两种长按钮风格制作项目内按钮，范围为Game两张共享底图、Home四张长按钮、Ranking两张胶囊底图、Shop三张操作底图、Streak一张继续底图，共12张；关闭、箭头、开关及首页大型模式卡保留。用户确认素材后才进入old/new安装，本轮未写入正式资源。
- 使用内置image_gen分别生成金黄、雾灰蓝、奶油、桃色、粉色、珊瑚红六种无字高分辨率底稿，清理自身透明边缘；沿用原文件名与像素画布，拼合前批主题爪印。候选位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Buttons/v01/，底稿、提示词、切图脚本、总览和原图对照位于previews/Buttons_v01/。不将文字、功能图标或状态烘焙入底图。
- 读取现有Prefab的22处Image尺寸、类型及PPU倍率制作离线对照。首轮发现原切线使爪印在短按钮中消失，仅改切线仍会在较高按钮中拉长；候选方案同时提出9张Sprite的边距调整，以及切片Image按图片像素高度／显示高度配置倍率。清单记录全部建议值，正式安装须同步相关导入、Image和工具／运行覆盖，不能只替PNG。首页无限模式按既有非等比显示尺寸对爪印宽度作补偿；未新增运行节点。
- 人工查看12张总览、按钮文字及教程图标示意、共享按钮与紧凑胶囊拉伸对照，修正后装饰形状在离线建议参数下保持完整。预览使用Windows字体和离线九宫格算法，不是Unity截图。保留完整22处对照用于核对原参数与建议参数差异。
- Windows／Python／Pillow／NumPy／SciPy检查：12张文件名与像素尺寸对应原图、RGBA、非空内容、透明外角有效；正式UI目录全部文件前后哈希不变，未复制或修改meta、Prefab、SO、场景、运行代码、编辑器工具和存档。结果previews/Buttons_v01/verification.json。同步STYLE.md、主题manifest及MigrationAcceptance.md的候选审图批次。
- 当前仅完成候选审图材料，未进行Unity导入、编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证。等待用户审图及后续安装确认。

## 2026-10-28 小狗面包房19款头像边框候选制作（未替换）

- 用户确认先交付19款边框新图、原尺寸透明PNG、总览与对应清单，正式Unity接入另行确认。核对ProfileConfig的21槽位中8／9为空；本批对应profile_frame_0至7、10至20，包含7基础、1金冠、6彩色皇冠、4段位与1会员框。
- 使用内置image_gen逐张生成，原边框作构图参考，已确认小狗作绘本画风参考；保留皇冠、丝带、徽章等级识别，采用奶油饼干与低饱和糖霜配色。徽章、会员牌留空，不烘焙数字、文字、锁头与现有光效。原始出图和提示词保存在Docs/UILibrary/themes/PuppyBakery/previews/Profile_Frames_v01/。
- 19张候选及manifest位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Profile/Frames/v01/。经预乘alpha的3×3坐标重采样归一化到原尺寸、对齐开口中心横纵截线；不套用原透明蒙版。生成contact_sheet_v01.png、original_vs_candidate_v01.png及puppy_overlay_v01.png，已人工检查；小狗叠放按开口离线拟合，不代表当前Widget运行比例。
- Windows／Python／Pillow／NumPy检查通过：名称19项对应、画布逐张等于正式原图、RGBA、中心与四角透明、开口中心截线误差0像素，正式Profile PNG及meta前后哈希未变。制作过程中Panel_Profile.prefab和ProfileConfig.asset由其他任务更新，本批不覆盖；处理前后单独复核197个快照文件没有进一步变化。验证记录为previews/Profile_Frames_v01/verification.json。
- 更新主题manifest、STYLE.md及Docs/MigrationAcceptance.md的候选审图批次；用户验收不代勾。本批未安装正式图片或修改脚本、Prefab、SO、Addressables和存档，未执行Unity导入、编译、Play Mode、内容打包、应用构建、安装及真机验证。后续需用户审图确认，再单独开展正式替换与运行验证。

## 2026-10-28 个人资料28款小狗头像及前爪候选制作

- 用户确认可见列表28项（普通16、会员12），要求沿用已确认的小狗面包房风格生成并先审图；隐藏槽位6不制作。使用内置image_gen逐张生成28张头像，并生成20张配套前爪层；遇到连接失败后重试成功，未切换CLI或其他模型。
- 48张独立透明PNG保存在Docs/UILibrary/themes/PuppyBakery/assets/UI/Profile/v01，沿用正式文件名与对应原画布尺寸，按比例归一化。底稿、完整头像及前爪提示词、来源清单、处理脚本、头像总览及前爪总览位于previews/Profile_Avatars_v01。同步维护主题manifest、STYLE和迁移验收文档，状态均为候选待用户确认、尚未安装。
- Windows离线Pillow检查48张文件名、画布尺寸、RGBA、非空内容及透明通道0至255范围通过；人工查看28张头像和20张前爪总览。前爪为AI分离后按原画布等比拟合，不是无损提取；尚未完成服装遮挡、Sprite轴心及运行叠放精确对齐验证。
- 本批未修改正式Assets、代码、Prefab、SO、Addressables或存档；制作期间其他任务修改的ProfileConfig和Panel_Profile保持不动。未执行Unity编译、Play Mode、内容打包、应用构建、安装或真机验收。用户尚未确认图片可用，不能记为已完成换皮接入。

## 2026-10-28 小狗头像整套美术确认与保留

- 用户明确回复“可以！都很好 都给我留下”，确认并保留Profile/v01中28款小狗头像与20张前爪层；同步更新本批manifest、主题manifest、STYLE与迁移验收记录。所有图片与底稿保留，未替换正式运行资源，前爪服装叠放尚未验证。
- 用户另提供灰白仓鼠会员12款截图，要求先确认再生成。本轮只核对对应原头像17至28及灰白仓鼠造型，尚未生成第二套；拟另存独立版本，等待用户确认具体制作范围。


## 2026-10-28 商店页原作核对与19张素材切图（未安装）

- 按用户授权核对原项目商店Sprite、ShopPopupLayout配置、恢复ShopPopup构建方法及当前正式Panel_Shop.prefab和图片导入参数。原项目保持只读，恢复代码含诊断，仅用于定位动态UI与图片／文本职责，不作为运行事实。
- 使用内置image_gen补绘无字整页、无顶部垂边背景及空分组面板，再用Pillow／NumPy按确认稿裁切、透明隔离和组装。19张PNG位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Shop/，11张保留当前商店正式图片名称及尺寸，8张为背景、面板、标签及小图标配套。商店标题、描述、期限、数量与价格均保持独立文字；ADS作为符号保留。礼包、限时优惠、会员及9张未覆盖正式图不在本稿切图范围。
- 清单记录原始来源、实际尺寸、透明边界、九宫格及哈希。关闭按钮沿用当前249×256，区别于原导出256×256。正式布局不同于确认稿；新增图层需绑定，原日历／无限粉色染色需校准，商品卡及按钮离线边距显示比例0.35，正式PPU倍率待Unity校准。未修改正式图片、meta、Prefab、场景、SO、脚本或存档。
- 生成提示词、原图、切图脚本、原始尺寸清单、方法证据、19图总览、1080×1920离线拼合和verification.json位于previews/Shop_Slicing/。Windows离线尺寸、RGBA、非空、透明角、边距及哈希检查通过，商店正式资源和相关实现前后哈希无变化。素材与拼合均人工查看；STYLE.md补充入口，Docs/MigrationAcceptance.md追加本批审图步骤，用户验收未勾选。
- 首次脚本缺SciPy，改用已有Pillow／NumPy；本机阶段性内存不足后重试成功。首次图像检查修复关闭按钮缺叉、面板边缘商品残影和九宫格色带过高问题；一次验证误在切图尚未结束时读取，报PNG截断，待切图完成后重新全量读取检查通过，未放宽损坏文件校验。未执行Unity导入验收、编译、Play Mode、内容打包、应用构建、安装或真机验证。

## 2026-10-28 用户确认19款边框全部保留，后续再替换

- 用户确认本批边框美术可用，并明确要求保存后留待后续替换。19张原尺寸成品、19张原始生成底稿、提示词、总览、原图对照、小狗叠放及ZIP素材包均已保留在主题目录；逐张核对成品SHA256与原清单一致，ZIP完整性及20个条目（19张PNG加清单）检查通过。
- 更新Frames/v01/manifest.json中整批及逐项状态、主题manifest、STYLE.md与迁移验收文档，记为user-approved-artwork-not-installed，批准日期2026-10-28；素材包同步包含最新状态清单。runtimeReplacementAuthorized仍为false，本次未替换正式图片或修改运行代码、Prefab、SO、Addressables、存档；没有新增Unity编译或运行验证。

## 2026-10-28 第二只小狗三种基础造型候选

- 用户纠正第二套不是小仓鼠，选定方向2奶油卷毛犬、5芝麻雪纳瑞、6黑糖奶牛犬，授权每种先生成一张基础头像。本批通过内置image_gen分别生成3张正面无身体头像，以既有确认图作画风参考，分别使用奶油卷耳、灰色短折耳与奶白眉胡、深可可双耳与不对称额头花纹区分；尚未扩展12表情或前爪层。
- 3张透明RGBA候选保存于Docs/UILibrary/themes/PuppyBakery/assets/UI/Profile/DogVariants/v01，归一化为509×439；原始生成PNG、完整提示词与同尺寸对照图位于previews/Profile_DogVariants_v01。离线检查3张非空内容、RGBA、尺寸与透明通道范围0—255通过，人工查看全部原图。当前均待用户选型，原已确认的小狗28头像及20前爪保持保留。
- 本批只制作候选图片，未修改运行资源、脚本、Prefab、SO、Addressables、存档或既有图片；未执行Unity编译、Play Mode、内容打包、应用构建、安装或真机验证，不视为已完成正式换皮。

## 2026-10-28 新增八款服装候选07–14，待用户选款

- 按用户确认范围，使用内置image_gen延续既有小狗面包房绘本风格，制作07吐司兜帽、08布丁帽、09曲奇贝雷帽、10蜂蜜小蜜蜂、11雨天小狗、12草莓田园帽、13冬日针织帽、14小画家。使用已确认正面小狗作为图像参考，保留奶油白脸、蓝色垂耳和画面右侧眼斑。
- 每款包含试戴图及独立服装透明PNG，共16张，均1350×1165，保存于Docs/UILibrary/themes/PuppyBakery/assets/UI/Profile/Costumes/v02/。续作保留已生成07–09，不重复生成；吐司／蜜蜂独立图首轮残留耳朵、雨帽首轮残留侧部蓝色耳片，已通过image_gen局部修正；弃用结果仅归档在previews中。
- 编号试戴总览contact_sheet.png、独立服装总览accessory_sheet.png、对照overview.html、规格与生成描述、修复提示词及verification.json位于previews/Profile_Costumes_v02/。Windows／System.Drawing检查全部16图为32位ARGB、抽样存在透明与高alpha可见像素并记录SHA256；人工查看各款及总览，修复后重查雨帽并重做独立图总览。
- 本轮是候选生成，试戴与独立服装分别生成，非逐像素对应分层；未按正式运行尺寸归一化，尚需选款后的遮挡、位置、尺寸与多头像组合适配。旧六款、正式Assets、脚本、Prefab、SO、场景、存档均不在本次修改范围。未执行Unity编译、Play Mode、Addressables打包、应用构建、安装或真机验证。Docs/MigrationAcceptance.md追加本批候选审图说明，用户验收保持未勾选。

## 2026-10-28 已确认19款边框正式替换

- 用户明确要求“现在就替换了”，授权立即安装此前确认的整套19款边框。预检正式profile_frame_0至7、10至20.png与原始来源哈希一致，确认稿SHA256、画布尺寸、RGBA及ProfileConfig对应槽位GUID有效；沿原路径只覆盖19张PNG，不迁移资源目录或重建UI。
- 正式目标为Assets/Res_Runtime/UI/Profile/，逐字节保留19份meta、GUID、PPU、Sprite模式和其他导入参数。确认稿、19张生成底稿和预览仍保留；旧图及原meta备份到Logs/ProfileFrameInstall/before/，不放回Assets产生重复GUID。未修改配置、Prefab、场景、代码、Addressables或玩家存档，现有边框ID、选择和解锁规则沿用。
- Windows／Python／Pillow／SHA256文件检查通过：19张正式PNG与确认稿一致，尺寸不变，原meta不变，19个GUID及配置槽位引用保持，备份完整；210个受保护文件中只有19张目标PNG变化。安装脚本、前后哈希和结果见Logs/ProfileFrameInstall/。主题清单、STYLE及迁移验收文档更新为已安装、运行待验收，用户验收未代勾。
- 本轮未执行Unity实际导入验收、编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证。会员牌沿确认稿留白，未新增文字组件；实际徽章位置、光效和头像裁剪仍需运行检查。离线小狗叠放图不能作为本轮运行截图。


## 2026-10-28 主线开局揭示、连击计分与通关收尾迁移

- 按用户确认范围，只读核对原GameConfig、level2 GameController／GameHUD、GameController及状态机的恢复索引、诊断和ARM64证据。接入前20关开局揭示（原预置优先，否则随机正确格）、基础分576、连击增量96、失误归零与无等待超时规则；揭示不增加连击，不把恢复代码或工具零值直接当作原源码。方法索引、15条相关诊断、配置副本、来源哈希及验证脚本留存Logs/GameFlowMigration/。
- 新增静态GameFlowController，在现有GameController、HomeController、GameState、PuzzleSaveModel、PuzzleModule、SaveModule及GameConfig中接入规则、校验和保存。存档版本9、文件名hamdoku-progress.json；v1—v8旧棋盘与已有分数保留，不追补揭示或推断历史连击，新得分另记逐格账本。非法账本明确报错，不重置用户进度。重开保留关卡地图，随机揭示允许变化。
- 最后一次正确放置立即确认通关并保存，计时冻结，界面及Controller锁定收尾期间操作；剩余空格0.45秒后补叉并按0.09秒斜向间隔扫光，随后庆祝与0.45秒结果等待；无空格跳过补叉等待。收尾前后可恢复，进度、排行榜、资料及连胜不重复记账；继续衔接现有结果／连胜／下一关及第20关边界。完整结果页、百分位与奖励库存没有计为完成。
- Panel_Game正式Prefab保留原层级并绑定ScoreGain／Combo两个非交互TMP，沿用棋盘现有落子、标叉、庆祝资源，增补扫光。Game_Gain／Game_Combo共两份中英文L10NSO注册default，复用语言字体；语音改按真实连击数调用SettingsController。GameFlowSetupEM用于维护制作结果，现有UI制作入口调用它，正式运行不要求先执行菜单。未新增启动、Canvas、Context依赖或UI／资源管理体系。
- Windows Unity 6000.5.2f1、D3D11，复用隔离验证目录D:/UnityProject/HamdokuCampaign20Validation-20261028。编译通过；最终Play Mode 135项检查通过，覆盖教程、完整20关、正确／失误／等待／提示／重开、v1—v8迁移、v9恢复、收尾前后中断、无空白格分支、错误账本拒绝及相邻连胜页。1080×1920中文、1080×2340英文安全区、1080×1600短屏后台渲染与新增文字字符检查通过，图像已查看。只使用gameflow-validation-only.json，没有同步测试通关数据。
- Windows Addressables内容打包通过（content-final.log），不等于APK构建。54份交付关键文件、meta、程序集相关代码、包版本和Addressables入口核对通过：52份逐字节一致，Default Local Group与AddressableAssetSettings仅列表序列化排序不同，按GUID／地址／标签及组引用逐项相同。正式HomeConfig为v9且恢复正式文件名；DefaultObject及EditorBuildSettings注册保留。资源组只合并本批两份文案，保留其他聊天的修改；当前游戏按钮、头像边框等并行变动未覆盖。
- 验证过程首轮脚本漏过已有连胜奖励页而停在第2关，修正验证衔接后通过；首次截图揭示飘字首帧alpha为0，修正ShowGain立即显示后重验。整合最新工程时发现HomeSetupEM新增Concat调用缺System.Linq导致编译失败，仅补引用后再次编译和全流程通过。UnityEditor.Search索引异常另记editor-diagnostics.txt，不计为项目运行错误；LF／CRLF Git提示不作为代码错误，改动内容无尾随空白检查错误。
- Docs/MigrationAcceptance.md追加本批12项用户操作、预期、兼容策略、边界与验证状态；保留所有历史及其他聊天新增批次，用户验收不代勾。Docs/GameplayMigrationTODO.md更新本批状态；AGENTS.md已有强制逐批验收规则与文档入口继续有效。本次未执行人工编辑器点击、原APK动态对照、Android应用构建、安装或真机触摸、音频、性能验收。


## 2026-10-28 商店图片old／new整理与正式安装

- 按确认范围整理Shop目录。实施前发现另一批统一长按钮已先建立Shop/old与new、安装3张按钮，并修改最新Prefab及ShopSetupEM；初次预检因现有目录停止，尚未修改正式文件。用户随后明确选择保留刚安装的统一shop_btn_magnifier。本轮基于最新文件继续，不覆盖3张统一按钮或其meta／归档。
- Assets/Res_Runtime/UI/Shop/new共28张：替换10张同名商店新图，迁移7张未重绘原图，保留3张统一长按钮，新增8张未绑定配套图。Shop/old共20张原图，独立GUID；20份当前正式meta逐字节保留，原GUID和Prefab引用继续指向new。最新Panel_Shop.prefab及meta、ShopConfig.asset保持本轮安装前内容。
- ShopSetupEM扫描范围由根目录加new收敛为new，通用图片查找首目录改为Shop/new；保留另一批统一按钮的显式路径、导入参数与像素倍率调整。新增shop_btn_hint、shop_tab_magnifier、shop_tab_hint、shop_section_panel九宫格边距，防止未来导入时丢失。未执行制作菜单，未修改布局、图标染色、TMP、业务流程、存档或共用Game背景。
- Windows／Python／Pillow文件检查：28张PNG完整读取、尺寸与RGBA通过；18张本批源图哈希一致，重叠按钮另记实际保留哈希；20张原图归档及原meta／GUID保留、20个Prefab商店引用、Shop范围及新增GUID无冲突检查通过，3张统一按钮与其meta及旧归档均逐字节保留。安装前完整快照、脚本、前后哈希及verification.json位于Logs/ShopImageInstall/。主题清单与STYLE更新安装状态，迁移验收文档追加步骤，用户验收未勾选。
- 本批不代表商店整页风格已接入：8张配套图仍未绑定，商品卡PPU倍率与旧粉色染色未校准。尚未执行Unity实际导入验收、编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证。

## 2026-10-28 芝麻雪纳瑞第二套12头像与8前爪候选

- 用户选定5号芝麻雪纳瑞并授权制作第二套。按此前确认的12个会员头像17至28及8个前爪层范围完成20张图片；17号自然微笑直接逐像素复用已确认基础图，另外11张头像和8张前爪通过内置image_gen分别生成，不改动蓝耳小狗48张已确认图片，另外两种基础造型也保留。
- 候选图片位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Profile/Schnauzer/v01；底稿、完整头像与前爪提示词、来源基准哈希、归一化脚本、12格头像总览、8格前爪总览及verification.json位于previews/Profile_Schnauzer_v01。同步主题manifest、STYLE、选型manifest和迁移验收条目；整套状态待用户审阅，尚未安装。
- Windows离线Pillow检查20张文件名、RGBA、非空内容、对应原画布尺寸、透明通道0至255范围通过；人工查看完整头像与前爪总览，基础款像素一致性通过。前爪为AI分离后按原画布等比适配，非无损提取；尚未验证服装遮挡、Sprite轴心和精确叠放。
- 本批未修改正式Assets、代码、Prefab、SO、Addressables或存档；期间其他工作修改的Panel_Profile与19张边框已在基准对照中列明并保留，不能声称全部Profile目录哈希未变。原蓝耳小狗图片未变。未执行Unity编译、Play Mode、内容打包、应用构建、安装或真机验证。

## 2026-10-28 新增八套服装候选15–22，待用户选款

- 按用户确认范围，使用内置image_gen，以已确认正面小狗为图像参考，制作15肉桂卷帽、16樱桃纸杯蛋糕帽、17小邮差、18园艺小狗、19海盐小水手、20雪夜小鹿、21小魔法师、22赏花小狗。延续奶油白脸、雾灰蓝垂耳、画面右侧眼斑和暖棕绘本描边。
- 每套交付试戴图与独立服装透明PNG，共16张，均1350×1165，位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Profile/Costumes/v03/。独立图保留帽子、装饰、配套领结／围巾／领口，不包含小狗脸和耳朵；没有覆盖前两批候选或正式运行资源。
- 逐图完整生成提示词、规格与清单generation.json、试戴总览contact_sheet.png、独立服装总览accessory_sheet.png、逐款对照overview.html及verification.json位于previews/Profile_Costumes_v03/。Windows／System.Drawing检查16图均32位ARGB、尺寸一致、抽样有透明及高alpha可见像素，并记录SHA256；人工查看16图和两张总览。
- 本轮为候选图生成，试戴与独立服装分别生成，非逐像素对应分层；未按正式尺寸归一化，选款后仍需校准叠加位置、耳朵遮挡及多头像适配。未修改正式Assets、脚本、Prefab、SO、场景或存档。未执行Unity编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证。迁移验收文档追加本批审图入口与限制，用户选款未勾选。

## 2026-10-28 项目12张长按钮正式替换与Unity视觉验证

- 用户确认候选并授权安装。安装12张：Game两张共享操作底图、Home四张长按钮、Ranking两张胶囊、Shop三张操作底图、Streak一张继续底图。各页面new继承原GUID、old使用独立GUID归档原图；原图及meta在Logs/ButtonsInstall/before另存操作前快照。其他新商店素材和用户未提交修改保留。
- 为保持爪印形状，同步9张Sprite的九宫格边距及9份现有Prefab内20处切片Image倍率；按图片像素高度／实际显示高度配置，文字内容、原图标、RectTransform尺寸和按钮回调不变。将浅色底图上原白色标签定点改为暖棕色，提高可读性。
- 修改8份既有编辑器脚本GameSetupEM、GameOriginalUIEM、HomeSetupEM、ProfileSetupEM、SettingsSetupEM、ShopSetupEM、RankingSetupEM、StreakSetupEM：对应按钮路径明确指向new，导入不扫描old，导入边距及新建／调整Image的倍率与正式Prefab一致，标签对比度保持。没有新增运行脚本、面板节点、资源加载或UI管理体系；缺失正式按钮资源明确抛错。
- Windows文件层检查：12张新图与确认稿、old与原PNG哈希一致，尺寸保持，new的meta除已确认边距外保持原导入字段及GUID；GUID唯一，按钮布局与回调保持。期间其他开发更新Panel_Game的计分节点，保留完整更新并仅重新应用目标按钮倍率；未回滚这些变更。静态检查、安装映射、差异快照和核验脚本在Logs/ButtonsInstall/。
- Unity6000.5.2f1独立工程D:/UnityProject/HamdokuButtonsValidation-20261028完成资源导入、C#编译、22处Prefab引用及倍率检查和隔离Play Mode。实际后台相机渲染检查首页、中文／英文设置页、1080×1920及1080×2400；使用既有L10NModule和文案资源，设置页重新开始／教程Action绑定、重复创建和TearDown释放通过。验证不启动ClientMain，不读写用户存档，不替代完整业务启动、Editor人工点击或真机触摸验收。
- 当前工程41份本批图片、meta、Prefab与编辑器脚本和验证副本内容一致。play-result.txt通过，sync-audit.json全项一致；人工查看渲染中按钮边线、爪印、原功能图标和中英文标签，无本批遮挡或明显变形。部分仅用Prefab默认值渲染的汇总图未初始化动态标签；本地化设置图另行完成，未将空标签当成运行状态。
- 初次隔离复制遇到其他并行每日挑战代码与枚举不同步导致编译失败，完整重同步后通过；首次图数断言把新增shop_btn_hint计入本批，按本批12个文件名限定后通过。UnityEditor.Search启动索引ArgumentOutOfRangeException单列保留，不计为项目业务运行异常。原失败、后续通过日志均在Logs/ButtonsInstall/。
- 更新主题manifest、按钮素材manifest、STYLE.md和MigrationAcceptance.md的A11安装验收批次。没有运行页面制作菜单、迁移验证存档或同步Library／验证专用脚本回正式工程；未执行Addressables内容打包、应用构建、APK、安装或真机验证。用户验收项目保持未勾选。


## 2026-10-28 每日挑战页面与本地玩法迁移

- 用户确认 UI 与内容一起迁移，并确认基于原作动态构建代码补建正式 Prefab。只读核对 DailySeasonPage／DailySeasonJigsaw／DailyDatabase／GameProgress／CostumeRewardService、方法索引及诊断、原作资源容器、Daily_seasons、Costumes 与两套拼图元数据。恢复 C# 的无诊断方法亦可能失真，日期周期和拼图夹取回查 ARM64／ISIL；来源与边界保存在 Logs/DailyMigration/source-methods.json。
- 新增 DailyConfigSO／DailySeasonSO、1820 份每日 PuzzleSO、202610／202611 赛季、146 份原图、35 份双语 L10NSO、Panel_Daily 与 Daily Controller／System／State／Save 模型；接入 ClientMain、AssetModule、首页和现有棋盘／结算／资料。沿用 PanelFactory／Repository 与 default 标签，正式 UI、序列化引用和导入设置已保存，无需先执行制作菜单。新题按 2026-08-17 起的 364 天周期表分配，不进入主线抽题池。
- 每日日期选择、首次挑战、未完成续局、完成／完美印章、最佳时间、拼图 1／2 块结算、服装切换、消耗拼图领取、资料穿戴、月度记录、规则和插画预览接通。存档升 v10，主线与每日使用独立棋盘槽；v9 升级保留原 20 关进度及主线棋盘，其他旧版本未逐版本重跑。补签、再次挑战和失败重试因广告／云端授权未接入而明确不可执行；不填造 TOP 百分比或云端发奖成功。
- 修复原 Sprite 导出网格破坏拼图透明边缘的问题，使用对应原 Texture2D；修正新资源引用类型、导入形状和 Rect 序列化版本。补烘焙现有两种字体字符，去除不受支持的箭头字符；按运行截图定点修正英文奖励文案重叠、按钮裁切、成绩行间距与服装预览的头像坐标继承。必要的 PanelRepository 修复处理 Unity 先销毁面板后接口引用仍非空的生命周期情况，避免退出时重复 TearDown 已销毁对象。
- Windows／Unity 6000.5.2f1／Addressables 2.10.3 隔离副本验证：编译与正式 Scene_Main 启动、主流程 23 项、奖励 6 项、重启恢复 2 项、失败 6 项通过，内容打包通过；人工查看中英文、两个月主题、棋盘、通关、记录与服装穿戴截图。安全区域验证覆盖长短屏缩放计算，不代表真机触摸验收。部分运行出现 Unity SearchDatabase 自身索引越界，单独记录；资源导入及项目问题均修复后复验。
- 正式交付同步原图、Prefab、SO、meta、离线字库和 Daily Addressables 分组；保留当前其他未提交修改，只增量加入分组，核对 EditorBuildSettings／DefaultObject 与包版本。交付审计确认无 GUID 冲突、全部每日引用有效、146 份原图与来源哈希一致、2106 份运行脚本／资源与验证镜像一致；从同步后的工程文件直接启动复验通过，未复制 Library、验证公司／产品设置或测试存档。记录见 Logs/DailyMigration/，Docs/MigrationAcceptance.md 追加本批操作与用户待验收项目。
- 尚未接入 Firebase 每日服务、广告授权、补签券／免费补签额度、云存档／校时与 TOP 排名；主题资源仅覆盖原资料已有的 2026 年 10／11 月，原作拼图精确揭示顺序未可靠恢复，当前为显式逐行配置。未自动操作当前已打开编辑器进入 Play Mode，未做应用／APK 构建、安装、原 APK 运行回归或真机验证，用户验收未代勾。

## 2026-10-28 按功能整理Git提交并推送

- 按用户要求，将本批已有改动按设置边框、头像边框、蓝耳小狗、基础造型、三批服装、雪纳瑞、奶油卷毛犬制作快照、商店素材、统一按钮、俱乐部说明、主线选关、连击收尾、结算页制作快照及每日挑战拆分提交；共用存档、生命周期和资源注册随每日挑战集成提交，公共规范、验收文档及主题汇总记录另行提交。
- 奶油卷毛犬和独立结算页使用wip提交描述，不宣称功能或资源已完成。整理期间观察到奶油卷毛犬文件继续生成；首次快照之后的新增及变化保留在工作区，不追逐并发写入内容。
- 本次检查Git差异、提交范围、单文件大小及远端分支，未重新运行Unity编译、Play Mode、Addressables打包、应用构建或真机验证；历史开发验证以各批记录为准。空白检查发现Unity生成meta的空字段行尾空格，未为提交批量改写序列化文件。推送及远端一致性结果以本次聊天交付为准。
## 2026-10-28 奶油卷毛犬12头像与8前爪候选

- 用户确认雪纳瑞整套可用，并要求给2号奶油卷毛犬也制作一套。同步记录雪纳瑞12头像与8前爪美术确认，保留既有蓝耳小狗、雪纳瑞及三种基础头像。按相同范围完成奶油卷毛犬12款头像（17号逐像素复用已选基础图、11张新表情）及8张前爪。
- 使用内置image_gen逐张制作，最后一张前爪输出被工具拦截，普通卡通前爪提取请求重试后成功；完整提示词及重试说明均保存。候选位于Docs/UILibrary/themes/PuppyBakery/assets/UI/Profile/CreamCurly/v01；底稿、提示词、脚本、来源基准、头像与前爪总览及verification.json位于previews/Profile_CreamCurly_v01。
- Windows离线检查20张原文件名、画布尺寸、RGBA、非空内容和透明通道范围0至255通过，基础图逐像素复用检查通过，人工查看12头像和8前爪总览。图片制作完成时，正式Profile与既有各套图片的基准哈希未变；其后仅更新雪纳瑞确认和选型manifest元数据，不改变旧图片。
- 本批候选待用户审图，未安装正式运行资源，未修改代码、Prefab、SO、Addressables或存档。前爪为AI分离后按原画布等比适配，服装遮挡、Sprite轴心与精确叠放仍待验证。未执行Unity编译、Play Mode、内容打包、应用构建、安装或真机验证。


## 2026-10-28 个人资料页签遮挡修复

- 用户截图反馈未选中页签盖在内容背景上。检查Panel_Profile正式Prefab发现Tab0／Grid0、Tab1／Grid1、Tab2／Grid2交错排列，当前激活Grid之前的页签和之后的页签绘制顺序不一致。仅调整contentRoot的6个既有子节点顺序，保留全部对象、GUID、尺寸和引用。
- Panel_Profile新增局部视觉方法RefreshTabLayering，由Panel_Profile_Controller每次刷新时调用：未选中的两个页签位于全部Grid之前，选中的页签位于Grid之后，提示和弹窗仍在更上层。ProfileCompletionEM沿用同一方法保存默认头像页签层级，不修改权威业务状态、存档或资源管理。
- Windows／Unity 6000.5.2f1，隔离目录D:/UnityProject/HamdokuProfileInfoValidation-20261028中编译与正式Prefab加载通过；12次重复选择验证实际Transform层级、全部未选中页签后置、选中页签前置、提示与弹窗上层、按钮Graphic和interactable状态通过。正式Prefab差异仅3行移除与3行添加，全部子引用保留，git diff --check通过。脚本、顺序核验及Unity日志位于Logs/ProfileTabLayering。
- 未进行本次Play Mode、实际渲染、人工编辑器点击、Addressables重打包、应用构建、安装和真机验证；未访问正式存档。迁移验收文档A4-B5补充截图问题、修复与待用户复验步骤。保留用户已有3份Home按钮meta修改。

## 2026-10-28 主线通关／失败结果页与本局成绩迁移

- 用户确认后，只读查找原ResultPopup场景层级和配置，并核对ResultScreen的Build、BuildClear、BuildFail、MakeStatRow、MakeHero、RevealClearStages及ResultLayoutConfig／ClearStats。原基础Popup不代表动态完整结果页；恢复代码含诊断，结合索引、诊断、ARM64和静态配置采用可核实部分。源配置、方法索引、诊断、SHA256及实施前文件快照保存在Logs/ResultPageMigration/，没有向原项目写入。
- 新增Panel_GameResult、Panel_GameResult_Controller、ResultConfigSO及ResultSetupEM，正式Prefab位于Assets/Res_Runtime/UI/GameResult/，clear.png／fail.png从原项目Sprite逐字节导入。GameConfig引用正式ResultConfig，原Canvas、PanelFactory、PanelRepository及default标签管理继续使用；没有新增Context依赖、启动流程、接口或并行UI系统。新增9份Result_* L10NSO（1000—1008），烘焙缺失中文字到现有静态字体，保留当前语言字体映射及既有字符，不增加Fallback。
- GameController仅将主线通关／失败接到新页面；退出／手动重开确认仍为Panel_GameDialog，每日挑战保留其原结果路径。成功页显示实际得分、最高连击、冻结用时，按原配置0.3／1.15／1.9秒分阶段显示、0.7秒统计递增和0.25秒淡入；按钮显示完成前阻止操作。失败页显示剩余仓鼠、重开、返回与明确未接入的广告入口，重复点击不加生命、不发奖励。第20关只显示一个返回主按钮，继续原连胜和主线边界衔接。
- 存档结构未改，沿用其他批次现有v10和正式hamdoku-progress.json；已有score、bestCombo、elapsed作为显示来源。承接旧分且无法确定完整连击历史时显示“—”并说明，未推算历史。关闭、恢复、重复打开、下一关及失败重开均沿用Controller管理和释放。失败插画纵坐标、失败按钮时机与返回入口为本工程适配，使用现有背景和已安装统一按钮，不把未复刻的光效／纸屑／分档插画记为完整原作效果。
- 本轮发现原连击飘字淡出后仍留原文案，语言切换会在新语言字体中触发缺字。Panel_Game增加ClearFeedback及过期文字清理，HomeController在切换字体前清理视觉内容；不清零业务连击或分数。首次专项验证因该问题失败，修复后重新检查中英文及缺字警告通过。失败数量英文改为Hamsters left以覆盖单复数，成功说明避免与标题重复。
- Windows Unity 6000.5.2f1、D3D11，复用隔离工程D:/UnityProject/HamdokuCampaign20Validation-20261028。编译通过，最终45项结果页检查与158项20关回归断言通过（203项检查／断言，不代表203个独立场景），覆盖渐显前禁点、5个GraphicRaycaster命中、实际成绩、失败广告不恢复、失败重开、返回、旧v8承接统计、v1—v8兼容回归、结果磁盘恢复、不重复通关、连胜、每日挑战和第20关边界。运行错误及最终缺字警告为0；UnityEditor.Search索引异常单独记录editor-diagnostics.txt。验证分别使用result-validation-only.json与result-campaign-validation-only.json，没有把测试通关数据同步回正式项目。
- 已查看1080×1920中文／英文、1080×2340模拟安全区英文、旧局未知统计及第20关边界截图；1080×1600短屏后台图与字体字符亦检查。结果图、测试脚本、初次失败与后续日志均保留；程序化PointerClick／Raycaster和后台渲染不代替人工编辑器点击、手机触摸或实际扬声器验收。
- Windows Addressables内容打包通过；正式HomeConfig保存v10与正式文件名。74份交付／依赖文件逐项核对通过，其中73份逐字节一致，AddressableAssetSettings组列表仅序列化顺序不同，按引用内容核对一致；包含新Prefab及SO、图片与meta、文案、字体、代码与程序集、既有按钮／背景依赖、DefaultObject、EditorBuildSettings及包版本。共享资源组只合并本批10个入口，保留每日挑战、商店、按钮、头像等其他批次内容；同步字体前检查当前字符集合未丢失。
- Docs/MigrationAcceptance.md追加本批13项用户验收、兼容策略、已知边界与验证状态，未代勾选；Docs/GameplayMigrationTODO.md补充本批与剩余范围，AGENTS.md现有逐批验收规则和入口保留有效。未执行Android APK构建、安装、真机、原APK动态对照；百分位／超过多少玩家、关卡奖励库存、真实广告及第21关以后不在本批完成范围。


## 2026-10-28 个人资料头像正式替换：蓝耳小狗与雪纳瑞

- 用户确认普通头像用蓝耳小狗、会员头像用雪纳瑞，只替换头像。安装此前已确认的普通头像16张（ID 0—5、7—16）、雪纳瑞12张（ID 17—28）及对应20张前爪图层；前爪属于头像的服装遮挡叠层，不改动服装图片。隐藏ID 6保持原状，奶油卷毛犬及未采用的蓝耳会员候选保留。
- 正式目标Assets/Res_Runtime/UI/Profile/，48张PNG按现有路径覆盖，48份meta、GUID、导入设置逐字节保留；头像框、服装、Prefab、脚本、SO、场景、存档及解锁规则未在本批修改。保留工作区已有个人资料页面等其他改动。
- Windows／Python／Pillow／SHA256文件检查通过：确认稿哈希、原画布尺寸、RGBA、透明通道、28个头像配置槽位、meta／GUID与备份完整；158份其他Profile文件及ProfileConfig保持安装前内容。首轮脚本预检发现清单将前爪单独存于hands键，尚未写入正式资源；修正读取后安装检查通过。脚本、原图与meta备份、逐项结果见Logs/ProfileAvatarInstall/。更新两套清单及主题汇总，迁移验收追加本批。
- 尚未执行Unity实际导入验收、编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证；头像缩放、边框裁剪及前爪／服装精确叠放待运行检查。文件替换通过不代表整页视觉验收通过，用户验收未代勾选。

## 2026-10-28 个人资料头像尺寸与选中样式修复

- 根据用户提供的原作与当前页面截图，定点修复头像网格。只读查阅原作恢复边界、ProfileGridView.MakeCell及ProfileConfig；MakeCell索引为recovered_with_diagnostics、43条诊断，动态UI的等比图片与圆角结构可辅助理解，精确视觉取用户截图，不把恢复零值当作尺寸或运行依据。
- 现有头像格使用176尺寸Portrait、0.8脸部缩放并重复显示底板。28个头像格改为复用Art以220×220等比显示并隐藏冗余Portrait；Controller刷新时绑定对应头像及可用性透明度，保留资料卡、边框和服装组合逻辑。54个选项引用新增几何UI资源profile_selection.png，修正为圆角橙色描边，保留勾选与交互。ProfileCompletionEM同步制作设置，正式Panel_Profile.prefab已保存，保留其他既有工作区修改。
- Windows／Unity 6000.5.2f1隔离目录D:/UnityProject/HamdokuProfileInfoValidation-20261028中编译与导入通过，54项绑定、点击委托身份、选中／锁定显示及TearDown监听清理检查通过，28个头像尺寸及单层显示检查通过。查看1080×820后台头像网格渲染，头像占比接近参考，圆角框与右下角勾选正常，无明显裁切。脚本、Prefab修复前备份、修改清单、截图及Unity日志保存在Logs/ProfileVisualCorrection/。授权客户端启动握手／令牌日志未阻止验证，最终退出码0；未访问正式存档。
- 头像及前爪源图、存档与解锁规则未修改。Docs/MigrationAcceptance.md在A15-B5追加用户问题、修复与待复验步骤。尚未做本次Play Mode人工点击、整页多比例渲染、内容打包、应用构建、安装或真机验证；未宣称原作按下／松开动画一致，未代勾选用户验收。

## 2026-10-28 个人资料选中页签同样后置

- 用户明确选中项也应显示在内容背景后方，撤掉首轮选中页签前置处理。Panel_Profile.RefreshTabLayering改为无参数，统一将三个页签排在全部Grid之前；同步Controller、ProfileCompletionEM及正式Panel_Profile.prefab既有节点顺序，保留样式切换和按钮回调。
- Windows／Unity 6000.5.2f1隔离工程HamdokuProfileInfoValidation-20261028编译与Prefab加载通过，12次重复选择检查全部页签（含选中项）均被内容区压住、提示／弹窗前置和按钮可交互状态通过，退出码0。本次修改范围git diff --check通过；未改动其他正在安装的头像、前爪及主题清单。
- 验证证据位于Logs/ProfileTabLayering/Unity-all-behind.log和result-all-behind.txt；MigrationAcceptance的A4-B5补充更正及待用户复验步骤，首轮记录保留。本次仅Edit Mode检查，未进行Play Mode、实际画面渲染、人工点击、Addressables内容打包、应用构建、安装或真机验收。


## 2026-10-28 个人资料服饰替换：首批六套小狗服装

- 用户要求个人资料服饰使用此前生成的前几套，其余暂不接入。沿用现有六个槽位ID 0—5，依次安装小面包师、可颂兜帽、草莓奶油帽、蓝白睡帽、南瓜帽、格纹头巾；独立服饰来自Costumes/v01，原候选和07—22全部保留。六张正式PNG保留原路径及meta／GUID，适配为透明正方形画布，避免既有preserveAspect按不同原图比例改变叠放。
- ProfileConfig的174项穿戴参数改为适合新图的无偏移、无旋转、单位缩放，ProfileCompletionEM同步制作设置；仅调整本批服饰参数，保留工作区已有页签等修改。六份Profile_Costume文案同步中英文名称，现有Chinese SDF离线补字符并保留全部既有字符、静态图集与meta，不增加语言或字体Fallback。每日奖励与资料共用该配置，因此预览图片及名称同步变化；发奖、解锁、价格、赛季、存档编号和已选状态不变。
- Windows／Unity 6000.5.2f1，隔离副本D:/UnityProject/HamdokuProfileInfoValidation-20261028编译及正式Prefab导入通过；遍历29头像×6服饰共174项绑定通过。人工查看Unity实际渲染的蓝耳小狗与雪纳瑞六套叠放图，六款名称可显示；开发渲染与文件检查不替代用户整页验收。图片、meta、字体字符超集及同步哈希检查通过，原图和原配置备份见Logs/ProfileCostumeInstall/before，脚本、渲染及Unity日志保留。
- 初轮渲染测试脚本释放active RenderTexture产生清理警告，修正释放顺序后复验；隔离导入发现既有Shop/new与Streak/new副本GUID冲突，由隔离Unity重分配，不将这些无关meta复制回正式工程。启动授权握手日志与最终成功退出分别保留。未执行本批Play Mode、完整页面人工点击、Addressables内容打包、应用构建、安装或真机验证；未访问正式存档。
- 复验启动记录：Unity-final.log一次在打开工程阶段以退出码1结束，未输出项目方法诊断；随后Unity-retry.log重新编译与执行成功，退出码0，未再出现RenderTexture清理警告或缺字警告。最终补入8个中文字，保留已有字体字符；按CRLF规则的本批git diff --check通过。


## 2026-10-28 商店新手礼包与特别优惠插画候选

- 用户确认先生成红框内两张礼包图审图，通过后再替换正式资源。以已确认蓝耳小狗头像为身份和画风参考，分别使用内置image_gen生成珊瑚粉新手礼包与金色特别优惠礼包；小狗替换仓鼠，保留放大镜、灯泡与去广告图示。
- 两张透明PNG保存在Docs/UILibrary/themes/PuppyBakery/assets/UI/Shop/Offers/v01/，分别为384×384与384×376；仅对AI底稿执行透明边距裁切、等比缩小和居中。底稿、完整提示词、制作脚本、总览和检查记录位于previews/Shop_Offers_v01/，STYLE.md新增A19入口。
- Windows离线检查尺寸、RGBA、透明通道、四角透明与非空内容通过，人工查看生成图及整理后总览。6份商店正式图片／meta／Prefab／SO基准哈希不变；未替换运行资源，未执行Unity导入、编译、Play Mode、Addressables内容打包、应用构建、安装或真机验证。候选美术待用户确认。

## 2026-10-28 商店两张礼包插画安装

- 用户确认候选美术可用并要求替换。将PuppyBakery/assets/UI/Shop/Offers/v01中的offer_art_starter_pack.png与offer_art_special_offer.png原样复制至Assets/Res_Runtime/UI/Shop/new；分别384×384和384×376，均为RGBA透明PNG。
- 沿用原路径及meta／GUID，保留单Sprite导入和Panel_Shop的21300000引用；未调整布局、脚本、商品配置或存档。旧图与meta备份于Logs/ShopOfferInstall/before，old目录不变。更新候选manifest安装状态、STYLE A19及MigrationAcceptance A17，用户运行验收未代勾选。
- Windows／Pillow检查正式图与确认候选哈希一致、尺寸／透明通道正确、两份meta不变，70份受保护资源／meta／Prefab／SO保持。证据见Logs/ShopOfferInstall/verification.json。本批未执行Unity实际导入验收、编译、Play Mode、Addressables打包、应用构建、安装或真机验证，未访问正式存档。


## 2026-10-28 UI参考库增加小狗面包房原图对比

- 用户确认后更新Docs/UILibrary/UI Library.html及同名CSS、JS；新增导航与14类234组并排对比，覆盖页面效果、Logo、设置、棋盘、商店、通用按钮、头像／前爪、犬种候选、边框和全部22套服饰。支持分类、名称／文件名筛选、空结果提示及点击放大。保留原有组件演示和工作区其他修改。
- 优先采用设置v02＋边框v03、Buttons/v01及商店当前选用按钮；原作整页、单项素材、离线重组和接入状态分别说明，无可靠对应的原图明确显示缺口。9个改名按钮补以原图RGBA像素一致性匹配，服饰前六套按已确认槽位对应，其余候选不伪造原槽位。未修改素材图、主题清单或Unity资源。
- Windows／Chrome后台浏览器file://验证通过：440张图无破损，669个链接文件存在，1440／768／375宽度无横向溢出，分类、搜索、空结果与新窗口原图正常，无脚本错误或资源失败；查看桌面与移动宽度截图。证据Logs/UILibraryComparison/，同步README.md、Verification.md。未执行Unity编译、Play Mode、内容打包、应用构建、安装、真机或用户验收；网页对比不代表运行效果一致。


## 2026-10-28 棋盘角色动画与连击庆祝迁移

- 用户确认迁移范围后，核查原作HamsterAnimator、CellView、GameController、ComboTextFx、ComboCelebrateFx、方法索引／恢复诊断、原Cell层级与HamsterAnimator配置。将10张角色帧及10张clap／cheer／dance／rainbow／sprout／sunflower庆祝图片独立接入Assets/Res_Runtime/UI/Game/Animation，保留资料页小狗头像及其他并行修改。原作资源目录只读，来源与SHA256见Logs/BoardAnimation/resource-manifest.json。
- Widget_BoardCell接入8帧待机、出现／放置、呼吸浮动、错误姿态和通关三次跳跃；Widget_Board和Panel_Game_Controller沿用现有Controller触发／UI播放分工。Panel_Game沿用原连击浮字，接入3次起文字弹出与星光／落点庆祝、8次起彩屑、12档封顶及失误／重开／释放清理；新增GameAnimationSetupEM用于维护正式绑定，正式场景无需先执行菜单。参数保存在GameConfigSO，结果延迟1.65秒避免截断庆祝，不改变计分／存档结构。
- 原作帧绑定、基础角色时序和连击门槛有静态证据；连击配色／曲线／彩屑轨迹和按次数轮换六类庆祝属于当前工程实现，未宣称原APK逐帧还原。正式Panel_Game原有节点未删除，新增37个非交互图片；保留现有层级布局，差异记录见prefab-semantic-diff.json。
- Unity 6000.5.2f1隔离工程首轮编译、资源导入及正式Scene_Main的Play Mode验证通过：真实教程进入主线、放置／待机换帧与浮动、错误反馈结束、关闭恢复、重开、真实3连击、完整庆祝后弹出结算。7—13档为合成表现预览，独立注明；查看中文／英文、1080×1920、1080×2340安全区及1080×1600后台截图。初轮验证脚本入口和庆祝阶段等待假设错误，修正后通过，日志保留；Unity编辑器Search索引异常和外部TLS日志另记，不冒充项目异常或隐藏。
- 最终彩屑发射位置分散到文字上方、关闭纸片图片等比约束；1—13档及8×8棋盘三种竖屏合成预览、门槛／到期清理和37处Raycast关闭检查通过。Windows Addressables内容打包通过（84.14秒），本批60项交付／依赖与隔离副本逐字节一致，20张帧与原作来源SHA256一致。MigrationAcceptance追加本批操作和未勾选用户验收。未执行Android应用构建、安装、真机触摸或原APK动态对照。
- 并行结算任务接入期间，新增rewardHero配置未就绪曾导致本轮结算入口报错，保留Unity-play-final.log及play-final-parallel-result-failure.txt；并行任务补齐正式资源后，重新同步，Unity-play-current-retry.log／play-result.txt完整Play Mode复验通过。短时放大阶段因后台帧间隔被跳过，验证脚本改为显式视觉时刻探测并恢复原时钟，真实结算等待流程继续验证，不改业务逻辑。 最终完整资源Windows Addressables内容打包再次通过（Unity-content-delivery.log，88.03秒、退出码0）。


## 2026-10-28 商店暖棕柔和投影

- 按用户确认的原作效果，为商店五张主卡、商品／权益卡、购买与恢复按钮及四张主要插画添加51个装饰阴影Image。只读核对原ui_soft_shadow、ShopPopupLayout参数、恢复方法／诊断及ISIL，原柔边图160×160、九宫格60边距原样复制；暖棕颜色和偏移／扩散取静态配置，六层轮廓投影归一化累计透明度属于本工程实现选择。
- 新增Assets/Src_Editor/EM/ShopShadowEM.cs，既有ShopConfigSO及正式SO增加四组阴影参数；ShopSetupEM保留正式制作逻辑并同步阴影导入与应用。正式Panel_Shop定点加入五个原尺寸布局容器及阴影，全部关闭Raycast Target；保留六个滚动槽位、11商品绑定、价格、图片、旧组件引用和玩家进度。正式打开Scene_Main运行无需先执行菜单。
- Windows／Unity 6000.5.2f1隔离副本编译／导入通过；51层排序、非交互属性、商品绑定、滚动槽、重复制作／Init通过，人工查看三种竖屏比例的上中下位置Unity后台渲染。首轮重复制作排序检查失败，修复同级已有节点移动时的索引偏移后复验退出码0；失败与复验分别保留Unity.log和Unity-retry.log。
- 同步核对正式Prefab哈希与验证副本一致，642个原对象保持；原组件仅两处1.77777778到1.7777778的Unity浮点格式变化，原SO配置前缀完整、确认礼包图片哈希不变，Prefab GUID及Addressables既有登记保持。旧Prefab与SO、来源与验证记录、对照图见Logs/ShopShadow；MigrationAcceptance追加本批用户验收。
- 本批未执行Play Mode交互、Addressables内容打包、应用构建、安装或真机验证，未访问正式存档，未同步Library或验证脚本至正式Assets。后台渲染与装饰层检查不代替用户点击验收。

- 编辑器诊断另记Logs/ShopShadow/editor-diagnostics.txt：启动时旧授权客户端握手／令牌提示后回退6000.5.2客户端并成功解析授权，最终验证成功退出码0；与已修复的首轮阴影排序断言分开记录。


## 2026-10-28 主线第21—50关与阶段奖励迁移

- 用户确认后建立Docs/Campaign50MigrationTODO.md，并在AGENTS增加入口。只读核对原作DynamicDifficulty／CampaignMapBank／CampaignDifficulty／LevelRewardService／ResultScreen／RankRewardPopup、索引诊断、ARM64／ISIL与正式配置；全部10498条DDA二进制记录交叉检查。增量加入2923张地图子资产，主线共6359张；既有地图ID、GUID、身份映射、池内顺序和游标保持。30／40／50固定题grade4／3／4优先覆盖普通池。
- GameConfigSO／正式GameConfig扩展至50关及阶段奖励参数；PuzzleModule扩展地图、配置与存档校验；CampaignController接入技能3抽样、首局抽样后减级、连续重试与延后降级历史。CampaignSaveModel新增领取标记和难度历史，SaveModule接入v11及旧版升级。保留旧棋盘、每日、资料和连胜数据；旧首页进度不追发历史奖励，旧已通关里程碑结果保留本次待领。
- GameController、Panel_GameResult及其Controller、ResultConfigSO、ResultSetupEM复用现有结果Prefab承接进度与领奖。每10关基础奖励1灯泡、0放大镜，点击领取后与标记一起原子保存；失败回滚内存余额／标记。广告翻倍明确未接入。新增6份双语L10NSO、正式字库烘焙和Addressables登记；定点修复原结果两个按钮失效GUID，不重建页面或新增资源管理架构。
- Windows／Unity6000.5.2f1／Addressables2.10.3隔离工程编译及正式场景Play Mode，355项程序化断言通过，包含教程、1—50关、固定题、失败重开、续局恢复、选图与难度、领奖防重、中断和写盘失败回滚、v1—v10升级、v11恢复。人工查看中英文1080×1920与1080×2400模拟安全区后台截图；Windows内容打包及正式存档配置复打包通过。54份本批文件／meta哈希与验证副本一致，无资产GUID冲突，相关引用、DefaultObject注册和正式存档名通过核对。
- 首轮截图发现按钮白块后修复并复验。复用旧隔离目录存在迁移前资源残留引发GUID冲突，仅删除隔离副本独有文件并恢复源meta后通过；源工程资源未清理。UnityEditor.Search自身索引异常单列。证据、首轮和复验日志、制作／审计脚本、截图、result.txt、content-result.txt及delivery-audit.json在Logs/Campaign50Migration/。未复制测试脚本、存档、Library或验证存档配置回正式工程，保留同期商店和棋盘等其他修改。
- 黄金挑战已确认使用FirebaseCallables会话／云函数及排行榜上报关联，存在恢复诊断，仅完成依赖分析。第51关地图变换、黄金挑战玩法、无限模式及真实服务留待后续批次；未做原APK动态对照、APK／应用构建、安装、真机触摸、扬声器试听或性能验收。迁移验收文档追加A20，用户项目未代勾。


## 2026-10-28 商店阴影后续Play Mode与内容打包验证

- 按用户“继续”，复用阴影隔离工程，从正式Scene_Main执行现有商店回归并增加阴影层／GraphicRaycaster命中检查。Play Mode共51项程序化断言通过，覆盖11商品请求、恢复／说明／AUTO、存档不被服务占位修改、重复打开初始化、关闭销毁、游戏商店暂停计时和返回原棋盘／道具／生命／计时、无运行错误或缺字。
- 前两轮测试分别缺少渲染后UI深度、直接标记教程完成导致残留教程棋盘与当前校验冲突；只修改隔离验证准备，使用既有v5完成教程fixture升级，最终通过。失败与最终日志保留于Logs/ShopShadow，检查列表见PlayMode/result.txt，审计见play-verification.json。
- Windows Addressables内容打包通过；验证的是阴影安装时的隔离快照，其他任务同期后续修改不回滚、不覆盖。正式商店7份关键文件与隔离快照哈希一致，未复制验证脚本、测试进度、Library或内容缓存回正式工程；MigrationAcceptance A19-B5追加结果，用户验收不代勾。
- 本轮未修改运行脚本／Prefab／资源，未做应用／APK构建、安装或真机验证。程序化onClick和EventSystem命中不等同人工触摸；内容打包退出时临时分配器清理提示与授权／Search诊断单独记录，不隐瞒或混作项目运行错误。

## 2026-10-28 主线无难度历史存档启动校验修复

- 用户反馈启动时报Campaign selection state conflicts with its configuration。只读核对当前正式v11存档：previousLevel=0、previousRecordedSkill=0、previousRetried=false，没有上一条难度历史；原校验无条件要求previousRecordedSkill至少1，误拒绝未建立历史的状态。未推断该存档的具体生成过程。
- 定点修改Assets/Src_Runtime/Module/Puzzle/PuzzleModule.cs：仅在无上一关记录且无重试记录时允许技能未设值0；有历史时仍要求有效技能，负值、超上限、奖励越界及游标校验保持。没有迁移或重置进度，没有改动配置、资源、Controller职责和其他已有修改。
- Windows／Unity6000.5.2f1隔离副本编译和Scene_Main的Play Mode通过，38项程序化断言包括实际存档完整校验、无历史／正常历史、六类非法数据、启动Ready、第2关原题／25格／分数／生命／道具恢复、保存再加载及重复初始化。使用单独测试存档名，正式存档读取前后SHA256一致；验证期间工作区另有配置拆分修改，正式脚本整体哈希因此不同；已核对本次修复条件仍完整保留。本轮编译与Play Mode验证针对修改前配置结构的隔离快照，不覆盖并行配置拆分后的完整工程。证据Logs/CampaignSelectionFix/。
- 同步更新MigrationAcceptance A20-B5用户反馈与复验步骤，用户验收未代勾。本次未重做Addressables打包、应用／APK构建、安装或真机验收；隔离运行不代表用户当前编辑器已重新进入Play Mode。


## 2026-10-28 按麻将参考项目整理独立地图SO与配置职责

- 用户确认完整方案后实施。保存当时运行／编辑器代码、配置、Addressables与ProjectSettings基线，不覆盖同期个人资料、商店和棋盘改动。将主线6359、每日1820、教程1共8180张地图整理到Assets/Res_Runtime/Puzzle的Campaign/Grade1、Grade2、Grade3、Fixed、Daily、Tutorial；1967份既有独立SO保留GUID，6213个GameConfig子资产提取成独立SO，地图ID、区域／答案、等级、固定关与身份字段逐项保持。
- 新增PuzzleRulesSO、CampaignConfigSO、PuzzlePresentationSO与正式资产；规则、主线选图／奖励、视觉表现分开，ResultConfig独立加载。GameConfig只维护默认语言、存档版本及文件名，原HomeConfig通用字段移入并删除旧字段；LogoConfig与启动Controller改绑同一GameConfig，存档版本仍为11。DailyConfig删除地图引用数组，日期表先按原数组解析再存整数地图ID，赛季配置保持。
- AssetModule沿用default标签，增加地图ID字典／有序列表和独立配置查询，重复ID与配置抛错；PuzzleModule继续负责校验与选图能力，Controller读取相应SO并修改状态，Panel／Widget仅接收表现配置与必要数据。同步更新Game／Home／Daily／Campaign／Profile／结果页Controller、SaveModule、Logo读取、棋盘Widget和配置制作工具；新增PuzzleConfigSetupEM用于正式资源登记，不新增启动／Context／UI管理架构。
- Windows／Unity6000.5.2f1／Addressables2.10.3，隔离工程编译、8180独立地图注册及正式场景Play Mode通过；覆盖教程、1—50关、固定题、难度、失败重开、同题恢复、阶段奖励防重／写盘失败回滚、v1—v10兼容和v11恢复、每日进入／续局／读盘恢复／通关返回、资料和商店。共18597项断言，大部分为逐地图及日期映射检查，不表示同等数量独立业务场景。查看中英文与两种竖屏安全区后台截图，Windows Addressables内容打包通过。
- 交付审计证明地图业务内容、拆分字段值和每日日期映射与基线一致，1967个既有GUID保持，无GUID冲突和旧子资产引用。运行／编辑器源码、地图与正式配置和验证副本一致；同步Gameplay组及设置，DefaultObject／EditorBuildSettings及正式hamdoku-progress.json文件名已核对。未复制测试脚本、存档、Library或隔离测试文件名。验收文档追加A21，既有迁移TODO追加本批完成记录。
- 初次提取遇到教程缺少普通地图grade字段，已按独立教程类别修正后从原基线恢复执行；一次HomeConfig写入与编辑器导入冲突通过限次重试完成。完整基线、映射、逐字段审计、制作／验证脚本、编译／Play／内容日志与截图在Logs/ConfigRestructure/，UnityEditor.Search索引异常单列。未做APK／应用构建、安装、真机触摸、扬声器试听、性能或原APK动态验收；开发通过不代替用户验收，未扩展50关以外内容或服务。


## 2026-10-28 棋盘12种图案与新配色候选制作

- 用户确认烘焙主题12种图案及12色方案后，使用内置image_gen逐张制作12张独立透明剪影；打蛋器首稿在原窄画布中偏小，重新生成为竖向粗线条版本。候选保存至Docs/UILibrary/themes/PuppyBakery/assets/UI/Game/Patterns/v01，沿用12张正式文件名与原画布尺寸，白色图案用于运行染色。
- 新配色、索引、原作资源来源、原正式图片及meta哈希、生成来源与完整提示词均已记录。图案／配色总览、棋盘离线搭配预览、底稿及整理脚本位于previews/Game_Patterns_v01；更新既有STYLE与主题manifest入口，不替换正式图片或配置。
- Windows／Pillow检查12张RGBA、原尺寸、非空alpha、透明范围0至255及四角透明通过；人工查看总览与棋盘预览并复查打蛋器调整。正式12张图案及meta、PuzzlePresentation.asset共25份SHA256保持。棋盘示意不是可解关卡或Unity运行截图；较深审图染色与现有运行淡纹样参数在预览中明确区分。
- 交付状态为美术候选待用户确认。未执行Unity导入验收、编译、Play Mode、Addressables打包、应用／APK构建、安装或真机验证；未访问存档。当前运行展示配置只有前10种绑定，12种素材交付不代表运行全覆盖；接入时再核对绑定、教程颜色提示、图案开关和实际辨识度，并维护迁移验收批次。


## 2026-10-28 区域图案正式接入及网页对比

- 用户追加要求网页对比并明确“接入”。12张烘焙图案复制到Assets/Res_Runtime/UI/Game，保留原meta／GUID、原画布尺寸及Sprite导入设置；PuzzlePresentation.asset仅更新regionColors与patterns，均完整12项。教程配色、教程文案、默认图案开关、规则、关卡、脚本、Prefab与存档格式保持。旧PNG／meta／SO备份与安装哈希见Logs/PatternInstall/before、installation.json。
- UI Library新增独立“区域图案 · 新配色”分类，将原棋盘分类中的12项原地对应更新并移动，棋盘分类剩22项；新增导航、逐项原作／新图案底色与色值、总览与离线棋盘链接。网页标注正式安装和隔离运行状态，制作阶段预览图保留历史用途。同步主题manifest、STYLE、网页Verification及MigrationAcceptance A22，用户验收未代勾。
- Windows／Chrome file://检查12组、24张图片、39个本地链接，筛选、名称搜索、无结果及原尺寸弹窗通过；1440、768、375宽度无横向溢出，已查看桌面／窄屏截图。证据Logs/PatternInstall/web-verification.json与web-*.png。
- Windows／Unity6000.5.2f1／Addressables2.10.3隔离副本完成资源导入、编译与Scene_Main Play Mode，256项断言通过：12Sprite导入及绑定顺序、default加载展示SO、自动教程原配色与无纹样、教程完成到主线、新区域颜色、图案开关、设置及棋盘重开、全部12种纹样在实际Widget上的绑定。1080×1920真实Canvas后台渲染人工查看，证据unity-result.txt、unity-game.png及unity.log；不是手机或编辑器鼠标验收。
- 首轮验证误将自动教程当主线比较颜色失败，调整验证流程后通过，首轮结果保留。batchmode ScreenCapture未产出PNG，改用RenderTexture后台渲染并确认生成。UnityEditor.Search索引异常单列editor-diagnostics.txt；本批项目验证无未解决错误。隔离测试使用随机唯一存档名，未访问正式存档。
- 25份正式图案PNG／meta／展示SO与隔离快照一致，运行脚本一致；验证期间并行新增／修改6份PuzzleImport编辑器工具，未覆盖其修改，其编译不在本次快照覆盖范围，详见synchronization.json。没有同步Library、验证脚本、临时配置或测试进度。未执行Addressables内容打包、应用／APK构建、安装或真机验收。

## 2026-10-28 原作地图全量导入工具及12319份独立地图

- 用户明确确认预览／执行／验证、全量10498条DDA、教程／每日增量、旧50000条数据库核对与GUID／资源登记维护后实施。新增Assets/Src_Editor/EM/PuzzleImportEM、Const及六份单类型Model，沿用PuzzleSO、CampaignConfig／DailyConfig／PuzzleRules及PuzzleConfigSetupEM登记，不新增运行架构或加载入口。
- 菜单Hamdoku → Puzzle Import提供1 Preview Migration、2 Execute Migration、3 Validate Migration。直接读取原作HDDA二进制与教程／每日JSON，逐条校验区域／答案／固定关／重复ID，按当前配置维护活动池；50000条旧CampaignDatabase校验后不生成逐关重复SO。来源SHA256、差异及执行回执写Logs/PuzzleImport，执行前备份地图／配置／Addressables；源文件在规划后变化会停止执行。
- 正式地图共12319份：原作DDA10498、每日1820、教程1；新增4139份资产及meta和3份目录meta，共8281份新文件，原有8180份地图与meta及目录文件共16367份哈希保持。同步AddressableAssetSettings.asset与Gameplay.asset两份必要登记；未回退并行接入的新图案／配色，不同步测试配置、存档、Library或验证脚本。
- 增量执行保留既有GUID、相同内容不重写，注册标签缺失单独修复；预览只写Logs。重复ID、源缺失、非法答案、目标占用、未保存地图／正式配置／登记编辑明确报错阻止写入。首轮在64条fixedLevel=-1原作记录处拦截且未写入，核对二进制后修正为原样保留标记；首轮日志保留tool-first.log／result-first.txt。
- Windows／Unity6000.5.2f1／Addressables2.10.3隔离副本完成编译与真实4139份批量创建、预览字节不变、异常不写入、重复执行零新增／零更新、旧资产与GUID保持、来源哈希变化、登记修复、脏编辑保护及12319份最终验证。Scene_Main Play Mode通过26875项检查，涵盖全部资源注册与索引、教程和1—50关、每日、阶段奖励／防重、旧存档及中途恢复、资料／商店相邻接入；专用测试存档未访问正式进度。中英文奖励后台渲染人工查看，UnityEditor.Search索引异常单列，未发现未解决的项目运行错误。
- Windows Addressables内容打包通过；正式交付审计核对12319唯一ID／GUID与default标签／地址、原地图和四份生产配置哈希不变、八份工具源码与meta和验证副本一致、DefaultObject／EditorBuildSettings及四份原作来源哈希。主线仍50关、活动池6359张、存档v11及hamdoku-progress.json；未实现第51关以后流程、无限模式或外部服务。证据Logs/PuzzleImportValidation/，验收文档追加A23，既有Campaign50MigrationTODO追加A6；用户验收未代勾。
- 内容构建快照先于另一批次的新图案接入，显示验证按A22独立记录，本批未覆盖或回退其资源。未做应用／APK构建、安装、手机触摸、真机性能、扬声器试听或原APK动态对照；后台渲染与程序化检查不代替用户实际验收。


## 2026-10-28

### 空格临时解锁测试

- 在 `ClientMain` 接入空格键，存档初始化后通过 Controller 临时开放首页模式及资料页已有头像、头像框和服装；文本输入时不触发。
- 测试解锁不修改通关数或持久化解锁状态；测试外观选择保存时保留正常可用的外观，重启恢复正常条件。相关文件：`ClientMain.cs`、`HomeController.cs`、`ProfileController.cs`、`Panel_Profile_Controller.cs`、`HomeState.cs`、`ProfileState.cs`。
- 已静态检查输入和保存调用路径；正式工程已被 Unity 6000.5.2f1 打开，未执行独立 Unity 编译、Play Mode、内容打包、应用构建或真机验证。每日拼图与奖励进度及未接入服务未由此快捷键授予。


## 2026-10-28

### F 键直接通关

- `ClientMain` 在非文本输入状态下将 F 键交给 `GameController`；仅在正在进行且没有弹窗或设置页遮挡的棋盘中生效。填入关卡答案后复用正常胜利、结算与存档流程，适用于教程、主线及每日棋盘。主线与每日进度会真实保存。
- 静态检查了胜利及存档调用路径；未执行 Unity 编译、Play Mode、构建或真机验证。

## 2026-10-28 第51—100关只读迁移分析

- 用户要求继续下一步分析。按原作README边界查阅DynamicDifficulty、CampaignMapBank、CampaignDifficulty与当前CampaignController／PuzzleModule／存档／棋盘渲染入口，核对方法索引与诊断、ARM64和Transform ISIL。未修改原作资料、正式玩法代码、SO、场景或Prefab。
- 静态证据确认51—100关技能／选图上限4，51关起干净通关晋级阈值从2降为1；固定关55／60／62／70／75／80／90／100，固定62为7阶，100为10阶。普通地图51关起按池cursor.cycle模8选择镜像／旋转，池走完一轮再推进变换；固定题独立分支保留原方向，不能用每局随机旋转代替。
- 当前实现仍限制50关及普通grade3，缺少池cycle和局内transform存档，直接扩数组无法保持原规则及续局一致。提出复用现有SO、活动池6359→9610、独立运行棋盘数据、配置后期门槛与变换参数及v12存档兼容方案；100关后无限模式保持现有准备边界，完整无限模式／黄金挑战另批确认。
- 原作二进制与11个方法索引、6份方法诊断及关键native地址记录至Logs/Campaign100Analysis/evidence.json；已有Campaign50MigrationTODO追加A7待确认范围。仅静态分析，未执行本批Unity编译、Play、内容打包、应用／APK构建、安装或真机；未宣称原APK动态核实或迁移完成。

## 2026-10-28 主线第51—100关、地图变换与v12存档

- 用户确认具体范围并要求开始后实施。CampaignConfig扩至100关，活动池6359→9610，保留前50关数组及原池顺序；固定55／60／62／70／75／80／90／100接入，62关7阶、100关10阶。全部12319份地图SO及meta保持，不生成旋转副本或重复资源。
- 新增纯数据PuzzleBoardModel；PuzzleModule按原作镜像列后旋转的0—7变换生成独立区域／答案／预置位置数组，GameState／GameController使用运行棋盘，既有GameFlow与棋盘Panel读取同一数据。CampaignSelectionModel携带本局变换与下一轮次；Controller写入cursorCycles。51关前不变换，51关起使用当前cycle，选中池尾后推进下一轮；固定题不变换，历史避重保持原顺序。
- CampaignConfigSO新增后期晋级起点／门槛及变换起点，51关起一次干净通关即可升至上限4。保留首局减级、道具与失败清零、延期降级、10关阶段奖励、防重复领取和100关完成边界。缩短Game_BatchComplete中英文为“前100关已完成！”／“First 100 levels complete!”，复查后不再换行；不在产品文案中显示迁移实现细节。
- SaveModule、CampaignSaveModel、PuzzleSaveModel升级v12，保存池轮次与本局transform，兼容v1—11；旧轮次补0、旧棋盘保持原方向，保留游标、历史、奖品、资料、每日及局内状态。v12非法／缺失轮次和非法变换明确错误，不静默重置。正式存档名保持hamdoku-progress.json，开发未访问该文件。
- Windows／Unity6000.5.2f1／Addressables2.10.3隔离工程编译和Play通过27296项检查，教程连续推进100关，覆盖难度／固定题／50→51／99→100、提示／失败／续局／奖励防重、v1—11恢复、每日及资料／商店回归。12319×8共98552种组合对照独立坐标公式检查区域、答案、预置位置和数组隔离。
- 补测60项通过：以旧批次实际保留且缺少新字段的v11测试存档验证50关升级，模拟旧50关结算结构继续51无重复奖；8种变换逐一验证提示／标记／分数账本、重开、返回和磁盘恢复。结果页另50项通过，中英文结束文案单行、100关返回、原无限解锁条件和现有准备边界已检查。51关、62关、镜像旋转7和最终中英文竖屏后台截图人工查看；修正前长文案截图保留。各组UnityEditor.Search自身索引异常单列，未发现未解决的项目错误。
- 最终Windows Addressables内容打包及导入工具validate通过，识别100关与9610活动池。正式14份修改／新增文件和验证副本一致，12319份地图及meta／目录共24648份文件哈希保持，原6359张活动池顺序、四份原作来源SHA256、包版本、DefaultObject／EditorBuildSettings和既有登记核对通过。未回同步Library、测试脚本／文件名或玩家进度。完整证据Logs/Campaign100Migration/，分析证据Logs/Campaign100Analysis/。
- 更新既有Campaign50MigrationTODO的后续队列及A7确认状态，新增A8开发完成记录；MigrationAcceptance追加A24，用户验收保持未勾选。完整无限模式、黄金挑战、101关以后与真实服务不在本批；未执行原APK动态对照、应用／APK构建、安装、手机触摸、真机性能或扬声器试听，开发后台验证不代替用户验收。

## 2026-10-28 棋盘仓鼠放大遮挡修复

- 用户反馈仓鼠放大时被相邻格子遮挡。核对Panel_Game的CellsGrid、正式Widget_BoardCell.prefab及角色动画后，调整Widget_BoardCell的兄弟节点顺序：显示仓鼠或误放表情时置前，消失时回到后层，仅在前后层状态变化时调整。保留格内装饰与遮罩关系，未改动画参数、布局、存档或资源引用。
- 棋盘仍以cells数组和坐标处理点击／拖动，不通过节点顺序索引。当前工程dotnet build Hamdoku.csproj --no-restore通过（0警告、0错误）；未执行Unity编辑器编译确认、Play Mode视觉复验、内容打包、应用构建或真机。MigrationAcceptance的A18-B6记录反馈及复验步骤，用户验收未代勾。

### 2026-10-28 通关大图首张风格修订候选
- 用户要求首张举爪庆祝优先修订：犬前爪、星形烤饼干、雾灰蓝软布带、较细浅描边及较小底部装饰。使用内置image_gen，参考已确认蓝耳小狗头像和首轮图生成；其余图暂不按旧风格继续定稿。
- 修订图保存Docs/UILibrary/themes/PuppyBakery/assets/UI/GameResult/v02/clear.png，990×951 RGBA；底稿、提示词及verification.json保存previews/GameResult_v02。红绶带clear初稿保存在previews/GameResult_v01/raw/clear.png，未覆盖。
- Windows离线尺寸、RGBA与alpha范围检查通过；生成边缘存在极低alpha像素，保留未抹除。人工查看生成图。STYLE追加A21，候选待用户确认；未修改正式运行资源，未进行Unity、打包、构建或真机验证。

## 2026-10-28 批量提交与推送整理

- 按用户要求整理当前工程代码、图片、配套Unity资源和既有文档，按工程实现、UI参考库、项目记录分批提交；原项目参考资料ZIP仅保留本地，不纳入提交。
- 提交前核对master与origin/master一致、变更范围及新增文件；不改写已有功能实现或历史验收结论。差异检查包含Unity序列化文件的行末空白，本轮不批量格式化资源。
- 本轮仅进行Git提交及远端同步检查，未重新执行Unity编译、Play Mode、Addressables内容打包、应用构建或真机验证；各功能验证范围以此前记录为准。


## 2026-10-28 七种通关大图与配置选图接入

- 用户确认资源、图池、Controller选图、正式配置及验证范围后实施。保留clear／fail与现有Prefab，补入clear_a—clear_f六张原作Sprite及独立GUID；ResultConfigSO／ResultConfig.asset配置1—7图片ID、完美／高难首过／重试／高／中／低／通用池和模型参数。ResultSetupEM同步维护配置，正式场景无需先执行菜单。
- 原作ResultCopy恢复C#带诊断，回查方法索引／ARM64／ELF重定位／字符串表后核实图池、PickKey哈希与避重、RankKey离线模型参数。源码不直接复制。当前按题目ID／关卡号提供种子／轮次，Controller使用现有失误、道具、重试、分数及用时选择；不接入在线统计或完整Percentile抖动，不伪造排名。旧计分账本走配置通用池，原作静态证据不等同APK动态核实。
- Panel_GameResult_Controller管理选择与本局缓存；GameState仅新增整数选图数据，GameController重开清理当前选择。Panel_GameResult保持原比例，增加原作14像素图文间距，避免皇冠等高图压住说明文字。相同结算刷新／重开稳定，下一局多图池避重；单图重试池允许重复，跨应用重启不保存选图／避重历史。未修改存档格式、版本或正式文件名，失败图、奖励图及结算业务沿用。
- Windows／Unity6000.5.2f1／Addressables2.10.3专用副本Unity编译与Scene_Main Play Mode通过2836项检查，覆盖全部图片／图池、独立哈希向量、避重、缺图错误、选图分支、教程至主线、保存读回、失败重开和第10关奖励防重。七张中文1080×1920及英文1080×2400安全区后台渲染人工查看，七图间距边界检查通过。首轮未处理已有首次连胜弹窗导致测试断言失败，补齐测试流程后通过；首轮及间距修正前证据保留。
- 当前工程dotnet build通过（0警告、0错误）；19份本批文件及10份关联资源／注册／包配置与副本一致，GUID唯一、原作来源哈希未变，审计Logs/ResultHeroMigration/delivery-audit.json。使用随机唯一测试存档，未访问正式进度；未回同步测试代码、测试配置或Library。验收文档追加A25，用户条目未代勾。Windows Addressables内容打包通过（恢复正式GameConfig后最终打包），见content-result.txt／unity-content.log；未做应用／APK构建、安装、手机触摸或真机验证。UnityEditor.Search自身异常单列，启动日志UnityTls证书错误保留；业务验证无未解决的项目错误。

## 2026-10-28 提交忽略范围检查

- 按用户补充要求检查本地文件及忽略规则，新增忽略根目录原项目参考资料（707 MiB ZIP保留本地）及Python工具缓存。Library、Temp、Logs、UserSettings、构建产物、自动生成解决方案及Addressables内容包已有规则覆盖。
- 正式地图SO／meta、Prefab、配置、图片及UI参考库的预览、来源清单和制作脚本属于项目交付与追溯资料，继续保留版本管理。没有删除本地资料或修改玩法代码。
- 使用git check-ignore核验新增规则；本次不新增Unity运行验证。此前SSH推送连接中断，继续尝试同步，最终状态以远端提交号核对为准。

## 2026-10-28 原项目参考资料便携压缩包

- 用户确认范围后，将只读来源 D:\0000001apkUnbag\01_逆向工程项目\Hamdoku2 中的 00_docs、07_reports、04_assets、05_data、03_code、08_tools 和总目录.md 打包至原项目参考资料/Hamdoku2_原项目参考资料_2026-10-28.zip；保留相对目录，位于 Assets 外。未包含 06_runtime、原始 APK／分包或其他范围外内容，原目录保持不动。
- 来源共 93,664 个文件、1,835,467,835 字节；ZIP 为 741,087,205 字节（约 706.8 MiB）。Windows 下用 .NET ZipArchive 创建并重新读取，全部条目数量、解压长度及逐文件 SHA-256 与来源一致。ZIP SHA-256：A68351F3C7A73209EE9AD599CD2386BCD1B7C1A35DCF27FF24BCE54BFBA726A2。
- 本次仅归档参考资料及更新本记录，不涉及功能或 Unity 资源迁移；未执行 Unity 编译、Play Mode、内容打包、应用构建或新电脑解压验证。工具在新电脑上的路径与运行环境仍需按当地环境配置。


### 2026-10-28 第一版八张小狗结果大图安装
- 用户选定第一版并要求继续原尺寸整理与Unity接入。八张底稿通过内置image_gen生成，clear_c使用骨头饼干、clear_f使用珊瑚粉蝴蝶结面包花束；第二版仅保留备选。底稿／提示词／总览保存在主题previews/GameResult_v01，原尺寸候选与manifest在assets/UI/GameResult/v01。
- 按alpha边界裁去外围透明边距并等比适配对应原画布，留至少12像素透明边距；八张PNG安装Assets/Res_Runtime/UI/GameResult原路径，保留原meta／GUID。原图与meta备份Logs/ResultArtInstall/before。正式Prefab／ResultConfig／图池／运行脚本／场景／包和注册未改，未访问正式玩家存档。
- Windows独立Unity6000.5.2f1编译和Scene_Main Play Mode共3049项检查通过；七张通关图1080×1920与1080×1620、英文1080×2400安全区及失败图后台截图人工检查。选图、关闭重开稳定、失败重开、阶段奖励通过。Windows Addressables内容打包通过。UnityEditor.Search索引异常单列，项目检查无失败。
- 八张PNG尺寸／RGBA／alpha与四角透明检查通过，正式图与候选／验证副本一致，受保护文件哈希不变，证据Logs/ResultArtInstall/delivery-audit.json。STYLE追加A22，MigrationAcceptance追加A26；未代勾用户验收。未进行APK构建、安装或真机验证；失败页原主题英文文案仍沿用现有工程，见验收边界。

- 本批最终内容打包已在恢复正式GameConfig后通过，见Logs/ResultArtInstall/unity-content-final.log；再次核对正式GameConfig及交付文件与验证副本一致。启动期Licensing IPC握手／access token消息保留于日志，未阻止有效许可证下的编译、Play和内容打包。

## 2026-10-29 下一批迁移启动与首轮证据核查

- 用户要求按照Docs/NextMigrationTODO.md开始执行。检查初始git工作区为空，读取Unity6000.5.2f1及Addressables2.10.3包版本，对25287份正式脚本／资源／地图／配置及必要工程文件建立SHA-256保护基线。按AGENTS A2-B7及TODO A2-B7，先完成可审阅的具体迁移清单，尚未修改功能代码或正式资源。
- 阅读原作恢复边界、类型／方法索引，查看当前主线、棋盘、首页、存档与结果面板接入，回查DynamicDifficulty及GameController部分原生／ISIL证据；38个关键方法索引、23份方法诊断、4条相关源码诊断及29份来源哈希保存Logs/NextMigrationAnalysis/evidence.json。归档不代表全部方法语义审查完成，原作静态资料不代表APK运行验证。
- 确认本批新增固定123／200／250关已存在正式地图，尺寸为9／11／11；原生技能上限101关起5、201关起6，普通选图上限仍4，困难／专家另有分支。确认无限模式原配置5阶起步、10阶上限、每3题推进、100关解锁，业务入口使用运行时PuzzleGenerator，而非主线题库抽取。专家缓解、完整出题／求解链、生命／道具及中断语义继续核查。
- NextMigrationTODO追加A9具体职责／脚本／资产／验证清单，包含运行时出题、唯一解及逻辑等级判断、11阶棋盘、独立无限状态／纪录、v1—12兼容和原作动态结果UI适配，等待实施范围确认；A2—A7完整条目未代勾。未执行Unity编译、Play、内容打包、应用／APK构建、安装或真机验证，未访问正式玩家存档或修改原作资料。

## 2026-10-28 下一批主线与无限模式迁移计划

- 用户确认后新增Docs/NextMigrationTODO.md，规划主线101—300关与无限模式完整本地玩法，覆盖原作规则／层级核查、目标脚本与正式资源、模式隔离、旧v1—12存档兼容、中英文、Unity验证及Addressables交付。工作量支持至少两小时连续实施，以实际功能与验证完成为准，不保证两小时内全部完成。
- 更新AGENTS.md文档入口、GameplayMigrationTODO及Campaign50MigrationTODO接续链接，保留原有批次与验收结论。当前仅获得文档整理授权，具体功能／资源迁移前须核实证据并确认实施清单；黄金挑战、真实外部服务及301关以后另批处理。
- 本次只读核对现有代码、配置类型、资源路径、原作恢复README／类型索引和部分业务入口；未把恢复代码视作运行事实。仅修改五份Markdown文档，未修改玩法、场景、Prefab、SO、包、玩家存档或原作资料；未执行Unity编译、Play Mode、内容打包、APK、安装或真机验证。


## 2026-10-29 主线300关与无限模式实施（开发验证进行中）

- 用户回复“好的继续”，确认NextMigrationTODO A9的脚本／资产／职责及验证范围后实施。复用ClientMain装配、Context纯字段、Controller状态修改、Module能力及现有Panel／AssetModule／存档体系，无新增interface、抽象基类、单例或平行管理入口。
- 主线配置扩至300关，活动池10432，保留原9610相对顺序及前100关尺寸；123／200／250固定题复用原SO，后两题11阶。接入技能上限5／6、普通选图上限4、困难／专家尺寸与grade、失败后的专家缓解及101关后的最低技能；旧游标及轮次扩展保留。没有修改地图／GUID或新建旋转副本。
- 无限模式使用可取消的后台运行时生成、区域生长／唯一解修复、逻辑判断及独立求解能力，不从主线池伪装生成。5阶起、每3题增阶至10阶、完成累计目标数量及1.3秒接题；每题恢复生命，道具读取持久库存。整轮计时、失败成绩／最佳、重试、退出、设置／商店／教程回看、临时入口纪录隔离及写盘失败保留／重试已实现；正式EndlessConfig和14份双语L10NSO已登记default。
- 存档v13兼容v1—12，仅新增专家缓解与无限最高纪录，不保存无限局内棋盘；返回／重启从第一题开始，正常失败结算保存最佳。未读取或覆盖正式hamdoku-progress.json；验证仅使用已知历史测试文件与随机唯一测试文件。
- 当前全运行代码.NET编译0警告／0错误。600张生成题通过7500项独立Python MRV求解／连通性等检查，600张互不重复；另1115项独立配置检查通过。首轮200次搜索预算在9阶出现出题失败，保留记录后以正式SO配置2000次预算复验通过，不使用占位题；该预算属于工程选择，不声明原APK逐种子／完整等级判定一致。
- Unity6000.5.2f1／Addressables2.10.3首轮无图形Play逐关至300，28245项通过；真实300奖励状态使旧“最终已领取100”断言失败，保留首轮，修订断言并继续专项。复用旧验证副本后发现部分未修改脚本／程序集缺失，保留编译失败日志，建立新的专用隔离副本并逐文件核对哈希。带图形设备的早期Play等待与Search索引异常单列；当前不把无图形结果写成视觉通过。
- MigrationAcceptance追加A27操作／兼容／边界及待用户验收，未代勾。真实v11／v12、完整无限及双语视觉、Windows Addressables内容打包、字体同步及最终正式交付审计仍在执行；应用／APK、安装、真机及原APK动态对照未执行。证据Logs/NextMigrationAnalysis及Logs/NextMigrationImplementation。


## 2026-10-29 主线300关与无限模式交付完成

- 完成此前“开发验证进行中”事项：真实v11／v12测试存档、旧100关未关闭结果、200关连续至300及19题无限5→10阶专项27291项通过。覆盖提示／排除／重开、旧111槽扩123槽、资料／每日／奖励、持久库存扣减、计时、设置／商店／教程、失败最佳135、重试／主动退出、试玩隔离、取消生成及实际写盘失败重试，未访问正式玩家进度。
- 修复保存成功后结果页最佳动画目标仍为旧值，Panel_GameResult由Controller刷新统计；接题期禁用商店。最终D3D12 Canvas297项通过，16张中英文／短长屏安全区截图人工查看，包含实际写盘失败最佳0→重试成功立即显示5。中文单字体离线烘焙并保留meta，英文现有单字体覆盖全部字符；未重建Prefab。
- 原生CampaignMapBank构造0x33c73e8／0x33c740c确认fixedLevel=-1不入普通池，修订导入工具；64张原记录保持非活动及原哈希。新增grade5、10／11阶grade6池尾跨轮、0—7变换及历史避重72项通过，不重复未改动的全地图98552组合历史验证。
- Windows Addressables内容打包通过，validate12319张全部unchanged，无冲突／缺失；审计正式资产与完整副本、全部地图／来源哈希、meta／GUID、既有登记、default、DefaultObject、EditorBuildSettings、程序集及包依赖。正式GameConfig v13、文件名未改；未复制测试脚本／存档／Library。证据Logs/NextMigrationImplementation/delivery-audit.json、FinalUI、Supplement、pool-result.txt、content-result.txt；原作证据Logs/NextMigrationAnalysis。
- 保留生成预算200在9阶失败、旧100奖励断言失败、旧副本缺文件编译失败、测试重载丢失savePath失败，分别修复配置／驱动后复验；Search异常和启动许可提示单列，最终无未解决项目检查错误。2000次预算属工程选择，不声明原APK逐种子／全部逻辑等级一致。全运行代码.NET编译0警告／0错误，隔离Unity编译、Play及Windows内容打包通过；应用／APK构建、安装、真机触摸／性能／扬声器及原APK动态对照未执行。
- NextMigrationTODO追加A10并更新完成状态，MigrationAcceptance A27更新最终开发结果，既有玩法／主线TODO同步接续；用户验收未代勾。黄金挑战、真实外部服务及301关以后属于范围外。
- 最终审计通过：25258份原保护文件未变，45份原作来源哈希未变；12371条既有Gameplay登记保留，新增15条default登记，13612份meta GUID唯一。全部交付资产与打包副本逐字节一致；AddressableAssetSettings仅同步Unity置零的序列化缓存Hash，注册关系未变。git diff --check通过。

## 2026-11-05 Android APK 打包

- 按用户要求在独立构建副本制作 Android APK，正式入口为 Scene_Main；保留当前竖屏、ARM64、IL2CPP 配置。原工程玩法、资源、场景及 ProjectSettings 未修改；构建副本设置 Android 包名 com.DefaultCompany.Hamdoku2，版本 1.0（1），使用 Android Debug 测试签名。
- 补装 Unity 6000.5.2f1 官方 Android 构建模块，配齐 JDK 17、NDK r27c、SDK Build Tools 36.0.0、Platform Tools 36.0.0、Cmdline Tools 16.0。模块 MD5、命令行工具 SHA-384 与官方元数据一致。首次构建因旧 SDK 命令行工具自动升级失败中止；补齐后原生编译遇到并发内存不足，改用 BEE_BUILD_THREADS=2 后构建成功。未通过关闭用户编辑器释放内存。
- 最终应用构建成功：0 错误、3 条 TMP 大方法拆分编译警告；Android Addressables 内容打包通过，APK 内含 7 个资源 bundle。APK ZIP 全条目 CRC、v2 签名、16 KiB ZIP 对齐、包名／版本／ARM64／SDK 信息核验通过；最低 Android 8.0（API 26），目标 API 36。未执行安装、Play Mode、手机触摸及真机验证。
- 包体位置：Builds/Android/Hamdoku2-1.0-arm64-20261105.apk，132,032,260 字节（约 125.9 MiB），SHA-256：C57D5DCCFEF8D1CEE15837CB67A3D30B11E0BF06049112F030CC73728CB5ECEB。构建日志、临时打包脚本及核验结果保存在 Logs/AndroidBuild_20261105；测试签名包用于安装试玩，尚未配置商店正式签名。

## 2026-11-05 编辑器顶部清除存档按钮

- 按用户要求参考 PandaSpin2/Assets/Src_Editor/Build/AndroidBuildToolbar.cs 的清除存档入口，在 Unity 顶部工具栏右侧添加“清除存档”。当前 Unity 6000.5.2f1 使用原生 MainToolbarElement 注册，默认右侧索引 0；不引入参考项目的打包、开始游戏或旧版反射工具栏代码。
- 新增 Assets/Src_Editor/EM/SaveToolbarEM.cs 及 meta；SaveModule.GetPath 改为公开供工具复用原有路径与配置校验。读取正式 LogoConfig.startupGameConfig，删除当前 Editor persistentDataPath 下对应 JSON 和 .tmp；弹窗展示路径并确认，取消不删除，Play／切换 Play 时禁用，文件访问失败明确报告。
- 本次正式编辑器自动导入并完成 Hamdoku 与 Hamdoku.Editor 编译，Logs/Editor.log 记录 Tundra build success（4.14 秒）。未实际点击确认删除，未清除玩家存档；按钮视觉位置、取消／确认交互及重启后进度重置尚未进行实际点击验收。未构建 APK。另启的独立 Mono 编译未完成，发现正式 Unity 已编译成功后终止，不计为验证通过。
- 保留修改前已有 ProjectSettings/ProjectSettings.asset 与 WORK_LOG.md 内容；已有 ProjectSettings 行尾空白未调整。本批代码差异空白检查通过。
## 2026-11-05 修复清除存档按钮不可见

- 用户截图确认顶部工具栏未出现按钮，上次编译通过不代表显示验收。核查当前 UnityEditor.CoreModule 的 OverlayCanvas.FindSaveData：新 MainToolbarOverlay 的默认显示状态仅为 UnityOnlyMainToolbarPresetAttribute 元素开启，自定义元素默认隐藏；MainToolbarElement 的默认右侧停靠配置不会自动显示元素。
- SaveToolbarEM 初始化延迟查找 MainToolbarWindow，通过公开 EditorWindow.TryGetOverlay 与 Overlay.displayed 显式显示本按钮，并刷新；新增 Hamdoku/显示清除存档按钮菜单作为重新显示入口。保留清除确认、Play 禁用及存档路径逻辑。
- 使用 Unity 6000.5.2f1 自带 Roslyn 和本项目程序集引用独立编译 SaveToolbarEM 通过，无错误；本次代码差异空白检查通过。截至记录时，打开的 Unity 尚未重新导入本次修改，实际按钮显示与点击仍待切回编辑器触发刷新后核验，不宣称视觉验证通过；未执行删除存档。
## 2026-11-05 添加 README 演示视频

- 按用户要求新增根目录 README.md，加入演示视频查看和原文件下载链接；原始录屏保存为 media/demo.mp4，并在 AGENTS.md 添加 README 入口。
- 视频 SHA-256 与源文件一致，检查 Markdown 链接路径及 git diff --check。本次仅提交展示文档和录屏；原有未提交改动保留，未执行 Unity 或构建验证。
