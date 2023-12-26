using SimpleJSON;
using System;
using System.Reflection;
using UnityEngine;

namespace TT
{
    [System.Serializable]
    public class NodeInfo
    {
        public string AssemblyName;
        public string NodeType;
        public string NodeTypeInfo;
    }

    public class NodeType
    {
        public Type Type;
        public object NodeData;
    }

    public class NodeUtils
    {
        // json data
        public static NodeType GetNodeType(JSONNode data)
        {
            NodeInfo info = JsonUtility.FromJson<NodeInfo>(data.ToString());
            if (info == null)
            {
                Debug.LogWarning("Data is null");
                return null;
            }

            Assembly assembly = Assembly.Load(info.AssemblyName);

            NodeType nodeType = new NodeType();
            nodeType.Type = assembly.GetType(info.NodeType);
            string componentData = data["data"].ToString();
            if (componentData != null && !info.NodeTypeInfo.Equals(string.Empty))
            {
                nodeType.NodeData = JsonUtility.FromJson(componentData, assembly.GetType(info.NodeTypeInfo));
            }

            return nodeType;
        }

        public static NodeType[] GetNodeTypes(JSONNode dataArray)
        {
            if (!dataArray.IsArray)
            {
                Debug.Log("dataArray is not array");
                return null;
            }
            int arrayCount = dataArray.AsArray.Count;
            NodeType[] components = new NodeType[arrayCount];

            for (int i = 0; i < arrayCount; ++i)
            {
                components[i] = NodeUtils.GetNodeType(dataArray.AsArray[i]);
            }

            return components;
        }

        // string data
        public static NodeType[] GetNodeTypes(string data)
        {
            JSONNode jsonData = JSONObject.Parse(data);
            JSONNode dataArray = jsonData["data"];
            if (!dataArray.IsArray)
            {
                Debug.Log("dataArray is not array");
                return null;
            }
            int arrayCount = dataArray.AsArray.Count;
            NodeType[] nodes = new NodeType[arrayCount];

            for (int i = 0; i < arrayCount; ++i)
            {
                nodes[i] = NodeUtils.GetNodeType(dataArray.AsArray[i]);
            }

            return nodes;
        }

        public static object GetNodeFromNodeType(NodeType nodeType, Action<object> callbackOnComplete = null)
        {
            object actionNode = null;
            actionNode = (object)Activator.CreateInstance(nodeType.Type);

            if (nodeType.NodeData != null)
            {
                IInfo info = (IInfo)actionNode;
                if (info != null)
                {
                    info.SetInfo(nodeType.NodeData);
                }
            }

            if (callbackOnComplete != null)
            {
                callbackOnComplete(actionNode);
            }
            return actionNode;
        }
    }
}
