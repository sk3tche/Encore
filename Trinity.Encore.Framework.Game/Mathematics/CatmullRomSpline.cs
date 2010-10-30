using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;
using Trinity.Encore.Framework.Core.Mathematics;

namespace Trinity.Encore.Framework.Game.Mathematics
{
    public sealed class CatmullRomSpline
    {
        private static readonly Matrix _hermitePoly = new Matrix(2.0f, -3.0f, 0.0f, 1.0f,
            -2.0f, 3.0f, 0.0f, 0.0f, 1.0f, -2.0f, 1.0f, 0.0f, 1.0f, -1.0f, 0.0f, 0.0f);

        public const float MinUnitInterval = 0.0f;

        public const float MaxUnitInterval = 1.0f;

        private readonly List<Vector3> _pointList = new List<Vector3>();

        private readonly List<Vector3> _tangentList = new List<Vector3>();

        public bool AutoCalculate { get; set; }

        public CatmullRomSpline(bool autoCalculate = true)
        {
            AutoCalculate = autoCalculate;
        }

        public int PointCount
        {
            get { return _pointList.Count; }
        }

        public void AddPoint(Vector3 point)
        {
            _pointList.Add(point);

            if (AutoCalculate)
                RecalculateTangents();
        }

        public void Clear()
        {
            _pointList.Clear();
            _tangentList.Clear();
        }

        public Vector3 GetPoint(int index)
        {
            Contract.Requires(index >= 0);
            Contract.Requires(index < _pointList.Count);

            return _pointList[index];
        }

        public Vector3 Interpolate(float t)
        {
            Contract.Requires(t >= MinUnitInterval);
            Contract.Requires(t <= MaxUnitInterval);

            var segment = t * _pointList.Count;
            var segIndex = (int)segment;
            t = segment - segIndex;

            return Interpolate(segIndex, t);
        }

        public Vector3 Interpolate(int index, float t)
        {
            Contract.Requires(index >= 0);
            Contract.Requires(index < _pointList.Count);
            Contract.Requires(t >= MinUnitInterval);
            Contract.Requires(t <= MaxUnitInterval);

            if (index + 1 == _pointList.Count)
                return _pointList[index];

            if (t == MinUnitInterval)
                return _pointList[index];

            if (t == MaxUnitInterval)
                return _pointList[index + 1];

            var t2 = t * t;
            var t3 = t2 * t;

            var powers = new Vector4(t3, t2, t, 1);

            var point1 = _pointList[index];
            var point2 = _pointList[index + 1];
            var tangent1 = _tangentList[index];
            var tangent2 = _tangentList[index + 1];

            var point = new Matrix();
            point.M11 = point1.X;
            point.M12 = point1.Y;
            point.M13 = point1.Z;
            point.M14 = 1.0f;
            point.M21 = point2.X;
            point.M22 = point2.Y;
            point.M23 = point2.Z;
            point.M24 = 1.0f;
            point.M31 = tangent1.X;
            point.M32 = tangent1.Y;
            point.M33 = tangent1.Z;
            point.M34 = 1.0f;
            point.M41 = tangent2.X;
            point.M42 = tangent2.Y;
            point.M43 = tangent2.Z;
            point.M44 = 1.0f;

            var result = Vector4.Transform(powers, _hermitePoly * point);
            return new Vector3(result.X, result.Y, result.Z);
        }

        public void RecalculateTangents()
        {
            _tangentList.Clear();

            var numPoints = _pointList.Count;

            if (numPoints < 2)
                return;

            var isClosed = _pointList[0] == _pointList[numPoints - 1];

            for (var i = 0; i < numPoints; i++)
            {
                const float round = FastMath.RoundValue;

                if (i == 0)
                {
                    if (isClosed)
                        _tangentList.Add(round * (_pointList[1] - _pointList[numPoints - 2]));
                    else
                        _tangentList.Add(round * (_pointList[1] - _pointList[0]));
                }
                else if (i == numPoints - 1)
                {
                    if (isClosed)
                        _tangentList.Add(_tangentList[0]);
                    else
                        _tangentList.Add(round * (_pointList[i] - _pointList[i - 1]));
                }
                else
                    _tangentList.Add(round * (_pointList[i + 1] - _pointList[i - 1]));
            }
        }
    }
}
