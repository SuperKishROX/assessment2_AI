using System;
using System.Collections.Generic;
using System.Text;

namespace InferenceEngine
{
    public class TruthTable
    {
        public List<string> Variables; //this list will store the variables needed for Truth table
        public string Alpha; //this string will store the query 
        private bool[,] _table; // a 2-dimensinal array that will store the true/false values aka the table

        public TruthTable()
        {
            //need list that contains KB
            //string that has alpha -> which is the query
            //list of variables for the table
            //need the number of rows for the table which we get from the operation 2 ^ number of variables - > could be seperate  method?
        }

    }
}
