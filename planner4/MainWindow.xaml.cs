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

namespace planner4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            date_text.Text = DateTime.Today.ToLongDateString();
        }



        private void calendar_planner_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime data = (DateTime)calendar_planner.SelectedDate;
            date_text.Text = data.ToLongDateString();
            calendar_planner.Visibility = Visibility.Hidden;
            date_text.Visibility = Visibility;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(count_of_affairs.Text);
            if (n == 7)
            {
                MessageBox.Show("Нельзя записать больше 7 дел на день");
            }
            else
            {
                n++;
                count_of_affairs.Text = n.ToString();
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(count_of_affairs.Text);
            if (n == 1)
            {
                MessageBox.Show("Ты чего дурак какие 0 дел иди работай");
            }
            else
            {
                n--;
                count_of_affairs.Text = n.ToString();
            }
        }


        private void button_calendar_Click(object sender, RoutedEventArgs e)
        {
            calendar_planner.Visibility = Visibility;
            date_text.Visibility = Visibility.Hidden;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

    }
}
