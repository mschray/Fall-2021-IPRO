using System;
using System.Collections.Generic;
using System.Text;

namespace JebraAzureFunctions.Questions
{
    class Trigonometry
    {
        /// <summary>
        /// Calculates the GCD of two positive integers using the Euclidean algorithm
        /// TODO: Move into separate math utilities class?
        /// </summary>
        public static int CalculateGCD(int a, int b)
        {
            // Use the Euclidean algorithm
            while (b > 0)
            {
                int remainder = a % b;
                a = b;
                b = remainder;
            }
            return a;
        }

        /// <summary>
        /// Takes a numerator and denominator and produces a string that is the reduced fraction.
        /// If the reduced denominator is 1, then just outputs the numerator.
        /// TODO: Move into separate math utilities class?
        /// </summary>
        public static string ReduceFraction(int num, int denom)
        {
            int gcd = CalculateGCD(num, denom);
            num /= gcd;
            denom /= gcd;
            if (denom == 1)
            {
                return num.ToString();
            }
            else
            {
                return $"{num}/{denom}";
            }
        }

        /// <summary>
        /// Generates an integral Pythagorean triple.
        /// First two side lengths are the legs; the third is the hypotenuse.
        /// </summary>
        public static (int, int, int) GeneratePythagoreanTriple(Random r)
        {
            // Side length parameters
            int m = r.Next(2, 6);
            int n = r.Next(1, m);

            // Side lengths (c is the hypotenuse)
            int a = m * m - n * n;
            int b = 2 * m * n;
            int c = m * m + n * n;

            return (a, b, c);
        }

        /// <summary>
        /// Takes in right triangle lengths.
        /// Randomly picks an angle and a trig function.
        /// Then, it calculates the numerator and denominator of that function's value for that angle.
        /// </summary>
        /// <returns>
        /// A quadruple of (angle name "A"/"B", sine function "sine"/"cosine"/"tangent", numerator, denominator)
        /// </returns>
        public static (string, string, int, int) PickAngleAndTrigFunction(Random r, int a, int b, int c)
        {
            // Pick which angle to calculate trig function for
            string angle = (r.NextDouble() < 0.5) ? "A" : "B";

            // Pick which trig function to calculate
            int funcIndex = r.Next(3);
            string func = (funcIndex == 0) ? "sine" : (funcIndex == 1) ? "cosine" : "tangent";

            // Figure out numerator and denominator of answer
            int num, denom;
            if (func == "sine")
            {
                num = (angle == "A") ? a : b;
                denom = c;
            }
            else if (func == "cosine")
            {
                num = (angle == "A") ? b : a;
                denom = c;
            }
            else // func == "tangent"
            {
                num = (angle == "A") ? a : b;
                denom = (angle == "A") ? b : a;
            }

            return (angle, func, num, denom);
        }

        /// <summary>
        /// Generates a trig function question for a right triangle.
        /// Answer is a reduced fraction.
        /// Ex. Calculate sin(A) given a = 3, b = 4, c = 5. Answer = 3/5.
        /// </summary>
        public static QuestionModel TrigFunctions()
        {
            Random r = new Random();

            // Generate triangle
            var (a, b, c) = GeneratePythagoreanTriple(r);

            // Pick angle, trig function, and calculate numerator and denominator
            var (angle, func, num, denom) = PickAngleAndTrigFunction(r, a, b, c);

            // Turn into reduced fraction
            string answer = ReduceFraction(num, denom);

            QuestionModel questionModel = new QuestionModel();

            // TODO: Create specific JSON model for trig function questions? Might be unnecessary.
            questionModel.question = $"{{\"a\": {a}, \"b\": {b}, \"c\": {c}, \"angle\": \"{angle}\", \"function\": \"{func}\"}}";

            questionModel.answer_a = answer;
            questionModel.answer_b = "null";
            questionModel.subject_id = Tools.GetSubjectIdFromString("Trig Functions");
            questionModel.is_json = true;

            return questionModel;
        }

        /// <summary>
        /// Generates an inverse trig function question for a right triangle.
        /// Answer is an angle in degrees rounded to the nearest degree.
        /// Ex. Calculate asin(A) given a = 3, b = 4, c = 5. Answer = 37.
        /// </summary>
        public static QuestionModel InverseTrigFunctions()
        {
            Random r = new Random();

            // Generate triangle
            var (a, b, c) = GeneratePythagoreanTriple(r);

            // Pick angle, trig function, and calculate numerator and denominator
            var (angle, func, num, denom) = PickAngleAndTrigFunction(r, a, b, c);

            // Calculate value of trig function
            double trigValue = (double)num / denom;

            // Calculate answer
            double angleMeasure = (func == "sine") ? Math.Asin(trigValue) : (func == "cosine") ? Math.Acos(trigValue) : Math.Atan2(num, denom);

            // Convert to degrees
            angleMeasure *= 180 / Math.PI;

            // Round to nearest hundredth of a degree
            angleMeasure = Math.Round(angleMeasure, 2);

            QuestionModel questionModel = new QuestionModel();

            // TODO: Create specific JSON model for trig function questions? Might be unnecessary.
            questionModel.question = $"{{\"a\": {a}, \"b\": {b}, \"c\": {c}, \"angle\": \"{angle}\", \"function\": \"arc{func}\"}}";

            questionModel.answer_a = angleMeasure.ToString();
            questionModel.answer_b = "null";
            questionModel.subject_id = Tools.GetSubjectIdFromString("Inverse Trig Functions");
            questionModel.is_json = true;

            return questionModel;
        }

    }
}
