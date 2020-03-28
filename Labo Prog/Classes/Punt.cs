using System;
using System.Runtime.Serialization;



namespace Labo_Prog
{
    [Serializable()]
    public class Punt : ISerializable
    {
        #region Constructor

        public Punt(double x, double y)
        {
            m_X = x;
            m_Y = y;
        }

        private Punt(SerializationInfo info, StreamingContext context)
        {
            m_X = (double)info.GetValue("m_X", typeof(double));
            m_Y = (double)info.GetValue("m_Y", typeof(double));
        }
        #endregion

        #region HelperFunctions
        public override string ToString()
        {
            return $"X: {m_X}, Y: {m_Y}";
        }
        public override bool Equals(object obj)
        {
            return obj is Punt punt &&
                   m_X == punt.m_X &&
                   m_Y == punt.m_Y;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(m_X, m_Y);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_X", m_X);
            info.AddValue("m_Y", m_Y);
            
            
        }
        #endregion

        #region Functions

        public static double DistanceBetweenTwoPoint(Punt point1, Punt point2)
        {
            double distanceBeteenXCoords = point2.m_X - point1.m_X;
            double distanceBeteenYCoords = point2.m_Y - point1.m_Y;

            return Math.Sqrt(Math.Pow(distanceBeteenXCoords, 2) + Math.Pow(distanceBeteenYCoords, 2));

        }
        #endregion

        #region Proprties
        public double m_X { get; set; }
        public double m_Y { get; set; }
        #endregion
    }
}
