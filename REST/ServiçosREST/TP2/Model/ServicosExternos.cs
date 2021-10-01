//Serviços REST
// 23/12/2020
//Classe serviços externos
//
//Criado por Diogo Rocha nº16966





using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP2.Model
{

    #region COVID POR CONCELHO PORTUGAL

    public class UniqueIdField
    {
        public string name { get; set; }
        public bool isSystemMaintained { get; set; }
    }

    public class SpatialReference
    {
        public int wkid { get; set; }
        public int latestWkid { get; set; }
    }

    public class Field
    {
        public string name { get; set; }
        public string type { get; set; }
        public string alias { get; set; }
        public string sqlType { get; set; }
        public object domain { get; set; }
        public string defaultValue { get; set; }
        public int? length { get; set; }
    }

    public class Attributes
    {
        public int OBJECTID { get; set; }
        public string Concelho { get; set; }
        public int ConfirmadosAcumulado { get; set; }
        public int Recuperados { get; set; }
        public int Obitos { get; set; }
        public object Data { get; set; }
        public string Dicofre { get; set; }
    }

    public class Geometry
    {
        public double x { get; set; }
        public double y { get; set; }
    }

    public class Feature
    {
        public Attributes attributes { get; set; }
        public Geometry geometry { get; set; }
    }

    public class Root
    {
        public string objectIdFieldName { get; set; }
        public UniqueIdField uniqueIdField { get; set; }
        public string globalIdFieldName { get; set; }
        public string geometryType { get; set; }
        public SpatialReference spatialReference { get; set; }
        public List<Field> fields { get; set; }
        public bool exceededTransferLimit { get; set; }
        public List<Feature> features { get; set; }
    }
    #endregion
}
