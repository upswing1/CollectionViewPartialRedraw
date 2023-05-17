using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CollectionViewPartialRedraw
{

    [Serializable]
    public class Fleet : ModelBase, ICloneable
    {
        private Color _color;
        private string _name = "Name";
        private string _description;
        private DateTime _created;
        private string _depotName;


        
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required]
        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }
        }

        [Required]
        public string ColorString { get; set; }

        [NotMapped]
        public Color Color
        {
            get => Color.FromArgb(ColorString);
            set
            {
                if (ColorString != value.ToHex())
                {
                    ColorString = value.ToHex();
                    OnPropertyChanged();
                }
            }
        }

        [Required]
        public DateTime Created
        {
            get { return _created; }
            set
            {
                if (_created != value)
                {
                    _created = value;
                    OnPropertyChanged();
                }
            }
        }


        //public string DepotName
        //{
        //    get { return _depotName; }
        //    set
        //    {
        //        if (_depotName != value)
        //        {
        //            _depotName = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _depotAddress;
        //public string DepotAddress
        //{
        //    get { return _depotAddress; }
        //    set
        //    {
        //        if (_depotAddress != value)
        //        {
        //            _depotAddress = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //private string _depotPhone;
        //public string DepotPhone
        //{
        //    get { return _depotPhone; }
        //    set
        //    {
        //        if (_depotPhone != value)
        //        {
        //            _depotPhone = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //#endregion

        //// Foreign key property
        //public int FleetHeaderId { get; set; }

        //// Navigation property for the related FleetHeader
        //[ForeignKey("FleetHeaderId")]
        //public FleetHeader FleetHeader
        //{
        //    get { return _header; }
        //    set
        //    {
        //        if (_header != value)
        //        {
        //            _header = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        //public byte[] ImageData { get; set; }

        //[NotMapped]
        //[XmlIgnore]
        //public ImageSource ImageSource
        //{
        //    get
        //    {
        //        // Convert the byte array to an image source
        //        if (ImageData != null)
        //        {
        //            using (var stream = new MemoryStream(ImageData))
        //            {
        //                return ImageSource.FromStream(() => stream);
        //            }
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    set
        //    {
        //        // Convert the image source to a byte array
        //        if (value != null)
        //        {
        //            using (var stream = new MemoryStream())
        //            {
        //                ImageData = ConvertImageSourceToBytesAsync(value).Result;
        //            }
        //        }
        //        else
        //        {
        //            ImageData = null;
        //        }
        //    }
        //}

        public ObservableCollection<Vehicle> Vehicles { get; set; } = new ObservableCollection<Vehicle>();



        public object Clone()
        {
            // Create a new XmlSerializer for the Fleet type
            XmlSerializer serializer = new XmlSerializer(typeof(Fleet));

            using (MemoryStream stream = new MemoryStream())
            {
                // Serialize the current object to the MemoryStream
                serializer.Serialize(stream, this);

                // Reset the position of the MemoryStream
                stream.Seek(0, SeekOrigin.Begin);

                // Deserialize the object from the MemoryStream to create a clone
                object clone = serializer.Deserialize(stream);

                return clone;
            }
        }
    }

    public class FleetsDataStore : IDataStore<Fleet>
    {
        public List<Fleet> _fleets;

        public FleetsDataStore()
        {
            _fleets = new List<Fleet>();
        }

        Task<bool> IDataStore<Fleet>.AddItemAsync(Fleet item)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDataStore<Fleet>.DeleteItemAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<Fleet> GetItemAsync(string id)
        {
            throw new NotImplementedException();
        }

 
        async Task<IEnumerable<Fleet>> IDataStore<Fleet>.GetItemsAsync(bool forceRefresh)
        {
            
            var random = new Random();
            for (int i = 0; i < 50; i++)
            {
                int r = random.Next(256);
                int g = random.Next(256);
                int b = random.Next(256);
                string colorString = $"#{r:X2}{g:X2}{b:X2}";

                var fleet = new Fleet
                {
                    Id = i,
                    Name = $"Fleet {i}",
                    Description = $"Fleet {i} Description",
                    ColorString = colorString,
                    Created = DateTime.Now,
                };

                // Generate a random number of vehicles for the fleet
                int numberOfVehicles = random.Next(1, 11); // Randomly choose a number between 1 and 10
                for (int j = 0; j < numberOfVehicles; j++)
                {
                    var vehicle = new Vehicle
                    {
                        VehicleId = $"Vehicle_{i}_{j}",
                        Make = $"Make {i}_{j}",
                        Model = $"Model {i}_{j}",
                        Year = random.Next(2000, 2023), // Randomly choose a year between 2000 and 2022
                        
                        FleetId = fleet.Id
                    };

                    fleet.Vehicles.Add(vehicle);
                }

                _fleets.Add(fleet);
            }

            return await Task.FromResult(_fleets);
        }

        Task<IEnumerable<Fleet>> IDataStore<Fleet>.GetItemsAsync(string scope, bool forceRefresh)
        {
            throw new NotImplementedException();
        }

        Task<bool> IDataStore<Fleet>.UpdateItemAsync(Fleet item)
        {
            return Task.FromResult(true);
        }


        async Task<IEnumerable<Fleet>> IDataStore<Fleet>.ReadLocal()
        {
            return await Task.FromResult(_fleets);
        }
    }


    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<IEnumerable<T>> GetItemsAsync(string scope, bool forceRefresh = false);
        Task<IEnumerable<T>> ReadLocal();
    }
}
