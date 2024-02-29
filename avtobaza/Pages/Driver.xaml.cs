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

namespace avtobaza.Pages
{
    /// <summary>
    /// Логика взаимодействия для Driver.xaml
    /// </summary>
    public partial class Driver : Page
    {
        public Driver()
        {
            InitializeComponent();
        }
        public void SetGreetingAndName(string greeting, string fullNameV)
        {
            userInfoTextBlock.Text = $"{greeting}, {fullNameV}!";
        }
    }
}
