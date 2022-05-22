using System;
using System.Collections.Generic;
using System.Text;

namespace InferenceEngine
{
    public class TruthTable : SearchMethods
    {
        private List<List<Clause>> _models;             // list of lists containing clauses that are the models of the truth table. each row is a model
        private int _count;                             // numbers of times the KB is true.


        // Used to clone each list/model - this is similar to deep copy in DSP making sure that we maintain seperate validity values
        private List<Clause> cloneList(List<Clause> test)
        {
            List<Clause> result = new List<Clause>();

            for (int i = 0; i < test.Count; i++)
            {
                // all parts of the clause are needed - see clause constuctor
                List<string> propositions = test[i].Proposition;
                string op = test[i].Operator;
                string value = test[i].Value;

                // create new list/model by passing in the above parts
                Clause temp = new Clause(propositions, op, value);

                // setting up the validity of test clause - whether it is true or false
                temp.Validity = test[i].Validity;

                result.Add(temp);
            }
            return result;
        }

        private void setValue(List<Clause> holder, Clause p)                                  //holder is a list of clauses, p is just one clause
        {
            for (int i = 0; i < holder.Count; i++)
            {
                if (holder[i].Value == p.Value && holder[i].Proposition == p.Proposition)     //if the value of the clause in position [i] of holder is the same
                {                                                                             //as clause p value AND clause in position [i] of holder has
                                                                                              //the same proposition as clause p, then the validity of the clause in position [i]
                    holder[i].Validity = true;                                                //of holder is true.


                }
            }
        }


        public TruthTable()
        {
            _models = new List<List<Clause>>(); // create a new list of list type clause - these are the rows of truth table
            _count = 0;                         // default amount of time KB is true.
        }

        public override bool Solve(KnowledgeBase kB, Clause q)          //override the solve method in Search Methods class
        {
            // symbols is a list of propositional symbols taken from Kb and query as described in lecture 07
            Stack<Clause> symbols = new Stack<Clause>();        // stack offers easy management of symbols - Last-in-First out
            List<Clause> init = new List<Clause>();             // the inital row has everything as false 

            symbols.Push(q);                                    // put clause q into symbols stack

            for (int i = 0; i < kB.KbSize; i++)
            {
                symbols.Push(kB.kbIndexer(i));
                init.Add(kB.kbIndexer(i));                        // Add the value returned by the kb indexer to the inital row  List.
            }

            // the clause q is added to the model AFTER the knowledge base -
            // this makes sure it does not replace its contents in the loop by being at the front of the list

            init.Add(q);

            _models.Add(init);      //add inital list(row) to models List

            List<Clause> testList = cloneList(init);                        //create test list by cloning the inital List 
            testList[0].Validity = true;                                    //testList clause in position 0 has validity set to true.

            return checkAll(kB, q, symbols, init);
        }

        public bool checkAll(KnowledgeBase Kb, Clause q, Stack<Clause> symbols, List<Clause> model)
        {
            if (symbols.Count == 0)
            {
                if (checkKb(Kb, model))
                {
                    return (checkQuery(q, model));
                }
                else
                {
                    return true;
                }
            }
            else
            {
                Clause p = symbols.Pop();
                List<Clause> t = cloneList(model);
                List<Clause> f = cloneList(model);

                setValue(t, p);                     // pass a list of clauses in this case 't' which is a clone of model AND p which is single clause/proposition symbol.

                _models.Add(t);
                _models.Add(f);

                bool testT = checkAll(Kb, q, symbols, t);
                bool testF = checkAll(Kb, q, symbols, t);

                if (testT == true || testF == true)             // one of them MUST be true in order to return true 
                {
                    return true;
                }
                else
                {
                    return false;                              //otherwise it is false
                }
            }

        }

        public bool checkKb(KnowledgeBase Kb, List<Clause> model)
        {
            for (int i = 0; i < Kb.KbSize; i++)
            {
                if (model[i].Validity == false)
                {
                    return false;
                }
            }
            _count++;
            return true;
        }

        public bool checkQuery(Clause q, List<Clause> model)
        {
            for (int i = 0; i < model.Count; i++)
            {
                if (model[i].Value == q.Value && model[i].Proposition == q.Proposition)     //if the clause at position i in model has a value that is equal to
                                                                                            //clause q's value AND the clause in position i in model has the same
                                                                                            //proposition as clause q do the following
                {
                    if (model[i].Validity == true)                                          // if the clause at position i in model has a validity equal to true
                                                                                            // then return true
                    {
                        return true;
                    }
                }
            }
            return false;                                                                   // otherwise return false
        }

        public int Count
        {
            get { return _count; }


            /*public bool checkKb(KnowledgeBase Kb, List<Clause> model)         // first attempt at Implimentation did not work. kept for documentation can be removed.
            {

                for (int i = 0; i <Kb.KbSize; i++)
                {
                    bool result;
                    if (model[i].Validity == false)
                    {
                        result = false;

                        _count++;
                    }
                    else
                    {
                        result = true;
                    }


                }
                return result;
            }*/



        }
    }
}
