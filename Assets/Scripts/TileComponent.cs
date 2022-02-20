using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bejeweled
{
    public enum TileType { Empty, Normal, Special }

    public class TileComponent : MonoBehaviour
    {
        bool isSelected = false;

        public int x;
        public int y;
        int index;
        public Color color;
        bool isChecked;
        public int value;

        public int numberTileOrder = 0;

        [SerializeField]
        Text text;

        TileType currentType;

        public static Action<TileComponent> onInteract;

        public void SetValueTileDrop(TileComponent tileDrop)
        {
            color = tileDrop.color;
            value = tileDrop.value;
            currentType = tileDrop.GetTypeTile();
        }

        public void OnHitTile()
        {
            onInteract?.Invoke(this);
        }

        public bool GetIsChecked()
        {
            return isChecked;
        }

        public void SetCheckedDone(bool isCheck)
        {
            isChecked = isCheck;
        }

        public TileType GetTypeTile()
        {
            return currentType;
        }

        public void SetType(TileType type)
        {
            currentType = type;

            switch (currentType)
            {
                case TileType.Empty:
                    this.SetColor(Color.white);
                    this.value = -1;
                    SetTextPos(this.value);
                    break;
                case TileType.Normal:
                    //this.SetColor(Color.red);
                    break;
                case TileType.Special:
                    //this.SetColor(Color.blue);
                    break;
            }
        }

        private void SetColor(Color colorSet)
        {
            var a = this.GetComponent<Image>();
            a.color = colorSet;
            color = colorSet;
        }

        public void SetNewValue()
        {
            this.value += 1;
            SetTextPos(this.value);
        }

        public void SetTextPos(int value)
        {
            text.text = value + "";

            switch (value)
            {
                case 1:
                    this.SetColor(Color.green);
                    break;
                case 2:
                    this.SetColor(Color.blue);
                    break;
                case 3:
                    this.SetColor(Color.red);
                    break;
                case 4:
                    this.SetColor(Color.yellow);
                    break;
                case 5:
                    this.SetColor(Color.gray);
                    break;
                case 6:
                    this.SetColor(Color.cyan);
                    break;
                case 7:
                    this.SetColor(Color.black);
                    break;
                case 8:
                    //this.SetColor(Color.red);
                    break;
                case 9:
                    //this.SetColor(Color.white);
                    break;
                case 10:
                    //this.SetColor(Color.red);
                    break;
                default:
                    this.SetColor(Color.white);
                    break;
            }

            //SetColor(Color.yellow);
        }

        public void SetTextPos(int x, int y)
        {
            text.text = x + ", " + y;
        }

        public void SetPosX(int posX)
        {
            x = posX;
        }

        public void SetPosY(int posY)
        {
            y = posY;
        }

        public void SetIndex(int indexTile)
        {
            index = indexTile;
        }

        public void SetTileSelected(bool select)
        {
            isSelected = select;
        }

        public int GetPosX()
        {
            return x;
        }

        public int GetPosY()
        {
            return y;
        }

        public bool GetTileSelected()
        {
            return isSelected;
        }
    }
}