using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Hardware.Print;
using ZXing;
using ZXing.Common;
using static Hardware.Print.Printer;

namespace IdoMaster_GensouWorld.Activitys.MainPage
{
    public class AsakuraPrinterAssisted
    {
        private Printer m_printer;

        public  void InitPrint()
        {
            m_printer = m_printer == null ? new Printer() : this.m_printer;
        }

        public void OpenPrinter()
        {
            Printer.Open();
            Printer.SetGrayLevel(0x05);
        }


        public void ClosePrinter()
        {
            Printer.Close();
        }

        private Bitmap Encode(string content, int width, int height, BarcodeFormat barcodeformat)
        {
            BitMatrix byteMatrix = new MultiFormatWriter().encode(
                content, barcodeformat, width, height);
            Bitmap bitmap = ToBitmap(byteMatrix);
            return bitmap;
        }

        private static Bitmap ToBitmap(BitMatrix matrix)
        {
            int width = matrix.Width;
            int height = matrix.Height;
            Bitmap bmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            for (int y = 0; y < height; y++)
            {
                int offset = y * width;
                for (int x = 0; x < width; x++)
                {
                    bmap.SetPixel(x, y, matrix[x, y] ? Color.Black : Color.White);
                }
            }
            return bmap;
        }

        public void PrintText()
        {
            m_printer.PrintStringEx("\nPrinter test\n", 40, false, true, Printer.PrintType.Centering);


        }


        private void PrintLine(string content, PrintType printType)
        {
            m_printer.PrintLineInit(16);
            m_printer.PrintLineString(content, 16, printType, true);
            m_printer.PrintLineEnd();
            GoToNextPage();
        }

        private void PrintLine(string content, int textSize, PrintType printType)
        {


        }
        private void PrintLine(string content, int textSize, bool isHaveUnderLine, PrintType printType)
        {


        }
        private void PrintLine(string content, int textSize, bool isHaveUnderLine, bool isBold, PrintType printType)
        {


        }
    }
}