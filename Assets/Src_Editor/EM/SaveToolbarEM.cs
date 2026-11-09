using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;

namespace Hamdoku.Editor {
    /// <summary>
    /// 在编辑器顶部右侧提供本地存档清除入口。
    /// </summary>
    public static class SaveToolbarEM {
        [InitializeOnLoadMethod]
        static void Initialize() {
            EditorApplication.playModeStateChanged -= RefreshButton;
            EditorApplication.playModeStateChanged += RefreshButton;
            EditorApplication.delayCall += ShowButton;
        }

        [MenuItem("Hamdoku/显示清除存档按钮")]
        public static void ShowButton() {
            // Unity 6.5 默认隐藏新增的自定义工具栏元素，需要显式开启此按钮。
            var windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (var window in windows) {
                if (window.GetType().FullName != "UnityEditor.MainToolbarWindow") {
                    continue;
                }
                if (window.TryGetOverlay("Hamdoku/ClearSave", out Overlay overlay)) {
                    bool wasDisplayed = overlay.displayed;
                    overlay.displayed = true;
                    MainToolbar.Refresh("Hamdoku/ClearSave");
                    Debug.Log("[SaveToolbarEM] 清除存档按钮已显示，原显示状态：" + wasDisplayed);
                    return;
                }
            }
            Debug.LogError("[SaveToolbarEM] 未找到清除存档工具栏元素。");
        }

        [MainToolbarElement("Hamdoku/ClearSave", defaultDockPosition = MainToolbarDockPosition.Right, defaultDockIndex = 0)]
        public static MainToolbarElement CreateButton() {
            var content = new MainToolbarContent("清除存档", "清除当前 Unity Editor 使用的本地游戏进度");
            var button = new MainToolbarButton(content, ClearSave);
            button.enabled = !EditorApplication.isPlayingOrWillChangePlaymode;
            return button;
        }

        static void RefreshButton(PlayModeStateChange state) {
            MainToolbar.Refresh("Hamdoku/ClearSave");
        }

        static void ClearSave() {
            if (EditorApplication.isPlayingOrWillChangePlaymode) {
                EditorUtility.DisplayDialog("清除存档", "请先停止游戏，再清除本地存档。", "确定");
                return;
            }

            var logoConfig = AssetDatabase.LoadAssetAtPath<LogoConfigSO>("Assets/Res_Runtime/Config/LogoConfig.asset");
            if (logoConfig == null || logoConfig.startupGameConfig == null) {
                Debug.LogError("[SaveToolbarEM] 缺少启动存档配置，无法清除存档。");
                return;
            }
            var saves = new SaveModule();
            string savePath = saves.GetPath(logoConfig.startupGameConfig, Application.persistentDataPath);
            bool confirmed = EditorUtility.DisplayDialog(
                "清除存档",
                "这会删除当前 Unity Editor 使用的本地游戏进度，是否继续？\n\n" + savePath,
                "清除",
                "取消");
            if (!confirmed) {
                return;
            }

            try {
                bool hadSave = File.Exists(savePath) || File.Exists(savePath + ".tmp");
                File.Delete(savePath);
                File.Delete(savePath + ".tmp");
                if (hadSave) {
                    Debug.Log("[SaveToolbarEM] 已清除本地存档：" + savePath);
                } else {
                    Debug.Log("[SaveToolbarEM] 当前没有本地存档：" + savePath);
                }
            } catch (IOException error) {
                Debug.LogException(error);
                EditorUtility.DisplayDialog("清除失败", error.Message, "确定");
            } catch (System.UnauthorizedAccessException error) {
                Debug.LogException(error);
                EditorUtility.DisplayDialog("清除失败", error.Message, "确定");
            }
        }
    }
}
