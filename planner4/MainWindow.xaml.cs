using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace planner4
{
    public partial class MainWindow : Window
    {
        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName); 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        public List<string> listik = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            choose_text.IsReadOnly = true;
            choose_text.Visibility = Visibility;
            choose_text.Text = "";
            water_text.IsReadOnly = true;
            mood_text.IsReadOnly = true;
            sleep_text.IsReadOnly = true;
            steps_text.IsReadOnly = true;
            date_text.Visibility = Visibility.Hidden;
            list_item_2.Visibility = Visibility.Hidden;
            list_item_3.Visibility = Visibility.Hidden;
            list_item_4.Visibility = Visibility.Hidden;
            list_item_5.Visibility = Visibility.Hidden;
            list_item_6.Visibility = Visibility.Hidden;
            list_item_7.Visibility = Visibility.Hidden;
            check_box_2.Visibility = Visibility.Hidden;
            check_box_3.Visibility = Visibility.Hidden;
            check_box_4.Visibility = Visibility.Hidden;
            check_box_5.Visibility = Visibility.Hidden;
            check_box_6.Visibility = Visibility.Hidden;
            check_box_7.Visibility = Visibility.Hidden;
            StreamReader sr = new StreamReader("motivation.txt");
            string[] lines = sr.ReadToEnd().Split("\r\n");
            foreach (var item in lines)
            {
                listik.Add(item);
            }
            motivation_text.Visibility = Visibility.Hidden;
        }
        private void calendar_planner_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            image_close_calendar.Visibility = Visibility.Hidden;
            Random rnd = new Random();
            motivation_text.Visibility = Visibility;
            motivation_text.Text = listik[rnd.Next(1,listik.Count())];
            choose_text.Visibility = Visibility.Hidden;
            DateTime data = (DateTime)calendar_planner.SelectedDate;
            date_text.Text = data.ToLongDateString();
            calendar_planner.Visibility = Visibility.Hidden;
            date_text.Visibility = Visibility;
            for (int i = 1; i <= 7; i++)
            {
                string TBText = "plan_" + i;
                FindChild<TextBox>(Application.Current.MainWindow, TBText).Text = "";
            }
            check_box_1.Visibility = Visibility;
            check_box_1.IsChecked = false;
            for (int i = 2; i <= 7; i++)
            {
                string TBCheck= "check_box_" + i;
                FindChild<CheckBox>(Application.Current.MainWindow, TBCheck).IsChecked = false;
                FindChild<CheckBox>(Application.Current.MainWindow, TBCheck).Visibility = Visibility.Hidden;
            }
            for (int i = 2; i <= 7; i++)
            {
                string TBList = "list_item_" + i;
                FindChild<ListBoxItem>(Application.Current.MainWindow, TBList).Visibility = Visibility.Hidden;
            }
            count_of_affairs.Text = "1";
            slider_water.Text = "0";
            slider_mood.Text = "0";
            slider_sleep.Text = "0";
            slider_steps.Text = "0";
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
                    foreach (var item in db.plans.Where(x => x.rel_day_id == dayIndex))//ищем план и записываем его
                    {
                        string TBText = "plan_" + item.plan_position;
                        string TBList = "list_item_" + item.plan_position;
                        string TBCheck = "check_box_" + item.plan_position;
                        var TB = FindChild<TextBox>(Application.Current.MainWindow, TBText);
                        var listTB = FindChild<ListBoxItem>(Application.Current.MainWindow, TBList);
                        var checkTB = FindChild<CheckBox>(Application.Current.MainWindow, TBCheck);
                        listTB.Visibility = Visibility;
                        checkTB.Visibility = Visibility;
                        if (item.check_plan == "True")
                        {
                            checkTB.IsChecked = true;
                        }
                        else
                        {
                            checkTB.IsChecked = false;
                        }
                        TB.Text = item.plan;
                        count_of_affairs.Text = item.count_of_plan.ToString();
                    }
                foreach (var track in db.tracker.Where(x => x.rel_tracker_day_id == dayIndex))
                {
                    slider_water_thumb.Value = track.water;
                    slider_mood_thumb.Value = track.mood;
                    slider_sleep_thumb.Value = track.sleep;
                    slider_steps_thumb.Value = track.steps;
                    slider_water.Text = track.water.ToString();
                    slider_mood.Text = track.mood.ToString();
                    slider_sleep.Text = track.sleep.ToString();
                    slider_steps.Text = track.steps.ToString();
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
                ListBoxItem list = Application.Current.MainWindow.FindName("list_item_" + n) as ListBoxItem;
                TextBox text = Application.Current.MainWindow.FindName("plan_" + n) as TextBox;
                text.Text = "";
                list.Visibility = Visibility.Visible;
                CheckBox checkbox = Application.Current.MainWindow.FindName("check_box_" + n) as CheckBox;
                checkbox.Visibility = Visibility;
                checkbox.IsChecked = false;
                count_of_affairs.Text = n.ToString();
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(count_of_affairs.Text);
            if (n == 1)
            {
                MessageBox.Show("Нельзя записать меньше 0 дел на день");
            }
            else
            {
                ListBoxItem list = Application.Current.MainWindow.FindName("list_item_" + n) as ListBoxItem;
                CheckBox checkbox = Application.Current.MainWindow.FindName("check_box_" + n) as CheckBox;
                checkbox.Visibility = Visibility.Hidden;
                checkbox.IsChecked = false;
                list.Visibility = Visibility.Hidden;
                n--;
                count_of_affairs.Text = n.ToString();
                ListBoxItem item = Application.Current.MainWindow.FindName("list_item_" + (n+1)) as ListBoxItem;
                item.Visibility = Visibility.Hidden;
            }
        }
        private void button_calendar_Click(object sender, RoutedEventArgs e)
        {
            choose_text.Text = "Выберите дату";
            choose_text.Visibility = Visibility;
            calendar_planner.Visibility = Visibility;
            date_text.Visibility = Visibility.Hidden;
            image_close_calendar.Visibility = Visibility;
        }
        private void button_load_Click(object sender, RoutedEventArgs e)
        {
            if (calendar_planner.SelectedDate == null)
            {
                MessageBox.Show("Сначала выберите дату");
                Logger.WriteLine("Ошибка: не выбрана дата перед сохранением данных");
            }
            else
            {
                using (var db = new ConnectBD())
                {
                    var selectedDate = (DateTime)calendar_planner.SelectedDate;
                    var day = db.days.FirstOrDefault(x => x.date == selectedDate);
                    int? dayIndex = day != null ? day.id : null;
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
                    int countaff = 0;
                    for (int i = 1; i <= 7; i++)
                    {
                        string TBText = "plan_" + i;
                        string TBCheck = "check_box_" + i;
                        var text = FindChild<TextBox>(Application.Current.MainWindow, TBText).Text;
                        var checkbox = FindChild<CheckBox>(Application.Current.MainWindow, TBCheck).IsChecked;
                        if (text.Length > 0)
                        {
                            countaff++;
                            db.plans.Add(new planModel { plan = text, rel_day_id = dayIndex.Value, plan_position = i, count_of_plan = countaff, check_plan = checkbox.ToString() });
                        }
                    }
                    db.tracker.Add(new TrackerModel
                    {
                        rel_tracker_day_id = dayIndex.Value,
                        water = int.Parse(slider_water_thumb.Value.ToString()),
                        mood = int.Parse(slider_mood_thumb.Value.ToString()),
                        sleep = int.Parse(slider_sleep_thumb.Value.ToString()),
                        steps = int.Parse(slider_steps_thumb.Value.ToString())
                    });
                    db.SaveChanges();
                }
                MessageBox.Show("Данные сохранены");
            }
        }
        private void Rectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            rectangle_save.Fill = new SolidColorBrush(Color.FromRgb(67, 18, 59));
        }
        private void rectangle_save_MouseLeave(object sender, MouseEventArgs e)
        {
            rectangle_save.Fill = new SolidColorBrush(Color.FromRgb(116, 47, 104));
        }
        private void button_load_MouseEnter(object sender, MouseEventArgs e)
        {
            rectangle_save.Fill = new SolidColorBrush(Color.FromRgb(67, 18, 59));
        }
        private void button_load_MouseLeave(object sender, MouseEventArgs e)
        {
            rectangle_save.Fill = new SolidColorBrush(Color.FromRgb(116, 47, 104));
        }
        private void image_close_calendar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            choose_text.Visibility = Visibility.Hidden;
            date_text.Visibility = Visibility;
            if (calendar_planner.SelectedDate == null)
            {
                MessageBox.Show("Сначала выберите дату"); Logger.WriteLine("Не выбрана дата" +
                "при закрытии календаря");
                choose_text.Visibility = Visibility;
            }
            else {
                DateTime data = (DateTime)calendar_planner.SelectedDate;
                date_text.Text = data.ToLongDateString();
                calendar_planner.Visibility = Visibility.Hidden;
                image_close_calendar.Visibility = Visibility.Hidden;
            }
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Инструкция по использованию ежедневника :" + "\n" +
                "1.Чтобы выбрать дату, нажмите на календарь в левом верхнем углу" + "\n" +
                "2.Ввести значения для трекеров вы можете как и вручную, так и с помощью слайдера" + "\n" +
                "3.Максимальное значение для трекреов:" + "\n" +
                "Трекер воды - от 0 до 10(измеряется в стаканах по 200 мл)" + "\n" +
                "Трекер настроения - от 0 до 10(0 - ужасно, 10 - отлично)" + "\n" +
                "Трекер сна - от 0 до 10(0 - ужасно, 10 - отлично)" + "\n" +
                "Трекер шагов - от 0 до 10 000" + "\n" +
                "4.Чтобы сохранить данные, нужно обязательно нажать на кнопку 'Сохранить'(при обновлении данных соответственно)");
        }
    }
    static class Logger
    {
        //---------------------------------------------------------
        // Статический метод записи строки в файл лога с переносом
        //---------------------------------------------------------
        public static void WriteLine(string message)
        {
            using (StreamWriter sw = new StreamWriter(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\log.txt", true))
            {
                sw.WriteLine(String.Format("{0,-23} {1}", DateTime.Now.ToString() + ":", message));
            }
        }
    }
}
