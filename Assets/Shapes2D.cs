using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Used To Create Basic 2D Shapes
    Made up of 4 Types: Triangle, Line, LineCircle, and Circle (Could add Square/Rectangle)
    Any Coordinate in the Z axis will be set to 0. 
    To bound the mesh data to 2D space.
*/
namespace Shapes2D {

    //Super Class
    public class Shape {
        public Vector3[] vertices;
        public Vector2[] uv;
        public int[] triangles;

        public virtual void Update() {}
        protected virtual void SetupVertices() {}
        protected virtual void SetupUV() {}
        protected virtual void SetupTriangles() {}
    }

    public class Triangle: Shape {

        //Properties
        public Vector3 leftPoint;
        public Vector3 rightPoint;
        public Vector3 topPoint;

        public Triangle () {
            vertices = new Vector3[3];
            uv = new Vector2[3];
            triangles = new int[3];

            leftPoint = new Vector3(-1, 0);
            rightPoint = new Vector3(1, 0);
            topPoint = new Vector3(0, 1);

            SetupVertices();
            SetupUV();
            SetupTriangles();
        }

        public Triangle(Vector3 _leftPoint, Vector3 _rightPoint, Vector3 _topPoint) {
            leftPoint = _leftPoint;
            rightPoint = _rightPoint;
            topPoint = _topPoint;

            Update();
        }

        //Update Mesh Based on Changes-----
        public override void Update() {
            SetupVertices();
            SetupUV();
            SetupTriangles();
        }
        //-----

        //Prepares the Triangle Data-----
        override protected void SetupVertices() {
            vertices[0] = leftPoint;
            vertices[1] = rightPoint;
            vertices[2] = topPoint;
        }

        override protected void SetupUV() {
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(1, 1);
        }

        override protected void SetupTriangles() {
            triangles[0] = 0;
            triangles[1] = 2;
            triangles[2] = 1;
        }
        //------

        //Getters
        public Vector3[] GetVertices() {
            return vertices;
        }

        public Vector2[] GetUV() {
            return uv;
        }

        public int[] GetTriangles() {
            return triangles;
        }
    }

    public class Line: Shape {

        //Properties
        public Vector3 startPoint;
        public Vector3 endPoint;
        public float thickness = 0f;

        public Line(Vector3 _startPoint, Vector3 _endPoint, float _thickness) {
            vertices = new Vector3[4];
            uv = new Vector2[4];
            triangles = new int[6];

            startPoint = _startPoint;
            endPoint = _endPoint;
            thickness = _thickness;

            Update();
        }

        //Update Mesh Based on Changes-----
        public override void Update() {
            SetupVertices();
            SetupUV();
            SetupTriangles();
        }
        //-----

        //Setup Mesh Data-----
        protected override void SetupVertices() {
            bool flip = OrientateVertices(startPoint, endPoint);

            //Flip Point Values
            Vector3 holder;
            if (flip) {
                holder = startPoint;
                startPoint = endPoint;
                endPoint = holder;
            }

            //Direction of Line
            float thk = thickness / 2;  //Half Thickness
            Vector3 startNorm = (startPoint - endPoint).normalized;
            float xModifier = Mathf.Tan(startNorm.y);
            float yModifier = Mathf.Tan(startNorm.x);

            vertices[0] = new Vector3(startPoint.x - thk * xModifier, startPoint.y + thk * yModifier);    //Top Left
            vertices[1] = new Vector3(startPoint.x + thk * xModifier, startPoint.y - thk * yModifier);    //Top Right
            vertices[2] = new Vector3(endPoint.x - thk * xModifier, endPoint.y + thk * yModifier);    //Bottom Left
            vertices[3] = new Vector3(endPoint.x + thk * xModifier, endPoint.y - thk * yModifier);    //Bottom Right
        }
       
        protected override void SetupUV() {
            uv[0] = new Vector2(0, 1);
            uv[1] = new Vector2(1, 1);
            uv[2] = new Vector2(0, 0);
            uv[3] = new Vector2(0, 1);
        }

        protected override void SetupTriangles() {
            //Triangle 1
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            //Triangle 2
            triangles[3] = 2;
            triangles[4] = 1;
            triangles[5] = 3;
        }
        //------

        //Render Mesh Clockwise: Checking Y Axes
        bool OrientateVertices(Vector3 startPoint, Vector3 endPoint) {
            bool shouldFlip = false;
            if (startPoint.y < endPoint.y) {
                shouldFlip = true;
            }
            
            return shouldFlip;
        }
    }

