using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TT
{
    public abstract class EntityController : MonoBehaviour
    {
        #region Entity Manager

        static Dictionary<string, EntityTypeDAL> types = new Dictionary<string, EntityTypeDAL>();

        public static EntityTypeDAL GetEntityTypeVO(string type)
        {
            if (!types.ContainsKey(type))
            {
                types.Add(type, new EntityTypeDAL(type));
            }
            return types[type];
        }

        #endregion

        [SerializeField] protected string type = "";
        [SerializeField] protected EntityInfo info;
        protected EntityTypeDAL entityTypeVO;

        public EntityTypeDAL TypeVO => entityTypeVO;
        [SerializeField] protected StatController statCtrl;
        public StatController StatCtrl => statCtrl;

        public EntityInfo Info
        {
            set
            {
                info = value;
                OnLevelUp(info.Level);
            }
            get
            {
                return info;
            }
        }

        public string Type => type;
        public string EntityName => info.Name;
        public virtual int Level
        {
            get => info.Level;
            set
            {
                info.Level = value;
                OnLevelUp(Level);
            }
        }

        protected virtual void Awake()
        {
            entityTypeVO = EntityController.GetEntityTypeVO(type);
        }

        protected abstract void OnLevelUp(int level);

        public virtual StatInfo[] GetStatsByLevel(int level)
        {
            return entityTypeVO.GetStatInfos(info.Name, level);
        }
    }
}
