
namespace Program
{
    [Serializable]
    public class Matrix{
        //private Random rand = new Random();
        public int _n;
        public int _m;
        
        public dynamic matrix;

        public Matrix(int n, int m)
        {//N rows my M columns
            _n=n;
            _m=m;
            matrix = new double[n,m];
        }
        public double this[int x, int y]{
            get{
                return this.matrix[x,y];
            }
            set{
                this.matrix[x,y] = value;
            }
        }


        public void Print(){
            for (int row=0; row<this._n;row++){
                for (int col=0; col<this._m;col++){
                    string s=String.Format("{0,10:0.000}" , this.matrix[row,col]);
                    if(this.matrix[row,col]==0) {
                        Console.ForegroundColor = ConsoleColor.Gray;                        
                    }
                    else if(this.matrix[row,col]<0){
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }else{
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    Console.Write($"{s}  ");
                }Console.WriteLine();
            }Console.ResetColor();Console.WriteLine("\n\n");
        }


        public Matrix Transpose(){
            Matrix r = new Matrix(this._m,this._n); //note n m order switch, for non square matrix
            for (int row=0; row<this._n;row++){
                for (int col=0; col<this._m;col++){
                    r.matrix[col,row]=this.matrix[row,col];//note col row for new, row col for old
                }
            }
            return r;
        }

        //public Matrix Randomise(double maximum, double minimum){
        //    Matrix r = new Matrix(this._n,this._m);
        //    for(int n =0; n<this._n;n++){
        //        for(int m=0; m<this._m;m++){
        //            this.matrix[n, m] = Tools.Tools.RandomDouble(minimum, maximum);//rand.NextDouble() * (maximum - minimum) + minimum;//this will just place random number in the range we want
        //        }
        //    }
        //    return(r);
        //}
        public Matrix Copy_To(){
            Matrix r =new Matrix(this._n,this._m);
            for (int x = 0; x < this._m; x++)
            {
                for (int y = 0; y < this._n; y++)
                {
                    r.matrix[y,x]=this.matrix[y,x];
                }
            }
            return r;
        }
        public void Copy_To(Matrix r){
            if((r._n<this._n)|(r._m<this._m)) throw new Exception("Too small");
            for (int x = 0; x < this._m; x++)
            {
                for (int y = 0; y < this._n; y++)
                {
                    r.matrix[y,x]=this.matrix[y,x];
                }
            }
        }
        public Matrix Upscale(int rows, int columns){
            Matrix temp = new Matrix(this._n+rows,this._m+columns);
            this.Copy_To(temp);
            return temp;
        }
//add an upscale this so no need for new matrix? 

        public Matrix Minor(int x,int y){
            if((this._n!=1)&(this._m!=1)){
                Matrix r=new Matrix(this._n-1,this._m-1);
                //pull up and/or pull left idea
                for (int row=0;row<this._n;row++){
                    for(int col=0;col<this._m;col++){
                       switch ((row>y),(col>x)){
                        case (true,true):
                            //pull up and left
                            r.matrix[row-1,col-1]=this.matrix[row,col];
                            break;
                        case (true,false):
                            //pull up
                            r.matrix[row-1,col]=this.matrix[row,col];
                            break;
                        case (false,true):
                            //pull left
                            r.matrix[row,col-1]=this.matrix[row,col];
                            break;
                        case (false,false):
                            //nothing   or   vanish
                            if((row!=y)&(col!=x)){
                            r.matrix[row,col]=this.matrix[row,col];}
                            //if they are equal nothing happens
                            break;}

                    }
                }
                return r;
            }return this.matrix[0,0];
        }
        
        public static Matrix operator *(Matrix a, Matrix b){
            if (a._m==b._n){//dimension check   n m   n m     mid 2 should be same remember Further Math
                Matrix r = new Matrix(a._n,b._m);//dimensions of new matrix
                //loop through each cell and set r's element to sum of elements of a b
                for (int rrow=0;rrow<r._n;rrow++){
                    for(int rcol=0;rcol<r._m;rcol++){
                        //remember my working out
                        //rcol is b's col, rrow is a's row

                        //go through the a's row (acol++) and pair it with b elems
                        for (int acol=0;acol<a._m;acol++){
                            //note, since dimension check at start acol mathces for a and b.
                            r.matrix[rrow,rcol] += a.matrix[rrow, acol]*b.matrix[acol,rcol];
                            //look at diagram. += adds on stuffs

                        }//i think thats it

                    }
                }
                return r;
            }else{
                throw new Exception("Dimension error");
            }
        }

        //public static Vector operator * (Matrix a, Vector b)
        //{
        //    if(a._m!=b.dimension) throw new Exception();
        //    Vector r = new Vector(a._n);
        //    for(int i =0;i<a._n;i++){
        //        for(int x=0;x<a._m;x++){
        //            r.vector[i]+=a.matrix[i,x]  *  b[x];//BUG WAS HERE. NOW FIXED, FORGOT TO MULTIPLY
        //        }
        //    }
        //    return r;
            
        //}


        public static Matrix operator +(Matrix a, Matrix b){
            if ((a._n==b._n)&(a._m==b._m)){
                Matrix r = new Matrix(a._n,a._m);
                //loop through each cell and set r's element to sum of elements of a b
                for (int row=0; row<a._n;row++){
                    for (int col=0; col<a._m;col++){
                        r.matrix[row,col]=a.matrix[row,col]+b.matrix[row,col];
                    } 
                }
                return r;
            }else{
                throw new Exception("Dimension error");
            }
        }

        public static Matrix operator *(double a, Matrix b){
            Matrix r = new Matrix(b._n,b._m);
            for(int x=0; x<b._n; x++){
                for (int y = 0; y < b._m; y++)
                {
                    r.matrix[x,y] = b.matrix[x,y]*a;
                }
            }
            return r;
        }
        public static Matrix operator /( Matrix b,double a){
            Matrix r = new Matrix(b._n,b._m);
            for(int x=0; x<b._n; x++){
                for (int y = 0; y < b._m; y++)
                {
                    r.matrix[x,y] = b.matrix[x,y]/a;
                }
            }
            return r;
        }

        public override int GetHashCode()
        {
            return stringy().GetHashCode();
        }
        public override bool Equals(object? obj)
        {
            if (obj is Matrix)
            {
                Matrix y = (Matrix)obj;
                Matrix x = this;
                if (x._n != y._n || x._m != y._m)
                {
                    return false;
                }
                for (int X = 0; X < x._m; X++)
                {
                    for (int Y = 0; Y < x._n; Y++)
                    {
                        if (x[X, Y] != y[X, Y]) return false;
                    }
                }
                return true;
            }return false;
        }
        public string stringy()
        {
            string r = "";
            for (int row = 0; row < this._n; row++)
            {
                for (int col = 0; col < this._m; col++)
                {
                    r += $"{matrix[row,col]} ";
                }
                r += "n";
            }
            return r;
        }
        public class EqualityComparer : IEqualityComparer<Matrix>
        {

            public bool Equals(Matrix x, Matrix y)
            {
                if (x._n!=y._n || x._m!=y._m)
                {
                    return false;
                }
                for (int X=0;X<x._m;X++)
                {
                    for (int Y = 0; Y < x._n; Y++)
                    {
                        if (x[X, Y] != y[X,Y]) return false;
                    }
                }
                return true;
            }

            public int GetHashCode(Matrix x)
            {
                return x.stringy().GetHashCode();
            }

        }

    }
}