using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace TransportSystem
{
    public class File
    {
        public string FileName { set; get; }
        private TrasportSystem system;
        private Word.Application applicationWord;
        private Word.Document document;
        private Word.Paragraph paragraph;
        public void CreateReport(TrasportSystem system, CheckedListBox checkedListBox)
        {
            this.system = system;
            applicationWord = new Word.Application();
            applicationWord.Visible = true;
            document = applicationWord.Documents.Add();
            this.SetStyleFirstParagraph();
            foreach (int index in checkedListBox.CheckedIndices)
                this.SaveData(index);
            object fileName = FileName;
            document.SaveAs(ref fileName);
            document.Close();
            applicationWord.Quit();
        }
        private void SetStyleFirstParagraph()
        {
            applicationWord.Selection.Paragraphs.Space1();
            applicationWord.Selection.Paragraphs.SpaceAfter = 0;
            paragraph = document.Paragraphs.Add();
            paragraph.Range.Font.Name = "Times New Roman";
            paragraph.Range.Font.Size = 20;
            paragraph.Range.Font.Bold = 1;
            paragraph.Range.Text = "Отчет";
            paragraph.Range.InsertParagraphAfter();
        }
        private void SetStyleParagraph(int fontSize, string fontName, int bold)
        {
            paragraph.Range.InsertParagraphAfter();
            paragraph.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            paragraph.Range.Font.Size = fontSize;
            paragraph.Range.Font.Name = fontName;
            paragraph.Range.Bold = bold;
        }
        private void SaveData(int command)
        {
            switch (command)
            {
                case 0:
                    this.SaveCharacteristic();
                    break;
                case 1:
                    this.SaveMathStatistics();
                    break;
                case 2:
                    this.SaveGraphic();
                    break;
                case 3:
                    this.SaveMatrices();
                    break;
                case 4:
                    this.SavePlanRoutes();
                    break;
            }
        }
        private void SaveCharacteristic()
        {
            SetStyleParagraph(16, "Times New Roman", 1);
            paragraph.Range.Text = string.Format("{0} Параметры генерации маршрутов", char.ConvertFromUtf32(167));
            SetStyleParagraph(10, "Times New Roman", 0);
            paragraph.Range.Text = string.Format("Количество маршрутов: {0}", system.NumberRoute);
            paragraph.Range.Text += string.Format("Диапазон количества остановок: {0}  {2}  {1}", system.MinNumberTransportStop, system.MaxNumberTransportStop, char.ConvertFromUtf32(0x0336));
            paragraph.Range.Text += string.Format("Вместимость транспорта: {0}", system.CapacityTransport);
            paragraph.Range.Text += string.Format("Коэффициент интенсивности пассажиропотока: {0}", system.FactorIntensity);
        }
        private void SaveMathStatistics()
        {
            SetStyleParagraph(16, "Times New Roman", 1);
            paragraph.Range.Text = string.Format("{0} Статистика", char.ConvertFromUtf32(167));
            SetStyleParagraph(12, "Times New Roman", 1);
            paragraph.Range.Text = string.Format("{0} Алгоритм по строкам", char.ConvertFromUtf32(167));
            SetStyleParagraph(10, "Times New Roman", 0);
            Table table = applicationWord.ActiveDocument.Tables.Add(paragraph.Range, 6, system.algorithmRow.MathStatistics.Count + 1);
            table.Borders.Enable = 1;
            foreach (Row row in table.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    cell.Range.Font.Bold = 0;
                    cell.Range.Font.Name = "Bahnschrift SemiBold SemiConden";
                    cell.Range.Font.Size = 8;
                    if (cell.RowIndex == 1)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Кол-во остановок";
                        else cell.Range.Text = system.algorithmRow.MathStatistics[cell.ColumnIndex - 2].numberTransportStop.ToString();
                    }
                    else if (cell.RowIndex == 2)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Кол-во маршрутов";
                        else cell.Range.Text = system.algorithmRow.MathStatistics[cell.ColumnIndex - 2].numberRoute.ToString();
                    }
                    else if (cell.RowIndex == 3)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Минимальное кол-во транспорта";
                        else cell.Range.Text = system.algorithmRow.MathStatistics[cell.ColumnIndex - 2].minNumberTransport.ToString();
                    }
                    else if (cell.RowIndex == 4)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Максимальное кол-во транспорта";
                        else cell.Range.Text = system.algorithmRow.MathStatistics[cell.ColumnIndex - 2].maxNumberTransport.ToString();
                    }
                    else if (cell.RowIndex == 5)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Математическое ожидание";
                        else cell.Range.Text = Math.Round(system.algorithmRow.MathStatistics[cell.ColumnIndex - 2].averageDistribution, 5).ToString();
                    }
                    else
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Дисперсия";
                        else cell.Range.Text = Math.Round(system.algorithmRow.MathStatistics[cell.ColumnIndex - 2].dispersion, 5).ToString();
                    }
                }
            }
            paragraph.Range.Text = string.Format("Время работы алгоритма в секундах: {0}", system.algorithmRow.AlgorithmRunningTime.Elapsed.TotalSeconds);

            SetStyleParagraph(12, "Times New Roman", 1);
            paragraph.Range.Text = string.Format("{0} Алгоритм по строкам", char.ConvertFromUtf32(167));
            SetStyleParagraph(10, "Times New Roman", 0);
            Table table2 = applicationWord.ActiveDocument.Tables.Add(paragraph.Range, 6, system.algorithmColumn.MathStatistics.Count + 1);
            table2.Borders.Enable = 1;
            foreach (Row row in table2.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    cell.Range.Font.Bold = 0;
                    cell.Range.Font.Name = "Bahnschrift SemiBold SemiConden";
                    cell.Range.Font.Size = 8;
                    if (cell.RowIndex == 1)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Кол-во остановок";
                        else cell.Range.Text = system.algorithmColumn.MathStatistics[cell.ColumnIndex - 2].numberTransportStop.ToString();
                    }
                    else if (cell.RowIndex == 2)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Кол-во маршрутов";
                        else cell.Range.Text = system.algorithmColumn.MathStatistics[cell.ColumnIndex - 2].numberRoute.ToString();
                    }
                    else if (cell.RowIndex == 3)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Минимальное кол-во транспорта";
                        else cell.Range.Text = system.algorithmColumn.MathStatistics[cell.ColumnIndex - 2].minNumberTransport.ToString();
                    }
                    else if (cell.RowIndex == 4)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Максимальное кол-во транспорта";
                        else cell.Range.Text = system.algorithmColumn.MathStatistics[cell.ColumnIndex - 2].maxNumberTransport.ToString();
                    }
                    else if (cell.RowIndex == 5)
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Математическое ожидание";
                        else cell.Range.Text = Math.Round(system.algorithmColumn.MathStatistics[cell.ColumnIndex - 2].averageDistribution, 5).ToString();
                    }
                    else
                    {
                        if (cell.ColumnIndex == 1)
                            cell.Range.Text = "Дисперсия";
                        else cell.Range.Text = Math.Round(system.algorithmColumn.MathStatistics[cell.ColumnIndex - 2].dispersion, 5).ToString();
                    }
                }
            }
            paragraph.Range.Text = string.Format("Время работы алгоритма в секундах: {0}", system.algorithmColumn.AlgorithmRunningTime.Elapsed.TotalSeconds);
        }
        private void SavePlanRoutes()
        {
            SetStyleParagraph(16, "Times New Roman", 1);
            paragraph.Range.InsertBreak(Word.WdBreakType.wdSectionBreakNextPage);
            paragraph.Range.Text = string.Format("{0} План Развозок", char.ConvertFromUtf32(167));
            SetStyleParagraph(10, "Times New Roman", 0);
            paragraph.Range.InsertParagraphAfter();
            for (int i = 0; i < system.PlanRoutes.Count; i++)
            {
                SetStyleParagraph(10, "Times New Roman", 1);
                paragraph.Range.Text = "Маршрут " + (i + 1).ToString();
                SetStyleParagraph(10, "Times New Roman", 0);
                for (int j = 0; j < system.PlanRoutes[i].NumberTransport; j++)
                {
                    string text = string.Format("#{0}.{1}:    ", i + 1, j + 1);
                    text += string.Join(", ", system.PlanRoutes[i].RouteTransport[j]);
                    paragraph.Range.Text = text;
                    SetStyleParagraph(10, "Times New Roman", 0);
                }
            }
            paragraph.Range.InsertParagraphAfter();
            paragraph.Range.PageSetup.TextColumns.SetCount(3);
        }
        private void SaveGraphic()
        {
            SetStyleParagraph(16, "Times New Roman", 1);
            paragraph.Range.Text = string.Format("{0} График", char.ConvertFromUtf32(167));
            SetStyleParagraph(10, "Times New Roman", 0);
            Form6 form6 = new Form6(system.algorithmRow.PlanRoutes, system.algorithmColumn.PlanRoutes);
            string pathFile = Path.GetFullPath("Image.jpeg");
            document.InlineShapes.AddPicture(pathFile, Range: paragraph.Range);
        }
        private void SaveMatrices()
        {
            SetStyleParagraph(16, "Times New Roman", 1);
            paragraph.Range.Text = string.Format("{0} Матрица Корреспонденций", char.ConvertFromUtf32(167));
            object direction = Word.WdCollapseDirection.wdCollapseEnd;
            for (int i = 0; i < system.Matrices.Count; i++)
            {
                SetStyleParagraph(10, "Times New Roman", 1);
                paragraph.Range.Text = "Матрица #" + (i + 1).ToString();
                SetStyleParagraph(10, "Times New Roman", 0);
                Table table = applicationWord.ActiveDocument.Tables.Add(paragraph.Range, system.Matrices[i].NumberTransportStop, system.Matrices[i].NumberTransportStop);
                Range range = table.Range;
                range.Collapse(ref direction);
                foreach (Row row in table.Rows)
                {
                    foreach (Cell cell in row.Cells)
                    {
                        cell.Range.Font.Bold = 0;
                        cell.Range.Font.Name = "Bahnschrift SemiBold SemiConden";
                        cell.Range.Font.Size = 8;
                        cell.Range.Text = system.Matrices[i][cell.RowIndex - 1, cell.ColumnIndex - 1].ToString();
                    }
                }
            }
        }
    }
}
