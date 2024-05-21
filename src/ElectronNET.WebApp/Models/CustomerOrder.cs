using System;
using System.Collections.Generic;

namespace ElectronNET.WebApp.Models
{
    public class CustomerOrder
    {
        public Guid OrderId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsPayment { get; set; }
        public bool IsConfirmed { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ConfirmedDate { get; set; }
        public List<CartMenuItem> OrderedItems { get; set; }
    }
    public class CartMenuItem
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public float Total { get; set; }

    }
}