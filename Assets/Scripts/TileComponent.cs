using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bejeweled
{
    public enum TileType { Empty, RocketRow, RocketCol, Bomb, BoxYellow, BoxGreen, BoxRed, BoxBlue, Yellow, Green, Red, Blue }

    public class TileComponent : MonoBehaviour
    {
        bool isSelected = false;

        int x;
        int y;
        int index;
        Color color;
        bool isChecked;

        [SerializeField]
        Text text;

        TileType currentType;

        public static Action<TileComponent> onInteract;

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
                    break;
                case TileType.RocketCol:
                    this.SetColor(Color.grey);
                    break;
                case TileType.RocketRow:
                    this.SetColor(Color.cyan);
                    break;
                case TileType.Bomb:
                    this.SetColor(Color.black);
                    break;
                case TileType.BoxYellow:
                    this.SetColor(Color.cyan);
                    break;
                case TileType.BoxGreen:
                    this.SetColor(Color.cyan);
                    break;
                case TileType.BoxRed:
                    this.SetColor(Color.cyan);
                    break;
                case TileType.BoxBlue:
                    this.SetColor(Color.cyan);
                    break;
                case TileType.Yellow:
                    this.SetColor(Color.yellow);
                    break;
                case TileType.Green:
                    this.SetColor(Color.green);
                    break;
                case TileType.Red:
                    this.SetColor(Color.red);
                    break;
                case TileType.Blue:
                    this.SetColor(Color.blue);
                    break;
            }
        }

        private void SetColor(Color colorSet)
        {
            var a = this.GetComponent<Image>();
            a.color = colorSet;
            color = colorSet;
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