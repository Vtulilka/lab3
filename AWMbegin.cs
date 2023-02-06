using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace lab3
{
    public class AWMbegin : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string path = "C:\\Users\\Виктория\\Desktop\\Study\\Программирование\\orders.txt";

        private Food selectedFood;

        private ObservableCollection<Food> food;

        private NumOrder selectedOrder;

        private ObservableCollection<NumOrder> numorder;

        private Transport selectedTransport;

        private ObservableCollection<Transport> transport;

        public AWMbegin()
        {
            transport = new ObservableCollection<Transport>()
            {
                new Transport("Машина"),
                new Transport("Поезд"),
                new Transport("Самолёт")
            };
            numorder = new ObservableCollection<NumOrder>();
            food = new ObservableCollection<Food>()
            {
                new Food("Пицца"),
                new Food("Суши"),
                new Food("Стрипсы")
            };
        }
        public Food SelectedFood
        {
            get
            {
                return (this.selectedFood);
            }
            set
            {
                this.selectedFood = value;
                OnPropertyChanged("SelectedFood");
            }
        }

        public ObservableCollection<Food> Foods
        {
            get
            {
                return this.food;
            }
        }

        public NumOrder SelectedOrder
        {
            get
            {
                return (this.selectedOrder);
            }
            set
            {
                this.selectedOrder = value;
                OnPropertyChanged("SelectedOrder");
            }
        }

        public ObservableCollection<NumOrder> NumOrders
        {
            get
            {
                return this.numorder;
            }
        }
        public Transport SelectedTransport
        {
            get
            {
                return (this.selectedTransport);
            }
            set
            {
                this.selectedTransport = value;
                OnPropertyChanged("SelectedTransport");
            }
        }

        public ObservableCollection<Transport> Transports

        {
            get
            {
                return this.transport;
            }
        }

        private int Price(int price, string city, string transport)
        {
            switch (city)
            {
                case "Москва":
                    {
                        switch (transport)
                        {
                            case "Машина":
                                {
                                    price = price * 4 + 250;
                                    break;
                                }
                            case "Поезд":
                                {
                                    price = price * 4 + 500;
                                    break;
                                }
                            case "Самолёт":
                                {
                                    price = price * 4 + 1000;
                                    break;
                                }
                        }
                        break;
                    }
                case "Санкт-Петербург":
                    {
                        switch (transport)
                        {
                            case "Машина":
                                {
                                    price = price * 3 + 250;
                                    break;
                                }
                            case "Поезд":
                                {
                                    price = price * 3 + 500;
                                    break;
                                }
                            case "Самолёт":
                                {
                                    price = price * 3 + 1000;
                                    break;
                                }
                        }
                        break;
                    }
                case "Владивосток":
                    {
                        switch (transport)
                        {
                            case "Машина":
                                {
                                    price = price * 2 + 250;
                                    break;
                                }
                            case "Поезд":
                                {
                                    price = price * 2 + 500;
                                    break;
                                }
                            case "Самолёт":
                                {
                                    price = price * 2 + 1000;
                                    break;
                                }
                        }
                        break;
                    }
            }
            return price;
        }
        
        private void WriteFile(string path, string text)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLineAsync(text);
            }
        }

        private Command makeOrder;

        public Command MakeOrder
        {
            get
            { 
                return makeOrder ?? (makeOrder = new Command(obj =>
                {
                    if (selectedFood != null)
                    {
                        Operator operator_ = new Operator();
                        OrderBuilder builder = new FoodOrderBuilder();
                        Order order = operator_.Operate(builder, int.Parse(obj.ToString()), SelectedFood.Type);
                        MessageBox.Show(order.ToString());
                        string str = SelectedFood.Type + " --- " + obj.ToString();
                        numorder.Add(new NumOrder(str));
                    }
                    else MessageBox.Show("Заказ некорректен!");
                }));
            }
        }

        string priceStr;
        int price;
        private Command sendOrder;
        public Command SendOrder
        {
            get
            {
                return sendOrder ?? (sendOrder = new Command(obj =>
                {
                    if (selectedOrder != null && selectedTransport != null)
                    {
                        if ((SelectedOrder.Order_.Contains("Пицца") && SelectedTransport.Tran.Contains("Самолёт"))
                        || (SelectedOrder.Order_.Contains("Суши") && SelectedTransport.Tran.Contains("Машина"))
                        || (SelectedOrder.Order_.Contains("Стрипсы") && SelectedTransport.Tran.Contains("Поезд")))
                            MessageBox.Show("Отправка невозможна, попробуйте выбрать другой заказ или транспорт");
                        else
                        {
                            Departurer departurer_ = new Departurer();
                            DepartureBuilder builder = new FoodDepartureBuilder();
                            Departure departure = departurer_.Depart(builder, SelectedOrder.Order_, SelectedTransport.Tran, obj.ToString());
                            priceStr = SelectedOrder.Order_;
                            priceStr = priceStr.Substring(priceStr.Length - 3);
                            price = int.Parse(priceStr);
                            price = Price(price, obj.ToString(), SelectedTransport.Tran);
                            numorder.Remove(SelectedOrder);
                            MessageBox.Show(departure.ToString() + "\nЦена: " + price + "рублей");
                            WriteFile(path, departure.ToString() + "\nЦена: " + price + "рублей\n");
                        }
                        
                    }
                    else MessageBox.Show("Сначала нужно выбрать заказ и транспорт!");
                }));
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
