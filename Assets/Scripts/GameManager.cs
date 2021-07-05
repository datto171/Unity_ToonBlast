using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bejeweled
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField]
        GameObject gridTile;

        private TileComponent[] cells;

        //public GameContext gameContext;



        private void Start()
        {
            Instance = this;

            SetIndexMap();
        }

        public TileComponent[] GetCellsArray()
        {
            return cells;
        }


        public void RemoveTileSelected(TileComponent target)
        {
            var oldType = target.GetTypeTile();


            target.SetIndex(-1);
            target.SetPosX(-1);
            target.SetPosY(-1);
            target.SetTileSelected(false);
            target.SetTextPos(-1, -1);
            target.SetType(oldType);
        }

        public void SetIndexMap()
        {
            cells = gridTile.GetComponentsInChildren<TileComponent>();

            for(int i = 0; i < cells.Length; i++)
            {
                cells[i].SetPosX(i % 8);
                cells[i].SetPosY(i / 8);
                cells[i].SetIndex(i);
                cells[i].SetTileSelected(false);
                cells[i].SetTextPos(i % 8, i / 8);

                // Random Setup Map
                var k = Random.Range(1, 4);
                switch (k)
                {
                    case 1:
                        cells[i].SetType(TileType.Yellow);
                        break;
                    case 2:
                        cells[i].SetType(TileType.Green);
                        break;
                    case 3:
                        cells[i].SetType(TileType.Red);
                        break;
                    case 4:
                        cells[i].SetType(TileType.Blue);
                        break;
                }
            }
            cells[0].SetType(TileType.BoxBlue);
            cells[1].SetType(TileType.RocketRow);
        }

        public TileComponent GetTile(int x, int y)
        {
            if(x < 0 || y < 0 || x > 7 || y > 7)
            {
                return null;
            }
            else
            {
                return cells[y * 8 + x];
            }
        }

        public void SetupMap()
        {
            //for (int i = 0; i < cells.Length; i++)
            //{
            //    cells[i].SetPosX(i % 8);
            //    cells[i].SetPosY(i / 8);
            //    cells[i].SetIndex(i);
            //    cells[i].SetTileState(false);
            //    cells[i].SetTextPos(i % 8, i / 8);
            //    cells[i].SetType(TileType.TypeA);
            //}
        }
    }
}