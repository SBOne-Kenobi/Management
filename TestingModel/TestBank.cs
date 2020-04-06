using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Management;

namespace TestingModel
{
    [TestClass]
    public class TestBank
    {
        public bool IsEqualLists<T>(List<T> a, List<T> b) where T : IComparable<T>
        {
            if (a.Count != b.Count)
                return false;
            a.Sort();
            b.Sort();
            for (int i = 0; i < a.Count; i++)
                if (a[i].CompareTo(b[i]) != 0)
                    return false;
            return true;
        }

        [TestMethod]
        public void TestRequestOfMat1()
        {
            //init
            int n = 3;
            List<Demand> demands = new List<Demand>
            {
                new Demand(400, 1, 0),
                new Demand(700, 100, 1),
                new Demand(200, 125, 2)
            };
            List<Demand> expected = new List<Demand>
            {
                new Demand(400, 0, 0),
                new Demand(700, 6, 1),
                new Demand(200, 0, 2)
            };

            //act
            Bank bank = new Bank(n);
            List<Demand> actual = bank.RequestOfMat(demands);

            //assert
            Assert.IsTrue(IsEqualLists<Demand>(expected, actual));
        }

        [TestMethod]
        public void TestRequestOfMat2()
        {
            //init
            int n = 3;
            List<Demand> demands = new List<Demand>
            {
                new Demand(400, 1, 0),
                new Demand(500, 100, 1),
                new Demand(700, 6, 2)
            };
            List<Demand> expected = new List<Demand>
            {
                new Demand(400, 0, 0),
                new Demand(500, 0, 1),
                new Demand(700, 6, 2)
            };
            //act
            Bank bank = new Bank(n);
            List<Demand> actual = bank.RequestOfMat(demands);

            //assert
            Assert.IsTrue(IsEqualLists<Demand>(expected, actual));
        }

        [TestMethod]
        public void TestRequestOfMat3()
        {
            //init
            int n = 3;
            List<Demand> demands = new List<Demand>
            {
                new Demand(400, 1, 0),
                new Demand(700, 2, 1),
                new Demand(500, 6, 2)
            };
            List<Demand> expected = new List<Demand>
            {
                new Demand(400, 0, 0),
                new Demand(700, 2, 1),
                new Demand(500, 4, 2)
            };
            //act
            Bank bank = new Bank(n);
            List<Demand> actual = bank.RequestOfMat(demands);

            //assert
            Assert.IsTrue(IsEqualLists<Demand>(expected, actual));
        }

        [TestMethod]
        public void TestRequestOfMat4()
        {
            //init
            int n = 3;
            List<Demand> demands = new List<Demand>
            {
                new Demand(400, 1, 0),
                new Demand(700, 1, 1),
                new Demand(500, 4, 2)
            };
            List<Demand> expected = new List<Demand>
            {
                new Demand(400, 0, 0),
                new Demand(700, 1, 1),
                new Demand(500, 4, 2)
            };

            //act
            Bank bank = new Bank(n);
            List<Demand> actual = bank.RequestOfMat(demands);

            //assert
            Assert.IsTrue(IsEqualLists<Demand>(expected, actual));
        }

        [TestMethod]
        public void TestRequestOfMat5()
        {
            //init
            int n = 3;
            List<Demand> demands = new List<Demand>
            {
                new Demand(10000, 1, 0),
                new Demand(500, 1, 1),
                new Demand(500, 6, 2)
            };
            List<Demand> expected = new List<Demand>
            {
                new Demand(10000, 1, 0),
                new Demand(500, 1, 1),
                new Demand(500, 4, 2)
            };
            //act
            Bank bank = new Bank(n);
            List<Demand> actual = bank.RequestOfMat(demands);

            //assert
            Assert.IsTrue(IsEqualLists<Demand>(expected, actual));
        }

        [TestMethod]
        public void TestRequestOfMat6()
        {
            //init
            int n = 3;
            List<Demand> demands = new List<Demand>
            {
                new Demand(500, 1, 0),
                new Demand(700, 1, 1),
                new Demand(800, 6, 2)
            };
            List<Demand> expected = new List<Demand>
            {
                new Demand(500, 0, 0),
                new Demand(700, 0, 1),
                new Demand(800, 6, 2)
            };
            //act
            Bank bank = new Bank(n);
            List<Demand> actual = bank.RequestOfMat(demands);

            //assert
            Assert.IsTrue(IsEqualLists<Demand>(expected, actual));
        }

        [TestMethod]
        public void TestRequestOfMat7()
        {
            //init
            int n = 3;
            List<Demand> demands = new List<Demand>
            {
                new Demand(700, 1, 0),
                new Demand(700, 1, 1),
                new Demand(800, 5, 2)
            };
            List<Demand> expected = new List<Demand>
            {
                new Demand(700, 1, 0),
                new Demand(700, 0, 1),
                new Demand(800, 5, 2)
            };
            //act
            Bank bank = new Bank(n);
            List<Demand> actual = bank.RequestOfMat(demands);

            //assert
            Assert.IsTrue(IsEqualLists<Demand>(expected, actual));
        }

        [TestMethod]
        public void TestRequestOfMat8()
        {
            //init
            int n = 3;
            List<Demand> demands = new List<Demand>
            {
                new Demand(700, 1, 0),
                new Demand(700, 1, 1),
                new Demand(800, 4, 2)
            };
            List<Demand> expected = new List<Demand>()
            {
                new Demand(700, 1, 0),
                new Demand(700, 1, 1),
                new Demand(800, 4, 2)
            };
            //act
            Bank bank = new Bank(n);
            List<Demand> actual = bank.RequestOfMat(demands);

            //assert
            Assert.IsTrue(IsEqualLists<Demand>(expected, actual));
        }
    }
}
