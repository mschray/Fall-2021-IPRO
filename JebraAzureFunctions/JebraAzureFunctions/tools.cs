using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections;
using JebraAzureFunctions.Models;

namespace JebraAzureFunctions
{
    class Tools
    {
        public Tools() { }

        /// <summary>
        /// Prints to Visual Studio debug console.
        /// </summary>
        /// <param name="s">String to print.</param>
        public static void PrintDebug(string s)
        {
            System.Diagnostics.Debug.WriteLine(s);
        }

        /// <summary>
        /// Coverts a SqlDataReader into a JSON object representing the query results.
        /// </summary>
        /// <param name="dataReader"></param>
        /// <returns>A JSON string representing the query results.</returns>
        public static String SqlDatoToJson(SqlDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }

        /// <summary>
        /// Executes an async non-query command on the Azure sql database.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>True if the command was sent.</returns>
        public static async Task<bool> ExecuteNonQueryAsync(string command)
        {
            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            try
            {
                using (SqlConnection conn = new SqlConnection(str))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(command, conn))
                    {
                        int exeTask = await cmd.ExecuteNonQueryAsync();
                    }

                    await conn.DisposeAsync();
                    return true;
                }
            }
            catch (SqlException ex) {
                // Catch SQL Exceptions and print them to console for debug purposes
                StringBuilder errorMessages = new StringBuilder();
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }
                Console.WriteLine(errorMessages.ToString());
                return false;
            }
            catch { return false; }
        }

        /// <summary>
        /// Executes a query on the remote database.
        /// To retrieve the result, you must do .GetAwaiter().GetResult();
        /// See below example from the GetQuestion function.
        /// EX: responseMessage = Tools.ExecuteQueryAsync(command).GetAwaiter().GetResult();
        /// </summary>
        /// <param name="command">Command to be executed.</param>
        /// <returns>A json string representing the query result.</returns>
        public static async Task<string> ExecuteQueryAsync(string command)
        {
            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                await conn.OpenAsync();

                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    Task<SqlDataReader> task = cmd.ExecuteReaderAsync();

                    SqlDataReader rows = task.GetAwaiter().GetResult();

                    string res = SqlDatoToJson(rows);

                    await rows.DisposeAsync();//Cleanup
                    await conn.CloseAsync();
                    return res;
                }
            }
        }
        
        public static int GetIdFromResponse(string s)
        {
            //Console.WriteLine(s.Substring(1, s.Length - 2));
            dynamic data = JsonConvert.DeserializeObject(s.Substring(1, s.Length - 2));//Removes [] from ends.
            //Console.WriteLine($"ID: {data?.id}");
            return data?.id;
        }

        /// <summary>
        /// Converts a list of questions from a request body into a List<QuestionModel>
        /// </summary>
        /// <param name="questionList"></param>
        /// <returns>List of QuestionModel</returns>
        public static List<QuestionModel> JsonQuestionsToModelArray(dynamic questionList)
        {
            List <QuestionModel> ret = new List<QuestionModel>() { };
            foreach (var obj in questionList)
            {
                QuestionModel q = new QuestionModel();
                q.answer_a = obj?.answer_a;
                q.answer_b = obj?.answer_b;
                q.question = obj?.question;
                q.id = obj?.id;
                q.subject_id = obj?.subject_id;
                q.is_json = obj?.is_json;
                ret.Add(q);
            }
            return ret;
        }

        /// <summary>
        /// Converts a list of events from a request body into a List<StageEventModel> 
        /// </summary>
        /// <param name="questionList"></param>
        /// <returns></returns>
        public static List<StageEventModel> JsonEventsToModelArray(dynamic questionList)
        {
            List<StageEventModel> ret = new List<StageEventModel>() { };
            foreach (var obj in questionList)
            {
                StageEventModel e = new StageEventModel();
                //q.answer_a = obj?.answer_a;
                //q.answer_b = obj?.answer_b;
                //q.question = obj?.question;
                //q.id = obj?.id;
                //q.subject_id = obj?.subject_id;

                e.id = obj?.id;
                e.inflicted_hp = obj?.inflicted_hp;
                e.was_correct = obj?.was_correct;
                e.event_time = obj?.event_time;

                ret.Add(e);
            }
            return ret;
        }

        /// <summary>
        /// Tests if a question has been entered into the database already.
        /// </summary>
        /// <param name="question"></param>
        /// <param name="questionList"></param>
        /// <returns>True if the question is unique.</returns>
        public static Boolean UniqueQuestion(QuestionModel question, List<QuestionModel> questionList)
        {
            foreach (QuestionModel obj in questionList)
            {
                //Console.WriteLine(obj.address);
                Console.WriteLine($"Q:{obj?.question}");
                if (question.question.Equals((string)obj.question))
                {
                    //Console.WriteLine("MATCH");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Takes a List of QuestionModels and inserts them into the database. 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static async Task<bool> InsertQuestionsAsync(List<QuestionModel> list)
        {
            try
            {
                string command = InsertQuestionsSQLCommandGenerator(list);
                await ExecuteNonQueryAsync(command);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /*
         * BROKEN
        public static int GetRandomIntInRange(int minNumber, int maxNumber)
        {
            return new Random().Next() * (maxNumber - minNumber) + minNumber;
        }
        */

        /// <summary>
        /// Generates a list of questions which are not already in the database.
        /// </summary>
        /// <param name="QuestionGeneratorFunct">Base function used to generate the question.</param>
        /// <param name="amount">Amount of questions to generate.</param>
        /// <param name="subjectId">Subject id of the questions to be generated.</param>
        /// <returns></returns>
        public static List<QuestionModel> GenerateUniqueQuestions(Func<QuestionModel> QuestionGeneratorFunct, int amount, int subjectId)
        {
            string questionsS = Tools.ExecuteQueryAsync($"SELECT * FROM question WHERE subject_id='{subjectId}'").GetAwaiter().GetResult();
            dynamic questionListD = JsonConvert.DeserializeObject(questionsS);
            List<QuestionModel> questionList = Tools.JsonQuestionsToModelArray(questionListD);
            List<QuestionModel> ret = new List<QuestionModel>() { };

            int maxAttempts = 100; //Maximum attempts to generate a unique question

            for (int i = 0; i < amount; i++)
            {
                QuestionModel question = QuestionGeneratorFunct();
                int count = 0;
                while (Tools.UniqueQuestion(question, questionList) == false && count < maxAttempts)
                {
                    question = QuestionGeneratorFunct();
                    count++;
                }
                if (count < maxAttempts)
                {
                    ret.Add(question);
                    questionList.Add(question);
                    //command += $"INSERT INTO question VALUES('{question.answer_a}', null, '{question.question}', {subjectId}) \n";
                }
                else
                {
                    break;
                }
            }

            return ret;
        }

        /// <summary>
        /// Given a list of QuestionModels, will generate a sql command to insert them into a database. 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string InsertQuestionsSQLCommandGenerator(List<QuestionModel> list)
        {
            string command = "";
            foreach(QuestionModel question in list)
            {
                command += $"INSERT INTO question VALUES('{question.answer_a}', {question.answer_b}, '{question.question}', {question.subject_id}, '{question.is_json}') \n";
            }
            return command;
        }

        /// <summary>
        /// Retreive the subject_id based on the subject_name field.
        /// </summary>
        /// <param name="subject"></param>
        /// <returns>The subject's subject_id</returns>
        public static int GetSubjectIdFromString(string subject)
        {
            string subjectIdString = Tools.ExecuteQueryAsync($"SELECT id FROM subject WHERE subject_name='{subject}'").GetAwaiter().GetResult();
            //[{"id":2}]
            subjectIdString = subjectIdString.Substring(1, subjectIdString.Length - 2);
            //{"id":2}
            dynamic data = JsonConvert.DeserializeObject(subjectIdString);
            return data?.id;
        }
        
        /// <summary>
        /// ex: 4^2 = 16
        /// </summary>
        /// <returns>A QuestionModel</returns>
        public static QuestionModel SimplifyExponents()
        {
            Random r = new Random();
            int num = r.Next(1, 10);
            //int square = num ^ 2;
            int square = num * num;

            QuestionModel questionModel = new QuestionModel();
            questionModel.answer_a = square.ToString();
            questionModel.answer_b = "null";
            questionModel.question = "$" + num + "^{2}$";
            questionModel.subject_id = GetSubjectIdFromString("Simplify Exponents");
            return questionModel;
        }

        //Result = Math.Pow(Number1, Number2);

        /// <summary>
        /// ex: 4^4 = 256
        /// </summary>
        /// <returns>A QuestionModel</returns>
        public static QuestionModel SimplifyExponents2()
        {
            Random r = new Random();
            int num = r.Next(1, 5);
            int exp = r.Next(1, 5);
            int exponential = (int)Math.Pow(num, exp);

            QuestionModel questionModel = new QuestionModel();
            questionModel.answer_a = exponential.ToString();
            questionModel.answer_b = "null";
            questionModel.question = "$" + num + "^{" + exp + "}$";

            questionModel.subject_id = GetSubjectIdFromString("Simplify Exponents 2");
            return questionModel;
        }

        /// <summary>
        /// ex: sqrt(16) = 4
        /// </summary>
        /// <returns>A QuestionModel</returns>
        public static QuestionModel SimplifySquareRoots()
        {
            Random r = new Random();
            int num = r.Next(1, 10);
            int square = num * num;

            QuestionModel questionModel = new QuestionModel();
            questionModel.answer_a = num.ToString();
            questionModel.answer_b = (0 - num).ToString();
            questionModel.question = "$\\sqrt{" + square + "}$";
            questionModel.subject_id = GetSubjectIdFromString("Simplify Square Roots");
            return questionModel;
        }

        /// <summary>
        /// ex: 4! = 24
        /// </summary>
        /// <returns>A QuestionModel</returns>
        public static QuestionModel Factorials()
        {
            Random r = new Random();
            int num = r.Next(1, 6);
            int factorial = 1;

            for (int j = 1; j <= num; j++)
            {
                factorial = j * factorial;
            }

            //string command = "";
            //command = $"INSERT INTO question(answer_a, answer_b, question) VALUES({factorial},null,'{num}!')";

            QuestionModel questionModel = new QuestionModel();
            questionModel.answer_a = factorial.ToString();
            questionModel.answer_b = "null";
            questionModel.question = num + "!";
            questionModel.subject_id = GetSubjectIdFromString("Factorials");
            return questionModel;
        }

        /// <summary>
        /// ex: (-2,3) is in quadrant II
        /// </summary>
        /// <returns>A QuestionModel</returns>
        public static QuestionModel CartesianCoordinates()
        {
            Random r = new Random();
            int x = r.Next(-10, -11);
            int y = r.Next(-10, -11);

            while (x == 0 || y == 0)
            {
                x = r.Next(-10, -11);
                y = r.Next(-10, -11);
            }

            QuestionModel questionModel = new QuestionModel();
            questionModel.subject_id = GetSubjectIdFromString("Cartesian Coordinates");

            if (x > 0 && y > 0)
            {
                questionModel.answer_a = "1";
                questionModel.question = "(" + x + "," + y + ")";            
            }
            else if (x < 0 && y > 0)
            {
                questionModel.answer_a = "2";
                questionModel.question = "(" + x + "," + y + ")";
            }
            else if (x < 0 && y < 0)
            {
                questionModel.answer_a = "3";
                questionModel.question = "(" + x + "," + y + ")";
            }
            else
            {
                questionModel.answer_a = "4";
                questionModel.question = "(" + x + "," + y + ")";
            }
            questionModel.answer_b = "null";
            return questionModel;
        }

        /// <summary>
        /// ex: 3x - 5 = 10 -> x = 5
        /// </summary>
        /// <returns>A QuestionModel</returns>
        public static QuestionModel SingleVariable()
        {
            Random r = new Random();
            //factor * x + sumNum = eNum
            int eNum = 0;
            int sumNum = 0;
            List<int> possibleFactors = new List<int>();
            while (possibleFactors.Count < 1)
            {
                eNum = r.Next(0, 200);
                sumNum = r.Next(-100, 100);

                int n = eNum - sumNum;

                for (int j = 1; j <= n; j++)//Get all round divisors and randomly pick one to be what x gets multiplied by.
                {
                    if (n % j == 0)
                    {
                        possibleFactors.Add(j);
                    }
                }
            }

            int factor = possibleFactors[r.Next(0, possibleFactors.Count - 1)];
            int x = (eNum - sumNum) / factor;

            QuestionModel questionModel = new QuestionModel();

            if (sumNum < 0)
            {
                int sumNumB = sumNum * -1;             
                questionModel.answer_a = x.ToString();
                questionModel.question = factor + x + "-" + sumNumB + "=" + eNum;
                
            }
            else
            {
                questionModel.answer_a = x.ToString();
                questionModel.question = factor + x + "+" + sumNum + "=" + eNum;
            }
            questionModel.subject_id = GetSubjectIdFromString("Single Variable");
            questionModel.answer_b = "null";
            return questionModel;
        }

        /// <summary>
        /// ex: x + 4 = 3x - 6 -> x = 5
        /// </summary>
        /// <returns></returns>
        public static QuestionModel SystemOfEquations()
        {
            Random r = new Random();
            int eNum = 0;
            int sumNum = 0;
            List<int> possibleFactors = new List<int>();
            while (possibleFactors.Count < 1)
            {
                eNum = r.Next(0, 200);
                sumNum = r.Next(-100, 100);

                int n = eNum - sumNum;

                for (int j = 1; j <= n; j++)//Get all round divisors and randomly pick one to be what x gets multiplied by.
                {
                    if (n % j == 0)
                    {
                        possibleFactors.Add(j);
                    }
                }
            }

            int factor = possibleFactors[r.Next(0, possibleFactors.Count - 1)];
            int x = (eNum - sumNum) / factor;

            int factor1 = factor - r.Next(1, factor);
            int factor2 = factor - factor1;

            QuestionModel questionModel = new QuestionModel();

            if (sumNum < 0)
            {
                int sumNumB = sumNum * -1;
                questionModel.answer_a = x.ToString();
                questionModel.question = factor + x + "-" + sumNum + "=" + factor2 + x + "+" + eNum;
                
                //command = $"INSERT INTO question(answer_a, answer_b, question) VALUES({x},null,'{factor1}x - {sumNum} = {factor2}x + {eNum}')";
            }
            else
            {  
                questionModel.answer_a = x.ToString();
                questionModel.question = factor + x + "+" + sumNum + "=" + factor2 + x + "+" + eNum;
                //command = $"INSERT INTO question(answer_a, answer_b, question) VALUES({x},null,'{factor1}x + {sumNum} = {factor2}x + {eNum}')";
            }

            questionModel.subject_id = GetSubjectIdFromString("System Of Equations");
            questionModel.answer_b = "null";
            return questionModel;
        }

        /// <summary>
        /// Generates a monic quadratic polynomial with integer roots between -10 and 10
        /// ex: x^2-5x+4 => roots are 1, 4
        /// </summary>
        /// <returns></returns>
        public static QuestionModel QuadraticRoots()
        {
            Random r = new Random();

            // Construct expression "x^2 + bx + c"
            int root1 = r.Next(-10, 11);
            int root2 = r.Next(root1, 11);
            int b = -root1 - root2; // coefficient of x term
            int c = root1 * root2; // constant

            string xTerm;
            if (b > 0)
            {
                xTerm = $"+{b}x";
            }
            else if (b == 0)
            {
                xTerm = "";
            }
            else // b < 0
            {
                xTerm = $"{b}x";
            }

            string constant;
            if (c > 0)
            {
                constant = $"+{c}";
            }
            else if (c == 0)
            {
                constant = "";
            }
            else // c < 0
            {
                constant = c.ToString();
            }

            string expression = $"x^2{xTerm}{constant}";

            QuestionModel questionModel = new QuestionModel();
            questionModel.question = expression;
            questionModel.answer_a = root1.ToString();
            questionModel.answer_b = root2.ToString();
            questionModel.subject_id = GetSubjectIdFromString("Quadratic Roots");

            return questionModel;
        }

        public static QuestionModel TrigFunctions()
        {
            Random r = new Random();

            // Clever way to generate an integral Pythagorean triple
            int m = r.Next(2, 6);
            int n = r.Next(1, m);

            // Side lengths (c is the hypotenuse)
            int a = m * m - n * n;
            int b = 2 * m * n;
            int c = m * m + n * n;

            QuestionModel questionModel = new QuestionModel();
            questionModel.question = "dummy";
            questionModel.answer_a = "dummy";
            questionModel.answer_b = null;
            questionModel.subject_id = GetSubjectIdFromString("Trig Functions");
            questionModel.is_json = true;

            return questionModel;
        }

        //answer type is string, wont work currently
        /*
        //ex: (x + 3)*(x + 4) = x^2 + 7x + 12
        private static async Task FactoringQuadratics(int amount, Boolean reverse)
        {
            for (int i = 0; i < amount; i++)
            {
                Random r = new Random();
                int firstNum = r.Next(-9, 10);
                int secondNum = r.Next(-9, 10);

                string command = "";

                //q: factors; a: factor expression
                //q: (x - firstNum) * (x + secondNum); a: x^2 - (firstNum + secondNum) - (firstNum*secondNum)
                if (reverse == false)
                {
                    if (firstNum < 0 && secondNum > 0)
                    {
                        int firstNumB = firstNum * -1;
                        if (firstNumB > secondNum)
                        {
                            QuestionModel questionModel = new QuestionModel();
                            questionModel.answer_a = "-" + (firstNumB - secondNum) + "-" + firstNumB * secondNum;
                            questionModel.question = factor + x + "-" + sumNum + "=" + factor2 + x + "+" + eNum;
                            questionList.Add(questionModel);
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('x^2 - {firstNumB - secondNum}x - {firstNumB * secondNum}', null, '(x - {firstNumB}) * (x + {secondNum})')";
                        }
                        else if (firstNumB < secondNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('x^2 + {secondNum - firstNumB}x - {firstNumB * secondNum}', null, '(x - {firstNumB}) * (x + {secondNum})')";
                        }
                        else if (firstNumB == secondNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('x^2 - {firstNumB * secondNum}', null, '(x - {firstNumB}) * (x + {secondNum})')";
                        }

                    }
                    else if (firstNum > 0 && secondNum < 0)
                    {
                        int secondNumB = secondNum * -1;
                        if (secondNumB > firstNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('x^2 - {secondNumB - firstNum}x - {firstNum * secondNumB}', null, '(x + {firstNum}) * (x - {secondNumB})')";
                        }
                        else if (secondNumB < firstNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('x^2 + {firstNum - secondNumB}x - {firstNum * secondNumB}', null, '(x + {firstNum}) * (x - {secondNumB})')";
                        }
                        else if (secondNumB == firstNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('x^2 - {secondNumB * firstNum}', null, '(x + {firstNum}) * (x - {secondNumB})')";
                        }
                    }
                }
                //q: factor expression; a: factors
                //q: x^2 - (firstNum + secondNum) - (firstNum*secondNum); a: (x - firstNum) * (x + secondNum)
                else if (reverse == true)
                {
                    if (firstNum < 0 && secondNum > 0)
                    {
                        int firstNumB = firstNum * -1;
                        if (firstNumB > secondNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('(x - {firstNumB}) * (x + {secondNum})', null, 'x^2 - {firstNumB - secondNum}x - {firstNumB * secondNum}')";
                        }
                        else if (firstNumB < secondNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('(x - {firstNumB}) * (x + {secondNum})', null, 'x^2 + {secondNum - firstNumB}x - {firstNumB * secondNum}')";
                        }
                        else if (firstNumB == secondNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('(x - {firstNumB}) * (x + {secondNum})', null, 'x^2 - {firstNumB * secondNum}')";
                        }

                    }
                    else if (firstNum > 0 && secondNum < 0)
                    {
                        int secondNumB = secondNum * -1;
                        if (secondNumB > firstNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('(x + {firstNum}) * (x - {secondNumB})', null, 'x^2 - {secondNumB - firstNum}x - {firstNum * secondNumB}')";
                        }
                        else if (secondNumB < firstNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('(x + {firstNum}) * (x - {secondNumB})', null, 'x^2 + {firstNum - secondNumB}x - {firstNum * secondNumB}')";
                        }
                        else if (secondNumB == firstNum)
                        {
                            command = $"INSERT INTO question(answer_a, answer_b, question) VALUES('(x + {firstNum}) * (x - {secondNumB})', null, 'x^2 - {secondNumB * firstNum}')";
                        }
                    }
                }
            }
        } */
    }
}
