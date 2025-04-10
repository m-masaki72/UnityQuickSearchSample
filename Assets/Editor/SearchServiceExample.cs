using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Search;

namespace Editor
{
    /// <summary>
    /// Unity Search Serviceクラスを使ってウィンドウに対して操作する
    /// ref. https://docs.unity3d.com/Packages/com.unity.quicksearch@2.0/api/Unity.QuickSearch.SearchService.html?q=SearchService
    /// </summary>
    public static class SearchServiceExample
    {
        // 1) まっさらな検索ウィンドウを開く
        [MenuItem("Examples/SearchService/01 Empty Window")]
        private static void EmptyWindow()
        {
            // 返ってくる ISearchView を使えば、後から検索語をセットすることも可能
            SearchService.ShowWindow()
                .SetSearchText(string.Empty);
        }

        // 2) 最初から検索語をセットして開く
        //    「m:」はメニューアイテムだけを検索するフィルター
        [MenuItem("Examples/SearchService/02 Prefilled Query")]
        private static void PrefilledQuery()
        {
            var ctx = SearchService.CreateContext("m: Profiler");
            SearchService.ShowWindow(ctx);
        }

        // 3) 任意のタイトル・サイズ・ドッキング可否を指定して開く
        [MenuItem("Examples/SearchService/03 Custom Title & Size")]
        private static void CustomTitleSize()
        {
            SearchService.ShowWindow(
                    topic: "My Tools", // タブタイトル
                    defaultWidth: 1200,
                    defaultHeight: 600,
                    dockable: false // ポップアップにする
                )
                .SetSearchText("t:prefab"); // ついでに検索語も投入
        }

        // 4) 検索プロバイダーを限定（ここでは「プロジェクト内アセット」のみ）
        [MenuItem("Examples/SearchService/04 Restrict Providers")]
        private static void RestrictProviders()
        {
            // プロバイダー ID は「asset」「scene」「menu」など。複数可
            var providers = new[] { "asset" };
            var ctx = SearchService.CreateContext(providers, "t:material");
            SearchService.ShowWindow(ctx);
        }

        // 5) 検索結果を UI で見せず、スクリプトだけで取得
        [MenuItem("Examples/SearchService/05 GetItems Programmatically")]
        private static void QueryItems()
        {
            var ctx = SearchService.CreateContext("p: Assets t:Texture2D");
            foreach (var item in SearchService.GetItems(ctx))
            {
                Debug.Log(item.ToString());
            }
        }

        // 6) オブジェクト選択ダイアログ代わりに “Picker” として使う
        [MenuItem("Examples/SearchService/06 Picker For Prefabs")]
        private static void ShowGenericPrefabPicker()
        {
            // 検索コンテキストを作成（asset プロバイダー + t:prefab）
            var ctx = SearchService.CreateContext("asset", "t:prefab");

            var viewState = new SearchViewState(
                ctx,
                SearchViewFlags.GridView | // グリッド表示
                SearchViewFlags.DisableSavedSearchQuery)
            {
                title = "Prefab Picker",
                windowTitle = new GUIContent("Prefab Picker"),
                position = SearchUtils.GetMainWindowCenteredPosition(new Vector2(600, 400)),

                // SearchItem が返る新しい型に合わせる
                selectHandler = (item, canceled) =>
                {
                    if (canceled) return;
                    var prefab = item.ToObject() as GameObject;
                    Debug.Log($"選択されたプレハブ: {prefab}");
                }
            };

            // ③ ピッカーを表示
            SearchService.ShowPicker(viewState);
        }

        // 7) SceneView などでマウス位置を起点にコンテキスト検索
        [MenuItem("Examples/SearchService/07 Contextual Search")]
        private static void Contextual()
        {
            // 右クリックメニューのような使い方
            SearchService.ShowContextual();
        }
    }
}