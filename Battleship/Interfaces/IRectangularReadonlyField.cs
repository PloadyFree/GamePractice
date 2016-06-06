﻿using System.Drawing;
using Battleship.Base;
using Battleship.Implementations;

namespace Battleship.Interfaces
{
    public interface IRectangularReadonlyField<out T>
    {
        Size Size { get; }
        T this[CellPosition position] { get; }
    }
}
