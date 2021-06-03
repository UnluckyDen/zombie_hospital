using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class ScreenshotUtility : EditorWindow
    {
        private Camera _camera;
        private string _path = string.Empty;
        private bool _transparent;

        [MenuItem("Allemo/Screenshot Utility")]
        public static void ShowWindow()
        {
            GetWindow<ScreenshotUtility>("Screenshot Utility");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Path", EditorStyles.boldLabel);
            EditorGUILayout.TextField(_path, GUILayout.ExpandWidth(false));
            if (GUILayout.Button("Browse", GUILayout.ExpandWidth(false)))
                _path = EditorUtility.SaveFolderPanel("Path to Save Images", _path, Application.dataPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Camera", EditorStyles.boldLabel);
            _camera = EditorGUILayout.ObjectField(_camera, typeof(Camera), true) as Camera;
            if (GUILayout.Button("Find")) _camera = FindObjectOfType<Camera>();
            EditorGUILayout.EndHorizontal();

            _transparent = EditorGUILayout.Toggle("Transparent Background", _transparent);

            EditorGUILayout.Space();

            if (GUILayout.Button("Take Screenshots", GUILayout.MinHeight(60)))
            {
                TakeScreenshot(new Vector2Int(1242, 2208));
                TakeScreenshot(new Vector2Int(2048, 2732));
                TakeScreenshot(new Vector2Int(1242, 2688));
            }

            if (GUILayout.Button("Open Folder", GUILayout.MinHeight(40))) Application.OpenURL("file://" + _path);
        }

        private void TakeScreenshot(Vector2Int screenshotSize)
        {
            var screenshotWidth = screenshotSize.x;
            var screenshotHeight = screenshotSize.y;
            var renderTexture = new RenderTexture(screenshotWidth, screenshotHeight, 24);
            var tFormat = _transparent ? TextureFormat.ARGB32 : TextureFormat.RGB24;
            var screenShot = new Texture2D(screenshotWidth, screenshotHeight, tFormat, false);

            _camera.targetTexture = renderTexture;
            _camera.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0);
            _camera.targetTexture = null;
            RenderTexture.active = null;

            var bytes = screenShot.EncodeToPNG();
            var filename = $"{_path}/{screenshotWidth}x{screenshotHeight}_{Random.Range(10000, 99999)}.png";

            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log($"Saved to: {filename}");
        }
    }
}