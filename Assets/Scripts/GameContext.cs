using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Bejeweled
{
    public class GameContext : MonoBehaviour
    {
        //public TileComponent tileSelected = null;
        public List<TileComponent> listTileCheck = new List<TileComponent>();
        //public List<TileComponent> listClear = new List<TileComponent>();

        // if (typeTarget = 0 => normal) else if typeTarget = 1 => special  else if typeTarget = 2 => empty
        //public int statusTile = 0;

        private void Awake()
        {
            TileComponent.onInteract = OnInteract;
        }

        public void Start()
        {
            //CheckMapHas();
        }

        public void OnInteract(TileComponent target)
        {
            //Debug.Log("Tile Check Now: " + " x: " + target.GetPosX() + ", y:" + target.GetPosY());
            target.value += 1;
            target.SetTextPos(target.value);

            // Check normal
            listTileCheck.Add(target);
            CheckTileAround(target);
            //CreateSpecialTile(target);     // Check list clear to create special tile

            // Clear List
            if (listTileCheck.Count > 2)
            {
                foreach (TileComponent tile in listTileCheck)
                {
                    if (tile != target)
                    {
                        tile.SetType(TileType.Empty);
                    }
                }

                //Debug.Log("so luong o empty" + listTileCheck.Count);

                // Set cac o roi xuong 
                DropTile();
                //for (int i = 0; i < 5; i++) // Xet hang ngang 
                //{
                //    for (int j = 4; j >= 0; j--) // Xet hang doc ~> Logic chinh 
                //    {
                //        TileComponent tile = GameManager.Instance.GetTile(i, j);
                //        //Debug.Log(tile.GetPosX() + ":" + tile.GetPosY() + "~>" + tile.value);
                //    }
                //}
            }
            ClearListCheck();

            bool checkMap = false;

            do
            {
                checkMap = CheckMapHas();
                Debug.Log("qiwueh" + checkMap);

            } while (checkMap == true);

        }

        public TileComponent FindTileTarget(List<TileComponent> listTileClear)
        {
            int yTile = 0;
            List<TileComponent> listCompare = new List<TileComponent>();
            listCompare.Add(listTileClear[0]);

            foreach (TileComponent t in listTileClear)
            {
                if (t.GetPosY() > yTile)
                {
                    yTile = t.GetPosY();
                    listCompare.Clear();
                    listCompare.Add(t);
                }
                else if (t.GetPosY() == yTile)
                {
                    //Debug.Log(t.GetPosX() + "asdas" + listCompare[0].GetPosX());
                    if (t.GetPosX() < listCompare[0].GetPosX())
                    {
                        listCompare.Clear();
                        listCompare.Add(t);
                    }
                }
            }

            TileComponent target = listCompare[0];
            return target;
        }

        public bool CheckMapHas()
        {
            bool isMapHasEat = false;
            for (int i = 0; i < 5; i++) // Xet hang ngang 
            {
                for (int j = 0; j < 5; j++) // Xet hang doc ~> Logic chinh 
                {
                    TileComponent tileCheck = GameManager.Instance.GetTile(i, j);

                    listTileCheck.Add(tileCheck);
                    CheckTileAround(tileCheck);

                    if (listTileCheck.Count > 2)
                    {
                        foreach (TileComponent t in listTileCheck)
                        {
                            Debug.Log("t in list" + t.GetPosX() + ":" + t.GetPosY() + "Value: " + t.value);
                        }

                        TileComponent target = FindTileTarget(listTileCheck);
                        target.value += 1;
                        target.SetTextPos(target.value);
                        Debug.Log("target in list" + target.GetPosX() + ":" + target.GetPosY() + "Value: " + target.value);

                        foreach (TileComponent tile in listTileCheck)
                        {
                            if (tile != target)
                            {
                                tile.SetType(TileType.Empty);
                            }
                        }
                        DropTile();

                        //Debug.Log("Clear something");

                        ClearListCheck();
                        isMapHasEat = true;
                        return isMapHasEat;
                    }
                    //else
                    //{
                    //    isMapCurrentHasX = false;
                    //}

                    ClearListCheck();
                }
            }

            return isMapHasEat;
            //Debug.Log("break");
        }

        public void ClearListCheck()
        {
            for (int i = 0; i < 5; i++) // Xet hang ngang 
            {
                for (int j = 0; j < 5; j++) // Xet hang doc ~> Logic chinh 
                {
                    //Debug.Log(i + "zxcxzc" + j);
                    TileComponent tileCheck = GameManager.Instance.GetTile(i, j);
                    tileCheck.SetCheckedDone(false);
                }
            }
            listTileCheck.Clear();
        }

        public void DropTile()
        {
            // Xét hàng dọc từ dưới lên ~>
            // - Nếu ô nào trạng thái là Empty thì thêm ô đó vào listTileReplace 
            // - Nếu không phải là trạng thái Empty thì sẽ set lại giá trị ô đấy vào ô cuối cùng của listTileReplace 
            // sau khi set lại giá trị thì thêm ô đang check vào listTileReplace
            // - Sau khi check xong cột thì khởi tạo số ô mới bằng số ô trong listTileReplace

            for (int i = 0; i < 5; i++) // Xet hang ngang 
            {
                List<TileComponent> listTileReplace = new List<TileComponent>();
                for (int j = 4; j >= 0; j--) // Xet hang doc ~> Logic chinh 
                {
                    TileComponent tile = GameManager.Instance.GetTile(i, j);
                    if (tile.GetTypeTile() == TileType.Empty)
                    {
                        //Debug.Log("tile Empty" + tile.GetPosX() + ":" + tile.GetPosY() + "~> value:" + tile.value);
                        listTileReplace.Add(tile);
                    }
                    else
                    {
                        if (listTileReplace.Count > 0)
                        {
                            TileComponent tileReplace = listTileReplace[0];
                            tileReplace.SetType(tile.GetTypeTile());
                            tileReplace.value = tile.value;
                            tileReplace.SetTextPos(tile.value);
                            listTileReplace.RemoveAt(0);

                            tile.SetType(TileType.Empty);
                            listTileReplace.Add(tile);
                        }
                    }

                    // Khởi tạo ô mới bằng số ô trong listTileReplace
                    if (j == 0)
                    {
                        foreach (TileComponent t in listTileReplace)
                        {
                            int x = Random.Range(1, 5);
                            t.value = x;
                            t.SetTextPos(x);
                            t.SetType(TileType.Normal);
                            Debug.Log("Create tile " + t.GetPosX() + ":" + t.GetPosY() + "~> value:" + t.value);
                        }
                    }
                }
            }
        }

        public void CreateSpecialTile(TileComponent target)
        {

        }

        public void CheckTileAround(TileComponent target)
        {
            //Debug.Log("Tile Check Now: " + " x: " + target.GetPosX() + ", y:" + target.GetPosY());

            // Type = 1 => Check Tile Normal / Type = 2 => Check Special (include: Special Alone and Special Combine)
            target.SetCheckedDone(true);

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var tileAround = GameManager.Instance.GetTile(target.GetPosX() + i, target.GetPosY() + j);
                    if (tileAround != null && !tileAround.GetIsChecked())
                    {
                        // Check tren duoi trai phai, ~> khong check cheo
                        if (i == 0 && j == -1 || i == 0 && j == 1 || i == -1 && j == 0 || i == 1 && j == 0)
                        {
                            if (target.value == tileAround.value)
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

            foreach (TileComponent tileCheck in listTileCheck)
            {
                if (!tileCheck.GetIsChecked())
                {
                    CheckTileAround(tileCheck);
                }
            }
        }
    }
}