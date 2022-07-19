using System;
using UnityEngine;

namespace Utils
{
    public class ObjectPoolContainer : MonoBehaviour
    {
        #region Fields

        [SerializeField] private ObjectPool _objectPool;

        #endregion

        #region Properties
        
        public ObjectPool ObjectPool => _objectPool;

        #endregion

        private void Awake()
        {
            _objectPool.InitObjectPool(transform);
        }
    }
}