using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using ZXing;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;
using Office = Microsoft.Office.Core;

namespace BarcodeGen
{
    public partial class Ribbon
    {
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void button1_Click(object sender, RibbonControlEventArgs e)
        {
            // 範囲選択
            var dialog = new Excel.Helpers.RangeDialog(Globals.ThisAddIn.Application);
            dialog.Show();
            if (dialog.IsCanceled)
            {
                return;
            }
            var rngTarget = dialog.Data;
            // 縦横位置を中央揃え
            rngTarget.HorizontalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;
            rngTarget.VerticalAlignment = Microsoft.Office.Interop.Excel.Constants.xlCenter;

            var bacodeWriter = new BarcodeWriter();

            // バーコードの種類
            bacodeWriter.Format = BarcodeFormat.EAN_13;

            // サイズ
            bacodeWriter.Options.Height = Settings.Config.Instance.Height;//pixels
            bacodeWriter.Options.Width = Settings.Config.Instance.Width;

            // バーコード左右の余白
            bacodeWriter.Options.Margin = Settings.Config.Instance.Margin;

            // バーコードのみ表示するか
            // falseにするとテキストも表示する
            bacodeWriter.Options.PureBarcode = false;

            // 作業フォルダ生成
            var suffix = System.Guid.NewGuid().ToString("N").Substring(0, 8);
            var appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            var baseDirName = Path.Combine(Path.GetTempPath(), appName + suffix);
            var workDir = Directory.CreateDirectory(baseDirName);

            foreach (Microsoft.Office.Interop.Excel.Range cell in rngTarget)
            {
                string janCode = cell.Text.ToString().Trim();
                if (janCode.Length == 0) continue;
                // バーコードBitmapを作成
                using (var bitmap = bacodeWriter.Write(janCode))
                {
                    // 画像として保存
                    var filePath = Path.Combine(workDir.FullName, janCode + ".png");
                    if (!File.Exists(filePath))
                    {
                        bitmap.Save(filePath, ImageFormat.Png);
                    }

                    var pic = new PictureOnSheet(filePath);
                    pic.PasteIn(cell);
                }
            }
        }

        private void group1_DialogLauncherClick(object sender, RibbonControlEventArgs e)
        {
            Globals.ThisAddIn.ShowSettingsPane();
        }

        class PictureOnSheet
        {
            private string Path;

            public PictureOnSheet(string path)
            {
                Path = path;
            }

            public void PasteIn(Microsoft.Office.Interop.Excel.Range cell)
            {
                cell.ColumnWidth = 17;
                cell.RowHeight = 33;
                //barcode.Save(path);

                //string datas = BarcodeScanner.ScanOne(path);
                var LinkToFile = false;
                var saveWithDocument = true;
                var Left = cell.Left + 2;
                var Top = cell.Top + 2;
                var Width = 0;
                var Height = 0;
                Microsoft.Office.Interop.Excel.Shape myShape = Globals.ThisAddIn.Application.ActiveSheet.Shapes.AddPicture(
                      Path,
                      LinkToFile,
                      saveWithDocument,
                      Left,
                      Top,
                      Width,
                      Height);
                ////'--(2) 挿入した画像に対して元画像と同じ高さ・幅にする
                myShape.ScaleHeight(0.6F, Office.MsoTriState.msoTrue);
                myShape.ScaleWidth(0.6F, Office.MsoTriState.msoTrue);

                //File.Delete(path);
            }
        }
    }
}
