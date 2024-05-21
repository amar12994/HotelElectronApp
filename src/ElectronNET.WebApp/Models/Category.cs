using System;
using System.Collections.Generic;

namespace ElectronNET.WebApp.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public bool IsDeleted { get; set; }
    }
    public class MenuItem
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string Picture { get; set; }
        public List<AddonItem> AddonItems { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
    public class AddonItem
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid MenuItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public bool IsDeleted { get; set; }
    }
}