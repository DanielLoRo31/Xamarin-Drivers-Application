using System;
using System.Collections.Generic;
using System.Text;

namespace AppAzureSQL.Models
{
    public enum MenuItemType
    {
        Inicio,
        Insertar,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
