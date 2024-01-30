using System.Windows.Forms;

namespace WorkDivision
{
    internal class Division
    {
        public static string id { get; set; }       //ID разделения
        public static string mm { get; set; }       //месяц
        public static string yy { get; set; }       //год
        public static string product { get; set; }  //изделие
        public static string model { get; set; }    //модель
        public static double absSumCost { get; set; }  //Суммарная расценка
        public static double absSumNVR { get; set; }     //Суммарная норма времени
        public static double absSumItem { get; set; }    //Суммарная стоимость


        // авторасширение колонок listView по размеру содержимого
        public static void autoResizeColumns(ListView lv)
        {
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            ListView.ColumnHeaderCollection cc = lv.Columns;
            for (int i = 0; i < cc.Count; i++)
            {

                int colWidth = TextRenderer.MeasureText(cc[i].Text, lv.Font).Width + 10;
                if (i > 0)
                {
                    if (colWidth > cc[i].Width)
                    {
                        if (cc[i].Text == "Операция") 
                        {
                            cc[i].Width = colWidth+700;
                        }
                        else
                        {     
                            cc[i].Width = colWidth;                         
                        }
                    }
                }
                else
                {
                    //Если справочник операций, отображать id
                    if (lv.Name != "lvDirOpers")
                    {
                        cc[i].Width = 0;
                    }
                }
            }
        }
    }
}
