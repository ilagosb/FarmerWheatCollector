using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "ObjectPool", menuName = "Assets/ObjectPool", order = 0)]
    public class ObjectPool : ScriptableObject
    {
        #region Fields

        [SerializeField] private GameObject _gameObject;
        [SerializeField] private int _capacity;
        [SerializeField] private bool _createOnInit;

        private Transform _parentObject;
        private List<GameObject> _gameObjects;

        #endregion

        #region Properties

        private List<GameObject> Inactive => _gameObjects.Where(obj => !obj.activeSelf).ToList();
        private bool HasCreatedInactive => Inactive.Count > 0;

        #endregion

        public void InitObjectPool(Transform parent)
        {
            _gameObjects = new List<GameObject>(_capacity);
            _parentObject = parent;
            if (_createOnInit)
            {
                for (int i = 0; i < _capacity; i++)
                {
                    var newObj = CreateNew(parent.position, parent);
                    newObj.SetActive(false);
                    _gameObjects.Add(newObj);
                }
            }
        }

        public void Release(GameObject obj)
        {
            if (_gameObjects.Contains(obj))
            {
                if (obj.transform.parent != _parentObject)
                {
                    obj.transform.SetParent(_parentObject);
                }
                obj.SetActive(false);
            }
            else
            {
                Debug.LogWarning("GameObject is not part of object pool", obj);
            }
        }

        public T Get<T>(Vector3 position, Transform parent = null, bool forceCreate = false)
        {
            return Get(position, parent, forceCreate).GetComponent<T>();
        }

        public GameObject Get(Vector3 position, Transform parent = null, bool forceCreate = false)
        {
            GameObject obj;
            Transform parentObject = parent == null ? _parentObject : parent;
            if (!forceCreate && HasCreatedInactive)
            {
                obj = Inactive.First();
                obj.SetActive(true);
                obj.transform.position = position;
                obj.transform.SetParent(parentObject);
            }
            else
            {
                obj = CreateNew(position, parentObject);
                _gameObjects.Add(obj);
            }

            return obj;
        }

        private GameObject CreateNew(Vector3 position, Transform parent) => Instantiate(_gameObject, position,
            Quaternion.identity, parent);
    }
}