using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CAFU.NumberRenderer
{
    [PublicAPI]
    internal static class InternalUtility
    {
        internal static string ProjectPath { get; } = Path.GetDirectoryName(Application.dataPath);

        internal static T FindAsset<T>() where T : Object
        {
            return FindAsset<T>(_ => true);
        }

        internal static T FindAsset<T>(Func<string, bool> pathFilter) where T : Object
        {
            return FindAssets<T>(pathFilter)?.FirstOrDefault();
        }

        internal static IEnumerable<T> FindAssets<T>() where T : Object
        {
            return FindAssets<T>(_ => true);
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
        internal static IEnumerable<T> FindAssets<T>(Func<string, bool> pathFilter) where T : Object
        {
            var guidList = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            if (guidList.Length == 0)
            {
                return Enumerable.Empty<T>();
            }

            var assets = guidList.Select(AssetDatabase.GUIDToAssetPath).Where(pathFilter).Select(AssetDatabase.LoadAssetAtPath<T>);
            if (!assets.Any())
            {
                return Enumerable.Empty<T>();
            }

            return assets;
        }

        internal static IEnumerable<string> FindGUIDs<T>() where T : Object
        {
            return FindGUIDs<T>(_ => true);
        }

        [SuppressMessage("ReSharper", "InvertIf")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
        internal static IEnumerable<string> FindGUIDs<T>(Func<string, bool> pathFilter) where T : Object
        {
            var guidList = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            if (guidList.Length == 0)
            {
                return Enumerable.Empty<string>();
            }

            var guids = guidList.Where(x => pathFilter(AssetDatabase.GUIDToAssetPath(x)));

            if (!guids.Any())
            {
                return Enumerable.Empty<string>();
            }

            return guids;
        }

        internal static void CreateFolder(string path)
        {
            var parentFolder = string.Empty;
            foreach (var item in path.Split('/', '\\'))
            {
                var folder = Path.Combine(parentFolder, item);
                if (!AssetDatabase.IsValidFolder(folder))
                {
                    AssetDatabase.CreateFolder(parentFolder, item);
                }

                parentFolder = folder;
            }
        }

        internal static IList<(GameObject gameObject, TComponent component)> SetupRendererComponents<TComponent>(int digit)
            where TComponent : Component
        {
            var list = new List<(GameObject, TComponent)>();
            for (var i = 0; i < digit; i++)
            {
                var go = new GameObject($"Child{i}");
                list.Add((go, go.AddComponent<TComponent>()));
            }

            return list;
        }
    }
}