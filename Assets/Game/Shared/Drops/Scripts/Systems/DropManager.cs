using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Drop
{
    public class DropManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] dropPrefabs;
        private readonly List<DropBase> _dropBases = new List<DropBase>();

        public List<DropBase> DropBases => _dropBases;

        public static Action<DropType, Vector3> RequestDrop;
        public static Action RequestCollectAll;

        private void OnEnable()
        {
            RequestDrop += SpawnDrop;
            RequestCollectAll += CollectAll;
        }

        private void OnDisable()
        {
            RequestDrop -= SpawnDrop;
            RequestCollectAll -= CollectAll;
        }

        private void SpawnDrop(DropType dropType, Vector3 position)
        {
            DropBase dropBase = GetDrop(dropType);
            dropBase.transform.position = position;
            dropBase.gameObject.SetActive(true);
        }

        private DropBase GetDrop(DropType dropType)
        {
            DropBase dropBase = _dropBases.Find(e => !e.gameObject.activeSelf && e.DropType == dropType) ?? CreateDrop(dropType);
            return dropBase;
        }
        private DropBase CreateDrop(DropType dropType)
        {
            DropBase dropBase = Instantiate(dropPrefabs[(int)dropType]).GetComponent<DropBase>();
            _dropBases.Add(dropBase);
            return dropBase;
        }

        private void CollectAll()
        {
            foreach (DropBase dropBase in _dropBases.Where(dropBase => dropBase.gameObject.activeSelf))
            {
                dropBase.CanGoToPlayer = true;
            }
        }
    }
}