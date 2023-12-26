using UnityEngine;
using System.Linq;

namespace TT
{
    public class EntityTypeDAL : MultiDAL
    {
        public EntityTypeDAL(string type) 
        {
            this.LoadData<EntityDAL>(type);
        }

        public EntityDAL[] GetEntityDALs()
        {
            return System.Array.ConvertAll(dic.Values.ToArray(), entityVO => (EntityDAL)entityVO);
        }

        public StatInfo[] GetStatInfos(string name, int level)
        {
            if (!dic.ContainsKey(name))
            {
                Debug.Log(string.Format("Entity name: {0} doesn't exists!", name));
                return null;
            }

            return (dic[name] as EntityDAL).GetStatInfos(level);
        }
    }
}
