using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace lab3
{
    public class Food : INotifyPropertyChanged
    {
        private string type;
        public Food(string type)
        {
            this.type = type;
        }
        public string Type
        {
            get
            {
                return this.type;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    class Weight
    {
        public int Gramms { get; set; }
    }

    class Order
    {
        public Food Food { get; set; }
        public Weight Weight { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Заказ:\n" + Food.Type + "\n");
            sb.Append(Weight.Gramms + "(г)\n отправлен оператору, ожидайте подтверждения заказа\n");
            return sb.ToString();
        }
    }
    abstract class OrderBuilder
    {
        public Order Order { get; private set; }
        public void CreateOrder()
        {
            Order = new Order();
        }
        public abstract void SetFood(string type);
        public abstract void SetWeight(int gramms);
    }
    class Operator
    {
        public Order Operate(OrderBuilder orderBuilder, int gramms, string type)
        {
            orderBuilder.CreateOrder();
            orderBuilder.SetFood(type);
            orderBuilder.SetWeight(gramms);
            return orderBuilder.Order;
        }
    }
    class FoodOrderBuilder : OrderBuilder
    {
        public override void SetFood(string type)
        {
            this.Order.Food = new Food(type);
        }

        public override void SetWeight(int gramms)
        {
            this.Order.Weight = new Weight { Gramms = gramms };
        }
    }

}