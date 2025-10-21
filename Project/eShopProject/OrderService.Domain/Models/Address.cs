using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Models
{
    public sealed class Address
    {
        // Parameterless constructor for EF Core
        public Address() { }

        public Address(string street, string city, string zip, string country)
        {
            Street = street;
            City = city;
            Zip = zip;
            Country = country;
        }

        public string Street { get; }
        public string City { get; }
        public string Zip { get; }
        public string Country { get; }
    }


}
