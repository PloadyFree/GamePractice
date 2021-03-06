﻿using Battleship.Base;

namespace Battleship.Interfaces
{
    public interface IGameCell
    {
        CellPosition Position { get; }
        bool Damaged { get; set; }
    }
}
