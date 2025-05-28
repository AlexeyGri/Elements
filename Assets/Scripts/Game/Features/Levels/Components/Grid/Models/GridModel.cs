using System;

namespace Game.Features.Levels.Components.Grid.Models
{
    [Serializable]
    public class GridModel
    {
        public int Columns;
        public int Rows;

        public GridModel(int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
        }
    }
}