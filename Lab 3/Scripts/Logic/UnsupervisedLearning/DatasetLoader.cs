using System.Globalization;
using Microsoft.Data.Analysis;

namespace Lab3;

internal class DatasetLoader
{
    public float[,] LoadData(string filename)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        var dataFrame = DataFrame.LoadCsv(filename, ',', false);

        var rows = (int)dataFrame.Rows.Count;
        var cols = dataFrame.Columns.Count;
        var data = new float[rows, cols];

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                data[i, j] = Convert.ToSingle(dataFrame[i, j]);
            }
        }

        return data;
    }
}