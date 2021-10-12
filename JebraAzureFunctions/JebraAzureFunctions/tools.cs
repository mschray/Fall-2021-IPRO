﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Threading.Tasks;

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
        public static String SqlDatoToJson(SqlDataReader dataReader)
        {
            var dataTable = new DataTable();
            dataTable.Load(dataReader);
            string JSONString = string.Empty;
            JSONString = JsonConvert.SerializeObject(dataTable);
            return JSONString;
        }

        public static async Task<bool> ExecuteNonQueryAsync(string command)
        {
            var str = Environment.GetEnvironmentVariable("SqlConnectionString");
            using (SqlConnection conn = new SqlConnection(str))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(command, conn))
                {
                    int exeTask = await cmd.ExecuteNonQueryAsync();
                }
            }
            return true;
        }

        List<QuestionModel> questionList = new List<QuestionModel>();
        
        string questionOut;
        for (int h = 0; h < questionList.size; i++) {
                questionOut = 
            }
        string command = "";

    INSERT INTO question(answer_a, answer_b, question) VALUES({factorial},null,'{num}!')

        //ex: 4^2 = 16
        private static async Task SimplifyExponents1(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Random r = new Random();
                int num = r.Next(1, 10);
                int square = num ^ 2;

                QuestionModel questionModel = new QuestionModel();
                questionModel.answer_a = square;
                questionModel.question = num + "^2";
                questionList.Add(questionModel);
            }
        }

        //ex: 4^4 = 256
        private static async Task SimplifyExponents2(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Random r = new Random();
                int num = r.Next(1, 5);
                int exp = r.Next(1, 5);
                int exponential = num ^ exp;

                QuestionModel questionModel = new QuestionModel();
                questionModel.answer_a = exponential;
                questionModel.question = num + "^" + exp;
                questionList.Add(questionModel);
            }

        }

        //ex: sqrt(16) = 4
        private static async Task SimplifySquareRoots(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Random r = new Random();
                int num = r.Next(1, 10);
                int square = num ^ 2;

                QuestionModel questionModel = new QuestionModel();
                questionModel.answer_a = num;
                questionModel.question = "sqrt(" + square + ")";
                questionList.Add(questionModel);
        }

        }

        //ex: 4! = 24
        private static async Task Factorials(int amount)
        {
            for (int i = 0; i < amount; i++)
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
                questionModel.answer_a = factorial;
                questionModel.question = num + "!";
                questionList.Add(questionModel);
            }

        }

        //ex: (-2,3) is in quadrant II
        private static async Task CartesianCoordinates(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Random r = new Random();
                int x = r.Next(-10, -11);
                int y = r.Next(-10, -11);

                while (x == 0 || y == 0)
                {
                    x = r.Next(-10, -11);
                    y = r.Next(-10, -11);
                }

                if (x > 0 && y > 0)
                {
                    QuestionModel questionModel = new QuestionModel();
                    questionModel.answer_a = 1;
                    questionModel.question = "(" + x + "," + y + ")";
                    questionList.Add(questionModel);
                }
                else if (x < 0 && y > 0)
                {
                    QuestionModel questionModel = new QuestionModel();
                    questionModel.answer_a = 2;
                    questionModel.question = "(" + x + "," + y + ")";
                    questionList.Add(questionModel);
                }
                else if (x < 0 && y < 0)
                {
                    QuestionModel questionModel = new QuestionModel();
                    questionModel.answer_a = 3;
                    questionModel.question = "(" + x + "," + y + ")";
                    questionList.Add(questionModel);
                }
                else if (x > 0 && y < 0)
                {
                    QuestionModel questionModel = new QuestionModel();
                    questionModel.answer_a = 4;
                    questionModel.question = "(" + x + "," + y + ")";
                    questionList.Add(questionModel);
                }

            }

        }

        //ex: 3x - 5 = 10 -> x = 5
        private static async Task SingleVariable(int amount)
        {
            Random r = new Random();
            for (int i = 0; i < amount; i++)
            {
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

                if (sumNum < 0)
                {
                    int sumNumB = sumNum * -1;
                    QuestionModel questionModel = new QuestionModel();
                    questionModel.answer_a = x;
                    questionModel.question = factor + x + "-" + sumNumB + "=" +eNum;
                    questionList.Add(questionModel);
                }
                else
                {
                    QuestionModel questionModel = new QuestionModel();
                    questionModel.answer_a = x;
                    questionModel.question = factor + x + "+" + sumNum + "=" + eNum;
                    questionList.Add(questionModel);
                }

            }
        }

        //ex: x + 4 = 3x - 6 -> x = 5
        private static async Task SystemOfEquations(int amount)
        {
            Random r = new Random();
            for (int i = 0; i < amount; i++)
            {
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


                if (sumNum < 0)
                {
                    int sumNumB = sumNum * -1;
                    QuestionModel questionModel = new QuestionModel();
                    questionModel.answer_a = x;
                    questionModel.question = factor + x + "-" + sumNum + "=" + factor2 + x + "+" + eNum;
                    questionList.Add(questionModel);
                    //command = $"INSERT INTO question(answer_a, answer_b, question) VALUES({x},null,'{factor1}x - {sumNum} = {factor2}x + {eNum}')";
                }
                else
                {
                    QuestionModel questionModel = new QuestionModel();
                    questionModel.answer_a = x;
                    questionModel.question = factor + x + "+" + sumNum + "=" + factor2 + x + "+" + eNum;
                    questionList.Add(questionModel);
                    //command = $"INSERT INTO question(answer_a, answer_b, question) VALUES({x},null,'{factor1}x + {sumNum} = {factor2}x + {eNum}')";
                }
            }
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
