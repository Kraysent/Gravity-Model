using Engine.Models;
using Engine.ViewModel;
using Newtonsoft.Json;
using System;
using System.IO;

namespace WPFUI.Realisations
{
    class JsonFileService : IFileService
    {
        public Universe Open(string filename)
        {
            string json;

            try
            {
                json = File.ReadAllText(filename);

                //We need to use these settings because in _universe.Bodies
                //we have List that consists not only of MaterialPoints
                //but also of PhysicalBody inherited from MaterialPoints.
                //As a result, when we serialize - we serialize PhysicalBodies
                //but when we deserialize - we deserialize MaterialPoints because
                //Bodies is a List<MaterialPoint>, so we lose properties of Physical body.
                //
                //The same thing need to be done when we save universe
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                return (Universe)JsonConvert.DeserializeObject(json, settings);
            }
            catch
            {
                return null;
            }
        }

        public void Save(string filename, Universe universe)
        {
            throw new NotImplementedException();
        }
    }
}
