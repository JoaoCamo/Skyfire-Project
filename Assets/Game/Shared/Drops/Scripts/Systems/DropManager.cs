using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Drop
{
    public class DropManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] dropPrefabs;
        private readonly List<DropBase> _dropBases = new List<DropBase>();

        public static Action<DropType, Vector3> RequestDrop;

        private void OnEnable()
        {
            RequestDrop += SpawnDrop;
        }

        private void OnDisable()
        {
            RequestDrop -= SpawnDrop;
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
    }
}