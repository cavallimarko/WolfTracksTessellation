using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;

public class LoadBSpline : MonoBehaviour

{
    public int PointsPerSegment=5;
    public List<Vector3> ControlPoints;
    public List<Vector3> SplinePoints;

    public List<Vector3> TangentPoints;
    public GameObject GameObject;
    public CustomParticleSystem CustomParticleSystem;
    public string fileName1="BSplineSpiral.obj";
    public string fileName2="BSpline.obj";
    private LineRenderer lineRenderer;
    void Start()
    {
        lineRenderer=GetComponent<LineRenderer>();
        ControlPoints=new List<Vector3>();
        foreach(string line in File.ReadAllLines("Assets/Pracenje krivulje/"+fileName1)){
            char[] charsToTrim = { 'v', ' '};
            string result = line.Trim(charsToTrim);
            string []floats=result.Split(' ');
            float x=float.Parse(floats[0], CultureInfo.InvariantCulture.NumberFormat);
            float y=float.Parse(floats[1], CultureInfo.InvariantCulture.NumberFormat);
            float z=float.Parse(floats[2], CultureInfo.InvariantCulture.NumberFormat);
            ControlPoints.Add(new Vector3(x,y,z));
        }
        DrawBSpline();
            
    }

    [ContextMenu ("DrawBSpline")]
    public void DrawBSpline(){
        SplinePoints=new List<Vector3>();
        TangentPoints=new List<Vector3>();
        if(ControlPoints!=null){
            for (int i = 0; i <ControlPoints.Count-3; i++)
            {
               
                float t=0;
                
                for (int j = 0; j < PointsPerSegment; j++)
                {
                    t+=1.0f/PointsPerSegment;
                     
                    
                    Matrix T4=new Matrix(1,4,new float[,]
                                                            {
                                                                {Mathf.Pow(t, 3),Mathf.Pow(t, 2),t,1}
                                                            });
                    

                   Matrix Bi3=new Matrix(4,4,new float[,]
                                                            {
                                                                {-1.0f/6,1.0f/2,-1.0f/2,1.0f/6}, 
                                                                {1.0f/2,-1,1.0f/2,0},
                                                                {-1.0f/2,0,1.0f/2,0},
                                                                {1.0f/6,2.0f/3,1.0f/6,0}
                                                            });
            
                    
                    Matrix Ri=new Matrix(4,3,new float[,]
                                                            {
                                                                {ControlPoints[i].x,ControlPoints[i].y,ControlPoints[i].z}, 
                                                                {ControlPoints[i+1].x,ControlPoints[i+1].y,ControlPoints[i+1].z},
                                                                {ControlPoints[i+2].x,ControlPoints[i+2].y,ControlPoints[i+2].z},
                                                                {ControlPoints[i+3].x,ControlPoints[i+3].y,ControlPoints[i+3].z}
                                                            });
                                                           
                    Matrix pit=Bi3.multiplyByMatrix(Ri);
                    Matrix pit3=T4.multiplyByMatrix(pit);
                    Vector3 pit3Vector=new Vector3(pit3.GetValue(0,0),pit3.GetValue(0,1),pit3.GetValue(0,2));
                    
                    SplinePoints.Add(pit3Vector);

                    Matrix Bi3Tangent=new Matrix(3,4,new float[,]
                                                            {
                                                                {-1.0f/2,3.0f/2,-3.0f/2,1.0f/2}, 
                                                                {1,-2,1,0},
                                                                {-1.0f/2,0,1.0f/2,0}
                                                            });
                        
                    Matrix T3=new Matrix(1,3,new float[,]
                                                            {
                                                                {Mathf.Pow(t, 2),t,1}
                                                            });   
                    Matrix pitTangent=Bi3Tangent.multiplyByMatrix(Ri);
                    Matrix pit3Tangent=T3.multiplyByMatrix(pitTangent);
                    Vector3 pit3TangentVector=new Vector3(pit3Tangent.GetValue(0,0),pit3Tangent.GetValue(0,1),pit3Tangent.GetValue(0,2));
                    pit3TangentVector.Normalize();
                    TangentPoints.Add(pit3TangentVector);    
                    
                }   
            }
        }
        if(SplinePoints!=null){
            for (int j = 0; j < SplinePoints.Count-1; j++)
            {
                Debug.DrawLine(SplinePoints[j],SplinePoints[j+1]);
            }
        }
        lineRenderer.positionCount=SplinePoints.Count;
        lineRenderer.SetPositions(SplinePoints.ToArray());
    }

    void OnDrawGizmos()
    {
        if(ControlPoints!=null){
            foreach (var controlPoint in ControlPoints)
            {
            Gizmos.DrawSphere(controlPoint,1);      
            }
        }
        if(SplinePoints!=null){
            for (int j = 0; j < SplinePoints.Count-1; j++)
            {
                Gizmos.DrawLine(SplinePoints[j],SplinePoints[j+1]);
                
            }
           
        }
        
        if(TangentPoints!=null){
            Gizmos.color=Color.magenta;
            for (int j = 0; j < TangentPoints.Count-1; j++)
            {
                Gizmos.DrawLine(SplinePoints[j],SplinePoints[j]+TangentPoints[j]*2);
            }
        }
        
    }
    [ContextMenu ("Animate")]
    public void Animate(){
        StartCoroutine("AnimateCoroutine");
    }
    public IEnumerator AnimateCoroutine(){
        for (int i = 0; i < SplinePoints.Count-1; i++){
            if(SplinePoints[i]!=null){
                GameObject.transform.position=  SplinePoints[i];
                Vector3 s=new Vector3(0,0,1);
                Vector3 e=new Vector3(TangentPoints[i].x,TangentPoints[i].y,TangentPoints[i].z);
                Vector3 os=new Vector3(s.y*e.z-e.y*s.z,-(s.x*e.z-e.x*s.z),s.x*e.y-s.y*e.x);
                float angleRadian=(Mathf.Acos((s.x*e.x+s.y*e.y+s.z*e.z)/(Mathf.Sqrt(s.x*s.x+s.y*s.y+s.z*s.z)*Mathf.Sqrt(e.x*e.x+e.y*e.y+e.z*e.z))));
                float angle = angleRadian * Mathf.Rad2Deg;
                GameObject.transform.rotation=Quaternion.Euler(Vector3.forward);
                GameObject.transform.RotateAround(SplinePoints[i], os, angle);
            }
            yield return new WaitForSeconds(.016f);
        }
    }
    [ContextMenu("AnimateParticleSystem")]
    public void AnimateParticleSystem()
    {
        StartCoroutine("AnimatePSCoroutine");
    }
    public IEnumerator AnimatePSCoroutine()
    {
        for (int i = 0; i < SplinePoints.Count - 1; i++)
        {
            if (SplinePoints[i] != null)
            {
                CustomParticleSystem.transform.position = SplinePoints[i];
            }
            yield return null;
        }
    }
}
