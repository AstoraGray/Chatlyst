﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace AVG.Editor
{
    public class NodeSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        private Type[] _nodeTypes;
        private PlotSoGraphView _plotSoGraphView;

        public void Info(PlotSoGraphView graphView)
        {
            _plotSoGraphView = graphView;
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            //反射获取所有继承了IGraphNode接口的类
            //IGraphNode，作用就是标记这个节点是否需要提供创建按钮
            var types = typeof(GraphNode<>).Assembly.GetTypes();
            _nodeTypes = types.Where(a => a.GetInterfaces().Contains(typeof(IGraphNode))).ToArray();
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node"), 0),
                new SearchTreeGroupEntry(new GUIContent("Nodes"), 1),
            };

            if (_nodeTypes is not { Length: > 0 }) return tree;
            //根据所有继承了INode接口的类，创建对应的按钮
            tree.AddRange(_nodeTypes.Select(t =>
            {
                var entry = new SearchTreeEntry(new GUIContent(t.Name))
                {
                    level = 2,
                    userData = t.Name
                };
                return entry;
            }));

            return tree;
        }


        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            var graphMousePosition = _plotSoGraphView.WorldToLocal(context.screenMousePosition);

            //根据类名创建节点
            var editorAssembly = typeof(GraphNode<>).Assembly;
            var typeName = (string)searchTreeEntry.userData;
            switch (typeName)
            {
                case "StartNode":
                    StartNode.NodeAdd(_plotSoGraphView, graphMousePosition, new StartNode());
                    break;
                default:
                    break;
            }

/*
            var newNode = editorAssembly.CreateInstance(typeName);
            if (newNode != null)
            {
                var nodeType = newNode.GetType();
                nodeType.GetMethod("NodeAdd")
                    ?.Invoke(newNode, new[] { _plotSoGraphView, graphMousePosition, newNode });
            }

*/
            return true;
        }
    }
}