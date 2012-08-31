using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ExpressionTreeTraining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Updated To Branch 2");
            Console.WriteLine("Expression Tree Test");
            Console.WriteLine();

            MethodThree();
            MethodFour();
        }

        private static void MethodOne()
        {
            //create exp on the fly
            Expression<Func<int, bool>> lambda = num => num < 5;
            Console.WriteLine(lambda.Compile()(5));
            Console.ReadLine();
        }

        private static void MethodTwo()
        {
            //create manually
            ParameterExpression numParam = Expression.Parameter(typeof(int), "num");
            ConstantExpression five = Expression.Constant(5, typeof(int));
            BinaryExpression numLessThanFive = Expression.LessThan(numParam, five);

            var exp = Expression.Lambda<Func<int, bool>>(numLessThanFive, new ParameterExpression[] { numParam });

            Console.WriteLine(exp.Compile()(10));
            Console.ReadLine();
        }

        private static void MethodThree()
        {
            ParameterExpression value = Expression.Parameter(typeof(int), "value");
            ParameterExpression result = Expression.Parameter(typeof(int), "result");

            LabelTarget label = Expression.Label(typeof(int));

            BlockExpression block = Expression.Block(
                    new[] { result },
                    Expression.Assign(result, Expression.Constant(1)),
                        Expression.Loop(
                            Expression.IfThenElse(
                                Expression.GreaterThan(value, Expression.Constant(1)),
                                Expression.MultiplyAssign(result, Expression.PostDecrementAssign(value)),
                                Expression.Break(label, result)
                            ),
                            label
                        )
                );

            int factorial = Expression.Lambda<Func<int, int>>(block, value).Compile()(5);
            Console.WriteLine(factorial);
            Console.ReadLine();
        }

        private static void MethodFour()
        {
            Expression<Func<int, bool>> exprTree = num => num < 5;

            //parameter expression
            var param = exprTree.Parameters[0];
            
            //operation
            var operation = (BinaryExpression)exprTree.Body;

            //left
            var left = (ParameterExpression)operation.Left;

            //right
            var right = (ConstantExpression)operation.Right;

            Console.WriteLine("Decomposed expression: {0} => {1} {2} {3}",
                param.Name,
                left.Name,
                operation.NodeType,
                right.Value);
            Console.ReadLine();
        }
    }
}
