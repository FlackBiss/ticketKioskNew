using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Lastik.Models.Schedules.Entities;
using Lastik.Models.Tickets.Entities;
using MapControlLib.Utilities;
using Newtonsoft.Json;

namespace Lastik.Models.Cart.Entities
{
    public partial class CartItem : ObservableObject
    {
        [JsonIgnore]
        public string PlaceToPrint { get; set; }

        [JsonProperty("place")]
        public string Place { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("eventId")]
        public int EventId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonIgnore]
        private int _count;
        [JsonProperty("count")]
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public int AvailableCount { get; set; }
    }
}
