using UnityEngine;

namespace TT
{
    public class ResourceManager : Singleton<ResourceManager>
    {
        public string GetDataText(string dataName)
        {
            TextAsset data = Resources.Load<TextAsset>("Data/" + dataName);
            if (data == null) return string.Empty;
            return data.text;
        }

        public DataTextInfo[] GetDataTexts(string name)
        {
            TextAsset[] texts = Resources.LoadAll<TextAsset>("Data/" + name);

            DataTextInfo[] datas = new DataTextInfo[texts.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                datas[i] = new DataTextInfo(texts[i].name, texts[i].text);
            }

            return datas;
        }

        public T GetAsset<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        public T[] GetAssets<T>(string path) where T : UnityEngine.Object
        {
            return Resources.LoadAll<T>(path);
        }
    }
}