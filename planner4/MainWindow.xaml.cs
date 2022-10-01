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
        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
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
            for (int i = 1; i <= 7; i++)
            {
                string TBText = "plan_" + i;
                FindChild<TextBox>(Application.Current.MainWindow, TBText).Text = "";
                
            }
            using (var db = new ConnectBD())
            {
                var selectedDate = data;
                var day = db.days.FirstOrDefault(x => x.date == selectedDate);
                int? dayIndex = day != null ? day.id : null; //записан ли такой день, если нет, то записывем именно дату и сохраняем
                if (!dayIndex.HasValue)
                {
                    var selectedDay = new dayModel { date = data };
                    db.days.Add(selectedDay);
                    db.SaveChanges();
                    dayIndex = selectedDay.id;
                }

                foreach (var item in db.plans.Where(x=> x.rel_day_id == dayIndex))//ищем план и записываем его
                {
                    string TBText = "plan_" + item.plan_position;
                    var TB = FindChild<TextBox>(Application.Current.MainWindow, TBText);
                    TB.Text = item.plan;
                }
            }
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
        public Boolean isDataExist()
        {
            return false;
        }
        private void button_load_Click(object sender, RoutedEventArgs e)
        {
            if (isDataExist()) return;
            using (var db = new ConnectBD())
            {
                var selectedDate = (DateTime)calendar_planner.SelectedDate;
                var day = db.days.FirstOrDefault(x => x.date == selectedDate);
                int? dayIndex = day!=null ? day.id:null;
                if (!dayIndex.HasValue)
                {
                    var selectedDay = new dayModel { date = selectedDate };
                    db.days.Add(selectedDay);
                    db.SaveChanges();
                    dayIndex = selectedDay.id;
                }
                else
                {
                    db.plans.RemoveRange(db.plans.Where(x => x.rel_day_id == dayIndex));
                    db.SaveChanges();
                }
                
                
                for (int i = 1; i <= 7; i++)
                {
                    string TBText = "plan_" + i;
                    var text = FindChild<TextBox>(Application.Current.MainWindow, TBText).Text;
                    if (text.Length>0)
                    {
                        db.plans.Add(new planModel { plan = text, rel_day_id = dayIndex.Value, plan_position=i});
                    }
                }
                db.SaveChanges();
            }
        }
    }
}
