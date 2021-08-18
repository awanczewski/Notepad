using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NET_Lab4.Controls
{
    /// <summary>
    /// Interaction logic for ToolTipContent.xaml
    /// </summary>
    public partial class ToolTipContent : UserControl
    {
        private string header;
        private string description;

        public string Header { get => header; set => header = value; }
        public string Description { get => description; set => description = value; }

        public ToolTipContent()
        {
            InitializeComponent();
            this.DataContext = this;
        }


    }
}
