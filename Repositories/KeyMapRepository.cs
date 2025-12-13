using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DiabloTwoMFTimer.Interfaces;
using DiabloTwoMFTimer.Models;
using DiabloTwoMFTimer.Utils;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace DiabloTwoMFTimer.Repositories;

public class KeyMapRepository : IKeyMapRepository
{
    private readonly ISerializer _serializer;
    private readonly IDeserializer _deserializer;

    public KeyMapRepository()
    {
        // 配置 YamlDotNet 使用驼峰命名 (camelCase)，符合 YAML 惯例
        _serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        _deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }

    public List<KeyMapNode> LoadKeyMap()
    {
        try
        {
            // 1. 如果文件不存在，创建默认配置
            if (!File.Exists(FolderManager.KeyMapConfigPath))
            {
                var defaults = GenerateDefaultKeyMap();
                SaveKeyMap(defaults);
                return defaults;
            }

            // 2. 读取文件
            using var reader = new StreamReader(FolderManager.KeyMapConfigPath, Encoding.UTF8);
            var nodes = _deserializer.Deserialize<List<KeyMapNode>>(reader);

            return nodes ?? new List<KeyMapNode>();
        }
        catch (Exception ex)
        {
            LogManager.WriteErrorLog("KeyMapRepository", "加载按键映射失败，将使用空配置", ex);
            // 这里为了不让程序崩溃，返回空列表，或者也可以返回默认配置
            return GenerateDefaultKeyMap();
        }
    }

    public void SaveKeyMap(List<KeyMapNode> nodes)
    {
        try
        {
            FolderManager.EnsureDirectoryExists(FolderManager.AppDataPath);
            using var writer = new StreamWriter(FolderManager.KeyMapConfigPath, false, Encoding.UTF8);
            _serializer.Serialize(writer, nodes);
        }
        catch (Exception ex)
        {
            LogManager.WriteErrorLog("KeyMapRepository", "保存按键映射失败", ex);
        }
    }

    /// <summary>
    /// 生成默认的硬核按键映射树
    /// </summary>
    private List<KeyMapNode> GenerateDefaultKeyMap()
    {
        return new List<KeyMapNode>
        {
            new KeyMapNode
            {
                Key = "t",
                Text = "计时器 (Timer)",
                Children = new List<KeyMapNode>
                {
                    new KeyMapNode { Key = "s", Text = "开始 (Start)", Action = "Timer.Start" },
                    new KeyMapNode { Key = "p", Text = "暂停 (Pause)", Action = "Timer.Pause" },
                    new KeyMapNode { Key = "r", Text = "重置 (Reset)", Action = "Timer.Reset" },
                    new KeyMapNode { Key = "e", Text = "停止 (Stop)", Action = "Timer.Stop" }
                }
            },
            new KeyMapNode
            {
                Key = "r",
                Text = "记录 (Record)",
                Children = new List<KeyMapNode>
                {
                    new KeyMapNode { Key = "d", Text = "删除最后一条", Action = "Record.DeleteLast" },
                    new KeyMapNode { Key = "u", Text = "撤销删除", Action = "Record.UndoDelete" }
                }
            },
            new KeyMapNode
            {
                Key = "u",
                Text = "工具 (Utils)",
                Children = new List<KeyMapNode>
                {
                    new KeyMapNode { Key = "s", Text = "截图 (Screenshot)", Action = "System.Screenshot" },
                    new KeyMapNode { Key = "c", Text = "设置 (Config)", Action = "System.Settings" },
                    new KeyMapNode { Key = "q", Text = "退出程序", Action = "App.Exit" }
                }
            }
        };
    }
}