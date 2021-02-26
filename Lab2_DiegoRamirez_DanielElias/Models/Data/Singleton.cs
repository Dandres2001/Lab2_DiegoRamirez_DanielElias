using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibreriaRD2;

namespace Lab2_DiegoRamirez_DanielElias.Models.Data
{
    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public Manual_List<Drug> DrugsList;
        private Singleton()
        {
            
            DrugsList = new Manual_List<Drug>();
        }

        public static Singleton Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
