public class Matrix{
    public int width,height;
    public float[,] matrix;
    public float GetValue(int row, int column)
    {
        return matrix[row,column];
    }
    public Matrix(int height,int width,float[,] matrix){
        this.matrix=matrix;
        this.height=height;
        this.width=width;
        
    }
    public Matrix multiplyByMatrix(Matrix matrixB){
        int aRows=height; int aCols=width;
        int bRows=matrixB.height; int bCols=matrixB.width;
        
        Matrix result=new Matrix(aRows,bCols,new float[aRows,bCols]);
        for (int i=0; i<aRows; ++i)
            for (int j=0; j<bCols; ++j) 
                for (int k=0; k<aCols; ++k)
                    result.matrix[i,j]+=matrix[i,k]*matrixB.matrix[k,j];
        return result;
    }
}