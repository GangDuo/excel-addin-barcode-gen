using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarcodeGen.Settings
{
    public sealed class Config
    {
        private static volatile Config instance;
        private static object syncRoot = new Object();

        private Config()
        {
            Height = 60;
            Width = 200;
            Margin = 5;
        }

        public static Config Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Config();
                    }
                }

                return instance;
            }
        }

        public int Height { get; set; }
        public int Width { get; set; }
        public int Margin { get; set; }
    }
}
