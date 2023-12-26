using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace TT
{
    public class SingleDAL
    {
        protected JSONNode data;
        protected string dataName;
        public virtual string DataName
        {
            set => dataName = value;
            get => dataName;
        }

        public bool IsArray => data.IsArray;
        public int LengthArray => data.IsArray ? data.AsArray.Count : 0;

        public JSONArray Array => data.AsArray;

        protected virtual void LoadData(string nameData)
        {
            this.DataName = nameData;
            LoadDataFromText(ResourceManager.Instance.GetDataText(nameData));
        }

        public virtual bool LoadDataFromText(string content)
        {
            data = JSONNode.Parse(content)["data"];
            return data != null;
        }

        public virtual T GetData<T>(int index)
        {
            if (!IsArray || index < 0 || index >= LengthArray) return default(T);
            return JsonUtility.FromJson<T>(data.AsArray[index].ToString());
        }
        public JSONObject GetData(int level)
        {
            JSONArray array = data.AsArray;
            if (level > array.Count) return array[array.Count - 1].AsObject;
            return array[level - 1].AsObject;
        }

        public virtual T[] GetDatas<T>()
        {
            T[] datas = new T[LengthArray];
            for(int i = 0; i < LengthArray; i++)
            {
                datas[i] = GetData<T>(i);
            }
            return datas;
        }
    }
}
