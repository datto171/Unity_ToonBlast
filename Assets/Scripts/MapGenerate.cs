using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bejeweled
{
    public class MapGenerate : MonoBehaviour
    {
        [SerializeField]
        TileComponent tileMap;

        [SerializeField]
        Transform locationSpawn;

        [SerializeField]
        int sizeMap = 225;

        private void Awake()
        {
            CreateMap();
        }

        public void CreateMap()
        {
            for(int x = 0; x < sizeMap; x++)
            {
                Instantiate(tileMap, locationSpawn);
            }
        }

        public int GetSizeMap()
        {
            return sizeMap;
        }
    }
}