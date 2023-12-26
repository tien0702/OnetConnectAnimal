using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TT
{
    public struct DataTextInfo
    {
        public string Name;
        public string Content;

        public DataTextInfo(string name, string content)
        {
            this.Name = name;
            this.Content = content;
        }
    }

    public class MultiDAL
    {
        string type;
        public string Type => type;

        protected Dictionary<string, SingleDAL> dic = new Dictionary<string, SingleDAL>();

        protected virtual bool LoadData<T>(string typeName) where T : SingleDAL, new()
        {
            this.type = typeName;
            DataTextInfo[] texts = ResourceManager.Instance.GetDataTexts(typeName);
            if (texts.Length == 0) return false;

            foreach (DataTextInfo text in texts)
            {
                T single = new T();
                single.DataName = text.Name;
                if (single.LoadDataFromText(text.Content))
                    dic.Add(text.Name, single);
            }
            return true;
        }

        public virtual SingleDAL GetBaseSingleVO(string name)
        {
            if (!dic.ContainsKey(name)) return null;
            return dic[name];
        }

        public virtual SingleDAL[] GetBaseSingleVOs()
        {
            return dic.Values.ToArray();
        }

        protected virtual T GetData<T>(string name, int index)
        {
            return GetBaseSingleVO(name).GetData<T>(index);
        }
    }
}