using System;
using System.Text;

namespace senkovlad.blazor.components.Helpers
{
    public class StyleGenerator
    {
        public static void GenerateStyle(ref string style, string columnsDefenition, string rowsDefenition, string width, string heidht)
        {
            style += "display: grid;";
            style += "grid-template-columns: " + columnsDefenition + ";";
            style += "grid-template-rows: " + rowsDefenition + ";";
            style += "width: " + width + ";";
            style += "Height: " + heidht + ";";
        }

        public static void GenerateChildStyle(ref string style, int column, int row, int columnSpan, int rowSpan, string width, string heidht)
        {
            style += "grid-column: " + GetColumnOrRow(column, columnSpan) + ";";
            style += "grid-row: " + GetColumnOrRow(row, rowSpan) + ";";
            style += "width: " + width + ";";
            style += "Height: " + heidht + ";";
        }

        private static string GetColumnOrRow(int colOrRow, int colOrRowSpan)
        {
            var sb = new StringBuilder();

            sb.Append(colOrRow > 1 ? colOrRow : 1);
            sb.Append("/span ");
            sb.Append(colOrRowSpan > 1 ? colOrRowSpan : 1);

            return sb.ToString();
        }
    }
}
