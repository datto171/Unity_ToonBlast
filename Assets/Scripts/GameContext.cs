using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bejeweled
{
    public class GameContext : MonoBehaviour
    {
        public TileComponent tileSelected = null;
        public List<TileComponent> listTileCheck;
        public List<TileComponent> listClear;

        // if (typeTarget = 0 => normal) else if typeTarget = 1 => special  else if typeTarget = 2 => empty
        public int statusTile = 0;

        private void Awake()
        {
            TileComponent.onInteract = OnInteract;
        }

        public void OnInteract(TileComponent target)
        {
            // divide 
            //statusTile = SetStatusTileTarget(target);
            SetupSpecial(target);

            // Check normal
            if (target.GetTypeTile() == TileType.Yellow || target.GetTypeTile() == TileType.Red || target.GetTypeTile() == TileType.Green || target.GetTypeTile() == TileType.Blue)
            {
                listTileCheck.Add(target);
                CheckTileAround(target, 1);

                // Check list clear to create special tile
                CreateSpecialTile(target);

                // Clear List
                if (listTileCheck.Count > 1)
                {
                    foreach (TileComponent tile in listTileCheck)
                    {
                        if (tile != target)
                        {
                            tile.SetType(TileType.Empty);
                        }
                    }

                    if (target.GetTypeTile() == TileType.Yellow || target.GetTypeTile() == TileType.Red || target.GetTypeTile() == TileType.Green || target.GetTypeTile() == TileType.Blue)
                    {
                        target.SetType(TileType.Empty);
                    }
                }
            }
            // check special
            else if (target.GetTypeTile() == TileType.Bomb || target.GetTypeTile() == TileType.RocketCol || target.GetTypeTile() == TileType.RocketRow)
            {
                if (target.GetTypeTile() == TileType.Bomb)
                {
                    //GetTileBomb(target);
                    //GetTileRocket(target, target.GetTypeTile());

                    //CombineDoubleRocket(target);
                    //CombineBombAndRocket(target);
                    //CombineDoubleBomb(target);

                    CombineBoxAndBomb();
                }
                else if (target.GetTypeTile() == TileType.RocketCol || target.GetTypeTile() == TileType.RocketRow)
                {
                    //GetTileRocket(target, target.GetTypeTile());
                    CombineBoxAndBomb();
                }

                // Clear List
                foreach (TileComponent tile in listTileCheck)
                {
                    tile.SetType(TileType.Empty);
                }
            }
            else if (target.GetTypeTile() == TileType.BoxYellow || target.GetTypeTile() == TileType.BoxBlue || target.GetTypeTile() == TileType.BoxGreen || target.GetTypeTile() == TileType.BoxRed)
            {
                GetTileMysteryBox(target);

                // Clear List
                foreach (TileComponent tile in listTileCheck)
                {
                    tile.SetType(TileType.Empty);
                }
            }

            listTileCheck.Clear();
            //StartCoroutine(DecreaseRowCo());
        }

        public IEnumerator DecreaseRowCo()
        {
            int nullCount = 0;
            for (int i = 7; i >= 0; i--)
            {
                for (int j = 7; j >= 0 ; j--)
                {
                    var typeTile = GameManager.Instance.GetTile(i, j).GetTypeTile();
                    if(typeTile == TileType.Empty)
                    {
                        nullCount++;
                    }
                    else if (nullCount > 0)
                    {
                        var test = GameManager.Instance.GetTile(i - nullCount, j);
                        GameManager.Instance.GetTile(i - nullCount, j).SetType(TileType.Empty);

                        Debug.Log("x: " + test.GetPosX() + ", y: " + test.GetPosY());

                        GameManager.Instance.GetTile(i - nullCount, j);
                        GameManager.Instance.GetTile(i - nullCount, j).SetType(TileType.Empty);

                        //allDots[i, j].GetComponent<Dot>().row -= nullCount;
                        //allDots[i, j] = null;
                    }
                }
                nullCount = 0;
            }
            yield return new WaitForSeconds(.4f);
            //StartCoroutine(FillBoardCo());
        }

        public void CombineBombAndRocket(TileComponent target)
        {
            List<TileComponent> listBombAndRocket = new List<TileComponent>();

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var tileAround = GameManager.Instance.GetTile(target.GetPosX() + i, target.GetPosY() + j);

                    if (tileAround != null)
                    {
                        listBombAndRocket.Add(tileAround);
                    }
                }
            }

            foreach(TileComponent t in listBombAndRocket)
            {
                CombineDoubleRocket(t);
            }
        }

        public void CombineDoubleRocket(TileComponent target)
        {
            for (int i = 0; i < 8; i++)
            {
                TileComponent tileDestroy1 = GameManager.Instance.GetTile(i, target.GetPosY());

                TileComponent tileDestroy2 = GameManager.Instance.GetTile(target.GetPosX(), i);


                if (!listTileCheck.Contains(tileDestroy1))
                {
                    listTileCheck.Add(tileDestroy1);
                }

                if (!listTileCheck.Contains(tileDestroy2))
                {
                    listTileCheck.Add(tileDestroy2);
                }
            }
        }

        public void CombineDoubleBomb(TileComponent target)
        {
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    var tileAround = GameManager.Instance.GetTile(target.GetPosX() + i, target.GetPosY() + j);

                    if (tileAround != null)
                    {
                        if (!listTileCheck.Contains(tileAround))
                        {
                            listTileCheck.Add(tileAround);
                        }
                    }
                }
            }
        }

        public void CombineBoxAndRocket(TileComponent target)
        {
            List<TileComponent> listChangeToRocket = new List<TileComponent>();
            //List<TileComponent> listTileWill = new List<TileComponent>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var tileAround = GameManager.Instance.GetTile(i, j);

                    if (tileAround != null)
                    {
                        if(tileAround.GetTypeTile() == TileType.Yellow)
                        {
                            listChangeToRocket.Add(tileAround);
                        }
                    }
                }
            }

            foreach(TileComponent t in listChangeToRocket)
            {
                var x = Random.Range(0, 2);
                Debug.Log("random:" + x);
                if (x == 1)
                {
                    t.SetType(TileType.RocketCol);
                    //listTileWill.Add(t);
                    GetTileRocket(t, TileType.RocketCol);
                }
                else
                {
                    t.SetType(TileType.RocketRow);
                    //listTileWill.Add(t);
                    GetTileRocket(t, TileType.RocketRow);

                }
            }
        }

        public void CombineBoxAndBomb()
        {
            List<TileComponent> listChangeToBomb = new List<TileComponent>();

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var tileAround = GameManager.Instance.GetTile(i, j);

                    if (tileAround != null)
                    {
                        if (tileAround.GetTypeTile() == TileType.Yellow)
                        {
                            listChangeToBomb.Add(tileAround);
                        }
                    }
                }
            }

            foreach (TileComponent t in listChangeToBomb)
            {
                GetTileBomb(t);
            }
        }

        public void SetupSpecial(TileComponent target)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                target.SetType(TileType.Bomb);
            }
        }

        public void CreateSpecialTile(TileComponent target)
        {
            if (listTileCheck.Count > 10)
            {
                if(target.GetTypeTile() == TileType.Yellow)
                {
                    target.SetType(TileType.BoxYellow);
                }
                else if (target.GetTypeTile() == TileType.Blue)
                {
                    target.SetType(TileType.BoxBlue);
                }
                else if (target.GetTypeTile() == TileType.Red)
                {
                    target.SetType(TileType.BoxRed);
                }
                else if (target.GetTypeTile() == TileType.Green)
                {
                    target.SetType(TileType.BoxGreen);
                }
            }
            else if (listTileCheck.Count > 6)
            {
                target.SetType(TileType.Bomb);
            }
            else if (listTileCheck.Count > 4)
            {
                var x = Random.Range(0, 2);
                Debug.Log("random:" + x);
                if (x == 1)
                {
                    target.SetType(TileType.RocketCol);
                }
                else
                {
                    target.SetType(TileType.RocketRow);
                }
            }
        }

        public void CheckTileAround(TileComponent target, int typeItem)
        {
            //Debug.Log("Tile Check Now: " + " x: " + tile.GetPosX() + ", y:" + tile.GetPosY());

            // Type = 1 => Check Tile Normal / Type = 2 => Check Special (include: Special Alone and Special Combine)
            if(typeItem == 1)
            {
                target.SetCheckedDone(true);

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        var tileAround = GameManager.Instance.GetTile(target.GetPosX() + i, target.GetPosY() + j);

                        if (tileAround != null && !tileAround.GetIsChecked())
                        {
                            if (i == 0 && j == -1 || i == 0 && j == 1 || i == -1 && j == 0 || i == 1 && j == 0)
                            {
                                if (target.GetTypeTile() == tileAround.GetTypeTile())
                                {
                                    if (!listTileCheck.Contains(tileAround))
                                    {
                                        listTileCheck.Add(tileAround);
                                    }
                                }
                            }
                        }
                    }
                }

                //Debug.Log(listTileCheck.Count);
                foreach (TileComponent tileCheck in listTileCheck)
                {
                    //Debug.Log("x: " + tileCheck.GetPosX() + ", y:" + tileCheck.GetPosY());
                }


                foreach (TileComponent tileCheck in listTileCheck.ToArray())
                {
                    if (!tileCheck.GetIsChecked())
                    {
                        CheckTileAround(tileCheck, 1);
                    }
                }
            }
            else
            {
                bool activeCombineSpecial = false;

                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        var tileAround = GameManager.Instance.GetTile(target.GetPosX() + i, target.GetPosY() + j);

                        if (tileAround != null)
                        {
                            if (i == 0 && j == -1 || i == 0 && j == 1 || i == -1 && j == 0 || i == 1 && j == 0)
                            {
                                if(tileAround.GetTypeTile() == TileType.Red || tileAround.GetTypeTile() == TileType.Yellow || 
                                    tileAround.GetTypeTile() == TileType.Green || tileAround.GetTypeTile() == TileType.Blue)
                                {
                                    activeCombineSpecial = false;
                                }
                                else
                                {
                                    activeCombineSpecial = true;
                                }

                                if (activeCombineSpecial)
                                {
                                    if (tileAround.GetTypeTile() == TileType.RocketRow && target.GetTypeTile() == TileType.Bomb ||
                                    tileAround.GetTypeTile() == TileType.RocketCol && target.GetTypeTile() == TileType.Bomb)
                                    {

                                    }
                                    else if (tileAround.GetTypeTile() == TileType.Bomb && target.GetTypeTile() == TileType.RocketRow ||
                                            tileAround.GetTypeTile() == TileType.Bomb && target.GetTypeTile() == TileType.RocketCol)
                                    {

                                    }
                                    else if (tileAround.GetTypeTile() == TileType.RocketRow && target.GetTypeTile() == TileType.RocketRow ||
                                           tileAround.GetTypeTile() == TileType.RocketCol && target.GetTypeTile() == TileType.RocketCol ||
                                           tileAround.GetTypeTile() == TileType.RocketRow && target.GetTypeTile() == TileType.RocketCol ||
                                           tileAround.GetTypeTile() == TileType.RocketCol && target.GetTypeTile() == TileType.RocketRow)
                                    {

                                    }
                                    else if (tileAround.GetTypeTile() == TileType.Bomb && target.GetTypeTile() == TileType.Bomb)
                                    {

                                    }
                                    else if (tileAround.GetTypeTile() == TileType.Bomb && target.GetTypeTile() == TileType.RocketRow ||
                                            tileAround.GetTypeTile() == TileType.Bomb && target.GetTypeTile() == TileType.RocketCol)
                                    {

                                    }
                                    else if (tileAround.GetTypeTile() == TileType.Bomb && target.GetTypeTile() == TileType.RocketRow ||
                                            tileAround.GetTypeTile() == TileType.Bomb && target.GetTypeTile() == TileType.RocketCol)
                                    {

                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }
            }

            //tileAround.GetTypeTile() == TileType.RocketCol || tileAround.GetTypeTile() == TileType.Bomb ||
            //tileAround.GetTypeTile() == TileType.BoxYellow || tileAround.GetTypeTile() == TileType.BoxGreen || tileAround.GetTypeTile() == TileType.BoxRed ||
            //tileAround.GetTypeTile() == TileType.BoxBlue



        }

        public void GetTileBomb(TileComponent tile)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var tileAround = GameManager.Instance.GetTile(tile.GetPosX() + i, tile.GetPosY() + j);

                    if (tileAround != null)
                    {
                        if (!listTileCheck.Contains(tileAround))
                        {
                            listTileCheck.Add(tileAround);
                        }
                    }
                }
            }
        }

        public void GetTileRocket(TileComponent tile, TileType typeRocket)
        {
            for (int i = 0; i < 8; i++)
            {
                TileComponent tileDestroy = GameManager.Instance.GetTile(i, tile.GetPosY());

                if (typeRocket == TileType.RocketRow)
                {
                    tileDestroy = GameManager.Instance.GetTile(tile.GetPosX(), i);
                }

                if (!listTileCheck.Contains(tileDestroy))
                {
                    listTileCheck.Add(tileDestroy);
                }
            }
        }

        public void GetTileMysteryBox(TileComponent target)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    TileComponent tileDestroy = GameManager.Instance.GetTile(i, j);
                    if(target.GetTypeTile() == TileType.BoxYellow)
                    {
                        if (tileDestroy.GetTypeTile() == TileType.Yellow)
                        {
                            if (!listTileCheck.Contains(tileDestroy))
                            {
                                listTileCheck.Add(tileDestroy);
                            }
                        }
                    }
                    else if (target.GetTypeTile() == TileType.BoxRed)
                    {
                        if (tileDestroy.GetTypeTile() == TileType.Red)
                        {
                            if (!listTileCheck.Contains(tileDestroy))
                            {
                                listTileCheck.Add(tileDestroy);
                            }
                        }
                    }
                    else if (target.GetTypeTile() == TileType.BoxGreen)
                    {
                        if (tileDestroy.GetTypeTile() == TileType.Green)
                        {
                            if (!listTileCheck.Contains(tileDestroy))
                            {
                                listTileCheck.Add(tileDestroy);
                            }
                        }
                    }
                    else if (target.GetTypeTile() == TileType.BoxBlue)
                    {
                        if (tileDestroy.GetTypeTile() == TileType.Blue)
                        {
                            if (!listTileCheck.Contains(tileDestroy))
                            {
                                listTileCheck.Add(tileDestroy);
                            }
                        }
                    }
                }
            }
        }

        public int SetStatusTileTarget(TileComponent target)
        {
            // case Normal
            if (target.GetTypeTile() == TileType.Yellow || target.GetTypeTile() == TileType.Red || target.GetTypeTile() == TileType.Green || target.GetTypeTile() == TileType.Blue)
            {
                return 0;
            }
            // case Special: MysteryBoxColor
            else if (target.GetTypeTile() == TileType.BoxYellow || target.GetTypeTile() == TileType.BoxBlue || target.GetTypeTile() == TileType.BoxGreen || target.GetTypeTile() == TileType.BoxRed)
            {
                return 1;
            }
            // case Special: Bomb / Rocket
            else if (target.GetTypeTile() == TileType.Bomb || target.GetTypeTile() == TileType.RocketCol || target.GetTypeTile() == TileType.RocketRow)
            {
                return 2;
            }
            // case Empty
            else
            {
                return 5;
            }
        }


        public void GetRocketCol(TileComponent tile)
        {
            for (int i = 0; i < 8; i++)
            {
                var tileInCol = GameManager.Instance.GetTile(i, tile.GetPosY());
                if (!listTileCheck.Contains(tileInCol))
                {
                    listTileCheck.Add(tileInCol);
                }
            }
        }

        public void GetRocketRow(TileComponent tile)
        {
            for (int i = 0; i < 8; i++)
            {
                var tileInCol = GameManager.Instance.GetTile(tile.GetPosX(), i);
                if (!listTileCheck.Contains(tileInCol))
                {
                    listTileCheck.Add(tileInCol);
                }
            }
        }

        public void Test()
        {
            var k = 5;
            if (k != 3 || k != 4 || k != 5 || k != 6)
            {
                k = 1;
            }
        }
    }
}