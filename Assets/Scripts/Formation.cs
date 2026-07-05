using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Formation
{
    public abstract List<List<Vector3>> CalculatePositions(int unitCount, Vector3 centerPosition, Vector3 direction, int maxRow, float spacing);
}

class LineFormation : Formation
{
    public override List<List<Vector3>> CalculatePositions(int unitCount, Vector3 centerPosition, Vector3 direction, int maxRow, float spacing)
    {
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> directions = new List<Vector3>();

        bool flag = maxRow % 2 == 0;

        int index = 0;
        int countRow = 0;
        Vector3 right = new Vector3(direction.z, 0.0f, -direction.x);
        float rowOffset = flag ? 0.5f : 0.0f;
        float columnOffset = 0.0f;

        while (index < unitCount)
        {
            positions.Add(centerPosition + (rowOffset * spacing) * right + (columnOffset * spacing) * direction);
            directions.Add(direction);

            if(columnOffset % 2 == 0)
            {
                if (rowOffset == 0.0f)
                {
                    rowOffset -= 1;
                }
                else if (rowOffset < 0)
                {
                    if (flag)
                        rowOffset -= 1;

                    rowOffset = -rowOffset;
                }
                else
                {
                    rowOffset = -rowOffset;

                    if (!flag)
                        rowOffset -= 1;
                }
            }
            else
            {
                if (rowOffset == 0.0f)
                {
                    rowOffset -= 1;
                }
                else if (rowOffset < 0)
                {
                    rowOffset = -rowOffset;
                }
                else
                {
                    rowOffset = -rowOffset;
                    rowOffset -= 1;
                }
            }

            index++;
            countRow++;

            if (countRow == maxRow)
            {
                columnOffset -= 1;
                countRow = 0;

                if(columnOffset % 2 == 0)
                {
                    rowOffset = flag ? 0.5f : 0.0f;
                }
                else
                {
                    rowOffset = flag ? -0.5f : 0.0f;
                }
            }
        }

        List<List<Vector3>> result = new List<List<Vector3>>();
        
        result.Add(positions);
        result.Add(directions);

        return result;
    }

}

class SquareFormation : Formation
{
    public override List<List<Vector3>> CalculatePositions(int unitCount, Vector3 centerPosition, Vector3 direction, int maxRow, float spacing)
    {
        List<Vector3> positions = new List<Vector3>();
        List<Vector3> directions = new List<Vector3>();
        
        maxRow = (int)Math.Round(Math.Sqrt(unitCount), 0);

        int index = 0;
        Vector3 right = new Vector3(direction.z, 0.0f, -direction.x);
        float maxRowOffset = (maxRow - 1) / 2.0f;
        float minRowOffset = -maxRowOffset;
        float maxColumnOffset = 0.0f;
        float minColumnOffset = Math.Pow(maxRow, 2) < unitCount ? -maxRow : -(maxRow - 1);

        float rowOffset = minRowOffset;
        float columnOffset = maxColumnOffset;

        Vector3 newDirection = Vector3.zero;

        while (index < unitCount)
        {
            positions.Add(centerPosition + rowOffset * spacing * right + columnOffset * spacing * direction);
            directions.Add(newDirection);

            if (rowOffset < maxRowOffset && columnOffset == maxColumnOffset)
            {
                rowOffset += 1;

                if (rowOffset == maxRowOffset && columnOffset == maxColumnOffset)
                {
                    maxColumnOffset -= 1;
                }

                newDirection = direction;
            }
            else if (columnOffset > minColumnOffset && rowOffset == maxRowOffset)
            {
                columnOffset -= 1;

                if (columnOffset == minColumnOffset && rowOffset == maxRowOffset)
                {
                    maxRowOffset -= 1;
                }

                newDirection = new Vector3(direction.z, 0.0f, -direction.x);
            }
            else if (rowOffset > minRowOffset && columnOffset == minColumnOffset)
            {
                rowOffset -= 1;

                if (rowOffset == minRowOffset && columnOffset == minColumnOffset)
                {
                    minColumnOffset += 1;
                }

                newDirection = direction * -1;
            }
            else if (columnOffset < maxColumnOffset && rowOffset == minRowOffset)
            {
                columnOffset += 1;

                if (columnOffset == maxColumnOffset && rowOffset == minRowOffset)
                {
                    minRowOffset += 1;
                }

                newDirection = new Vector3(-direction.z, 0.0f, direction.x);
            }

            index++;
        }

        List<List<Vector3>> result = new List<List<Vector3>>();
        
        result.Add(positions);
        result.Add(directions);

        return result;
    }

}
