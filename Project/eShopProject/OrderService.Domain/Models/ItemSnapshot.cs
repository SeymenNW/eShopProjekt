using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Models
{
    public sealed class ItemSnapshot
    {
        // Parameterless constructor required by EF Core
        public ItemSnapshot() { }

        public ItemSnapshot(Guid itemId, string name, string? pictureUri)
        {
            ItemId = itemId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            PictureUri = pictureUri;
        }

        public Guid ItemId { get; }
        public string Name { get; }
        public string? PictureUri { get; }
    }


}