    public class Circle: Shape {
        
        //Variables
        public float radius;
        public int steps;

        public Circle(float _radius, int _steps) {
            radius = _radius;
            steps = _steps;

            vertices = new Vector3[steps];
            uv = new Vector2[steps];
            triangles = new int[steps * 3];

            Update();
        }

        //Update Data Based on Changes-----
        public override void Update() {
            SetupVertices();
            SetupUV();
            SetupTriangles();
        }
        //------

        //Setup Mesh Data-----
        protected override void SetupVertices() {
            float increment = (2 * Mathf.PI) / steps;
            float point = 0;

            for (int i = 0; i < steps; i++) {
                point += increment;
                vertices[i] = new Vector3(Mathf.Cos(point) * radius, Mathf.Sin(point) * radius); 
            }
        }

        protected override void SetupUV() {
            Vector2[] uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++) {
                uvs[i] = new Vector2(vertices[i].x / (radius * 2) + 0.5f, vertices[i].y / (radius * 2) + 0.5f);
            }
        }

        protected override void SetupTriangles() {
            for (int i = 0; i < vertices.Length - 2; i++) {
               triangles[i * 3] = 0;
               triangles[(i * 3) + 2] = i + 1;
               triangles[(i * 3) + 1] = i + 2;
            }
        }
        //-----
    }

    public class LineCircle: Shape {

        //Properties
        public int steps;
        public float radius;
        public float thickness;

        public LineCircle() {
            steps = 5;
            radius = 1f;
            thickness = 1f;
            
            vertices = new Vector3[steps * 2];
            uv = new Vector2[steps * 2];
            triangles = new int[(steps * 2) * 3];
            Update();
        }

        public LineCircle(int _steps, float _radius, float _thickness) {
            steps = _steps;
            radius = _radius;
            thickness = _thickness;

            vertices = new Vector3[steps * 2];
            uv = new Vector2[steps * 2];
            triangles = new int[(steps * 2) * 3];
            Update();
        }

        //Update Data Based on Changes-----
        public override void Update() {
            SetupVertices();
            SetupUV();
            SetupTriangles();
        }
        //-----

        //Setup Mesh Data-----
        protected override void SetupVertices() {
            float increment = (2 * Mathf.PI) / steps;
            float point = 0;    //Point in the Circle
            float thk = thickness / 2;

            //Used to Mimic Positions for all vertices
            Vector3[] circleInner = new Vector3[steps];
            Vector3[] circleOuter = new Vector3[steps];

            //Set Up Circle
            for (int i = 0; i < circleInner.Length; i++) {
                point += increment;
                circleInner[i] = new Vector3(Mathf.Sin(point) * radius, Mathf.Cos(point) * radius);
                circleOuter[i] = new Vector3(Mathf.Sin(point) * (radius + thickness), Mathf.Cos(point) * (radius + thickness));
            }

            //Set Actual Mesh Data Vertices Based Off Circle Array
            for (int i = 0; i < circleInner.Length; i++) {
                Vector3 normalized = Vector3.zero;

                //Set Vertice Positions
                vertices[i * 2] = new Vector3(circleInner[i].x, circleInner[i].y);
                vertices[(i * 2) + 1] = new Vector3((circleOuter[i].x), (circleOuter[i].y));
            }
        }

        protected override void SetupUV() {
            Vector2[] uvs = new Vector2[vertices.Length];
            for (int i = 0; i < uvs.Length; i++) {
                uvs[i] = new Vector2(vertices[i].x / (radius * 2) + 0.5f, vertices[i].y / (radius * 2) + 0.5f);
            }
        }

        protected override void SetupTriangles() {
            //Assign last 2 Triangles First
            triangles[triangles.Length - 6] = 0;
            triangles[triangles.Length - 5] = vertices.Length - 2;
            triangles[triangles.Length - 4] = vertices.Length - 1;

            triangles[triangles.Length - 3] = 1;
            triangles[triangles.Length - 2] = 0;
            triangles[triangles.Length - 1] = vertices.Length - 1;

            for (int i = 0; i < vertices.Length - 2; i++) {
                if (i % 2 != 0) {
                    triangles[i * 3] = i;
                    triangles[(i * 3) + 2] = i + 1;
                    triangles[(i * 3) + 1] = i + 2;
                } else {
                    triangles[i * 3] = i + 1;
                    triangles[(i * 3) + 2] = i;
                    triangles[(i * 3) + 1] = i + 2;
                }
            }
        }
        //-----
    }
}
